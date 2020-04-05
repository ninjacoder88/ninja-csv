using System;
using AutoFixture;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.CellDataParserTests
{
    [TestFixture]
    public class ParseDecimalTests
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
            var result = sut.Parse(typeof(decimal), null);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(decimal)));
        }

        [Test]
        public void CellIsEmpty_ReturnsDefaultValue()
        {
            //SETUP
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(decimal), string.Empty);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(decimal)));
        }

        [Test]
        public void CellHasValidData_ReturnsValueAsInt()
        {
            //SETUP
            var cellValue = _fixture.Create<decimal>();
            var cellValueAsString = cellValue.ToString();
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(decimal), cellValueAsString);

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
            void TestDelegate() => sut.Parse(typeof(decimal), cellValueAsString);

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Could not parse {cellValueAsString} to {typeof(decimal).Name}"));
        }

        private Fixture _fixture;
    }
}