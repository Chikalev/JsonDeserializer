using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TestApp.Core
{
    static class JsonWriter
    {
        private static readonly List<JsonConverter> _jsonConverters = new List<JsonConverter> {new StringEnumConverter()};

        public static string SerializeToJson<T>(this T inputData, bool dateTimeZoneUtcHandling, bool ignoreDefaultValues = false)
        {
            if (inputData == null)
                throw new ArgumentNullException(nameof(inputData));

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                //TypeNameHandling = TypeNameHandling.Auto,
                //TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                Converters = _jsonConverters,
                DateTimeZoneHandling = dateTimeZoneUtcHandling ? DateTimeZoneHandling.Utc : DateTimeZoneHandling.RoundtripKind,
                NullValueHandling = ignoreDefaultValues ? NullValueHandling.Ignore : NullValueHandling.Include
            };

            return JsonConvert.SerializeObject(inputData, Formatting.Indented, jsonSerializerSettings);
        }
    }
}
