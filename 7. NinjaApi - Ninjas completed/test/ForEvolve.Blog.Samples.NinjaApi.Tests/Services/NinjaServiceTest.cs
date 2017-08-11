using ForEvolve.Blog.Samples.NinjaApi.Models;
using ForEvolve.Blog.Samples.NinjaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Services
{
    public class NinjaServiceTest
    {
        protected NinjaService ServiceUnderTest { get; }
        protected Mock<INinjaRepository> NinjaRepositoryMock { get; }
        protected Mock<IClanService> ClanServiceMock { get; }

        public NinjaServiceTest()
        {
            NinjaRepositoryMock = new Mock<INinjaRepository>();
            ClanServiceMock = new Mock<IClanService>();
            ServiceUnderTest = new NinjaService(NinjaRepositoryMock.Object, ClanServiceMock.Object);
        }

        public class ReadAllAsync : NinjaServiceTest
        {
            [Fact]
            public async void Should_return_all_Ninja()
            {
                // Arrange
                var expectedNinjas = new Ninja[]
                {
                    new Ninja { Name = "Test Ninja 1" },
                    new Ninja { Name = "Test Ninja 2" },
                    new Ninja { Name = "Test Ninja 3" }
                };
                NinjaRepositoryMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(expectedNinjas);

                // Act
                var result = await ServiceUnderTest.ReadAllAsync();

                // Assert
                Assert.Same(expectedNinjas, result);
            }
        }

        public class ReadAllInClanAsync : NinjaServiceTest
        {
            [Fact]
            public async void Should_return_all_Ninja_in_Clan()
            {
                // Arrange
                var clanName = "Some clan name";
                var expectedNinjas = new Ninja[]
                {
                    new Ninja { Name = "Test Ninja 1" },
                    new Ninja { Name = "Test Ninja 2" },
                    new Ninja { Name = "Test Ninja 3" }
                };
                NinjaRepositoryMock
                    .Setup(x => x.ReadAllInClanAsync(clanName))
                    .ReturnsAsync(expectedNinjas)
                    .Verifiable();
                ClanServiceMock
                    .Setup(x => x.IsClanExistsAsync(clanName))
                    .ReturnsAsync(true)
                    .Verifiable();

                // Act
                var result = await ServiceUnderTest.ReadAllInClanAsync(clanName);

                // Assert
                Assert.Same(expectedNinjas, result);
                NinjaRepositoryMock
                    .Verify(x => x.ReadAllInClanAsync(clanName), Times.Once);
                ClanServiceMock
                    .Verify(x => x.IsClanExistsAsync(clanName), Times.Once);
            }

            [Fact]
            public async void Should_throw_ClanNotFoundException_when_clan_does_not_exist()
            {
                // Arrange
                var unexistingClanName = "Some clan name";
                NinjaRepositoryMock
                    .Setup(x => x.ReadAllInClanAsync(unexistingClanName))
                    .Verifiable();
                ClanServiceMock
                    .Setup(x => x.IsClanExistsAsync(unexistingClanName))
                    .ReturnsAsync(false)
                    .Verifiable();

                // Act & Assert
                await Assert.ThrowsAsync<ClanNotFoundException>(() => ServiceUnderTest.ReadAllInClanAsync(unexistingClanName));

                NinjaRepositoryMock
                    .Verify(x => x.ReadAllInClanAsync(unexistingClanName), Times.Never);
                ClanServiceMock
                    .Verify(x => x.IsClanExistsAsync(unexistingClanName), Times.Once);
            }
        }

        public class ReadOneAsync : NinjaServiceTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_a_Ninja()
            {
                // Arrange
                var clanName = "Some clan name";
                var ninjaKey = "Some ninja key";
                var expectedNinja = new Ninja { Name = "Test Ninja 1" };
                NinjaRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanName, ninjaKey))
                    .ReturnsAsync(expectedNinja);

                // Act
                var result = await ServiceUnderTest.ReadOneAsync(clanName, ninjaKey);

                // Assert
                Assert.Same(expectedNinja, result);
            }

            [Fact]
            public async void Should_throw_NinjaNotFoundException_when_ninja_does_not_exist()
            {
                // Arrange
                var unexistingClanName = "Some clan name";
                var unexistingNinjaKey = "Some ninja key";
                NinjaRepositoryMock
                    .Setup(x => x.ReadOneAsync(unexistingClanName, unexistingNinjaKey))
                    .ReturnsAsync(default(Ninja));

                // Act & Assert
                await Assert.ThrowsAsync<NinjaNotFoundException>(() => ServiceUnderTest.ReadOneAsync(unexistingClanName, unexistingNinjaKey));
            }
        }

        public class CreateAsync : NinjaServiceTest
        {
            [Fact]
            public async void Should_create_and_return_the_created_Ninja()
            {
                // Arrange
                const string clanName = "Some clan name";
                var expectedNinja = new Ninja { Clan = new Clan { Name = clanName } };
                NinjaRepositoryMock
                    .Setup(x => x.CreateAsync(expectedNinja))
                    .ReturnsAsync(expectedNinja)
                    .Verifiable();
                ClanServiceMock
                    .Setup(x => x.IsClanExistsAsync(clanName))
                    .ReturnsAsync(true);

                // Act
                var result = await ServiceUnderTest.CreateAsync(expectedNinja);

                // Assert
                Assert.Same(expectedNinja, result);
                NinjaRepositoryMock.Verify(x => x.CreateAsync(expectedNinja), Times.Once);
            }

            [Fact]
            public async void Should_throw_a_ClanNotFoundException_when_clan_does_not_exist()
            {
                const string clanName = "Some clan name";
                var expectedNinja = new Ninja { Clan = new Clan { Name = clanName } };
                NinjaRepositoryMock
                    .Setup(x => x.CreateAsync(expectedNinja))
                    .ReturnsAsync(expectedNinja)
                    .Verifiable();
                ClanServiceMock
                    .Setup(x => x.IsClanExistsAsync(clanName))
                    .ReturnsAsync(false);

                // Act & Assert
                await Assert.ThrowsAsync<ClanNotFoundException>(() => ServiceUnderTest.CreateAsync(expectedNinja));
                
                // Make sure CreateAsync is never called 
                NinjaRepositoryMock.Verify(x => x.CreateAsync(expectedNinja), Times.Never);
            }
        }

        public class UpdateAsync : NinjaServiceTest
        {
            [Fact]
            public async void Should_update_and_return_the_updated_Ninja()
            {
                // Arrange
                const string ninjaKey = "Some key";
                const string clanKey = "Some clan";
                var expectedNinja = new Ninja
                {
                    Key = ninjaKey, 
                    Clan = new Clan { Name = clanKey }
                };
                NinjaRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanKey, ninjaKey))
                    .ReturnsAsync(expectedNinja);
                NinjaRepositoryMock
                    .Setup(x => x.UpdateAsync(expectedNinja))
                    .ReturnsAsync(expectedNinja)
                    .Verifiable();
                ClanServiceMock
                    .Setup(x => x.IsClanExistsAsync(clanKey))
                    .ReturnsAsync(true);

                // Act
                var result = await ServiceUnderTest.UpdateAsync(expectedNinja);

                // Assert
                Assert.Same(expectedNinja, result);
                NinjaRepositoryMock.Verify(x => x.UpdateAsync(expectedNinja), Times.Once);
            }

            [Fact]
            public async void Should_throw_NinjaNotFoundException_when_ninja_does_not_exist()
            {
                // Arrange
                const string ninjaKey = "SomeKey";
                const string clanKey = "Some clan";
                var unexistingNinja = new Ninja { Key = ninjaKey, Clan = new Clan { Name = clanKey } };
                NinjaRepositoryMock
                    .Setup(x => x.UpdateAsync(unexistingNinja))
                    .Verifiable();
                NinjaRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanKey, ninjaKey))
                    .ReturnsAsync(default(Ninja))
                    .Verifiable();
                ClanServiceMock
                    .Setup(x => x.IsClanExistsAsync(clanKey))
                    .ReturnsAsync(true);

                // Act & Assert
                await Assert.ThrowsAsync<NinjaNotFoundException>(() => ServiceUnderTest.UpdateAsync(unexistingNinja));

                // Make sure UpdateAsync is never hit
                NinjaRepositoryMock
                    .Verify(x => x.UpdateAsync(unexistingNinja), Times.Never);

                // Make sure we read the ninja from the repository before atempting an update.
                NinjaRepositoryMock
                    .Verify(x => x.ReadOneAsync(clanKey, ninjaKey), Times.Once);
            }

            [Fact]
            public async void Should_throw_a_ClanNotFoundException_when_clan_does_not_exist()
            {
                // Arrange
                const string ninjaKey = "SomeKey";
                const string clanKey = "Some clan";
                var unexistingNinja = new Ninja { Key = ninjaKey, Clan = new Clan { Name = clanKey } };
                NinjaRepositoryMock
                    .Setup(x => x.UpdateAsync(unexistingNinja))
                    .Verifiable();
                ClanServiceMock
                    .Setup(x => x.IsClanExistsAsync(clanKey))
                    .ReturnsAsync(false);

                // Act & Assert
                await Assert.ThrowsAsync<ClanNotFoundException>(() => ServiceUnderTest.UpdateAsync(unexistingNinja));

                // Make sure UpdateAsync is never called
                NinjaRepositoryMock
                    .Verify(x => x.UpdateAsync(unexistingNinja), Times.Never);
            }
        }

        public class DeleteAsync : NinjaServiceTest
        {
            [Fact]
            public async void Should_delete_and_return_the_deleted_Ninja()
            {
                // Arrange
                var clanName = "My clan";
                var ninjaKey = "Some key";
                var expectedNinja = new Ninja { Name = "Test Ninja 1" };
                NinjaRepositoryMock
                    .Setup(x => x.DeleteAsync(clanName, ninjaKey))
                    .ReturnsAsync(expectedNinja)
                    .Verifiable();
                NinjaRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanName, ninjaKey))
                    .ReturnsAsync(expectedNinja);

                // Act
                var result = await ServiceUnderTest.DeleteAsync(clanName, ninjaKey);

                // Assert
                Assert.Same(expectedNinja, result);
                NinjaRepositoryMock.Verify(x => x.DeleteAsync(clanName, ninjaKey), Times.Once);
            }

            [Fact]
            public async void Should_throw_NinjaNotFoundException_when_ninja_does_not_exist()
            {
                // Arrange
                const string clanName = "Some clan name";
                const string ninjaKey = "Some ninja key";

                NinjaRepositoryMock
                    .Setup(x => x.DeleteAsync(clanName, ninjaKey))
                    .Verifiable();
                NinjaRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanName, ninjaKey))
                    .ReturnsAsync(default(Ninja))
                    .Verifiable();

                // Act & Assert
                await Assert.ThrowsAsync<NinjaNotFoundException>(() => ServiceUnderTest.DeleteAsync(clanName, ninjaKey));

                // Make sure UpdateAsync is never hit
                NinjaRepositoryMock
                    .Verify(x => x.DeleteAsync(clanName, ninjaKey), Times.Never);

                // Make sure we read the ninja from the repository before atempting an update.
                NinjaRepositoryMock
                    .Verify(x => x.ReadOneAsync(clanName, ninjaKey), Times.Once);
            }
        }
    }
}