using System;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using NinjaCsv.Common;
using NinjaCsv.Internal.UnitTests.Stubs;
using NUnit.Framework;

namespace NinjaCsv.Internal.UnitTests.PropertyInfoToColumnMapperTests
{
    [TestFixture]
    public class MapTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new TypeRelay(typeof(PropertyInfo), typeof(UnitTestPropertyInfo)));
        }

        [Test]
        public void PropertiesIsNull_ThrowsArgumentNullException()
        {
            //SETUP
            var sut = new PropertyInfoToColumnMapper();

            //TEST
            void TestDelegate() => sut.Map(null, false).ToList();

            //VALIDATE
            Assert.Throws<ArgumentNullException>(TestDelegate);
        }

        [Test]
        public void PropertiesIsEmpty_ReturnsEmptyList()
        {
            //SETUP
            var sut = new PropertyInfoToColumnMapper();

            //TEST
            var result = sut.Map(new PropertyInfo[0], false).ToList();

            //VALIDATE
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void NoColumnAttributesExist_ReturnsEmptyList()
        {
            //SETUP
            var propertyInfo = _fixture.Create<PropertyInfo>();
            var sut = new PropertyInfoToColumnMapper();

            //TEST
            var result = sut.Map(new[]{propertyInfo}, false).ToList();

            //VALIDATE
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void HasCustomAttributeNotOfTypeColumn_ReturnsEmptyList()
        {
            //SETUP
            var propertyInfo = _fixture.Create<UnitTestPropertyInfo>();
            propertyInfo.AddCustomAttribute(new UnitTestAttribute());
            var sut = new PropertyInfoToColumnMapper();

            //TEST
            var result = sut.Map(new PropertyInfo[] { propertyInfo }, false).ToList();

            //VALIDATE
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void HasMultipleColumnAttributes_ThrowsInvalidOperationException()
        {
            //SETUP
            var propertyInfo = _fixture.Create<UnitTestPropertyInfo>();
            propertyInfo.AddCustomAttribute(new Column(1));
            propertyInfo.AddCustomAttribute(new Column(2));
            var propertyName = _fixture.Create<string>();
            propertyInfo.CustomizeName(propertyName);
            var sut = new PropertyInfoToColumnMapper();

            //TEST
            void TestDelegate() => sut.Map(new PropertyInfo[] { propertyInfo }, false).ToList();

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"{propertyInfo.Name} has multiple {nameof(Column)} attributes"));
        }

        [Test]
        public void HasCustomColumnAttribute_ReturnsKvpListWithSingleItem()
        {
            //SETUP
            var propertyInfo = _fixture.Create<UnitTestPropertyInfo>();
            propertyInfo.AddCustomAttribute(new Column(1));
            var propertyName = _fixture.Create<string>();
            propertyInfo.CustomizeName(propertyName);
            var sut = new PropertyInfoToColumnMapper();

            //TEST
            var result = sut.Map(new PropertyInfo[] { propertyInfo }, false).ToList();

            //VALIDATE
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Key, Is.EqualTo(1));
            Assert.That(result[0].Value.PropertyName, Is.EqualTo(propertyName));
            //TODO: add additional assertions for Value
        }

        private Fixture _fixture;
    }
}