using System;
using AutoFixture;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.CsvParserTests
{
    [TestFixture]
    public class CtorTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void FilePathIsNull_ThrowsArgumentNullException()
        {
            //SETUP
            
            //TEST
            void TestDelegate() => new CsvParser(null);

            //VALIDATE
            Assert.Throws<ArgumentNullException>(TestDelegate);
        }

        [Test]
        public void FilePathIsEmpty_ThrowsArgumentException()
        {
            //SETUP

            //TEST
            void TestDelegate() => new CsvParser("");

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
            void TestDelegate() => new CsvParser(filePath);

            //VALIDATE
            var ex = Assert.Throws<ArgumentException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"The file path {filePath} does not exist"));
        }

        [Test]
        public void FileExistAsPath_InstanceCreated()
        {
            //SETUP
            string filePath = "TestCsvFiles/empty.csv";

            //TEST
            var SUT = new CsvParser(filePath);

            //VALIDATE
            Assert.That(SUT, Is.Not.Null);
        }

        private Fixture _fixture;
    }
}