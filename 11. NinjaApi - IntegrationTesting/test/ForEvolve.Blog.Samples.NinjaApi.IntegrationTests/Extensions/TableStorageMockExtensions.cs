using ForEvolve.Azure.Storage.Table;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using Moq;

namespace ForEvolve.Blog.Samples.NinjaApi.IntegrationTests
{
    public static class TableStorageMockExtensions
    {
        public static NinjaEntity SetupEnforceNinjaExistenceAsync(this Mock<ITableStorageRepository<NinjaEntity>> tableStorageMock, string clanName, string ninjaKey)
        {
            var entity = new NinjaEntity(); // Only need to be not null
            tableStorageMock
                .Setup(x => x.ReadOneAsync(clanName, ninjaKey))
                .ReturnsAsync(entity);
            return entity;
        }
    }
}
