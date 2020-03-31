using System.Security.Cryptography;
using System.Text;

namespace Database
{
    // This class is used to create the password hash to be stored in the database
    // The username and password are hased using the SHA256 algorithm the results are combined
    // using the XOR to create the hashed password. The username is converted to lowercase to 
    // make the username case insensetive
    internal static class PasswordHasher
    {
        internal static byte[] HashPassword(string username, string password)
        {
            byte[] encryptedPassword = new byte[32];
            var hashing = SHA256.Create();

            if (!string.IsNullOrEmpty(username.ToLower()) && string.IsNullOrEmpty(password))
            {
                var hashedUser = hashing.ComputeHash(Encoding.ASCII.GetBytes(username));
                var hashedPassword = hashing.ComputeHash(Encoding.ASCII.GetBytes(password));

                for (int i = 0; i < hashedPassword.Length; i++)
                    encryptedPassword[i] = (byte)(hashedUser[i] ^ hashedPassword[31 - i]);
            }

            return encryptedPassword;
        }
    }
}
