namespace TeamABootcampAplication.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TeamABootcampAplication.Controllers;

    [TestClass]
    public class DBConnectTests
    {
        private DBConnect _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new DBConnect();
        }

        [TestMethod]
        public void TestDBConnection()
        {
            Assert.AreEqual(_sut.Connection.State.ToString(), "Open");
        }
    }
}