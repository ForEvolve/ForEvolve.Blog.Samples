using ForEvolve.Blog.Samples.NinjaApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi.Services
{
    public class ClanService : IClanService
    {
        public Task<Clan> CreateAsync(Clan clan)
        {
            throw new NotImplementedException();
        }

        public Task<Clan> DeleteAsync(string clanName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsClanExistsAsync(string clanName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Clan>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Clan> ReadOneAsync(string clanName)
        {
            throw new NotImplementedException();
        }

        public Task<Clan> UpdateAsync(Clan clan)
        {
            throw new NotImplementedException();
        }
    }
}
