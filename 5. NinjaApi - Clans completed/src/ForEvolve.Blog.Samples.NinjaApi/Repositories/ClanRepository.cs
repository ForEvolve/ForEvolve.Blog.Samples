using ForEvolve.Blog.Samples.NinjaApi.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Repositories
{
    public class ClanRepository : IClanRepository
    {
        private readonly List<Clan> _clans;

        public ClanRepository(IEnumerable<Clan> clans)
        {
            if (clans == null) { throw new ArgumentNullException(nameof(clans)); }
            _clans = new List<Clan>(clans);
        }

        public Task<IEnumerable<Clan>> ReadAllAsync()
        {
            return Task.FromResult(_clans.AsEnumerable());
        }

        public Task<Clan> ReadOneAsync(string clanName)
        {
            var clan = _clans.FirstOrDefault(c => c.Name == clanName);
            return Task.FromResult(clan);
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
