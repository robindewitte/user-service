using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using twatter_userservice.DTO;
using twatter_userservice.Repositories;
using twatter_userservice.Datamodels;
using twatter_userservice.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Prometheus;

namespace twatter_userservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: Controller
    {
        private readonly UserContext _context;

        private readonly IConfiguration _config;

        public UserController(UserContext context, IConfiguration config)
        {
            lastone = DateTime.Now;
            amountofWrongAttempts = 0;
            _context = context;
            _config = config;
            totalTime = 0;
        }

        private string GenerateJSONWebToken(LoginDTO loginAttempt)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, loginAttempt.Username)
            };


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

          private static int amountofWrongAttempts;
          private static double totalTime;
          private static DateTime lastone;

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task< ActionResult<string>> Login(LoginDTO loginDTO)
        {
            Console.WriteLine("binnen");
            //hardcoded for now
            User fetchFromDB = await _context.Users.Where(b => b.Username == loginDTO.Username).SingleAsync();
            if(fetchFromDB != null && Encryptor.ValidatePassword(loginDTO.Password, fetchFromDB.Password))
            {
                string token = GenerateJSONWebToken(loginDTO);
                return token;
            }
            else
            {
                amountofWrongAttempts++;
                TimeSpan difference = DateTime.Now.Subtract(lastone);
                totalTime = totalTime + difference.TotalSeconds;
                if(totalTime.TotalSeconds > 30 && amountofWrongAttempts > 10)
                {
                    //theoretische email service hier
                    return "nu zou er een alert in de mail komen van een dictionary attack of ddos";
                }
                else if( totalTime.TotalSeconds > 30)
                {
                    totalTime = 0;
                }
                return "Verkeerd";
            }           
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task< ActionResult<RegisterResponseDTO>> Register(RegisterDTO registerDTO)
        {
            //hardcoded for now
            if (ValidateEmail(registerDTO.EmailAdress) && ValidatePassword(registerDTO.Password))
            {
                User user = new User();
                user.Username = registerDTO.Username;
                user.Password = Encryptor.EncryptPassword(registerDTO.Password);
                user.EmailAdress = registerDTO.EmailAdress;
                if(_context.Users.Any(o => o.Username == user.Username) || _context.Users.Any(o => o.EmailAdress == user.EmailAdress))
                {
                    return Json(new RegisterResponseDTO("FOUT! de gebruikersnaam of het emailadress is in gebruik."));
                }
                else 
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                }             

                return Json(new RegisterResponseDTO("U bent geregistreerd"));
            }
            return Json(new RegisterResponseDTO("FOUT! De ingevoerde gegevens voldoen niet aan de eisen."));
        }


        public static bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidatePassword(string pw)
        {   
            if(pw.Length >11 && pw.Any(char.IsUpper) && pw.Any(char.IsNumber) && pw.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return true;
            }
            return false;            
        }
    }
}
