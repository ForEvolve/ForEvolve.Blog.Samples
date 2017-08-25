using ForEvolve.Azure.Storage.Table;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using ForEvolve.Blog.Samples.NinjaApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Repositories
{
    public class NinjaRepository : INinjaRepository
    {
        private readonly INinjaMappingService _ninjaMappingService;
        private readonly ITableStorageRepository<NinjaEntity> _ninjaEntityTableStorageRepository;

        public NinjaRepository(INinjaMappingService ninjaMappingService, ITableStorageRepository<NinjaEntity> ninjaEntityTableStorageRepository)
        {
            _ninjaMappingService = ninjaMappingService ?? throw new ArgumentNullException(nameof(ninjaMappingService));
            _ninjaEntityTableStorageRepository = ninjaEntityTableStorageRepository ?? throw new ArgumentNullException(nameof(ninjaEntityTableStorageRepository));
        }

        public async Task<Ninja> CreateAsync(Ninja ninja)
        {
            var entityToCreate = _ninjaMappingService.Map(ninja);
            var createdEntity = await _ninjaEntityTableStorageRepository.InsertOrReplaceAsync(entityToCreate);
            var createNinja = _ninjaMappingService.Map(createdEntity);
            return createNinja;
        }

        public async Task<Ninja> DeleteAsync(string clanName, string ninjaKey)
        {
            var deletedEntity = await _ninjaEntityTableStorageRepository.DeleteOneAsync(clanName, ninjaKey);
            var deletedNinja = _ninjaMappingService.Map(deletedEntity);
            return deletedNinja;
        }

        public async Task<IEnumerable<Ninja>> ReadAllAsync()
        {
            var entities = await _ninjaEntityTableStorageRepository.ReadAllAsync();
            var ninja = _ninjaMappingService.Map(entities);
            return ninja;
        }

        public async Task<IEnumerable<Ninja>> ReadAllInClanAsync(string clanName)
        {
            var entities = await _ninjaEntityTableStorageRepository.ReadPartitionAsync(clanName);
            var ninja = _ninjaMappingService.Map(entities);
            return ninja;
        }

        public async Task<Ninja> ReadOneAsync(string clanName, string ninjaKey)
        {
            var entity = await _ninjaEntityTableStorageRepository.ReadOneAsync(clanName, ninjaKey);
            var ninja = _ninjaMappingService.Map(entity);
            return ninja;
        }

        public async Task<Ninja> UpdateAsync(Ninja ninja)
        {
            var entityToUpdate = _ninjaMappingService.Map(ninja);
            var updatedEntity = await _ninjaEntityTableStorageRepository.InsertOrMergeAsync(entityToUpdate);
            var updatedNinja = _ninjaMappingService.Map(updatedEntity);
            return updatedNinja;
        }
    }
}
