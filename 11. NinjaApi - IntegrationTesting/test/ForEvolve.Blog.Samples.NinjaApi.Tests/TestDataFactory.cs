using ForEvolve.Blog.Samples.NinjaApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForEvolve.Blog.Samples.NinjaApi
{
    public static class TestDataFactory
    {
        public static NinjaEntity CreateEntity(string partitionKey, string rowKey, string name, int level)
        {
            var entity = new NinjaEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Name = name,
                Level = level
            };
            return entity;
        }

        public static Ninja CreateNinja(string clanName, string key, string name, int level)
        {
            var entity = new Ninja
            {
                Key = key,
                Clan = new Clan { Name = clanName },
                Name = name,
                Level = level
            };
            return entity;
        }
    }
}
