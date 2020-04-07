using NUnit.Framework;

namespace NinjaCsv.IntegrationTests.Tests
{
    [TestFixture]
    public class EmptyCsvTests
    {
        [Test]
        public void EmptyCsvFile()
        {
            //SETUP
            var csvParser = new CsvParser();

            //TEST
            var list = csvParser.Parse<UnitTestItem>("EmptyCsvFileTests/empty.csv");

            //VALIDATE
            Assert.That(list, Is.Empty);
        }

        private class UnitTestItem
        {
            [Column(1)]
            public int Id { get; set; }

            public string Name { get; set; }

            public double Value { get; set; }
        }
    }
}