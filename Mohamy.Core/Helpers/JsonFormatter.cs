﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mohamy.Core.Helpers
{
    public static class JsonFormatter
    {
        public static string FormatJson(string json)
        {
            try
            {
                var parsedJson = JToken.Parse(json);
                return parsedJson.ToString(Formatting.Indented);
            }
            catch
            {
                return json; // Return the original JSON if parsing fails
            }
        }
    }
}
