using FluentAssertions;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Core;
using NetArchTest.Rules;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace LIT.Smabu.ArchitectureTests
{
    [TestClass]
    public class DomainTests
    {
        private static readonly Assembly DomainAssembly = typeof(Domain.Common.Address).Assembly;

        [TestMethod]
        public void IdTypes_ShouldHaveDependencyToEntityId()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .HaveNameEndingWith("Id")
                .Should()
                .HaveDependencyOn(typeof(EntityId<>).FullName)
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void Specifications_ShouldEndWithSpec()
        {
            var result = Types.InAssembly(DomainAssembly)
                              .That()
                              .HaveDependencyOn(typeof(Specification<>).FullName)
                              .Should()
                              .HaveNameEndingWith("Spec")
                              .Or()
                              .HaveNameMatching("Spec`")                              
                              .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }


        [TestMethod]
        public void EnumTypes_ShouldHaveJsonConverterAttribute()
        {
            var enumTypes = DomainAssembly.GetTypes()
                .Where(t => t.IsEnum);

            foreach (var enumType in enumTypes)
            {
                var jsonConverterAttribute = enumType.GetCustomAttribute<JsonConverterAttribute>();
                Assert.IsNotNull(jsonConverterAttribute);
                Assert.AreEqual(typeof(StringEnumConverter), jsonConverterAttribute.ConverterType);
            }
        }
    }
}
