using System;
using AutoFixture;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.CellDataParserTests
{
    public class ParseInt32Tests
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
            var result = sut.Parse(typeof(int), null);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(int)));
        }

        [Test]
        public void CellIsEmpty_ReturnsDefaultValue()
        {
            //SETUP
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(int), string.Empty);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(int)));
        }

        [Test]
        public void CellHasValidData_ReturnsValueAsInt()
        {
            //SETUP
            var cellValue = _fixture.Create<int>();
            var cellValueAsString = cellValue.ToString();
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(int), cellValueAsString);

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
            void TestDelegate() => sut.Parse(typeof(int), cellValueAsString);

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Could not parse {cellValueAsString} to {typeof(int).Name}"));
        }

        private Fixture _fixture;
    }
}