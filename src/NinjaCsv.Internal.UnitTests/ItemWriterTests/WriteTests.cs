using NinjaCsv.UnitTests.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NinjaCsv.Internal.UnitTests.ItemWriterTests
{
    [TestFixture]
    internal class WriteTests
    {
        [Test]
        public void Test()
        {
            //SETUP
            Dictionary<int, PropertyInfoView> columnMap = new Dictionary<int, PropertyInfoView>();
            columnMap.Add(0, new PropertyInfoView("", null, false, 1));
            var item = new UnitTestItem();
            var sut = new ItemWriter();

            //TEST
            using(var ms = new MemoryStream())
            {
                using(var sw = new StreamWriter(ms))
                {
                    sut.Write(columnMap, -1, -1, sw, ",", item);
                }
            }
            

            //VALIDATE
        }
    }
}
