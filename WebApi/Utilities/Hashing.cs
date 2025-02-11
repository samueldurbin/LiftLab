using System.Text;

using System.Security.Cryptography;

namespace WebApi.Utilities
{
    public class Hashing
    {
        public string Hash(string password)  // password has been selected for an example
        {
            using (var sha256 = SHA256.Create()) // Initializes a SHA256
            {
                var bytes = Encoding.UTF8.GetBytes(password); // Converts plaintext into bytes

                var hash = sha256.ComputeHash(bytes); // hash

                return Convert.ToBase64String(hash); // readable string

            }
        }
    }
}
