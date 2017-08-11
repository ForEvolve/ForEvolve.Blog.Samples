using ForEvolve.Blog.Samples.NinjaApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Controllers
{
    public class ClansControllerTest
    {
        protected ClansController ControllerUnderTest { get; }

        public ClansControllerTest()
        {
            ControllerUnderTest = new ClansController();
        }

        public class ReadAllAsync : ClansControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_clans()
            {
                // Arrange
                var expectedClans = new Clan[]
                {
                    new Clan { Name = "Test clan 1" },
                    new Clan { Name = "Test clan 2" },
                    new Clan { Name = "Test clan 3" }
                };

                // Act
                var result = await ControllerUnderTest.ReadAllAsync();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedClans, okResult.Value);
            }
        }
    }
}
