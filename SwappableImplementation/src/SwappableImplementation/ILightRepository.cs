using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwappableImplementation
{
    public interface ILightRepository
    {
        Task<IEnumerable<Light>> ReadAllAsync();
        Task<Light> ReadOneAsync(string key);
        Task<Light> CreateOrUpdateAsync(Light light);
    }
}
