using ForEvolve.Blog.Samples.NinjaApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using ForEvolve.Azure.Storage.Table;

namespace ForEvolve.Blog.Samples.NinjaApi.IntegrationTests
{
    public class StartupTest : BaseHttpTest
    {
        public class ServiceProvider : StartupTest
        {
            [Fact]
            public void Should_return_both_Iga_and_Kōga_clans()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var result = serviceProvider.GetService<IEnumerable<Clan>>();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Collection(result,
                    clan => Assert.Equal("Iga", clan.Name),
                    clan => Assert.Equal("Kōga", clan.Name)
                );
            }

            [Fact]
            public void Should_return_TableStorageSettings()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var result = serviceProvider.GetService<ITableStorageSettings>();

                // Assert
                var settings = Assert.IsType<TableStorageSettings>(result);
                Assert.NotNull(settings.AccountKey);
                Assert.NotNull(settings.AccountName);
                Assert.Equal("MyTableName", settings.TableName);
            }

            [Fact]
            public void Should_return_TableStorageRepository_of_NinjaEntity()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var result = serviceProvider.GetService<ITableStorageRepository<NinjaEntity>>();

                // Assert
                Assert.IsType<TableStorageRepository<NinjaEntity>>(result);
            }
        }
    }
}
