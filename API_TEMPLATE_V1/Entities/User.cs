namespace API_TEMPLATE_V1.Entities;

using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public Role Role { get; set; }

    [JsonIgnore]
    public string? PasswordHash { get; set; }

    [JsonIgnore]
    public List<RefreshToken>? RefreshTokens { get; set; }

    public string? Cluster_id { get; set; }

}