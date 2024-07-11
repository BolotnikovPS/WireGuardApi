namespace WireGuardApi.Domain.Abstractions;

/// <summary>
/// Скрытие интерфейса IMapper
/// </summary>
public interface IMap
{
    /// <summary>
    /// Скрытый вызов Mapper
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    TDestination Map<TDestination>(object source);

    /// <summary>
    /// Скрытый вызов Mapper
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>
    /// Скрытый вызов Mapper
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

    /// <summary>
    /// Скрытый вызов Mapper
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sourceType"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    object Map(object source, Type sourceType, Type destinationType);

    /// <summary>
    /// Скрытый вызов Mapper
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="sourceType"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    object Map(object source, object destination, Type sourceType, Type destinationType);
}