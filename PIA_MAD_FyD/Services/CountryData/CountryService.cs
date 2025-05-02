using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace PIA_MAD_FyD.Services.CountryData
{
    class CountryService
    {
        public static List<Country> LoadCountries(string jsonFilePath)
        {
            // Usamos StreamReader para asegurarnos de que la codificación sea UTF-8
            string json;
            using (StreamReader reader = new StreamReader(jsonFilePath, Encoding.UTF8))
            {
                json = reader.ReadToEnd();
            }

            // Opciones para deserializar el JSON
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserializar el JSON
            return JsonSerializer.Deserialize<List<Country>>(json, options);
        }

    }
}
