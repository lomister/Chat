namespace TeamABootcampAplication.Tests
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TeamABootcampAplication.Controllers;

    [TestClass]
    [TestCategory("Unit")]
    public class LoginControllerTests
    {
        private LoginController _sut; // SUT - service/system under test

        [TestInitialize]
        public void Initialize()
        {
            _sut = new LoginController();
        }

        [TestMethod]
        public void TestLoginFailedWithBlankUsernameAndPass()
        {
            ViewResult result = _sut.Login("", "", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestLoginFailedWithValidUsernameInvalidPass()
        {
            ViewResult result = _sut.Login("testuser", "dd", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestLoginFailedWithInvalidUsernameValidPass()
        {
            ViewResult result = _sut.Login("user", "password", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestLoginFailed()
        {
            ViewResult result = _sut.Login("ssdfdsf", "sdf", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestLoginFailedWithSQLInjection()
        {
            ViewResult result = _sut.Login("SELECT * FROM Users ", "SELECT * FROM Users", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestLoginFailedWithSQLInjectionSecond()
        {
            ViewResult result = _sut.Login("1=1;–", "password", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestLoginFailedWithSQLInjectionThird()
        {
            ViewResult result = _sut.Login("'", "password", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestLoginFailedWithSQLInjectionFourth()
        {
            ViewResult result = _sut.Login("testuser", "drop table Users;", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestLoginFailedWithLotOfCharacters()
        {
            ViewResult result = _sut.Login("testtesttesttesttesttesttesttesttesttesttest" +
                "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestt" +
                "esttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest" +
                "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttes" +
                "ttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttes" +
                "ttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttes" +
                "ttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttes" +
                "ttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestt" +
                "esttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestt" +
                "esttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttes" +
                "ttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestt" +
                "esttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestt" +
                "esttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttest" +
                "testtesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttes" +
                "ttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttest" +
                "testtesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttestte" +
                "sttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttestte" +
                "sttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttestte" +
                "sttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttestt" +
                "esttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttes" +
                "ttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttestt" +
                "esttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttest" +
                "testesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttestte" +
                "sttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttestte" +
                "sttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttest" +
                "testtesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttestte" +
                "sttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttestt" +
                "esttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttest" +
                "testtesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttest" +
                "testtesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttest" +
                "testtesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttest" +
                "testtesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttest" +
                "testtesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttest" +
                "testtesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttest" +
                "testtesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttest" +
                "testtesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttes" +
                "ttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestes" +
                "ttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttestt" +
                "estesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttest" +
                "testtestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttest" +
                "testtesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttes" +
                "ttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttestte" +
                "sttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttestte" +
                "sttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttestt" +
                "esttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttesttest" +
                "testtesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestesttestt" +
                "esttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestestte" +
                "sttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttestes" +
                "ttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttes" +
                "testtesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttestt" +
                "estesttesttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttes" +
                "ttestesttesttesttesttesttesttesttesttesttesttesttesttestestt" +
                "esttesttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttest", " test", "null") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Username or password invalid"));
            Assert.AreEqual(result.ViewName, "Index");
        }
    }
}