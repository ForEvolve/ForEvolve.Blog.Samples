using ForEvolve.Blog.Samples.NinjaApi.Models;
using ForEvolve.Blog.Samples.NinjaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Services
{
    public class NinjaService : INinjaService
    {
        private readonly INinjaRepository _ninjaRepository;
        private readonly IClanService _clanService;

        public NinjaService(INinjaRepository ninjaRepository, IClanService clanService)
        {
            _ninjaRepository = ninjaRepository ?? throw new ArgumentNullException(nameof(ninjaRepository));
            _clanService = clanService ?? throw new ArgumentNullException(nameof(clanService));
        }

        public async Task<Ninja> CreateAsync(Ninja ninja)
        {
            await EnforceClanExistenceAsync(ninja.Clan.Name);
            var createdNinja = await _ninjaRepository.CreateAsync(ninja);
            return createdNinja;
        }

        public async Task<Ninja> DeleteAsync(string clanName, string ninjaKey)
        {
            await EnforceNinjaExistenceAsync(clanName, ninjaKey);
            var deletedNinja = await _ninjaRepository.DeleteAsync(clanName, ninjaKey);
            return deletedNinja;
        }

        public Task<IEnumerable<Ninja>> ReadAllAsync()
        {
            return _ninjaRepository.ReadAllAsync();
        }

        public async Task<IEnumerable<Ninja>> ReadAllInClanAsync(string clanName)
        {
            await EnforceClanExistenceAsync(clanName);
            return await _ninjaRepository.ReadAllInClanAsync(clanName);
        }

        public async Task<Ninja> ReadOneAsync(string clanName, string ninjaKey)
        {
            var ninja = await EnforceNinjaExistenceAsync(clanName, ninjaKey);
            return ninja;
        }

        public async Task<Ninja> UpdateAsync(Ninja ninja)
        {
            await EnforceClanExistenceAsync(ninja.Clan.Name);
            await EnforceNinjaExistenceAsync(ninja.Clan.Name, ninja.Key);
            var updatedNinja = await _ninjaRepository.UpdateAsync(ninja);
            return updatedNinja;
        }

        private async Task EnforceClanExistenceAsync(string clanName)
        {
            var clanExist = await _clanService.IsClanExistsAsync(clanName);
            if (!clanExist)
            {
                throw new ClanNotFoundException(clanName);
            }
        }

        private async Task<Ninja> EnforceNinjaExistenceAsync(string clanName, string ninjaKey)
        {
            var remoteNinja = await _ninjaRepository.ReadOneAsync(clanName, ninjaKey);
            if (remoteNinja == null)
            {
                throw new NinjaNotFoundException(clanName, ninjaKey);
            }
            return remoteNinja;
        }
    }
}
