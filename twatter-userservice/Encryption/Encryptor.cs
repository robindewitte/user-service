using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace twatter_userservice.Encryption
{
    public abstract class Encryptor
    {
        public static string encryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public static bool validatePassword(string originalPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(originalPassword, storedPassword);
        }
    }
}
