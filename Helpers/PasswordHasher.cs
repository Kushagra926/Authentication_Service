using System.Security.Cryptography;

namespace Authentication_Servie.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100_000,
                HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(32);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100_000,
                HashAlgorithmName.SHA256);

            var computedHash = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(hash, computedHash);
        }
    }
}
