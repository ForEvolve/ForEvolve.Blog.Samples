using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwappableImplementation
{
    public class LightService : ILightService
    {
        private readonly ILightRepository _lightRepository;
        public LightService(ILightRepository lightRepository)
        {
            _lightRepository = lightRepository ?? throw new ArgumentNullException(nameof(lightRepository));
        }

        public async Task<IEnumerable<Light>> AllAsync()
        {
            return await _lightRepository.ReadAllAsync();
        }

        public async Task<Light> CreateAsync(Light light)
        {
            if (string.IsNullOrWhiteSpace(light.Key))
            {
                light.Key = Guid.NewGuid().ToString();
            }
            return await _lightRepository.CreateOrUpdateAsync(light);
        }

        public async Task ToggleAsync(Light light)
        {
            light.State = light.State == LightState.On ? LightState.Off : LightState.On;
            await _lightRepository.CreateOrUpdateAsync(light);
        }
    }
}
