namespace API_TEMPLATE_V1.Helpers;

using Microsoft.EntityFrameworkCore;
using API_TEMPLATE_V1.Entities;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    private readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
        //options.UseInMemoryDatabase("TestDb");
        options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
    }


}