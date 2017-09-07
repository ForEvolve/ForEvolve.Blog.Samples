using ForEvolve.Blog.Samples.NinjaApi.Mappers;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Services
{
    public interface INinjaMappingService : IMapper<Ninja, NinjaEntity>, IMapper<NinjaEntity, Ninja>, IMapper<IEnumerable<NinjaEntity>, IEnumerable<Ninja>>
    {
    }
}

