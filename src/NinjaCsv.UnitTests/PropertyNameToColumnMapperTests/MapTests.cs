using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using NinjaCsv.UnitTests.Utility;
using NSubstitute;
using NUnit.Framework;

namespace NinjaCsv.UnitTests.PropertyNameToColumnMapperTests
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
            var sut = new PropertyNameToColumnMapper();

            //TEST
            void TestDelegate() => sut.Map(null).ToList();

            //VALIDATE
            Assert.Throws<ArgumentNullException>(TestDelegate);
        }

        [Test]
        public void NoColumnAttributesExist_ReturnsEmptyList()
        {
            //SETUP
            var propertyInfo = _fixture.Create<PropertyInfo>();
            var sut = new PropertyNameToColumnMapper();

            //TEST
            var result = sut.Map(new[]{propertyInfo}).ToList();

            //VALIDATE
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void HasCustomAttributeNotOfTypeColumn_ReturnsEmptyList()
        {
            //SETUP
            var propertyInfo = _fixture.Create<UnitTestPropertyInfo>();
            propertyInfo.AddCustomAttribute(new UnitTestAttribute());
            var sut = new PropertyNameToColumnMapper();

            //TEST
            var result = sut.Map(new PropertyInfo[] { propertyInfo }).ToList();

            //VALIDATE
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void HasCustomColumnAttribute_ReturnsKvpListWithSingleItem()
        {
            //SETUP
            var propertyInfo = _fixture.Create<UnitTestPropertyInfo>();
            propertyInfo.AddCustomAttribute(new Column(1));
            var propertyName = _fixture.Create<string>();
            propertyInfo.SetName(propertyName);
            var sut = new PropertyNameToColumnMapper();

            //TEST
            var result = sut.Map(new PropertyInfo[] { propertyInfo }).ToList();

            //VALIDATE
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Key, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(propertyName));
        }

        [Test]
        public void HasMultipleColumnAttributes_ThrowsInvalidOperationException()
        {
            //SETUP
            var propertyInfo = _fixture.Create<UnitTestPropertyInfo>();
            propertyInfo.AddCustomAttribute(new Column(1));
            propertyInfo.AddCustomAttribute(new Column(2));
            var propertyName = _fixture.Create<string>();
            propertyInfo.SetName(propertyName);
            var sut = new PropertyNameToColumnMapper();

            //TEST
            //var result = sut.Map(new PropertyInfo[] {propertyInfo}).ToList();

            //Assert.That(result, Is.Null);

            void TestDelegate() => sut.Map(new PropertyInfo[] { propertyInfo }).ToList();

            //VALIDATE
            var ex = Assert.Throws<InvalidOperationException>(TestDelegate);
            Assert.That(ex.Message, Is.EqualTo($"{propertyInfo.Name} has multiple {nameof(Column)} attributes"));
        }

        private Fixture _fixture;
    }
}