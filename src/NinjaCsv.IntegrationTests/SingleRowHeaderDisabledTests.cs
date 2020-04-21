using System;
using System.Linq;
using NinjaCsv.Common;
using NUnit.Framework;

namespace NinjaCsv.IntegrationTests
{
    public class SingleRowHeaderDisabledTests
    {
        [Test]
        public void HeaderOnlyCsvFile()
        {
            //SETUP
            var csvParser = new CsvParser();

            //TEST
            TestDelegate testDelegate = () => csvParser.Parse<UnitTestItem>("CsvFiles/header-row-only.csv", x => x.ContainsHeaderRow = false).ToList();

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(testDelegate);
            Assert.That(ex.Message.Contains("Could not parse value 'id' to Int32"));
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