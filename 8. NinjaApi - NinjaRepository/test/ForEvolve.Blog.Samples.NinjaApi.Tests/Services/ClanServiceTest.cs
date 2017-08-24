using ForEvolve.Blog.Samples.NinjaApi.Models;
using ForEvolve.Blog.Samples.NinjaApi.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Services
{
    public class ClanServiceTest
    {
        protected ClanService ServiceUnderTest { get; }
        protected Mock<IClanRepository> ClanRepositoryMock { get; }

        public ClanServiceTest()
        {
            ClanRepositoryMock = new Mock<IClanRepository>();
            ServiceUnderTest = new ClanService(ClanRepositoryMock.Object);
        }

        public class ReadAllAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_return_all_clans()
            {
                // Arrange
                var expectedClans = new ReadOnlyCollection<Clan>(new List<Clan>
                {
                    new Clan { Name = "My Clan" },
                    new Clan { Name = "Your Clan" },
                    new Clan { Name = "His Clan" }
                });
                ClanRepositoryMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(expectedClans);

                // Act
                var result = await ServiceUnderTest.ReadAllAsync();

                // Assert
                Assert.Same(expectedClans, result);
            }
        }

        public class ReadOneAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_return_the_expected_clan()
            {
                // Arrange
                var clanName = "My Clan";
                var expectedClan = new Clan { Name = clanName };
                ClanRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanName))
                    .ReturnsAsync(expectedClan);

                // Act
                var result = await ServiceUnderTest.ReadOneAsync(clanName);

                // Assert
                Assert.Same(expectedClan, result);
            }

            [Fact]
            public async Task Should_return_null_if_the_clan_does_not_exist()
            {
                // Arrange
                var clanName = "My Clan";
                ClanRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanName))
                    .ReturnsAsync(default(Clan));

                // Act
                var result = await ServiceUnderTest.ReadOneAsync(clanName);

                // Assert
                Assert.Null(result);
            }
        }

        public class IsClanExistsAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_return_true_if_the_clan_exist()
            {
                // Arrange
                var clanName = "Your Clan";
                ClanRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanName))
                    .ReturnsAsync(new Clan());

                // Act
                var result = await ServiceUnderTest.IsClanExistsAsync(clanName);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public async Task Should_return_false_if_the_clan_does_not_exist()
            {
                // Arrange
                var clanName = "Unexisting Clan";
                ClanRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanName))
                    .ReturnsAsync(default(Clan));

                // Act
                var result = await ServiceUnderTest.IsClanExistsAsync(clanName);

                // Assert
                Assert.False(result);
            }
        }

        public class CreateAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_throw_a_NotSupportedException()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.CreateAsync(null));
            }
        }

        public class UpdateAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_throw_a_NotSupportedException()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.UpdateAsync(null));
            }
        }

        public class DeleteAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_throw_a_NotSupportedException()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.DeleteAsync(null));
            }
        }
    }
}
