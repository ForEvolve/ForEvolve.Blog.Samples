using ForEvolve.Blog.Samples.NinjaApi.Models;
using ForEvolve.Blog.Samples.NinjaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Services
{
    public class ClanService : IClanService
    {
        private IClanRepository _clanRepository;

        public ClanService(IClanRepository clanRepository)
        {
            _clanRepository = clanRepository ?? throw new ArgumentNullException(nameof(clanRepository));
        }

        public Task<IEnumerable<Clan>> ReadAllAsync()
        {
            return _clanRepository.ReadAllAsync();
        }

        public Task<Clan> ReadOneAsync(string clanName)
        {
            return _clanRepository.ReadOneAsync(clanName);
        }

        public async Task<bool> IsClanExistsAsync(string clanName)
        {
            var clan = await _clanRepository.ReadOneAsync(clanName);
            return clan != null;
        }

        public Task<Clan> CreateAsync(Clan clan)
        {
            throw new NotSupportedException();
        }

        public Task<Clan> UpdateAsync(Clan clan)
        {
            throw new NotSupportedException();
        }

        public Task<Clan> DeleteAsync(string clanName)
        {
            throw new NotSupportedException();
        }
    }
}
