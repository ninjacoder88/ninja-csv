using NUnit.Framework;

namespace NinjaCsv.IntegrationTests.HeaderRowOnlyCsvFileTests
{
    [TestFixture]
    public class HeaderRowOnlyTests
    {
        [Test]
        public void HeaderOnlyCsvFile()
        {
            //SETUP
            var csvParser = new CsvParser();

            //TEST
            var list = csvParser.Parse<UnitTestItem>("HeaderRowOnlyCsvFileTests/header-row-only.csv");

            //VALIDATE
            Assert.That(list, Is.Empty);
        }

        private class UnitTestItem
        {
            [Column(1)]
            public int Id { get; set; }

            [Column(2)]
            public string Name { get; set; }

            [Column(3)]
            public double Value { get; set; }
        }
    }
}