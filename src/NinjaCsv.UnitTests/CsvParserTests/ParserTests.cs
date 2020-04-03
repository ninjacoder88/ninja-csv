using System;
using AutoFixture;
using NinjaCsv.UnitTests.Utility;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.CsvParserTests
{
    [TestFixture]
    public class ParserTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new CsvParser();
        }

        [Test]
        public void FilePathIsNull_ThrowsArgumentNullException()
        {
            //SETUP

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>(null);

            //VALIDATE
            Assert.Throws<ArgumentNullException>(TestDelegate);
        }

        [Test]
        public void FilePathIsEmpty_ThrowsArgumentException()
        {
            //SETUP

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>("");

            //VALIDATE
            var ex = Assert.Throws<ArgumentException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo("filePath cannot be empty"));
        }

        [Test]
        public void FileDoesNotExist_ThrowsArgumentException()
        {
            //SETUP
            string filePath = _fixture.Create<string>();

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>(filePath);

            //VALIDATE
            var ex = Assert.Throws<ArgumentException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"The file path {filePath} does not exist"));
        }

        private Fixture _fixture;
        private CsvParser _sut;
    }
}