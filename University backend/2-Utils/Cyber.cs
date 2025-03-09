using System.Security.Cryptography;
using System.Text;

namespace University_backend;

public static class Cyber
{
    public static string HashPassword(string plainText)
    {
        string salt = "I made it with alot of thinking and im proud of it!";
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(plainText, saltBytes, 17, HashAlgorithmName.SHA512);
        byte[] hashBytes = rfc.GetBytes(64);
        string hashPassword = Convert.ToBase64String(hashBytes);
        return hashPassword;
    }

}
