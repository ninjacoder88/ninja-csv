using System;
using System.Collections.Generic;
using AutoFixture;
using NinjaCsv.Internal.Interfaces;
using NinjaCsv.UnitTests.Utility;
using NSubstitute;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.FileLineProcessorTests
{
    [TestFixture]
    public class ProcessTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _cellDataParser = Substitute.For<ICellDataParser>();
            _sut = new FileLineProcessor(_cellDataParser);
        }

        [TestCase("")]
        [TestCase((string)null)]
        public void FileLineIsNullOrEmpty_ThrowsArgumentException(string fileLine)
        {
            //SETUP

            //TEST
            void TestDelegate() => _sut.Process<UnitTestItem>(fileLine, _fixture.Create<string>(), _fixture.Create<Dictionary<int, string>>());

            //VALIDATE
            var ex = Assert.Throws<ArgumentException>(TestDelegate);
            Assert.That(ex.Message.Contains($"{nameof(fileLine)} must have a non-empty value"));
        }

        [TestCase("")]
        [TestCase((string)null)]
        public void DelimiterIsNullOrEmpty_ThrowsArgumentException(string delimiter)
        {
            //SETUP

            //TEST
            void TestDelegate() => _sut.Process<UnitTestItem>(_fixture.Create<string>(), delimiter, _fixture.Create<Dictionary<int, string>>());

            //VALIDATE
            var ex = Assert.Throws<ArgumentException>(TestDelegate);
            Assert.That(ex.Message.Contains($"{nameof(delimiter)} must have a non-empty value"));
        }

        [Test]
        public void DictionaryIsNull_ThrowsArgumentNullException()
        {
            //SETUP

            //TEST
            void TestDelegate() => _sut.Process<UnitTestItem>(_fixture.Create<string>(), _fixture.Create<string>(), null);

            //VALIDATE
            Assert.Throws<ArgumentNullException>(TestDelegate);
        }

        [Test]
        public void DictionaryIsEmpty_ThrowsArgumentException()
        {
            //SETUP
            var nameForPosition = new Dictionary<int, string>();

            //TEST
            void TestDelegate() => _sut.Process<UnitTestItem>(_fixture.Create<string>(), _fixture.Create<string>(), nameForPosition);

            //VALIDATE
            var ex = Assert.Throws<ArgumentException>(TestDelegate);
            Assert.That(ex.Message.Contains($"{nameof(nameForPosition)} cannot be empty"));
        }

        [Test]
        public void CellIsMappedCorrectly_ItemIsReturnedWithValueMapped()
        {
            //SETUP
            var delimiter = ",";
            var intCell = _fixture.Create<int>();
            var stringCell = _fixture.Create<string>();
            var fileLine = $"{intCell}{delimiter}{stringCell}";
            var nameForPosition = new Dictionary<int, string>{{0, nameof(UnitTestItem.IntId)}};

            _cellDataParser.Parse(typeof(int), $"{intCell}").Returns(intCell);

            //TEST
            var result = _sut.Process<UnitTestItem>(fileLine, delimiter, nameForPosition);

            //VALIDATE
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<UnitTestItem>());
            var uti = result as UnitTestItem;
            Assert.That(uti.IntId, Is.EqualTo(intCell));
            Assert.That(uti.DecimalId, Is.EqualTo(default(decimal)));
            Assert.That(uti.DoubleId, Is.EqualTo(default(double)));
            Assert.That(uti.StringId, Is.EqualTo(default(string)));
        }

        [Test]
        public void CellParsedTypeDoesNotMatchColumnType_ThrowsInvalidOperationException()
        {
            //SETUP
            var delimiter = ",";
            var decimalCell = _fixture.Create<decimal>();
            var stringCell = _fixture.Create<string>();
            var fileLine = $"{decimalCell}{delimiter}{stringCell}";
            var propertyName = nameof(UnitTestItem.IntId);
            var propertyType = typeof(int);
            var nameForPosition = new Dictionary<int, string> { { 0, propertyName } };

            _cellDataParser.Parse(propertyType, $"{decimalCell}").Returns(decimalCell);

            //TEST
            void TestDelegate() => _sut.Process<UnitTestItem>(fileLine, delimiter, nameForPosition);

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"The type of {propertyName} ({propertyType.FullName}) does not match the parsed value of {decimalCell} ({decimalCell.GetType().FullName})"));
        }

        private Fixture _fixture;
        private ICellDataParser _cellDataParser;
        private FileLineProcessor _sut;
    }
}