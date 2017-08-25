using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ForEvolve.Azure.Storage.Table;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.IntegrationTests
{
    public class NinjaControllerTest : BaseHttpTest
    {
        protected NinjaEntityTableStorageRepositoryFake TableStorageFake { get; }

        protected string ClanName1 => "Iga";
        protected string ClanName2 => "Kōga";

        public NinjaControllerTest()
        {
            TableStorageFake = new NinjaEntityTableStorageRepositoryFake();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITableStorageRepository<NinjaEntity>>(x => TableStorageFake);
            services.AddSingleton<IEnumerable<Clan>>(x => new List<Clan> {
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
                var superClanNinja = PopulateTableStorageFake(amountOfNinjaToCreate: 2, clanName: ClanName1);
                var otherClanNinja = PopulateTableStorageFake(amountOfNinjaToCreate: 2, clanName: ClanName2);
                var expectedNinjaLength = 4;

                // Act
                var result = await Client.GetAsync("v1/ninja");

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja[]>();
                Assert.NotNull(ninja);
                Assert.Equal(expectedNinjaLength, ninja.Length);
                Assert.Collection(ninja,
                    n => AssertNinjaEntityEqualNinja(superClanNinja[0], n),
                    n => AssertNinjaEntityEqualNinja(superClanNinja[1], n),
                    n => AssertNinjaEntityEqualNinja(otherClanNinja[0], n),
                    n => AssertNinjaEntityEqualNinja(otherClanNinja[1], n)
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
                var superClanNinja = PopulateTableStorageFake(amountOfNinjaToCreate: 2, clanName: ClanName1);
                var otherClanNinja = PopulateTableStorageFake(amountOfNinjaToCreate: 2, clanName: expectedClanName);
                var expectedNinja = superClanNinja.Concat(otherClanNinja);
                var expectedNinjaLength = 2;

                // Act
                var result = await Client.GetAsync($"v1/ninja/{expectedClanName}");

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja[]>();
                Assert.NotNull(ninja);
                Assert.Equal(expectedNinjaLength, ninja.Length);
                Assert.Collection(ninja,
                    n => AssertNinjaEntityEqualNinja(otherClanNinja[0], n),
                    n => AssertNinjaEntityEqualNinja(otherClanNinja[1], n)
                );
            }
        }

        public class ReadOneAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_return_one_ninja_from_azure_table()
            {
                // Arrange
                var superClanNinja = PopulateTableStorageFake(amountOfNinjaToCreate: 5, clanName: ClanName1);
                var otherClanNinja = PopulateTableStorageFake(amountOfNinjaToCreate: 5, clanName: ClanName2);
                var expectedNinja = superClanNinja[3];
                var clanName = expectedNinja.PartitionKey;
                var ninjaKey = expectedNinja.RowKey;

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

                // Act
                var result = await Client.PostAsync("v1/ninja", ninjaBody);

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja>();
                Assert.NotNull(ninja);
                Assert.Equal(1, TableStorageFake.EntityCount);
                AssertNinjaEntityEqualNinja(TableStorageFake.ElementAt(0), ninja);
            }
        }

        public class UpdateAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_update_the_ninja_in_azure_table()
            {
                // Arrange
                var superClanNinja = PopulateTableStorageFake(amountOfNinjaToCreate: 1, clanName: ClanName1);
                var ninjaToUpdate = new Ninja
                {
                    Clan = new Clan { Name = superClanNinja[0].PartitionKey },
                    Key = superClanNinja[0].RowKey,
                    Name = "My new name",
                    Level = 1234
                };
                var ninjaBody = ninjaToUpdate.ToJsonHttpContent();

                // Act
                var result = await Client.PutAsync("v1/ninja", ninjaBody);

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja>();
                Assert.NotNull(ninja);
                Assert.Equal(1, TableStorageFake.EntityCount);
                TableStorageFake.HasBeenMerged(superClanNinja[0]);
            }
        }

        public class DeleteAsync : NinjaControllerTest
        {
            [Fact]
            public async Task Should_delete_the_ninja_from_azure_table()
            {
                // Arrange
                var superClanNinja = PopulateTableStorageFake(amountOfNinjaToCreate: 3, clanName: ClanName1);
                var ninjaToDelete = superClanNinja[0];
                var clanName = ninjaToDelete.PartitionKey;
                var ninjaKey = ninjaToDelete.RowKey;

                // Act
                var result = await Client.DeleteAsync($"v1/ninja/{clanName}/{ninjaKey}");

                // Assert
                result.EnsureSuccessStatusCode();
                var ninja = await result.Content.ReadAsJsonObjectAsync<Ninja>();
                Assert.NotNull(ninja);
                Assert.Equal(2, TableStorageFake.EntityCount);
                AssertNinjaEntityEqualNinja(ninjaToDelete, ninja);
            }
        }

        protected List<NinjaEntity> PopulateTableStorageFake(int amountOfNinjaToCreate, string clanName)
        {
            var ninjaList = new List<NinjaEntity>();
            for (int i = 0; i < amountOfNinjaToCreate; i++)
            {
                var ninja = new NinjaEntity
                {
                    Level = i,
                    Name = $"Ninja {i}",
                    PartitionKey = clanName,
                    RowKey = $"NinjaKey {i}"
                };
                ninjaList.Add(ninja);
            }
            TableStorageFake.AddRange(ninjaList);
            return ninjaList;
        }

        protected void AssertNinjaEntityEqualNinja(NinjaEntity entity, Ninja ninja)
        {
            Assert.Equal(entity.PartitionKey, ninja.Clan.Name);
            Assert.Equal(entity.RowKey, ninja.Key);
            Assert.Equal(entity.Name, ninja.Name);
            Assert.Equal(entity.Level, ninja.Level);
        }
    }
}
