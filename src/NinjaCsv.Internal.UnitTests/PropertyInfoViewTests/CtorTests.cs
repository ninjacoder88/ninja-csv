using System;
using AutoFixture;
using NinjaCsv.Internal.UnitTests.Stubs;
using NUnit.Framework;

namespace NinjaCsv.Internal.UnitTests.PropertyInfoViewTests
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
        public void PropertyNameIsNull_ThrowsArgumentNullException()
        {
            //SETUP
            var propertyInfoView = _fixture.Create<UnitTestPropertyInfo>();

            //TEST
            TestDelegate testDelegate = () => new PropertyInfoView(null, propertyInfoView, false);

            //VALIDATE
            var ex = Assert.Throws<ArgumentNullException>(testDelegate);
            Assert.That(ex.Message.Contains("propertyName"));
        }

        [Test]
        public void PropertyNameIsEmpty_ThrowsArgumentNullException()
        {
            //SETUP
            var propertyInfoView = _fixture.Create<UnitTestPropertyInfo>();

            //TEST
            TestDelegate testDelegate = () => new PropertyInfoView(string.Empty, propertyInfoView, false);

            //VALIDATE
            var ex = Assert.Throws<ArgumentNullException>(testDelegate);
            Assert.That(ex.Message.Contains("propertyName"));
        }

        [Test]
        public void PropertyInfoIsNull_ThrowsArgumentNullException()
        {
            //SETUP
            var propertyName = _fixture.Create<string>();

            //TEST
            TestDelegate testDelegate = () => new PropertyInfoView(propertyName, null, false);

            //VALIDATE
            var ex = Assert.Throws<ArgumentNullException>(testDelegate);
            Assert.That(ex.Message.Contains("propertyInfo"));
        }

        [Test]
        public void ValidParameters_PropertiesSetAppropriately()
        {
            //SETUP
            var propertyName = _fixture.Create<string>();
            var setMethod = _fixture.Create<UnitTestMethodInfo>();
            var propertyType = _fixture.Create<UnitTestType>();
            var propertyInfoView = new UnitTestPropertyInfo();
            propertyInfoView.CustomizeSetMethod(setMethod);
            propertyInfoView.CustomizePropertyType(propertyType);

            //TEST
            var result = new PropertyInfoView(propertyName, propertyInfoView, false);

            //VALIDATE
            Assert.That(result.PropertyName, Is.EqualTo(propertyName));
            Assert.That(result.SetMethod, Is.EqualTo(setMethod));
            Assert.That(result.PropertyType, Is.EqualTo(propertyType));
        }

        private Fixture _fixture;
    }
}