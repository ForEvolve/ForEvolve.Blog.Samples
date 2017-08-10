using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;
using System.Threading.Tasks;
using ForEvolve;
using System.Collections.Generic;

namespace SwappableImplementation.Tests
{
    public class HttpTestFixture : IDisposable
    {
        private TestServer _server;
        private HttpClient _client;

        public void Initialize(Action<IServiceCollection> configureServices)
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                // The EFRepoTest & InMemoryTest constructor Action is passed to here.
                // This allow to create a different DI container object graph for each test class.
                .ConfigureServices(configureServices) 
                .UseStartup<Startup>();

            _server = new TestServer(builder);
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("http://localhost");
        }

        public async Task TestLightServiceAsync()
        {
            // Get an implementation of ILightService (configured in the ConfigureServices method)
            // This implementation will change depending on the test class.
            // In EFRepoTest it will be an EntityFrameworkLightRepository instance
            // while in InMemoryTest it will be an InMemoryLightRepository instance.
            //
            // If you put a breakpoint here, you will be able to observe the implementation
            // and explore the result of the operations.
            var lightService = _server.Host.Services.GetService<ILightService>();

            // Creating some lights
            var light1 = await lightService.CreateAsync(new Light { Name = "Light 1", State = LightState.Off });
            var light2 = await lightService.CreateAsync(new Light { Name = "Light 2", State = LightState.On });
            var light3 = await lightService.CreateAsync(new Light { Name = "Light 3", State = LightState.Off });

            // Toggleling light 3 on
            await lightService.ToggleAsync(light3);

            // Getting all lights from the service and make sure they are all there
            // with the right LightState.
            var all = await lightService.AllAsync();

            Assert.Collection(all,
                light => Assert.Same(light1, light),
                light => Assert.Same(light2, light),
                light => Assert.Same(light3, light)
            );
            Assert.Collection(all,
                light => Assert.Equal(LightState.Off, light.State),
                light => Assert.Equal(LightState.On, light.State),
                light => Assert.Equal(LightState.On, light.State)
            );
        }

        public async Task TestLightEnpointsOverHttpAsync()
        {
            // Create lights
            var createLight1Response = await _client.PostAsync(
                "lights/create", 
                new Light { Name = "Light 1", State = LightState.Off }.ToJsonHttpContent()
            );
            var createLight2Response = await _client.PostAsync(
                "lights/create", 
                new Light { Name = "Light 2", State = LightState.On }.ToJsonHttpContent()
            );
            var createLight3Response = await _client.PostAsync(
                "lights/create", 
                new Light { Name = "Light 3", State = LightState.Off }.ToJsonHttpContent()
            );

            // Make sure the HTTP request were successful (status 2XX).
            Assert.True(createLight1Response.IsSuccessStatusCode);
            Assert.True(createLight2Response.IsSuccessStatusCode);
            Assert.True(createLight3Response.IsSuccessStatusCode);

            // Convert the JSON responses to object using ForEvolve.AspNetCore extension method.
            var light1 = await createLight1Response.Content.ReadAsJsonObjectAsync<Light>();
            var light2 = await createLight2Response.Content.ReadAsJsonObjectAsync<Light>();
            var light3 = await createLight3Response.Content.ReadAsJsonObjectAsync<Light>();

            // Make sure they are not null
            Assert.NotNull(light1);
            Assert.NotNull(light2);
            Assert.NotNull(light3);

            // Toggle on light #3
            var toggleResponse = await _client.SendAsync(
                new HttpRequestMessage(new HttpMethod("PATCH"), "lights/toggle")
                {
                    Content = light3.ToJsonHttpContent()
                }
            );
            Assert.True(toggleResponse.IsSuccessStatusCode);

            // Getting all lights from the service and make sure they are all there
            // with the right LightState.
            var readAllResponse = await _client.GetAsync("lights/all");
            Assert.True(readAllResponse.IsSuccessStatusCode);

            var all = await readAllResponse.Content.ReadAsJsonObjectAsync<IEnumerable<Light>>();
            Assert.NotNull(all);

            Assert.Collection(all,
                light => {
                    Assert.Equal(light1.Key, light.Key);
                    Assert.Equal(light1.Name, light.Name);
                    Assert.Equal(LightState.Off, light.State);
                },
                light => {
                    Assert.Equal(light2.Key, light.Key);
                    Assert.Equal(light2.Name, light.Name);
                    Assert.Equal(LightState.On, light.State);
                },
                light => {
                    Assert.Equal(light3.Key, light.Key);
                    Assert.Equal(light3.Name, light.Name);
                    Assert.Equal(LightState.On, light.State);
                }
            );
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                    _server.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
