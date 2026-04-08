using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using MoonlightSquad.Class.DAL;

namespace MoonlightSquad.Class.BLL;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> ValidateAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return null;
        return VerifyPassword(password, user.PasswordHash) ? user : null;
    }

    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100_000, 32);
        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split(':');
        if (parts.Length != 2) return false;
        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] expectedHash = Convert.FromBase64String(parts[1]);
        byte[] actualHash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100_000, 32);
        return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
    }

    /// <summary>
    /// Crée un nouveau compte. Retourne false si le username est déjà pris.
    /// </summary>
    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (await _db.Users.AnyAsync(u => u.Username == username))
            return false;

        _db.Users.Add(new User
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Role = "Member"
        });
        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Crée un utilisateur admin par défaut si la base est vide.
    /// Changer le mot de passe après le premier démarrage !
    /// </summary>
    public async Task SeedAdminAsync()
    {
        if (!await _db.Users.AnyAsync())
        {
            _db.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = HashPassword("password"),
                Role = "Admin"
            });
            await _db.SaveChangesAsync();
        }
    }
}
