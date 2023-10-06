using Ninjasoft.Csv.Internal;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ninjasoft.Csv.IntegrationTests
{
    [TestFixture]
    public class CsvReaderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadTest1()
        {
            CsvReader reader = new CsvReader();

            List<UnitTestReadItem> items;
            using(var streamReader = new StreamReader(@$"D:\git\ninja-csv\test\{nameof(ReadTest1)}.csv"))
            {
                items = reader.Read<UnitTestReadItem>(streamReader, options => options.ContainsHeaderRow = false).ToList();
            }

            Assert.That(items.Count, Is.EqualTo(2), "Item count is 2");
            Assert.That(items[0].Id, Is.EqualTo(1));
            Assert.That(items[0].Name, Is.EqualTo("John"));
            Assert.That(items[0].IsDeleted, Is.EqualTo(false));
            Assert.That(items[1].Id, Is.EqualTo(2));
            Assert.That(items[1].Name, Is.EqualTo("Bill"));
            Assert.That(items[1].IsDeleted, Is.EqualTo(true));
        }
    }

    public class UnitTestReadItem
    {
        [Column(0)]
        public int Id { get; set; }

        [Column(1)]
        public string Name { get; set; }

        [Column(3)]
        public bool IsDeleted { get; set; }
    }
}
