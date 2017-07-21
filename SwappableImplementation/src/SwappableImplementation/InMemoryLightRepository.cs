using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SwappableImplementation
{
    public class InMemoryLightRepository : ILightRepository
    {
        private List<Light> InternalLights { get; } = new List<Light>();

        public async Task<Light> CreateOrUpdateAsync(Light light)
        {
            var existingLight = await ReadOneAsync(light.Key);
            if (existingLight != null)
            {
                InternalLights.Remove(existingLight);
            }
            InternalLights.Add(light);
            return await Task.FromResult(light);
        }

        public async Task<IEnumerable<Light>> ReadAllAsync()
        {
            var all = new ReadOnlyCollection<Light>(InternalLights);
            return await Task.FromResult(all);
        }

        public async Task<Light> ReadOneAsync(string key)
        {
            var light = InternalLights.FirstOrDefault(x => x.Key == key);
            return await Task.FromResult(light);
        }
    }
}
