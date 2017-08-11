using System.Linq;
using System.Collections.Generic;
using ForEvolve.Azure.Storage.Table;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using System.Threading.Tasks;
using System;

namespace ForEvolve.Blog.Samples.NinjaApi.IntegrationTests
{
    public class NinjaEntityTableStorageRepositoryFake : ITableStorageRepository<NinjaEntity>
    {
        private List<NinjaEntity> InternalEntities { get; }
        private List<NinjaEntity> MergedEntities { get; }

        public NinjaEntityTableStorageRepositoryFake()
        {
            InternalEntities = new List<NinjaEntity>();
            MergedEntities = new List<NinjaEntity>();
        }

        public async Task<NinjaEntity> InsertOrMergeAsync(NinjaEntity item)
        {
            var current = await ReadOneAsync(item.PartitionKey, item.RowKey);
            if (current != null)
            {
                current.Level = item.Level;
                current.Name = item.Name;
                MergedEntities.Add(current);
                return current;
            }
            InternalEntities.Add(item);
            return item;
        }

        public async Task<NinjaEntity> InsertOrReplaceAsync(NinjaEntity item)
        {
            var current = await ReadOneAsync(item.PartitionKey, item.RowKey);
            if(current != null)
            {
                InternalEntities.Remove(current);
            }
            InternalEntities.Add(item);
            return item;
        }

        public Task<IEnumerable<NinjaEntity>> ReadAllAsync()
        {
            return Task.FromResult(InternalEntities.AsEnumerable());
        }

        public async Task<NinjaEntity> ReadOneAsync(string partitionKey, string rowkey)
        {
            var item = (await ReadPartitionAsync(partitionKey))
                .FirstOrDefault(x => x.RowKey == rowkey);
            return item;
        }

        public Task<IEnumerable<NinjaEntity>> ReadPartitionAsync(string partitionKey)
        {
            var items = InternalEntities
                .Where(x => x.PartitionKey == partitionKey);
            return Task.FromResult(items);
        }

        public async Task<NinjaEntity> RemoveAsync(string partitionKey, string rowkey)
        {
            var item = await ReadOneAsync(partitionKey, rowkey);
            InternalEntities.Remove(item);
            return item;
        }

        public async Task<IEnumerable<NinjaEntity>> RemoveAsync(string partitionKey)
        {
            var items = await ReadPartitionAsync(partitionKey);
            InternalEntities.RemoveAll(x => x.PartitionKey == partitionKey);
            return items;
        }

        internal bool HasBeenMerged(NinjaEntity item)
        {
            var mergedItem = MergedEntities.FirstOrDefault(x => x.PartitionKey == item.PartitionKey && x.RowKey == item.RowKey);
            return mergedItem != null;
        }

        internal void AddRange(IEnumerable<NinjaEntity> ninjaCollection)
        {
            InternalEntities.AddRange(ninjaCollection);
        }

        internal int EntityCount => InternalEntities.Count;

        internal NinjaEntity ElementAt(int index)
        {
            return InternalEntities[index];
        }
    }
}
