namespace TeamABootcampAplication.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TeamABootcampAplication.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using TeamABootcampAplication.Models;
    using System;
    using System.Linq;

    [TestClass]
    public class RegisterControllerTests
    {
        private RegisterController _sut;

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [TestInitialize]
        public void Initialize()
        {
            _sut = new RegisterController();
        }

        [TestMethod]
        public void TestRegisterSuccesful()
        {
            string random = RandomString(5);

            User testUser = new User(random, random + "@" + random + ".com", "", "Password123" );
            ViewResult result = _sut.Register(testUser.Username, testUser.Password, testUser.Email, testUser.Avatar) as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Registration successful"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
         public void TestRegisterFailed()
        {
            User testUser = new User("testuser", "email@google.com", "avatar", "password");
            var result = _sut.Register(testUser.Username, testUser.Password, testUser.Email, testUser.Avatar) as ViewResult;

            Assert.AreEqual(result.ViewName, "Index");
            Assert.IsTrue(result.ViewData.Values.Contains("Cannot register"));
        }

        [TestMethod]
        public void TestRegisterFailedBecauseGivenNullValue()
        {
            User testUser = new User(null, null, null, null);
            var result = _sut.Register(testUser.Username, testUser.Password, testUser.Email, testUser.Avatar) as ViewResult;

            Assert.AreEqual(result.ViewName, "Index");
        }
    }
}