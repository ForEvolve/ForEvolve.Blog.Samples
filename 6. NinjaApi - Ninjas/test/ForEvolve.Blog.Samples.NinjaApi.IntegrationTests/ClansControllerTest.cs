using System;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ForEvolve.Blog.Samples.NinjaApi.IntegrationTests
{
    public class ClansControllerTest : BaseHttpTest
    {
        public class ReadAllAsync : ClansControllerTest
        {
            private IEnumerable<Clan> Clans => new Clan[] {
                new Clan { Name = "My clan" },
                new Clan { Name = "Your clan" },
                new Clan { Name = "His clan" }
            };

            protected override void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton(Clans);
            }

            [Fact]
            public async Task Should_return_the_default_clans()
            {
                // Arrange
                var expectedNumberOfClans = Clans.Count();

                // Act
                var result = await Client.GetAsync("v1/clans");

                // Assert
                result.EnsureSuccessStatusCode();
                var clans = await result.Content.ReadAsJsonObjectAsync<Clan[]>();
                Assert.NotNull(clans);
                Assert.Equal(expectedNumberOfClans, clans.Length);
                Assert.Collection(clans,
                    clan => Assert.Equal(Clans.ElementAt(0).Name, clans[0].Name),
                    clan => Assert.Equal(Clans.ElementAt(1).Name, clans[1].Name),
                    clan => Assert.Equal(Clans.ElementAt(2).Name, clans[2].Name)
                );
            }
        }
    }
}
