using ForEvolve.Blog.Samples.NinjaApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ForEvolve.Blog.Samples.NinjaApi.Mappers
{
    public class NinjaToNinjaEntityMapperTest
    {
        protected NinjaToNinjaEntityMapper MapperUnderTest { get; }

        public NinjaToNinjaEntityMapperTest()
        {
            MapperUnderTest = new NinjaToNinjaEntityMapper();
        }

        public class Map : NinjaToNinjaEntityMapperTest
        {
            [Fact]
            public void Should_return_a_well_formatted_entity()
            {
                // Arrange
                var ninja = new Ninja
                {
                    Key = "Some key",
                    Name = "Some name",
                    Level = 45,
                    Clan = new Clan { Name = "Super clan" }
                };

                // Act
                var result = MapperUnderTest.Map(ninja);

                // Assert
                Assert.Equal("Some key", result.RowKey);
                Assert.Equal("Some name", result.Name);
                Assert.Equal(45, result.Level);
                Assert.Equal("Super clan", result.PartitionKey);
            }
        }
    }
}
