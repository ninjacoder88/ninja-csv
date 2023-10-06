using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using NinjaCsv.Internal;
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
            _systemFile = Substitute.For<IFile>();
            _propertyInfoToColumnMapper = Substitute.For<IPropertyInfoToColumnMapper>();
            _streamReader = Substitute.For<IStreamReader>();
            _fileLineProcessor = Substitute.For<IFileLineProcessor>();
            _sut = new CsvParser(_propertyInfoToColumnMapper, (s) => _streamReader, _systemFile, _fileLineProcessor);
        }

        [Test]
        public void FilePathIsNull_ThrowsArgumentNullException()
        {
            //SETUP

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>(null).ToList();

            //VALIDATE
            Assert.Throws<ArgumentNullException>(TestDelegate);
        }

        [Test]
        public void FilePathIsEmpty_ThrowsArgumentException()
        {
            //SETUP

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>("").ToList();

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
            void TestDelegate() => _sut.Parse<UnitTestItem>(filePath).ToList();

            //VALIDATE
            var ex = Assert.Throws<ArgumentException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"The file path {filePath} does not exist"));
        }

        [Test]
        public void TestName()
        {
            
        }

        [Test, Ignore("Endless loop")]
        public void FileLinesIsNull_ThrowsInvalidOperationException()
        {
            //SETUP
            string filePath = _fixture.Create<string>();
            _systemFile.Exists(filePath).Returns(true);
            //_propertyInfoToColumnMapper.Map(Arg.Any<PropertyInfo[]>(), false).Returns(new List<KeyValuePair<int, PropertyInfoView>>());
            _streamReader.Peek().Returns(0);
            _fileLineProcessor.Process<UnitTestItem>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Type>(), Arg.Any<IEnumerable<KeyValuePair<int, PropertyInfoView>>>(), Arg.Any<int>()).Returns(new UnitTestItem());

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>(filePath).ToList();

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Reading lines from {filePath} returned no results"));
        }

        [Test, Ignore("Endless loop")]
        public void FileLinesIsEmpty_ReturnsEmptyList()
        {
            //SETUP
            string filePath = _fixture.Create<string>();
            _systemFile.Exists(filePath).Returns(true);

            //TEST
            var result = _sut.Parse<UnitTestItem>(filePath);

            //VALIDATE
            Assert.That(result, Is.Empty);
        }

        [Test, Ignore("Endless loop")]
        public void MapReturnsNull_ThrowsException()
        {
            //SETUP
            string headerFileLine = _fixture.Create<string>();
            string firstFileLine = _fixture.Create<string>();

            string filePath = _fixture.Create<string>();
            _systemFile.Exists(filePath).Returns(true);
            _propertyInfoToColumnMapper.Map(Arg.Any<PropertyInfo[]>(), false).ReturnsNullForAnyArgs();

            //TEST
            void TestDelegate() => _sut.Parse<UnitTestItem>(filePath).ToList();

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Failed to map"));
        }

        //[Test]
        //public void TestName()
        //{
        //    //SETUP
        //    string headerFileLine = _fixture.Create<string>();
        //    string firstFileLine = _fixture.Create<string>();
        //    var dictionary = new Dictionary<int, > { { 0, nameof(UnitTestItem.IntId) } };
        //    var item = new UnitTestItem();

        //    string filePath = _fixture.Create<string>();
        //    _systemFile.Exists(filePath).Returns(true);
        //    _systemFile.ReadAllLines(filePath).Returns(new[] { headerFileLine, firstFileLine });
        //    _propertyInfoToColumnMapper.Map(Arg.Any<PropertyInfo[]>()).Returns(dictionary);

        //    //TEST
        //    var result = _sut.Parse<UnitTestItem>(filePath);

        //    //VALIDATE
        //    var list = result.ToList();
        //    Assert.That(list.Count, Is.EqualTo(1));
        //    Assert.That(list[0], Is.EqualTo(item));
        //}

        private Fixture _fixture;
        private CsvParser _sut;
        private IFile _systemFile;
        private IPropertyInfoToColumnMapper _propertyInfoToColumnMapper;
        private IStreamReader _streamReader;
        private IFileLineProcessor _fileLineProcessor;
    }
}