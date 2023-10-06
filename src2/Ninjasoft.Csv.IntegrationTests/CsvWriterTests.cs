using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Ninjasoft.Csv.IntegrationTests
{
    [TestFixture]
    public class CsvWriterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void WriteTest1()
        {
            var list = new List<UnitTestWriteItem>
            {
                new UnitTestWriteItem { Id = 1, Name = "A", IsDeleted = false },
                new UnitTestWriteItem { Id = 2, Name = "B", IsDeleted = false },
                new UnitTestWriteItem { Id = 3, Name = "C", IsDeleted = true },
            };

            CsvWriter writer = new CsvWriter();
            using(var streamWriter = new StreamWriter(@$"D:\git\ninja-csv\test\{nameof(WriteTest1)}.csv"))
            {
                writer.Create(streamWriter, list);
            } 
        }

        [Test]
        public void WriteTest2()
        {
            var list = new List<UnitTestWriteItem>
            {
                new UnitTestWriteItem { Id = 1, Name = "A", IsDeleted = false },
                new UnitTestWriteItem { Id = 2, Name = "B", IsDeleted = false },
                new UnitTestWriteItem { Id = 3, Name = "C", IsDeleted = true },
            };

            CsvWriter writer = new CsvWriter();
            using (var streamWriter = new StreamWriter(@$"D:\git\ninja-csv\test\{nameof(WriteTest2)}.csv"))
            {
                writer.Create(streamWriter, list, options => options.CreateHeaderRow(t => t.UseHeaderNames = true));
            }
        }
    }

    public class UnitTestWriteItem
    {
        [Column(0)]
        public int Id { get; set; }

        [Column(2)]
        public string Name { get; set; }

        [Column(3)]
        public bool IsDeleted { get; set; }
    }
}