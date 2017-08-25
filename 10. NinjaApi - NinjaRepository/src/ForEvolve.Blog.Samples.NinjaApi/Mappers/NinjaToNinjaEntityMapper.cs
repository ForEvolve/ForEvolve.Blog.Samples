using ForEvolve.Blog.Samples.NinjaApi.Models;

namespace ForEvolve.Blog.Samples.NinjaApi.Mappers
{
    public class NinjaToNinjaEntityMapper : IMapper<Ninja, NinjaEntity>
    {
        public NinjaEntity Map(Ninja ninja)
        {
            var entity = new NinjaEntity
            {
                PartitionKey = ninja.Clan.Name,
                RowKey = ninja.Key,
                Name = ninja.Name,
                Level = ninja.Level
            };
            return entity;
        }
    }
}
