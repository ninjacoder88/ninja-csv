using System;
using System.Linq;
using NinjaCsv.Common;
using NUnit.Framework;

namespace NinjaCsv.IntegrationTests.SingleRowTests
{
    [TestFixture]
    public class SingleRowHeaderEnabledTests
    {
        [Test]
        public void Test()
        {
            //SETUP
            var csvParser = new CsvParser();

            //TEST
            var list = csvParser.Parse<UnitTestItem>("CsvFiles/header-row-only.csv").ToList();

            //VALIDATE
            Assert.That(list, Is.Empty);
        }

        private class UnitTestItem
        {
            [Column(0)]
            public int Id { get; set; }

            [Column(1)]
            public string Name { get; set; }

            [Column(2)]
            public double Value { get; set; }
        }
    }
}