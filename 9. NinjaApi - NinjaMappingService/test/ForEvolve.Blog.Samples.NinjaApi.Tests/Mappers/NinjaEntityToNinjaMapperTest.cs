using ForEvolve.Blog.Samples.NinjaApi.Models;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Mappers
{
    public class NinjaEntityToNinjaMapperTest
    {
        protected NinjaEntityToNinjaMapper MapperUnderTest { get; }

        public NinjaEntityToNinjaMapperTest()
        {
            MapperUnderTest = new NinjaEntityToNinjaMapper();
        }

        public class Map : NinjaEntityToNinjaMapperTest
        {
            [Fact]
            public void Should_return_a_well_formatted_ninja()
            {
                // Arrange
                var entity = new NinjaEntity
                {
                    Level = 10,
                    Name = "Some fake name",
                    PartitionKey = "Some clan name",
                    RowKey = "Some ninja key"
                };

                // Act
                var result = MapperUnderTest.Map(entity);

                // Assert
                Assert.Equal(10, result.Level);
                Assert.Equal("Some fake name", result.Name);
                Assert.NotNull(result.Clan);
                Assert.Equal("Some clan name", result.Clan.Name);
                Assert.Equal("Some ninja key", result.Key);
            }
        }
    }
}
