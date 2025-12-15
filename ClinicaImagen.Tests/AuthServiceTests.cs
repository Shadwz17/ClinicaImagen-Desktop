using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClinicaImagen;

namespace ClinicaImagen.Tests
{
    [TestClass]
    public class AuthServiceTests
    {
        [TestMethod]
        public void HashPassword_Roundtrip_Succeeds()
        {
            var password = "StrongPassword!";

            var hashed = AuthService.HashPassword(password);

            Assert.IsTrue(AuthService.VerifyPassword(hashed, password));
            Assert.IsFalse(AuthService.VerifyPassword(hashed, "WrongPassword"));
        }

        [TestMethod]
        public void RegisterUser_InvalidEmail_ReturnsError()
        {
            var factory = new FakeConnectionFactory();
            var service = new AuthService(factory);

            var result = service.RegisterUser("User", "invalid-email", "secret1");

            Assert.IsFalse(result.Success);
            Assert.IsFalse(factory.WasCalled);
        }

        [TestMethod]
        public void AuthenticateUser_EmptyFields_ReturnsError()
        {
            var factory = new FakeConnectionFactory();
            var service = new AuthService(factory);

            var result = service.AuthenticateUser(string.Empty, string.Empty);

            Assert.IsFalse(result.Success);
            Assert.IsFalse(factory.WasCalled);
        }

        private class FakeConnectionFactory : IConnectionFactory
        {
            public bool WasCalled { get; private set; }

            public IDbConnection CreateConnection()
            {
                WasCalled = true;
                throw new InvalidOperationException("Should not create connection for invalid data.");
            }
        }
    }
}
