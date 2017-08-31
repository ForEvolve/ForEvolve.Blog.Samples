using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ForEvolve.Azure.Storage.Table;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;

namespace ForEvolve.Blog.Samples.NinjaApi.IntegrationTests
{
    public class NinjaControllerTest : BaseHttpTest
    {
        protected Mock<ITableStorageRepository<NinjaEntity>> TableStorageMock { get; }

        protected string ClanName1 => "Iga";
        protected string ClanName2 => "Kōga";

        public NinjaControllerTest()
        {
            TableStorageMock = new Mock<ITableStorageRepository<NinjaEntity>>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton(x => TableStorageMock.Object)
                .AddSingleton<IEnumerable<Clan>>(x => new List<Clan> {
                    new Clan{ Name = ClanName1 },
                    new Clan{ Name = ClanName2 }
                });
        }

        public class ReadAllAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_return_all_ninja_in_azure_table()
            {
                // Arrange
                var superClanNinja = CreateEntities(amountOfNinjaToCreate: 2, clanName: ClanName1);
                var otherClanNinja = CreateEntities(amountOfNinjaToCreate: 2, clanName: ClanName2);
                var all = superClanNinja.Union(otherClanNinja).ToArray();
                var expectedNinjaLength = 4;

                TableStorageMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(all);

                // Act
                var result = await Client.GetAsync("v1/ninja");

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja[]>();
                Assert.NotNull(ninja);
                Assert.Equal(expectedNinjaLength, ninja.Length);
                Assert.Collection(ninja,
                    n => AssertNinjaEntityEqualNinja(all[0], n),
                    n => AssertNinjaEntityEqualNinja(all[1], n),
                    n => AssertNinjaEntityEqualNinja(all[2], n),
                    n => AssertNinjaEntityEqualNinja(all[3], n)
                );
            }
        }

        public class ReadAllInClanAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_return_all_ninja_in_azure_table_partition()
            {
                // Arrange
                var expectedClanName = ClanName2;
                var expectedNinja = CreateEntities(amountOfNinjaToCreate: 2, clanName: expectedClanName).ToArray();
                var expectedNinjaLength = 2;

                TableStorageMock
                    .Setup(x => x.ReadPartitionAsync(expectedClanName))
                    .ReturnsAsync(expectedNinja);

                // Act
                var result = await Client.GetAsync($"v1/ninja/{expectedClanName}");

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja[]>();
                Assert.NotNull(ninja);
                Assert.Equal(expectedNinjaLength, ninja.Length);
                Assert.Collection(ninja,
                    n => AssertNinjaEntityEqualNinja(expectedNinja[0], n),
                    n => AssertNinjaEntityEqualNinja(expectedNinja[1], n)
                );
            }
        }

        public class ReadOneAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_return_one_ninja_from_azure_table()
            {
                // Arrange
                var expectedNinja = CreateEntity(ClanName1);
                var clanName = expectedNinja.PartitionKey;
                var ninjaKey = expectedNinja.RowKey;

                TableStorageMock
                    .Setup(x => x.ReadOneAsync(clanName, ninjaKey))
                    .ReturnsAsync(expectedNinja);

                // Act
                var result = await Client.GetAsync($"v1/ninja/{clanName}/{ninjaKey}");

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja>();
                Assert.NotNull(ninja);
                AssertNinjaEntityEqualNinja(expectedNinja, ninja);
            }
        }

        public class CreateAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_create_the_ninja_in_azure_table()
            {
                // Arrange
                var ninjaToCreate = new Ninja
                {
                    Name = "Bob",
                    Level = 6,
                    Key = "12345",
                    Clan = new Clan { Name = ClanName1 }
                };
                var ninjaBody = ninjaToCreate.ToJsonHttpContent();

                var mapper = new Mappers.NinjaEntityToNinjaMapper();
                NinjaEntity createdEntity = null;
                TableStorageMock
                    .Setup(x => x.InsertOrReplaceAsync(It.IsAny<NinjaEntity>()))
                    .ReturnsAsync((NinjaEntity x) => {
                        createdEntity = x;
                        return x;
                    });

                // Act
                var result = await Client.PostAsync("v1/ninja", ninjaBody);

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja>();
                Assert.NotNull(ninja);
                Assert.NotNull(createdEntity);
                AssertNinjaEntityEqualNinja(createdEntity, ninja);
            }
        }

        public class UpdateAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_update_the_ninja_in_azure_table()
            {
                // Arrange
                var ninjaToUpdate = new Ninja
                {
                    Clan = new Clan { Name = ClanName1 },
                    Key = "Some UpdateAsync Ninja Key",
                    Name = "My new name",
                    Level = 1234
                };
                var ninjaBody = ninjaToUpdate.ToJsonHttpContent();

                NinjaEntity updatedEntity = null;
                TableStorageMock
                    .Setup(x => x.InsertOrMergeAsync(It.IsAny<NinjaEntity>()))
                    .ReturnsAsync((NinjaEntity n) =>
                    {
                        updatedEntity = n;
                        return n;
                    });
                TableStorageMock
                    .SetupEnforceNinjaExistenceAsync(ClanName1, ninjaToUpdate.Key);

                // Act
                var result = await Client.PutAsync("v1/ninja", ninjaBody);

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja>();
                Assert.NotNull(ninja);
                Assert.NotNull(updatedEntity);
                AssertNinjaEntityEqualNinja(updatedEntity, ninja);
            }
        }

        public class DeleteAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_delete_the_ninja_from_azure_table()
            {
                // Arrange
                var ninjaToDelete = CreateEntity(ClanName1);
                var clanName = ninjaToDelete.PartitionKey;
                var ninjaKey = ninjaToDelete.RowKey;

                TableStorageMock
                    .SetupEnforceNinjaExistenceAsync(clanName, ninjaKey);
                TableStorageMock
                    .Setup(x => x.DeleteOneAsync(clanName, ninjaKey))
                    .ReturnsAsync(ninjaToDelete);

                // Act
                var result = await Client.DeleteAsync($"v1/ninja/{clanName}/{ninjaKey}");

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja>();
                Assert.NotNull(ninja);
                AssertNinjaEntityEqualNinja(ninjaToDelete, ninja);
            }
        }


        protected NinjaEntity CreateEntity(string clanName)
        {
            return CreateEntities(1, clanName).First();
        }

        protected IEnumerable<NinjaEntity> CreateEntities(int amountOfNinjaToCreate, string clanName)
        {
            for (int i = 0; i < amountOfNinjaToCreate; i++)
            {
                var ninja = new NinjaEntity
                {
                    Level = i,
                    Name = $"Ninja {i}",
                    PartitionKey = clanName,
                    RowKey = $"NinjaKey {i}"
                };
                yield return ninja;
            }
        }

        protected void AssertNinjaEntityEqualNinja(NinjaEntity entity, Ninja ninja)
        {
            Assert.Equal(entity.PartitionKey, ninja.Clan.Name);
            Assert.Equal(entity.RowKey, ninja.Key);
            Assert.Equal(entity.Name, ninja.Name);
            Assert.Equal(entity.Level, ninja.Level);
        }
    }

    public static class TableStorageMockExtensions
    {
        public static NinjaEntity SetupEnforceNinjaExistenceAsync(this Mock<ITableStorageRepository<NinjaEntity>> tableStorageMock, string clanName, string ninjaKey, NinjaEntity entityToReturn = null)
        {
            var entity = entityToReturn ?? new NinjaEntity();
            tableStorageMock
                .Setup(x => x.ReadOneAsync(clanName, ninjaKey))
                .ReturnsAsync(entity);
            return entity;
        }
    }
}
