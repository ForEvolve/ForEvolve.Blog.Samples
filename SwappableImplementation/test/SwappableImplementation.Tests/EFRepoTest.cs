using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SwappableImplementation.Tests
{
    public class EFRepoTest : BaseTest, IClassFixture<HttpTestFixture>
    {
        private static string DatabaseName => $"{nameof(EFRepoTest)}Database";

        public EFRepoTest(HttpTestFixture httpTestFixture)
            : base(httpTestFixture, services =>
            {
                // Registering EntityFrameworkLightRepository as the ILightRepository implementation
                services.AddDbContext<LightContext>(opt => opt.UseInMemoryDatabase(DatabaseName));
                services.AddSingleton<ILightRepository, EntityFrameworkLightRepository>();

                // Common services
                services.AddSingleton<ILightService, LightService>();
                services.AddRouting();
            })
        {
        }
    }
}
