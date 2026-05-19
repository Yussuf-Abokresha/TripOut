using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.Application.Serialization
{
    public static class SerializerSettings
    {
        public static readonly JsonSerializerSettings Instance = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
            Converters = { new JsonElementConverter() }
        };
    }

}