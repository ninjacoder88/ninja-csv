using NUnit.Framework;

namespace NinjaCsv.UnitTests.CsvParserOptionsTests
{
    [TestFixture]
    public class ConsiderNonPublicPropertyTests
    {
        [Test]
        public void ValueIsDefault_ReturnsTrue()
        {
            //SETUP

            //TEST
            var sut = new CsvParserOptions();

            //VALIDATE
            Assert.That(sut.ConsiderNonPublic, Is.False);
        }

        [Test]
        public void ValueIsSetToValue_ReturnsFalse()
        {
            //SETUP

            //TEST
            var sut = new CsvParserOptions();
            sut.ConsiderNonPublic = true;

            //VALIDATE
            Assert.That(sut.ConsiderNonPublic, Is.True);
        }
    }
}