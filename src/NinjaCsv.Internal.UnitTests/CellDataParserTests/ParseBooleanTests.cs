﻿using System;
using AutoFixture;
using NUnit.Framework;

namespace NinjaCsv.Internal.UnitTests.CellDataParserTests
{
    [TestFixture]
    public class ParseBooleanTests
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
            var result = sut.Parse(typeof(bool), null);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(bool)));
        }

        [Test]
        public void CellIsEmpty_ReturnsDefaultValue()
        {
            //SETUP
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(bool), string.Empty);

            //VALIDATE
            Assert.That(result, Is.EqualTo(default(bool)));
        }

        [Test]
        public void CellHasValidData_ReturnsValueAsInt()
        {
            //SETUP
            var cellValue = _fixture.Create<bool>();
            var cellValueAsString = cellValue.ToString();
            var sut = new CellDataParser();

            //TEST
            var result = sut.Parse(typeof(bool), cellValueAsString);

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
            void TestDelegate() => sut.Parse(typeof(bool), cellValueAsString);

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"Could not parse value '{cellValueAsString}' to {typeof(bool).Name}"));
        }

        private Fixture _fixture;
    }
}