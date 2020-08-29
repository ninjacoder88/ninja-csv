using System;
using System.Linq;
using NinjaCsv.Common;
using NUnit.Framework;

namespace NinjaCsv.IntegrationTests.SingleRowTests
{
    [TestFixture]
    public class SingleRowWithDateAllColumnsMappedCsvTests
    {
        [Test]
        public void Test()
        {
            //SETUP
            var csvParser = new CsvParser();

            //TEST
            var list = csvParser.Parse<UnitTestItem>("CsvFiles/single-value-row-with-date.csv").ToList();

            //VALIDATE
            Assert.That(list.Count, Is.EqualTo(1));
            var firstRow = list[0];
            Assert.That(firstRow.Id, Is.EqualTo(1));
            Assert.That(firstRow.Name, Is.EqualTo("jon"));
            Assert.That(firstRow.Salary, Is.EqualTo(55000));
            Assert.That(firstRow.Position, Is.EqualTo("junior developer"));
            Assert.That(firstRow.HireDate.Year, Is.EqualTo(2020));
            Assert.That(firstRow.HireDate.Month, Is.EqualTo(8));
            Assert.That(firstRow.HireDate.Day, Is.EqualTo(1));
        }

        private class UnitTestItem
        {
            [Column(0)]
            public int Id { get; set; }

            [Column(1)]
            public string Name { get; set; }

            [Column(2)]
            public int Salary { get; set; }

            [Column(3)]
            public string Position { get; set; }

            [Column(4)]
            public DateTime HireDate {get;set;}
        }
    }
}