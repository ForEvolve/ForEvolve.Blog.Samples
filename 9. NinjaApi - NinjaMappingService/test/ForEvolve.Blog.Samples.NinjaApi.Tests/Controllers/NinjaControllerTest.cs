using ForEvolve.Blog.Samples.NinjaApi.Models;
using ForEvolve.Blog.Samples.NinjaApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Controllers
{
    public class NinjaControllerTest
    {
        protected NinjaController ControllerUnderTest { get; }
        protected Mock<INinjaService> NinjaServiceMock { get; }

        public NinjaControllerTest()
        {
            NinjaServiceMock = new Mock<INinjaService>();
            ControllerUnderTest = new NinjaController(NinjaServiceMock.Object);
        }

        public class ReadAllAsync : NinjaControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_all_Ninja()
            {
                // Arrange
                var expectedNinjas = new Ninja[]
                {
                    new Ninja { Name = "Test Ninja 1" },
                    new Ninja { Name = "Test Ninja 2" },
                    new Ninja { Name = "Test Ninja 3" }
                };
                NinjaServiceMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(expectedNinjas);

                // Act
                var result = await ControllerUnderTest.ReadAllAsync();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedNinjas, okResult.Value);
            }
        }

        public class ReadAllInClanAsync : NinjaControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_all_Ninja_in_Clan()
            {
                // Arrange
                var clanName = "Some clan name";
                var expectedNinjas = new Ninja[]
                {
                    new Ninja { Name = "Test Ninja 1" },
                    new Ninja { Name = "Test Ninja 2" },
                    new Ninja { Name = "Test Ninja 3" }
                };
                NinjaServiceMock
                    .Setup(x => x.ReadAllInClanAsync(clanName))
                    .ReturnsAsync(expectedNinjas);

                // Act
                var result = await ControllerUnderTest.ReadAllInClanAsync(clanName);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedNinjas, okResult.Value);
            }

            [Fact]
            public async void Should_return_NotFoundResult_when_ClanNotFoundException_is_thrown()
            {
                // Arrange
                var unexistingClanName = "Some clan name";
                NinjaServiceMock
                    .Setup(x => x.ReadAllInClanAsync(unexistingClanName))
                    .ThrowsAsync(new ClanNotFoundException(unexistingClanName));

                // Act
                var result = await ControllerUnderTest.ReadAllInClanAsync(unexistingClanName);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        public class ReadOneAsync : NinjaControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_a_Ninja()
            {
                // Arrange
                var clanName = "Some clan name";
                var ninjaKey = "Some ninja key";
                var expectedNinja = new Ninja { Name = "Test Ninja 1" };
                NinjaServiceMock
                    .Setup(x => x.ReadOneAsync(clanName, ninjaKey))
                    .ReturnsAsync(expectedNinja);

                // Act
                var result = await ControllerUnderTest.ReadOneAsync(clanName, ninjaKey);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedNinja, okResult.Value);
            }

            [Fact]
            public async void Should_return_NotFoundResult_when_NinjaNotFoundException_is_thrown()
            {
                // Arrange
                var unexistingClanName = "Some clan name";
                var unexistingNinjaKey = "Some ninja key";
                NinjaServiceMock
                    .Setup(x => x.ReadOneAsync(unexistingClanName, unexistingNinjaKey))
                    .ThrowsAsync(new NinjaNotFoundException(unexistingClanName, unexistingNinjaKey));

                // Act
                var result = await ControllerUnderTest.ReadOneAsync(unexistingClanName, unexistingNinjaKey);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        public class CreateAsync : NinjaControllerTest
        {
            [Fact]
            public async void Should_return_CreatedAtActionResult_with_the_created_Ninja()
            {
                // Arrange
                var expectedNinjaKey = "SomeNinjaKey";
                var expectedClanName = "My Clan";
                var expectedCreatedAtActionName = nameof(NinjaController.ReadOneAsync);
                var expectedNinja = new Ninja { Name = "Test Ninja 1", Clan = new Clan { Name = expectedClanName } };
                NinjaServiceMock
                    .Setup(x => x.CreateAsync(expectedNinja))
                    .ReturnsAsync(() =>
                    {
                        expectedNinja.Key = expectedNinjaKey;
                        return expectedNinja;
                    });

                // Act
                var result = await ControllerUnderTest.CreateAsync(expectedNinja);

                // Assert
                var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Same(expectedNinja, createdResult.Value);
                Assert.Equal(expectedCreatedAtActionName, createdResult.ActionName);
                Assert.Equal(
                    expectedNinjaKey,
                    createdResult.RouteValues.GetValueOrDefault("key")
                );
                Assert.Equal(
                    expectedClanName,
                    createdResult.RouteValues.GetValueOrDefault("clan")
                );
            }

            [Fact]
            public async void Should_return_BadRequestResult()
            {
                // Arrange
                var ninja = new Ninja { Name = "Test Ninja 1" };
                ControllerUnderTest.ModelState.AddModelError("Key", "Some error");

                // Act
                var result = await ControllerUnderTest.CreateAsync(ninja);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequestResult.Value);
            }
        }

        public class UpdateAsync : NinjaControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_the_updated_Ninja()
            {
                // Arrange
                var expectedNinja = new Ninja { Name = "Test Ninja 1" };
                NinjaServiceMock
                    .Setup(x => x.UpdateAsync(expectedNinja))
                    .ReturnsAsync(expectedNinja);

                // Act
                var result = await ControllerUnderTest.UpdateAsync(expectedNinja);

                // Assert
                var createdResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedNinja, createdResult.Value);
            }

            [Fact]
            public async void Should_return_NotFoundResult_when_NinjaNotFoundException_is_thrown()
            {
                // Arrange
                var unexistingNinja = new Ninja { Name = "Test Ninja 1", Clan = new Clan { Name = "Some clan" } };
                NinjaServiceMock
                    .Setup(x => x.UpdateAsync(unexistingNinja))
                    .ThrowsAsync(new NinjaNotFoundException(unexistingNinja));

                // Act
                var result = await ControllerUnderTest.UpdateAsync(unexistingNinja);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async void Should_return_BadRequestResult()
            {
                // Arrange
                var ninja = new Ninja { Name = "Test Ninja 1" };
                ControllerUnderTest.ModelState.AddModelError("Key", "Some error");

                // Act
                var result = await ControllerUnderTest.UpdateAsync(ninja);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequestResult.Value);
            }
        }

        public class DeleteAsync : NinjaControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_the_deleted_Ninja()
            {
                // Arrange
                var clanName = "My clan";
                var ninjaKey = "Some key";
                var expectedNinja = new Ninja { Name = "Test Ninja 1" };
                NinjaServiceMock
                    .Setup(x => x.DeleteAsync(clanName, ninjaKey))
                    .ReturnsAsync(expectedNinja);

                // Act
                var result = await ControllerUnderTest.DeleteAsync(clanName, ninjaKey);

                // Assert
                var createdResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedNinja, createdResult.Value);
            }

            [Fact]
            public async void Should_return_NotFoundResult_when_NinjaNotFoundException_is_thrown()
            {
                // Arrange
                var unexistingClanName = "Some clan name";
                var unexistingNinjaKey = "Some ninja key";
                NinjaServiceMock
                    .Setup(x => x.DeleteAsync(unexistingClanName, unexistingNinjaKey))
                    .ThrowsAsync(new NinjaNotFoundException(unexistingClanName, unexistingNinjaKey));

                // Act
                var result = await ControllerUnderTest.DeleteAsync(unexistingClanName, unexistingNinjaKey);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
