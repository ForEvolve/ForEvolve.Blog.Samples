using ForEvolve.Blog.Samples.NinjaApi.Models;

namespace ForEvolve.Blog.Samples.NinjaApi.Mappers
{
    public class NinjaEntityToNinjaMapper : IMapper<NinjaEntity, Ninja>
    { 
        public Ninja Map(NinjaEntity entity)
        {
            var ninja = new Ninja
            {
                Key = entity.RowKey,
                Clan = new Clan { Name = entity.PartitionKey },
                Level = entity.Level,
                Name = entity.Name
            };
            return ninja;
        }
    }
}
