using System.Linq;
using NUnit.Framework;

namespace NinjaCsv.IntegrationTests.SingleRowCsvFileTests
{
    [TestFixture]
    public class SingleRowAllColumnsMappedCsvTests
    {
        [Test]
        public void TestName()
        {
            //SETUP
            var csvParser = new CsvParser();

            //TEST
            var list = csvParser.Parse<UnitTestItem>("SingleRowCsvFileTests/single-value-row.csv").ToList();

            //VALIDATE
            Assert.That(list.Count, Is.EqualTo(1));
            var firstRow = list[0];
            Assert.That(firstRow.Id, Is.EqualTo(1));
            Assert.That(firstRow.Name, Is.EqualTo("jon"));
            Assert.That(firstRow.Salary, Is.EqualTo(55000));
            Assert.That(firstRow.Position, Is.EqualTo("junior developer"));
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
        }
    }
}