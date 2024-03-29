﻿using NinjaCsv.Common;
using NUnit.Framework;
using System.Collections.Generic;

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

        [Test]
        public void TestHeaders()
        {
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "A"},
                new Item { Id = 2, Name = "B"},
                new Item { Id = 3, Description = "C"},
            };

            var creator = new CsvCreator();
            creator.Create(@"c:\temp\result2.csv", items, options => options.UseHeaderNames = true);
        }
    }

    public class Item
    {
        [Column(0,"Id")]
        public int Id { get; set; }

        [Column(1, "Item Name")]
        public string Name { get; set; }

        [Column(2)]
        public string Description { get; set; }
    }
}
