using NUnit.Framework;

namespace NinjaCsv.UnitTests.CsvParserOptionsTests
{
    [TestFixture]
    public class ContainsHeaderRowPropertyTests
    {
        [Test]
        public void ValueIsDefault_ReturnsTrue()
        {
            //SETUP

            //TEST
            var sut = new CsvParserOptions();

            //VALIDATE
            Assert.That(sut.ContainsHeaderRow, Is.True);
        }

        [Test]
        public void ValueIsSetToValue_ReturnsFalse()
        {
            //SETUP

            //TEST
            var sut = new CsvParserOptions();
            sut.ContainsHeaderRow = false;

            //VALIDATE
            Assert.That(sut.ContainsHeaderRow, Is.False);
        }
    }
}