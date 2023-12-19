using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logger
{
    public static class Extentions
    {
        public static string GetStringFromConfig(this JToken configObject, string key)
        {
            try
            {
                JToken token = configObject.SelectToken(key);
                return token?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving value for key '{key}': {ex.Message}");
                return null;
            }
        }
    }
}
