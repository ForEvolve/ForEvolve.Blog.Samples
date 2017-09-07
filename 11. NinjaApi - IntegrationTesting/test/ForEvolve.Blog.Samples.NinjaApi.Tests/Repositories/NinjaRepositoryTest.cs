using ForEvolve.Azure.Storage.Table;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using ForEvolve.Blog.Samples.NinjaApi.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Repositories
{
    public class NinjaRepositoryTest
    {
        protected NinjaRepository RepositoryUnderTest { get; }

        protected Mock<INinjaMappingService> NinjaMappingServiceMock { get; }
        protected Mock<ITableStorageRepository<NinjaEntity>> NinjaEntityTableStorageRepositoryMock { get; }

        public NinjaRepositoryTest()
        {
            NinjaMappingServiceMock = new Mock<INinjaMappingService>();
            NinjaEntityTableStorageRepositoryMock = new Mock<ITableStorageRepository<NinjaEntity>>();
            RepositoryUnderTest = new NinjaRepository(
                NinjaMappingServiceMock.Object,
                NinjaEntityTableStorageRepositoryMock.Object
            );
        }

        public class ReadAllAsync : NinjaRepositoryTest
        {
            [Fact]
            public async Task Should_map_ReadAll_and_return_the_expected_ninja()
            {
                // Arrange
                var entities = new NinjaEntity[0];
                var expectedNinja = new Ninja[0];
                
                NinjaEntityTableStorageRepositoryMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(entities)
                    .Verifiable();
                NinjaMappingServiceMock
                    .Setup(x => x.Map(entities))
                    .Returns(expectedNinja)
                    .Verifiable();

                // Act
                var result = await RepositoryUnderTest.ReadAllAsync();

                // Assert
                NinjaMappingServiceMock
                    .Verify(x => x.Map(entities), Times.Once);
                NinjaEntityTableStorageRepositoryMock
                    .Verify(x => x.ReadAllAsync(), Times.Once);
                Assert.Same(expectedNinja, result);
            } 
        }

        public class ReadAllInClanAsync : NinjaRepositoryTest
        {
            [Fact]
            public async Task Should_map_ReadPartition_and_return_the_expected_ninja()
            {
                // Arrange
                var clanName = "My clan";
                var entities = new NinjaEntity[0];
                var expectedNinja = new Ninja[0];
                NinjaEntityTableStorageRepositoryMock
                    .Setup(x => x.ReadPartitionAsync(clanName))
                    .ReturnsAsync(entities)
                    .Verifiable();
                NinjaMappingServiceMock
                    .Setup(x => x.Map(entities))
                    .Returns(expectedNinja)
                    .Verifiable();

                // Act
                var result = await RepositoryUnderTest.ReadAllInClanAsync(clanName);

                // Assert
                NinjaMappingServiceMock
                    .Verify(x => x.Map(entities), Times.Once);
                NinjaEntityTableStorageRepositoryMock
                    .Verify(x => x.ReadPartitionAsync(clanName), Times.Once);
                Assert.Same(expectedNinja, result);
            }
        }

        public class ReadOneAsync : NinjaRepositoryTest
        {
            [Fact]
            public async Task Should_map_ReadOne_and_return_the_expected_ninja()
            {
                // Arrange
                var clanName = "My clan";
                var ninjaKey = "123FB950-57DB-4CD0-B4D1-7E6B00A6163A";
                var entity = new NinjaEntity();
                var expectedNinja = new Ninja();

                NinjaEntityTableStorageRepositoryMock
                    .Setup(x => x.ReadOneAsync(clanName, ninjaKey))
                    .ReturnsAsync(entity)
                    .Verifiable();
                NinjaMappingServiceMock
                    .Setup(x => x.Map(entity))
                    .Returns(expectedNinja)
                    .Verifiable();

                // Act
                var result = await RepositoryUnderTest.ReadOneAsync(clanName, ninjaKey);

                // Assert
                NinjaMappingServiceMock
                    .Verify(x => x.Map(entity), Times.Once);
                NinjaEntityTableStorageRepositoryMock
                    .Verify(x => x.ReadOneAsync(clanName, ninjaKey), Times.Once);
                Assert.Same(expectedNinja, result);
            }
        }

        public class CreateAsync : NinjaRepositoryTest
        {
            [Fact]
            public async Task Should_map_InsertOrReplace_and_return_the_expected_ninja()
            {
                // Arrange
                var ninjaToCreate = new Ninja();
                var entityToCreate = new NinjaEntity();
                var createdEntity = new NinjaEntity();
                var expectedNinja = new Ninja();

                NinjaMappingServiceMock
                    .Setup(x => x.Map(ninjaToCreate))
                    .Returns(entityToCreate)
                    .Verifiable();
                NinjaEntityTableStorageRepositoryMock
                    .Setup(x => x.InsertOrReplaceAsync(entityToCreate))
                    .ReturnsAsync(createdEntity)
                    .Verifiable();
                NinjaMappingServiceMock
                    .Setup(x => x.Map(createdEntity))
                    .Returns(expectedNinja)
                    .Verifiable();

                // Act
                var result = await RepositoryUnderTest.CreateAsync(ninjaToCreate);

                // Assert
                NinjaMappingServiceMock.Verify(x => x.Map(ninjaToCreate), Times.Once);
                NinjaEntityTableStorageRepositoryMock.Verify(x => x.InsertOrReplaceAsync(entityToCreate), Times.Once);
                NinjaMappingServiceMock.Verify(x => x.Map(createdEntity), Times.Once);
                Assert.Same(expectedNinja, result);
            }
        }

        public class UpdateAsync : NinjaRepositoryTest
        {
            [Fact]
            public async Task Should_map_InsertOrMerge_and_return_the_expected_ninja()
            {
                // Arrange
                var ninjaToUpdate = new Ninja();
                var entityToUpdate = new NinjaEntity();
                var updatedEntity = new NinjaEntity();
                var expectedNinja = new Ninja();

                NinjaMappingServiceMock
                    .Setup(x => x.Map(ninjaToUpdate))
                    .Returns(entityToUpdate)
                    .Verifiable();
                NinjaEntityTableStorageRepositoryMock
                    .Setup(x => x.InsertOrMergeAsync(entityToUpdate))
                    .ReturnsAsync(updatedEntity)
                    .Verifiable();
                NinjaMappingServiceMock
                    .Setup(x => x.Map(updatedEntity))
                    .Returns(expectedNinja)
                    .Verifiable();

                // Act
                var result = await RepositoryUnderTest.UpdateAsync(ninjaToUpdate);

                // Assert
                NinjaMappingServiceMock.Verify(x => x.Map(ninjaToUpdate), Times.Once);
                NinjaEntityTableStorageRepositoryMock.Verify(x => x.InsertOrMergeAsync(entityToUpdate), Times.Once);
                NinjaMappingServiceMock.Verify(x => x.Map(updatedEntity), Times.Once);
                Assert.Same(expectedNinja, result);
            }
        }

        public class DeleteAsync : NinjaRepositoryTest
        {
            [Fact]
            public async Task Should_map_Remove_and_return_the_expected_ninja()
            {
                // Arrange
                var clanName = "My clan";
                var ninjaKey = "123FB950-57DB-4CD0-B4D1-7E6B00A6163A";
                var deletedEntity = new NinjaEntity();
                var expectedNinja = new Ninja();

                NinjaEntityTableStorageRepositoryMock
                    .Setup(x => x.DeleteOneAsync(clanName, ninjaKey))
                    .ReturnsAsync(deletedEntity)
                    .Verifiable();
                NinjaMappingServiceMock
                    .Setup(x => x.Map(deletedEntity))
                    .Returns(expectedNinja)
                    .Verifiable();

                // Act
                var result = await RepositoryUnderTest.DeleteAsync(clanName, ninjaKey);

                // Assert
                NinjaEntityTableStorageRepositoryMock.Verify(x => x.DeleteOneAsync(clanName, ninjaKey), Times.Once);
                NinjaMappingServiceMock.Verify(x => x.Map(deletedEntity), Times.Once);
                Assert.Same(expectedNinja, result);
            }
        }
    }
}
