using System;
using twatter_userservice.Controllers;
using Xunit;

namespace TestUserservice
{
    public class UnitTest
    {
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

    }
}

