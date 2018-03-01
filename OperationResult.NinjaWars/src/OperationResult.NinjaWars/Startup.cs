using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace OperationResult.NinjaWars
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRouter(builder =>
            {
                var clanService = new ClanWarService();
                builder.MapGet("api/clans/{clanId}/warstatus", async (request, response, data) =>
                {
                    // Read param
                    var clanId = data.Values["clanId"].ToString();

                    // Execute operation
                    var result = clanService.ReadAllWarStatusOf(clanId);

                    // Handle the result
                    string jsonResponse;
                    if (result.IsSuccessful)
                    {
                        // Do something with the value
                        jsonResponse = JsonConvert.SerializeObject(result.Value);
                    }
                    else
                    {
                        // Do somthing with the error
                        jsonResponse = JsonConvert.SerializeObject(new { error = result.Error });
                    }
                    await response.WriteAsync(jsonResponse);
                });
            });
        }
    }

    public class ClanWarService
    {
        public ReadWarStatusOperationResult ReadAllWarStatusOf(string clanId)
        {
            if (clanId == "c810e13c-1083-4f39-aebc-e150c82dc770")
            {
                return new ReadWarStatusOperationResult
                {
                    Value = new WarStatus[]
                    {
                        new WarStatus
                        {
                            TargetClanId = "6155d646-98c9-492e-a17d-335b1b69898e",
                            IsAtWar = true
                        },
                        new WarStatus
                        {
                            TargetClanId = "f43d6384-4419-4e41-b561-9f96e0620779",
                            IsAtWar = true
                        },
                        new WarStatus
                        {
                            TargetClanId = "84712492-1b0a-40b7-b1cd-20661fc6b6b6",
                            IsAtWar = true
                        },
                        new WarStatus
                        {
                            TargetClanId = "16446317-3CCE-431E-AEBE-8A5511312594",
                            IsAtWar = false
                        },
                        new WarStatus
                        {
                            TargetClanId = "8C0AE03F-202D-4352-BD59-2F794779F3E9",
                            IsAtWar = false
                        },
                    }
                };
            }
            else
            {
                return new ReadWarStatusOperationResult
                {
                    Error = $"Clan {clanId} not found"
                };
            }
        }
    }

    public class ReadWarStatusOperationResult
    {
        [JsonProperty("successful")]
        public bool IsSuccessful => string.IsNullOrWhiteSpace(Error);

        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public WarStatus[] Value { get; set; }

        [JsonProperty("error", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Error { get; set; }
    }

    public class WarStatus
    {
        [JsonProperty("targetClanId")]
        public string TargetClanId { get; set; }

        [JsonProperty("isAtWar")]
        public bool IsAtWar { get; set; }
    }
}
