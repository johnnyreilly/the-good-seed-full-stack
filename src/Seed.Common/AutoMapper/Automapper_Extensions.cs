using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;

namespace Seed.Common.AutoMapper
{
    public static class Automapper_Extensions
    {
        public static void AddProfiles<T>(this IMapperConfigurationExpression cfg)
        {
            cfg.AddProfiles(typeof(T).GetTypeInfo().Assembly);
        }

        public static IEnumerable<TMapTo> MapAll<TMapTo>(this IMapper mapper, IEnumerable collection)
        {
            foreach (var item in collection)
                yield return mapper.Map<TMapTo>(item);
        }
    }
}