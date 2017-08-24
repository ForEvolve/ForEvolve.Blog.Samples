using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ForEvolve.Blog.Samples.NinjaApi.Services;
using ForEvolve.Blog.Samples.NinjaApi.Repositories;
using ForEvolve.Blog.Samples.NinjaApi.Models;

namespace ForEvolve.Blog.Samples.NinjaApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IClanService, ClanService>();
            services.TryAddSingleton<IClanRepository, ClanRepository>();
            services.TryAddSingleton<IEnumerable<Clan>>(new Clan[]{
                new Clan { Name = "Iga" },
                new Clan { Name = "Kōga" },
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ApplicationServices.GetService<IDisposable>();
            app.UseMvc();
        }
    }
}
