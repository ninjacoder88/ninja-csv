using System.Linq;
using NUnit.Framework;

namespace NinjaCsv.IntegrationTests.NoColumnsMappedCsvTests
{
    [TestFixture]
    public class NoColumnsMappedTests
    {
        [Test]
        public void TestName()
        {
            //SETUP
            var csvParser = new CsvParser();

            //TEST
            var list = csvParser.Parse<UnitTestItem>("CsvFiles/single-value-row.csv").ToList();

            //VALIDATE
            Assert.That(list.Count, Is.EqualTo(1));
            var firstRow = list[0];
            Assert.That(firstRow.Id, Is.EqualTo(0));
            Assert.That(firstRow.Name, Is.EqualTo(null));
            Assert.That(firstRow.Salary, Is.EqualTo(0));
            Assert.That(firstRow.Position, Is.EqualTo(null));
        }

        private class UnitTestItem
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int Salary { get; set; }

            public string Position { get; set; }
        }
    }
}