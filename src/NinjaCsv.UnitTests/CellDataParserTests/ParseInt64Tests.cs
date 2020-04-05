using System;
using AutoFixture;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.CellDataParserTests
{
    [TestFixture]
    public class ParseInt64Tests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void CellIsNull_ReturnsDefaultValue()
        {
            //SETUP
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(long), null);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(long)));
        }

        [Test]
        public void CellIsEmpty_ReturnsDefaultValue()
        {
            //SETUP
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(long), string.Empty);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(long)));
        }

        [Test]
        public void CellHasValidData_ReturnsValueAsInt()
        {
            //SETUP
            var cellValue = _fixture.Create<long>();
            var cellValueAsString = cellValue.ToString();
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(long), cellValueAsString);

            //VALIDATE
            Assert.That(result, Is.EqualTo(cellValue));
        }

        [Test]
        public void CellHasInvalidData_ThrowsInvalidOperationException()
        {
            //SETUP
            var cellValueAsString = _fixture.Create<string>();
            var sut = new CellDataParser();

            //TEST
            void TestDelegate() => sut.Parse(typeof(long), cellValueAsString);

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Could not parse {cellValueAsString} to {typeof(long).Name}"));
        }

        private Fixture _fixture;
    }
}