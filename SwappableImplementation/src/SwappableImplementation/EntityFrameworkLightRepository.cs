using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwappableImplementation
{
    public class EntityFrameworkLightRepository : ILightRepository
    {
        private readonly LightContext _lightContext;

        public EntityFrameworkLightRepository(LightContext lightContext)
        {
            _lightContext = lightContext ?? throw new ArgumentNullException(nameof(lightContext));
        }

        public async Task<Light> CreateOrUpdateAsync(Light light)
        {
            var existingLight = await _lightContext.Lights.FindAsync(light.Key);
            EntityEntry<Light> entry;
            if (existingLight == null)
            {
                entry = await _lightContext.Lights.AddAsync(light);
            }
            else
            {
                try
                {
                    entry = _lightContext.Lights.Update(light);
                }
                catch (Exception ex)
                {
                    existingLight.Name = light.Name;
                    existingLight.State = light.State;
                    entry = _lightContext.Entry(existingLight);
                    entry.State = EntityState.Modified;
                }
            }
            await _lightContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<IEnumerable<Light>> ReadAllAsync()
        {
            var all = await _lightContext.Lights.ToArrayAsync();
            return all;
        }

        public async Task<Light> ReadOneAsync(string key)
        {
            var light = await _lightContext.Lights.FindAsync(key);
            return light;
        }
    }
}
