using LIT.Smabu.UseCases.Base;
using NetArchTest.Rules;
using System.Reflection;

namespace LIT.Smabu.ArchitectureTests
{
    [TestClass]
    public class UseCaseTests
    {
        private static readonly Assembly DomainAssembly = typeof(Domain.Common.Address).Assembly;
        private static readonly Assembly UseCasesAssembly = typeof(UseCases.Common.AddressDTO).Assembly;
        private static readonly Assembly SharedAssembly = typeof(Shared.IReport).Assembly;

        [TestMethod]
        public void Handlers_ShouldHaveDependenciesToDomainAssembly()
        {
            var result = Types.InAssembly(UseCasesAssembly)
                              .That()
                              .HaveNameEndingWith("Handler")
                              .And()
                              .DoNotHaveNameEndingWith("ReportHandler")
                              .Should()
                              .HaveDependencyOn(DomainAssembly.FullName)
                              .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void ReportHandlers_ShouldHaveDependenciesToSharedAssembly()
        {
            var result = Types.InAssembly(UseCasesAssembly)
                              .That()
                              .HaveNameEndingWith("ReportHandler")
                              .Should()
                              .HaveDependencyOn(SharedAssembly.FullName)
                              .And()
                              .HaveDependencyOn(typeof(Shared.IReport).FullName)
                              .And()
                              .HaveDependencyOn(typeof(Shared.IReportFactory).FullName)
                              .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void Handlers_ShouldEndWithHandler()
        {
            var result = Types.InAssembly(UseCasesAssembly)
                              .That()
                              .HaveDependencyOn(typeof(IQueryHandler<,>).FullName)
                              .Or()
                              .HaveDependencyOn(typeof(ICommandHandler<>).FullName)
                              .Should()
                              .HaveNameEndingWith("Handler")
                              .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void Commands_ShouldEndWithCommand()
        {
            var result = Types.InAssembly(UseCasesAssembly)
                              .That()
                              .ImplementInterface(typeof(ICommand))
                              .Or()
                              .ImplementInterface(typeof(ICommand<>))
                              .Should()
                              .HaveNameEndingWith("Command")
                              .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void Queries_ShouldEndWithQuery()
        {
            var result = Types.InAssembly(UseCasesAssembly)
                              .That()
                              .ImplementInterface(typeof(IQuery<>))
                              .Should()
                              .HaveNameEndingWith("Query")
                              .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void DTOs_ShouldEndWithDtoOrResponse()
        {
            var result = Types.InAssembly(UseCasesAssembly)
                              .That()
                              .ImplementInterface(typeof(IDTO))
                              .Should()
                              .HaveNameEndingWith("DTO")
                              .Or()
                              .HaveNameEndingWith("Response")
                              .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
