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
using ForEvolve.Blog.Samples.NinjaApi.Mappers;
using ForEvolve.Azure.Storage.Table;

namespace ForEvolve.Blog.Samples.NinjaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Clans
            services.TryAddSingleton<IClanService, ClanService>();
            services.TryAddSingleton<IClanRepository, ClanRepository>();
            services.TryAddSingleton<IEnumerable<Clan>>(new Clan[]{
                new Clan { Name = "Iga" },
                new Clan { Name = "Kōga" },
            });

            // Mappers
            services.TryAddSingleton<IMapper<Ninja, NinjaEntity>, NinjaToNinjaEntityMapper>();
            services.TryAddSingleton<IMapper<NinjaEntity, Ninja>, NinjaEntityToNinjaMapper>();
            services.TryAddSingleton<IMapper<IEnumerable<NinjaEntity>, IEnumerable<Ninja>>, NinjaEntityEnumerableToNinjaMapper>();

            // Ninja
            services.TryAddSingleton<INinjaService, NinjaService>();
            services.TryAddSingleton<INinjaRepository, NinjaRepository>();
            services.TryAddSingleton<INinjaMappingService, NinjaMappingService>();

            // ForEvolve.Azure
            services.TryAddSingleton<ITableStorageRepository<NinjaEntity>, TableStorageRepository<NinjaEntity>>();
            services.TryAddSingleton<ITableStorageSettings>(x => new TableStorageSettings
            {
                AccountKey = Configuration.GetValue<string>("AzureTable:AccountKey"),
                AccountName = Configuration.GetValue<string>("AzureTable:AccountName"),
                TableName = Configuration.GetValue<string>("AzureTable:TableName")
            });

            // Others
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
