using System;
using AutoFixture;
using NinjaCsv.UnitTests.Utility;
using NUnit.Framework;

namespace NinjaCsv.Internal.UnitTests.CellDataParserTests
{
    [TestFixture]
    public class ParseTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void InstancePropertyTypeIsNull_ThrowsArgumentNullException()
        {
            //SETUP
            var sut = new CellDataParser();

            //TEST
            void TestDelegate() => sut.Parse(null, string.Empty, 1);

            //VALIDATE
            Assert.Throws<ArgumentNullException>(TestDelegate);
        }

        [Test]
        public void TypeIsNotSupport_ThrowsInvalidOperationException()
        {
            //SETUP
            var cellData = _fixture.Create<string>();
            var sut = new CellDataParser();

            //TEST
            void TestDelegate() => sut.Parse(typeof(UnitTestItem), cellData, 1);

            //VALIDATE
            Assert.Throws<InvalidOperationException>(TestDelegate);
        }

        private Fixture _fixture;
    }
}