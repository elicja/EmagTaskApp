using EmagTask.Model.Config;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace EmagTask.Implementation.App.Helper
{
    public class AppHelper
    {
        public static AppSettings GetAppSettings(string settingsFileName)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();

                path = Path.Combine(path, $"Settings/{settingsFileName}");


                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile(path)
                    .Build();

                AppSettings appSettings = new AppSettings();
                config.Bind(appSettings);

                return appSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"App failed to load appSettings from :{settingsFileName}  file. Ex:{ex.Message}");
            }
        }

        public static string SerializeLowerCase(object objToSerialize)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string jsonValue = JsonConvert.SerializeObject(objToSerialize, serializerSettings);
            return jsonValue;
        }
    }
}
