using ForEvolve.Blog.Samples.NinjaApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Moq;
using ForEvolve.Blog.Samples.NinjaApi.Services;

namespace ForEvolve.Blog.Samples.NinjaApi.Controllers
{
    public class ClansControllerTest
    {
        protected ClansController ControllerUnderTest { get; }
        protected Mock<IClanService> ClanServiceMock { get; }

        public ClansControllerTest()
        {
            ClanServiceMock = new Mock<IClanService>();
            ControllerUnderTest = new ClansController(ClanServiceMock.Object);
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
                ClanServiceMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(expectedClans);

                // Act
                var result = await ControllerUnderTest.ReadAllAsync();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedClans, okResult.Value);
            }
        }
    }
}
