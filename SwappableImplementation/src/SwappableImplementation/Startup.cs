using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace SwappableImplementation
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouter(builder =>
            {
                var lightService = builder.ServiceProvider.GetService<ILightService>();
                var serializer = new JsonSerializer();

                // Read all
                builder.MapGet("lights/all", async (request, response, data) =>
                {
                    var all = await lightService.AllAsync();
                    await WriteJsonAsync(response, all);
                });

                // Toggle state
                builder.MapVerb("PATCH", "lights/toggle", async (request, response, data) =>
                {
                    using (var sr = new StreamReader(request.Body))
                    {
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            var light = serializer.Deserialize<Light>(jsonTextReader);
                            await lightService.ToggleAsync(light);
                        }
                    }
                });

                // Create
                builder.MapPost("lights/create", async (request, response, data) =>
                {
                    using (var sr = new StreamReader(request.Body))
                    {
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            var light = serializer.Deserialize<Light>(jsonTextReader);
                            var createdLight = await lightService.CreateAsync(light);
                            await WriteJsonAsync(response, createdLight);
                        }
                    }
                });
            });
        }

        private static async Task WriteJsonAsync<T>(HttpResponse response, T all)
        {
            var serializedAll = JsonConvert.SerializeObject(all);
            await response.WriteAsync(serializedAll);
        }
    }
}
