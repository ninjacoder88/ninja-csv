using NUnit.Framework;

namespace NinjaCsv.UnitTests.CsvParserOptionsTests
{
    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void ReturnsInstanceWithDefaultOptions()
        {
            //SETUP

            //TEST
            var sut = new CsvParserOptions();

            //VALIDATE
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Delimiter, Is.EqualTo(","));
            Assert.That(sut.ContainsHeaderRow, Is.EqualTo(true));
            Assert.That(sut.ConsiderNonPublic, Is.False);
        }
    }
}