using NinjaCsv.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NinjaCsv.IntegrationTests.CsvCreatorTests
{
    [TestFixture]
    internal class CreateTests
    {
        [Test]
        public void Test()
        {
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "A"},
                new Item { Id = 2, Name = "B"},
                new Item { Id = 3, Description = "C"},
            };

            var creator = new CsvCreator();
            creator.Create(@"c:\temp\result.csv", items);
        }
    }

    public class Item
    {
        [Column(0)]
        public int Id { get; set; }

        [Column(1)]
        public string Name { get; set; }

        [Column(2)]
        public string Description { get; set; }
    }
}
