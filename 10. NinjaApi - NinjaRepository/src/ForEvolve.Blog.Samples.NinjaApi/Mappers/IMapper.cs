namespace ForEvolve.Blog.Samples.NinjaApi.Mappers
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource entity);
    }
}
