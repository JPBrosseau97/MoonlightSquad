namespace MoonlightSquad.Class.DAL;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Member";
    public string? ProfilePicture { get; set; }  // data URL base64 (ex: "data:image/png;base64,...")
    public string? AboutMe { get; set; }
}
