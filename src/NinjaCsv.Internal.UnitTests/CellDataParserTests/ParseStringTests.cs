using AutoFixture;
using NUnit.Framework;

namespace NinjaCsv.Internal.UnitTests.CellDataParserTests
{
    [TestFixture]
    public class ParseStringTests
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
            var result = sut.Parse(typeof(string), null, 1);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(string)));
        }

        [Test]
        public void CellIsEmpty_ReturnsDefaultValue()
        {
            //SETUP
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(string), string.Empty, 1);

            //VALIDATE
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CellHasValidData_ReturnsValueAsString()
        {
            //SETUP
            var cellValue = _fixture.Create<string>();
            var cellValueAsString = cellValue.ToString();
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(string), cellValueAsString, 1);

            //VALIDATE
            Assert.That(result, Is.EqualTo(cellValue));
        }

        //[Test]
        //public void CellHasInvalidData_ReturnsValueAsInt()
        //{
        //    //SETUP
        //    var cellValueAsString = _fixture.Create<string>();
        //    var sut = new CellDataParser();

        //    //TEST
        //    void TestDelegate() => sut.Parse(typeof(int), cellValueAsString);

        //    //VALIDATE
        //    var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
        //    Assert.That(ex.Message, Is.EqualTo($"Could not parse {cellValueAsString} to {typeof(int).Name}"));
        //}

        private Fixture _fixture;
    }
}