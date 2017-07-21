using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace SwappableImplementation.Tests
{
    public class InMemoryTest : BaseTest, IClassFixture<HttpTestFixture>
    {
        public InMemoryTest(HttpTestFixture httpTestFixture)
            : base(httpTestFixture, services =>
            {
                // Registering InMemoryLightRepository as the ILightRepository implementation
                services.AddSingleton<ILightRepository, InMemoryLightRepository>();

                // Common services
                services.AddSingleton<ILightService, LightService>();
                services.AddRouting();
            })
        {
        }
    }
}
