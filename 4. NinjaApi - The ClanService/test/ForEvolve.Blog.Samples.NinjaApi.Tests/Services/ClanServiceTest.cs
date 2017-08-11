using ForEvolve.Blog.Samples.NinjaApi.Models;
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

        public ClanServiceTest()
        {
            ServiceUnderTest = new ClanService();
        }

        public class ReadAllAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_return_all_clansAsync()
            {
                // Arrange
                var expectedClans = new ReadOnlyCollection<Clan>(new List<Clan>
                {
                    new Clan { Name = "My Clan" },
                    new Clan { Name = "Your Clan" },
                    new Clan { Name = "His Clan" }
                });

                // Act
                var result = await ServiceUnderTest.ReadAllAsync();

                // Assert
                Assert.Same(expectedClans, result);
            }
        }

        public class ReadOneAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_return_the_expected_clanAsync()
            {
                // Arrange
                var clanName = "My Clan";
                var expectedClan = new Clan { Name = clanName };

                // Act
                var result = await ServiceUnderTest.ReadOneAsync(clanName);

                // Assert
                Assert.Same(expectedClan, result);
            }

            [Fact]
            public async Task Should_return_null_if_the_clan_does_not_existAsync()
            {
                // Arrange
                var clanName = "My Clan";

                // Act
                var result = await ServiceUnderTest.ReadOneAsync(clanName);

                // Assert
                Assert.Null(result);
            }
        }

        public class IsClanExistsAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_return_true_if_the_clan_existAsync()
            {
                // Arrange
                var clanName = "Your Clan";

                // Act
                var result = await ServiceUnderTest.IsClanExistsAsync(clanName);

                // Assert
                Assert.True(result);
            }
            [Fact]
            public async Task Should_return_false_if_the_clan_does_not_existAsync()
            {
                // Arrange
                var clanName = "Unexisting Clan";

                // Act
                var result = await ServiceUnderTest.IsClanExistsAsync(clanName);

                // Assert
                Assert.False(result);
            }
        }

        public class CreateAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_create_and_return_the_specified_clanAsync()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.CreateAsync(null));
            }
        }

        public class UpdateAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_update_and_return_the_specified_clan()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.UpdateAsync(null));
            }
        }

        public class DeleteAsync : ClanServiceTest
        {
            [Fact]
            public async Task Should_delete_and_return_the_specified_clan()
            {
                // Arrange, Act, Assert
                var exception = await Assert.ThrowsAsync<NotSupportedException>(() => ServiceUnderTest.DeleteAsync(null));
            }
        }
    }
}
