using ForEvolve.Blog.Samples.NinjaApi.Models;
using ForEvolve.Blog.Samples.NinjaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Controllers
{
    [Route("v1/[controller]")]
    public class ClansController : Controller
    {
        private readonly IClanService _clanService;

        public ClansController(IClanService clanService)
        {
            _clanService = clanService ?? throw new ArgumentNullException(nameof(clanService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Clan>), 200)]
        public async Task<IActionResult> ReadAllAsync()
        {
            var allClans = await _clanService.ReadAllAsync();
            return Ok(allClans);
        }
    }
}
