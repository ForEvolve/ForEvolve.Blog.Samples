using ForEvolve.Blog.Samples.NinjaApi.Mappers;
using ForEvolve.Blog.Samples.NinjaApi.Models;
using System;
using System.Collections.Generic;

namespace ForEvolve.Blog.Samples.NinjaApi.Services
{
    public class NinjaMappingService : INinjaMappingService
    {
        private readonly IMapper<Ninja, NinjaEntity> _ninjaToNinjaEntityMapper;
        private readonly IMapper<NinjaEntity, Ninja> _ninjaEntityToNinjaMapper;
        private readonly IMapper<IEnumerable<NinjaEntity>, IEnumerable<Ninja>> _ninjaEntityEnumerableToNinjaMapper;

        public NinjaMappingService(
            IMapper<Ninja, NinjaEntity> ninjaToNinjaEntityMapper, 
            IMapper<NinjaEntity, Ninja> ninjaEntityToNinjaMapper, 
            IMapper<IEnumerable<NinjaEntity>, IEnumerable<Ninja>> ninjaEntityEnumerableToNinjaMapper
        )
        {
            _ninjaToNinjaEntityMapper = ninjaToNinjaEntityMapper ?? throw new ArgumentNullException(nameof(ninjaToNinjaEntityMapper));
            _ninjaEntityToNinjaMapper = ninjaEntityToNinjaMapper ?? throw new ArgumentNullException(nameof(ninjaEntityToNinjaMapper));
            _ninjaEntityEnumerableToNinjaMapper = ninjaEntityEnumerableToNinjaMapper ?? throw new ArgumentNullException(nameof(ninjaEntityEnumerableToNinjaMapper));
        }

        public NinjaEntity Map(Ninja entity)
        {
            return _ninjaToNinjaEntityMapper.Map(entity);
        }

        public Ninja Map(NinjaEntity entity)
        {
            return _ninjaEntityToNinjaMapper.Map(entity);
        }

        public IEnumerable<Ninja> Map(IEnumerable<NinjaEntity> entities)
        {
            return _ninjaEntityEnumerableToNinjaMapper.Map(entities);
        }
    }
}
