using System.Linq;
using NinjaCsv.Common;
using NUnit.Framework;

namespace NinjaCsv.IntegrationTests.MiscellaneousTests
{
    [TestFixture]
    public class MultipleRowsWithIssues
    {
        [Test]
        public void Test()
        {
            //SETUP
            var csvParser = new CsvParser();

            //TEST
            var list = csvParser.Parse<UnitTestItem>("CsvFiles/missing-values.csv").ToList();

            //VALIDATE
            Assert.That(list.Count, Is.EqualTo(6));
            ValidateRow(list[0], 1, "jon", 55000, "junior software developer");
            ValidateRow(list[1], 2,"david", 0,"software developer");
            ValidateRow(list[2], 3,"sally",123000,"directory of things");
            ValidateRow(list[3], 0, null, 0, null);
            ValidateRow(list[4], 4, "jim", 96000, "manager of widgets");
            ValidateRow(list[5], 5, "ryan", 28000, null);
        }

        private void ValidateRow(UnitTestItem item, int id, string name, int salary, string position)
        {
            Assert.That(item.Id, Is.EqualTo(id));
            Assert.That(item.Name, Is.EqualTo(name));
            Assert.That(item.Salary, Is.EqualTo(salary));
            Assert.That(item.Position, Is.EqualTo(position));
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