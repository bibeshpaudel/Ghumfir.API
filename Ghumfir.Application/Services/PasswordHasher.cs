using System.Security.Cryptography;

namespace Ghumfir.Application.Services;

public class PasswordHasher
{
    private const int Iterations = 10000;
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public static string HashPassword(string password)
    {
        var salt = new byte[SaltSize];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        var hash = new Rfc2898DeriveBytes(password, salt, Iterations);
        var hashBytes = hash.GetBytes(HashSize);

        var hashBytesWithSalt = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytesWithSalt, 0, SaltSize);
        Array.Copy(hashBytes, 0, hashBytesWithSalt, SaltSize, HashSize);

        return Convert.ToBase64String(hashBytesWithSalt);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var hashBytesWithSalt = Convert.FromBase64String(hashedPassword);
        var salt = new byte[SaltSize];
        Array.Copy(hashBytesWithSalt, 0, salt, 0, SaltSize);

        var hash = new Rfc2898DeriveBytes(password, salt, Iterations);
        var hashBytes = hash.GetBytes(HashSize);

        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytesWithSalt[SaltSize + i] != hashBytes[i])
            {
                return false;
            }
        }

        return true;
    }
}