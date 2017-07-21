using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace SwappableImplementation.Tests
{
    public abstract class BaseTest
    {
        private readonly HttpTestFixture _httpTestFixture;

        public BaseTest(HttpTestFixture httpTestFixture, Action<IServiceCollection> configureServices)
        {
            _httpTestFixture = httpTestFixture;
            _httpTestFixture.Initialize(configureServices);
        }

        [Fact]
        public virtual async Task TestLightServiceAsync()
        {
            await _httpTestFixture.TestLightServiceAsync();
        }

        [Fact]
        public virtual async Task TestLightEnpointsOverHttpAsync()
        {
            await _httpTestFixture.TestLightEnpointsOverHttpAsync();
        }
    }
}
