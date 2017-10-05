using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Seed.Common.Extensions
{
    public static class JContainer_Extensions
    {
        public static JContainer ConvertPascalToCamelCase(this JContainer input)
        {
            if (input == null)
                return null;

            if (input is JObject)
            {
                var result = new JObject();

                foreach (var property in ((JObject) input).Properties())
                {
                    var propertyValue = property.Value;

                    if (propertyValue is JObject)
                        result.Add(property.Name.FirstCharToLower(),
                            (propertyValue as JContainer).ConvertPascalToCamelCase());
                    else if (propertyValue is JArray)
                        result.Add(property.Name.FirstCharToLower(),
                            (propertyValue as JContainer).ConvertPascalToCamelCase());
                    else
                        result.Add(property.Name.FirstCharToLower(), propertyValue);
                }

                return result;
            }
            if (input is JArray)
            {
                var result = new JArray();

                foreach (var child in (input as JArray).Children())
                    result.Add((child as JContainer).ConvertPascalToCamelCase());

                return result;
            }

            throw new Exception(
                $"Unhandled case of applying ConvertPascalToCamelCase to:\n{input.ToString(Formatting.Indented)}");
        }
    }
}