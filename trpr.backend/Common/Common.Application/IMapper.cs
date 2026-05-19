namespace Common.Application;

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
}