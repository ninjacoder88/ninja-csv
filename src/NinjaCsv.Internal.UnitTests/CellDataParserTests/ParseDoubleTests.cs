using System;
using AutoFixture;
using NUnit.Framework;

namespace NinjaCsv.Internal.UnitTests.CellDataParserTests
{
    [TestFixture]
    public class ParseDoubleTests
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
            var result = sut.Parse(typeof(double), null, 1);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(double)));
        }

        [Test]
        public void CellIsEmpty_ReturnsDefaultValue()
        {
            //SETUP
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(double), string.Empty, 1);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(double)));
        }

        [Test]
        public void CellHasValidData_ReturnsValueAsInt()
        {
            //SETUP
            var cellValue = _fixture.Create<double>();
            var cellValueAsString = cellValue.ToString();
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(double), cellValueAsString, 1);

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
            void TestDelegate() => sut.Parse(typeof(double), cellValueAsString, 1);

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Could not parse value '{cellValueAsString}' to {typeof(double).Name} on line 1"));
        }

        private Fixture _fixture;
    }
}