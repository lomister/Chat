namespace TeamABootcampAplication.Tests
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TeamABootcampAplication.Controllers;

    [TestClass]
    public class ResetPasswordControllerTests
    {
        private ResetPasswordController _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new ResetPasswordController();
        }

        [TestMethod]
        public void TestValidateResetPasswordWithNullValues()
        {
            var result = _sut.ResetPassword(null, null) as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Can not generate new password. Try Again!"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestValidateResetPasswordSuccessful()
        {
            var result = _sut.ResetPassword("RegisterTest", "RegisterTest@RegisterTest.com") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("Check your email!"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestValidateResetPasswordFailedWithWrongValues()
        {
            var result = _sut.ResetPassword("dsf", "sdv") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Something went wrong. Try Again!"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestValidateResetPasswordWithOneNullValue()
        {
            var result = _sut.ResetPassword(null, "RegisterTest@RegisterTest.com") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Can not generate new password. Try Again!"));
            Assert.AreEqual(result.ViewName, "Index");

            result = _sut.ResetPassword("vcsfsdv", null) as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("*Can not generate new password. Try Again!"));
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestResetPasswordFromUserPanelFailedWithWrongValues()
        {
            var result = _sut.ResetPasswordFromUserPanel("Qwert1", "Qwert1") as ViewResult;

            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void TestResetPasswordFromUserPanelInvalidOldPassword()
        {
            var result = _sut.ResetPasswordFromUserPanel("sdfgh", "asdfgh") as ViewResult;

            Assert.IsTrue(result.ViewData.Values.Contains("Invalid old password"));
        }
    }
}