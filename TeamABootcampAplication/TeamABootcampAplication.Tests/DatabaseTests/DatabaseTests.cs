namespace TeamABootcampAplication.Tests.DatabaseTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TeamABootcampAplication.Models;

    [TestClass]
    public class DatabaseTests
    {
        private Database _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new Database();
        }

        [TestMethod]
        public void TestCheckIfUserExistsWhenUserExists()
        {
            User testUser = new User("RegisterTest", "RegisterTest@RegisterTest.com", "", "Qwert1");

            bool result = _sut.CheckIfUserExists(testUser);

            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void TestCheckIfUserExistsWhenUserDoesNotExist()
        {
            User testUser = new User("NotRealUser", "NotRealUser@Test.com", "", "Qwert1");

            bool result = _sut.CheckIfUserExists(testUser);

            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void TestCheckIfUserExistsWhenInputNull()
        {
            User testUser = new User(null, null, null, null);
            bool result = _sut.CheckIfUserExists(testUser);

            Assert.AreEqual(result, false);

            User testUser2 = new User(null, null, "", "Qwert1");
            result = _sut.CheckIfUserExists(testUser2);

            Assert.AreEqual(result, false);

            User testUser3 = new User(null, "RegisterTest@RegisterTest.com", "", "Qwert1");
            result = _sut.CheckIfUserExists(testUser3);

           Assert.AreEqual(result, false);

        }

        [TestMethod]
        public void TestCreateUserSuccessful()
        {
            string random = RegisterControllerTests.RandomString(6);

            User testUser = new User(random, random + "@" + random + ".com", "avatar", "Password123");

            bool result = _sut.CreateUser(testUser);

            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void TestCreateUserFailedWithNullValues()
        {
            User testUser = new User(null, null, null, null);

            bool result = _sut.CreateUser(testUser);

            Assert.AreEqual(result, false);
        }
    }
}