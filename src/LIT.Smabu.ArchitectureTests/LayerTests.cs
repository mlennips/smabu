using NetArchTest.Rules;
using System.Reflection;

namespace LIT.Smabu.ArchitectureTests
{
    [TestClass]
    public class LayerTests
    {
        private static readonly Assembly DomainAssembly = typeof(Domain.Common.Address).Assembly;
        private static readonly Assembly UseCasesAssembly = typeof(UseCases.Common.AddressDTO).Assembly;
        private static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.Identity.Services.CurrentUserService).Assembly;
        private static readonly Assembly ApiAssembly = typeof(API.Endpoints.Common).Assembly;

        [TestMethod]
        public void DomainLayer_ShouldNotHaveDependencyOn()
        {
            // Arrange  
            var notIn = new[] { UseCasesAssembly, InfrastructureAssembly, ApiAssembly };

            // Act  
            var result = Types.InAssembly(DomainAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(notIn.Select(x => x.FullName).ToArray())
                .GetResult();

            // Assert  
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void ApplicationLayer_ShouldNotHaveDependencyOn()
        {
            // Arrange  
            var notIn = new[] { InfrastructureAssembly, ApiAssembly };

            // Act  
            var result = Types.InAssembly(InfrastructureAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(notIn.Select(x => x.FullName).ToArray())
                .GetResult();

            // Assert  
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void InfrastructureLayer_ShouldNotHaveDependencyOn()
        {
            // Arrange  
            var notIn = new[] { ApiAssembly };

            // Act  
            var result = Types.InAssembly(InfrastructureAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(notIn.Select(x => x.FullName).ToArray())
                .GetResult();

            // Assert  
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}