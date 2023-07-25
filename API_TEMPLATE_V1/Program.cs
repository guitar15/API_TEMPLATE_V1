using API_TEMPLATE_V1.Authorization;
using API_TEMPLATE_V1.Entities;
using API_TEMPLATE_V1.Helpers;
using API_TEMPLATE_V1.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers()
        .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();
}

builder.Services.AddSwaggerGen();

var app = builder.Build();

// add hardcoded test user to db on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    var Users = new List<User>
    {
         new User
            {
                FirstName = "Admin",
                LastName = "Admin",
                Username = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin_P@ssw0rd"),
                Role = Role.Admin
            },
         new User
            {
                FirstName = "User",
                LastName = "User",
                Username = "User",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User_P@ssw0rd"),
                Role = Role.User
            }
    };


    var user = context.Users.SingleOrDefault(x => x.Username == Users.First().Username);
    if (user == null)
    {
        context.Users.AddRange(Users);
        context.SaveChanges();
    }
}

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var port = Environment.GetEnvironmentVariable("PORT") ?? "8190";

app.Run($"http://0.0.0.0:{port}");
