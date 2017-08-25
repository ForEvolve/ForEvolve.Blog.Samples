using ForEvolve.Blog.Samples.NinjaApi.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Mappers
{
    public class EnumerableMapperTest
    {
        public class Map : EnumerableMapperTest
        {
            public class NinjaEntity2Ninja : Map
            {
                protected EnumerableMapper<NinjaEntity, Ninja> MapperUnderTest { get; }
                protected Mock<IMapper<NinjaEntity, Ninja>> NinjaEntityToNinjaMapperMock { get; }

                public NinjaEntity2Ninja()
                {
                    NinjaEntityToNinjaMapperMock = new Mock<IMapper<NinjaEntity, Ninja>>();
                    MapperUnderTest = new EnumerableMapper<NinjaEntity, Ninja>(NinjaEntityToNinjaMapperMock.Object);
                }

                [Fact]
                public void Should_delegate_mapping_to_the_single_entity_mapper()
                {
                    // Arrange
                    var ninja1 = new NinjaEntity();
                    var ninja2 = new NinjaEntity();
                    var ninjaEntities = new List<NinjaEntity> { ninja1, ninja2 };

                    NinjaEntityToNinjaMapperMock
                        .Setup(x => x.Map(It.IsAny<NinjaEntity>()))
                        .Returns(new Ninja())
                        .Verifiable();

                    // Act
                    var result = MapperUnderTest.Map(ninjaEntities);

                    // Assert
                    NinjaEntityToNinjaMapperMock.Verify(x => x.Map(ninja1), Times.Once);
                    NinjaEntityToNinjaMapperMock.Verify(x => x.Map(ninja2), Times.Once);
                }
            }
        }
    }
}
