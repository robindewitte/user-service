using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using twatter_userservice;
using twatter_userservice.Controllers;
using twatter_userservice.DTO;
using Xunit;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace TestUserservice
{
    public class UnitTest
    {

         private readonly TestServer _server;
         private readonly HttpClient _client;

        public IConfigurationRoot Configuration { get; private set; }

        public UnitTest()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>().ConfigureAppConfiguration(config =>
               {
                   Configuration = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json")
                     .Build();

                   config.AddConfiguration(Configuration);
               }));
            _client = _server.CreateClient();
        }


        [Theory]
        [InlineData("ikdoemaarwat")]
        [InlineData("ikdoemaarwat@ergens,net")]
        [InlineData("ikdoemaarwatergens.net")]
        public void ValidateEmailReturnFalse(string value)
        {
            bool result = UserController.ValidateEmail(value);

            Assert.False(result);
        }

        [Theory]
        [InlineData("ikdoemaarwat@ergens.net")]
        [InlineData("T0o$hort")]
        [InlineData("longenoughnosigns")]
        [InlineData("Longenoughnosigns")]
        [InlineData("L0ngenoughnosigns")]
        public void ValidatePwReturnFalse(string value)
        {
            bool result = UserController.ValidatePassword(value);

            Assert.False(result);
        }

        [Fact]
        public void ValidateEmailReturnTrue()
        {
            bool result = UserController.ValidateEmail("ikdoemaarwat@ergens.net");

            Assert.True(result);
        }

        [Fact]
        public void ValidatePwReturnTrue()
        {
            bool result = UserController.ValidatePassword("L0ngenoughw!thsigns");

            Assert.True(result);
        }

        [Fact]
        public async Task ValidateCall()
        {
            LoginDTO loginDTO = new LoginDTO("robintest", "V!rkeerd1234");
            var loginDTOstring = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://localhost:5003/api/user/login", loginDTOstring);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.NotEqual("verkeerd", responseString);

            loginDTO = new LoginDTO("robintest", "ditmoetfalen");
            loginDTOstring = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");
            response = await _client.PostAsync("https://localhost:5003/api/user/login", loginDTOstring);
            responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Verkeerd", responseString);


        }

    }
}

