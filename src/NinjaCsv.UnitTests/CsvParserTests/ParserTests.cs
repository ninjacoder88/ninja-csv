using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using NinjaCsv.Internal.Interfaces;
using NinjaCsv.UnitTests.Utility;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
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
            _systemFile = Substitute.For<ISystemFile>();
            _fileLineProcessor = Substitute.For<IFileLineProcessor>();
            _nameToCsvMapper = Substitute.For<INameToCsvMapper>();
            _sut = new CsvParser {SystemFile = _systemFile, FileLineProcessor = _fileLineProcessor, NameToCsvMapper = _nameToCsvMapper};
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
            _systemFile.Exists(filePath).Returns(false);

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>(filePath);

            //VALIDATE
            var ex = Assert.Throws<ArgumentException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"The file path {filePath} does not exist"));
        }

        [Test]
        public void FileLinesIsNull_ThrowsInvalidOperationException()
        {
            //SETUP
            string filePath = _fixture.Create<string>();
            _systemFile.Exists(filePath).Returns(true);
            _systemFile.ReadAllLines(filePath).ReturnsNullForAnyArgs();

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>(filePath);

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Reading lines from {filePath} returned no results"));
        }

        [Test]
        public void FileLinesIsEmpty_ReturnsEmptyList()
        {
            //SETUP
            string filePath = _fixture.Create<string>();
            _systemFile.Exists(filePath).Returns(true);
            _systemFile.ReadAllLines(filePath).Returns(new string[0]);

            //TEST
            var result = _sut.Parse<UnitTestItem>(filePath);

            //VALIDATE
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void MapReturnsNull_ThrowsException()
        {
            //SETUP
            string headerFileLine = _fixture.Create<string>();
            string firstFileLine = _fixture.Create<string>();

            string filePath = _fixture.Create<string>();
            _systemFile.Exists(filePath).Returns(true);
            _systemFile.ReadAllLines(filePath).Returns(new[] { headerFileLine, firstFileLine });
            _nameToCsvMapper.Map(Arg.Any<PropertyInfo[]>()).ReturnsNullForAnyArgs();

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>(filePath);

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Failed to map"));
        }

        [Test]
        public void TestName()
        {
            //SETUP
            string headerFileLine = _fixture.Create<string>();
            string firstFileLine = _fixture.Create<string>();
            var dictionary = new Dictionary<int, string> {{0, nameof(UnitTestItem.IntId)}};
            var item = new UnitTestItem();

            string filePath = _fixture.Create<string>();
            _systemFile.Exists(filePath).Returns(true);
            _systemFile.ReadAllLines(filePath).Returns(new []{headerFileLine, firstFileLine});
            _nameToCsvMapper.Map(Arg.Any<PropertyInfo[]>()).Returns(dictionary);
            _fileLineProcessor.Process<UnitTestItem>(firstFileLine, ",", Arg.Any<Dictionary<int, string>>()).Returns(item);

            //TEST
            var result = _sut.Parse<UnitTestItem>(filePath);

            //VALIDATE
            var list = result.ToList();
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0], Is.EqualTo(item));
        }

        private Fixture _fixture;
        private CsvParser _sut;
        private ISystemFile _systemFile;
        private INameToCsvMapper _nameToCsvMapper;
        private IFileLineProcessor _fileLineProcessor;
    }
}