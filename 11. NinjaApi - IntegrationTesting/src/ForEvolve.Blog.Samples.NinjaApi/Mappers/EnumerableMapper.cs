using ForEvolve.Blog.Samples.NinjaApi.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForEvolve.Blog.Samples.NinjaApi.Mappers
{
    public class EnumerableMapper<TSource, TDestination> : IMapper<IEnumerable<TSource>, IEnumerable<TDestination>>
    {
        private readonly IMapper<TSource, TDestination> _singleMapper;

        public EnumerableMapper(IMapper<TSource, TDestination> singleMapper)
        {
            _singleMapper = singleMapper ?? throw new ArgumentNullException(nameof(singleMapper));
        }

        public IEnumerable<TDestination> Map(IEnumerable<TSource> source)
        {
            var count = source.Count();
            var destination = new TDestination[count];
            for (int i = 0; i < count; i++)
            {
                var currentSource = source.ElementAt(i);
                var currentDestination = _singleMapper.Map(currentSource);
                destination[i] = currentDestination;
            }
            return destination;
        }
    }
}
