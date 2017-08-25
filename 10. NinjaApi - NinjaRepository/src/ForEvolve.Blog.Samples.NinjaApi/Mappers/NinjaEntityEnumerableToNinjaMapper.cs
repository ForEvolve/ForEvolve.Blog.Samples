using ForEvolve.Blog.Samples.NinjaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForEvolve.Blog.Samples.NinjaApi.Mappers
{
    public class NinjaEntityEnumerableToNinjaMapper : IMapper<IEnumerable<NinjaEntity>, IEnumerable<Ninja>>
    {
        private readonly IMapper<NinjaEntity, Ninja> _ninjaEntityToNinjaMapper;

        public NinjaEntityEnumerableToNinjaMapper(IMapper<NinjaEntity, Ninja> ninjaEntityToNinjaMapper)
        {
            _ninjaEntityToNinjaMapper = ninjaEntityToNinjaMapper ?? throw new ArgumentNullException(nameof(ninjaEntityToNinjaMapper));
        }

        public IEnumerable<Ninja> Map(IEnumerable<NinjaEntity> entities)
        {
            var count = entities.Count();
            var all = new Ninja[count];
            for (int i = 0; i < count; i++)
            {
                var entity = entities.ElementAt(i);
                var ninja = _ninjaEntityToNinjaMapper.Map(entity);
                all[i] = ninja;
            }
            return all;
        }
    }
}
