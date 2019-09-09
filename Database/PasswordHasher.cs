using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    internal static class PasswordHasher
    {
        internal static byte[] HashPassword(string username, string password)
        {
            byte[] encryptedPassword = new byte[32];
            var hashing = SHA256.Create();

            var hashedUser = hashing.ComputeHash(Encoding.ASCII.GetBytes(username));
            var hashedPassword = hashing.ComputeHash(Encoding.ASCII.GetBytes(password));

            for (int i = 0; i < hashedPassword.Length; i++)
                encryptedPassword[i] = (byte)(hashedUser[i] ^ hashedPassword[31 - i]);

            return encryptedPassword;
        }
    }
}
