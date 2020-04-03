using System;
using AutoFixture;
using NinjaCsv.UnitTests.Utility;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.CsvParserTests
{
    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void ReturnsInstance()
        {
            //SETUP

            //TEST
            var sut = new CsvParser();

            //VALIDATE
            Assert.That(sut, Is.Not.Null);
        }
    }
}