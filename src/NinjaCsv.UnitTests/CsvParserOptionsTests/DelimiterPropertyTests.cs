using AutoFixture;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.CsvParserOptionsTests
{
    [TestFixture]
    public class DelimiterPropertyTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void DelimiterIsNull_SetsToDefaultValue_ReturnsInstance()
        {
            //SETUP
            var sut = new CsvParserOptions();
            sut.Delimiter = null;

            //TEST
            var result = sut.Delimiter;

            //VALIDATE
            Assert.That(result, Is.EqualTo(","));
        }

        [Test]
        public void DelimiterIsEmpty_ThrowsInvalidOperationException()
        {
            //SETUP
            var sut = new CsvParserOptions();
            sut.Delimiter = "";

            //TEST
            var result = sut.Delimiter;

            //VALIDATE
            Assert.That(result, Is.EqualTo(","));
        }

        [Test]
        public void DelimiterDefaulted_ReturnsInstance()
        {
            //SETUP
            var sut = new CsvParserOptions();

            //TEST
            var result = sut.Delimiter;

            //VALIDATE
            Assert.That(result, Is.EqualTo(","));
        }

        [Test]
        public void DelimiterSet_ReturnsInstance()
        {
            //SETUP
            var delimiter = _fixture.Create<string>();
            var sut = new CsvParserOptions();
            sut.Delimiter = delimiter;

            //TEST
            var result = sut.Delimiter;

            //VALIDATE
            Assert.That(result, Is.EqualTo(delimiter));
        }

        private Fixture _fixture;
    }
}