using ForEvolve.Blog.Samples.NinjaApi.Models;
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
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Clan>), 200)]
        public Task<IActionResult> ReadAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
