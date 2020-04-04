using NUnit.Framework;

namespace NinjaCsv.UnitTests.CsvParserTests
{
    [TestFixture]
    public class CtorTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void ReturnsInstance()
        {
            //SETUP

            //TEST
            var sut = new CsvParser();

            //VALIDATE
            Assert.That(sut, Is.Not.Null);
        }
    }
}