namespace API_TEMPLATE_V1.Models.Users;

using System.Text.Json.Serialization;
using API_TEMPLATE_V1.Entities;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }

    public AuthenticateResponse(User user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
        Role = user.Role;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}