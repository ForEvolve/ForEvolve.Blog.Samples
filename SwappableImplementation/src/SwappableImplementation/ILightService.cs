using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwappableImplementation
{
    public interface ILightService
    {
        Task<IEnumerable<Light>> AllAsync();
        Task<Light> CreateAsync(Light light);
        Task ToggleAsync(Light light);
    }
}
