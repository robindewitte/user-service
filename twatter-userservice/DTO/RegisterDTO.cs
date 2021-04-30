using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace twatter_userservice.DTO
{
    public class RegisterDTO
    {
        #region fields
        private string username;
        private string emailAdress;
        private string password;
        private string confirmPassword;
        #endregion

        #region constructors
        //empty constructor for JSON
        public RegisterDTO()
        {

        }

        public RegisterDTO(string username, string emailAdress, string password, string confirmPassword)
        {
            this.Username = username;
            this.EmailAdress = emailAdress;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }
        #endregion

        #region properties

        public string Username { get => username; set => username = value; }
        public string EmailAdress { get => emailAdress; set => emailAdress = value; }
        public string Password { get => password; set => password = value; }
        public string ConfirmPassword { get => confirmPassword; set => confirmPassword = value; }

        #endregion


    }
}
