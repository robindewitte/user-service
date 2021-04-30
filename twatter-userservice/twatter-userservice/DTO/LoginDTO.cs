using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace twatter_userservice.DTO
{
    public class LoginDTO
    {
        #region fields
        private string username;
        private string password;
        #endregion

        #region constructors
        //empty constructor for JSON
        public LoginDTO()
        {

        }

        public LoginDTO(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        #endregion

        #region properties

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        #endregion
    }
}
