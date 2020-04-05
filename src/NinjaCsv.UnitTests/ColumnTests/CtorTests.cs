using System;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NinjaCsv.UnitTests.ColumnTests
{
    [TestFixture]
    public class CtorTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void ValueIsLessThan0_ThrowsArgumentException()
        {
            //SETUP
            //TEST
            void TestDelegate() => new Column(_fixture.Create<int>() * -1);

            //VALIDATE
            Assert.Throws<ArgumentException>(TestDelegate);
        }

        [Test]
        public void ValueIs0_ReturnsInstance()
        {
            //SETUP
            //TEST
            var result = new Column(0);

            //VALIDATE
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ValueIsGreaterThan0_ReturnsInstance()
        {
            //SETUP
            //TEST
            var result = new Column(_fixture.Create<int>());

            //VALIDATE
            Assert.That(result, Is.Not.Null);
        }

        private Fixture _fixture;
    }
}