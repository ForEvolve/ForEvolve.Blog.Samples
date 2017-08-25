using ForEvolve.Blog.Samples.NinjaApi.Mappers;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Services
{
    public class NinjaMappingServiceTest
    {
        protected NinjaMappingService ServiceUnderTest { get; }
        protected Mock<IMapper<Ninja, NinjaEntity>> NinjaToNinjaEntityMapperMock { get; }
        protected Mock<IMapper<NinjaEntity, Ninja>> NinjaEntityToNinjaMapperMock { get; }
        protected Mock<IMapper<IEnumerable<NinjaEntity>, IEnumerable<Ninja>>> NinjaEntityEnumerableToNinjaMapperMock { get; }

        public NinjaMappingServiceTest()
        {
            NinjaToNinjaEntityMapperMock = new Mock<IMapper<Ninja, NinjaEntity>>();
            NinjaEntityToNinjaMapperMock = new Mock<IMapper<NinjaEntity, Ninja>>();
            NinjaEntityEnumerableToNinjaMapperMock = new Mock<IMapper<IEnumerable<NinjaEntity>, IEnumerable<Ninja>>>();
            ServiceUnderTest = new NinjaMappingService(
                NinjaToNinjaEntityMapperMock.Object,
                NinjaEntityToNinjaMapperMock.Object,
                NinjaEntityEnumerableToNinjaMapperMock.Object
            );
        }

        [Fact]
        public void Map_Ninja_to_NinjaEntity_should_delegate_to_NinjaToNinjaEntityMapper()
        {
            // Arrange
            var ninja = new Ninja();
            var expectedEntity = new NinjaEntity();
            NinjaToNinjaEntityMapperMock
                .Setup(x => x.Map(ninja))
                .Returns(expectedEntity);

            // Act
            var result = ServiceUnderTest.Map(ninja);

            // Assert
            Assert.Same(expectedEntity, result);
        }

        [Fact]
        public void Map_NinjaEntity_to_Ninja_should_delegate_to_NinjaEntityToNinjaMapper()
        {
            // Arrange
            var ninjaEntity = new NinjaEntity();
            var expectedNinja = new Ninja();
            NinjaEntityToNinjaMapperMock
                .Setup(x => x.Map(ninjaEntity))
                .Returns(expectedNinja);

            // Act
            var result = ServiceUnderTest.Map(ninjaEntity);

            // Assert
            Assert.Same(expectedNinja, result);
        }

        [Fact]
        public void Map_NinjaEntityEnumerable_to_NinjaEnumerable_should_delegate_to_NinjaEntityEnumerableToNinjaMapper()
        {
            // Arrange
            var ninjaEntities = new List<NinjaEntity>();
            var expectedNinja = new List<Ninja>();
            NinjaEntityEnumerableToNinjaMapperMock
                .Setup(x => x.Map(ninjaEntities))
                .Returns(expectedNinja);

            // Act
            var result = ServiceUnderTest.Map(ninjaEntities);

            // Assert
            Assert.Same(expectedNinja, result);
        }
    }
}
