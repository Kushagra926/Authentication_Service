using System.Security.Cryptography;

namespace Authentication_Servie.Helpers
{
    public class RefreshTokenGenerator
    {
        public static string Generate()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
