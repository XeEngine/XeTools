using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public class Settings<T> 
    {
        private string _fileName;
        
        public static async Task<Settings<T>> OpenAsync(string path, string moduleName)
        {
            Settings<T> settings = null;
            var fileName = Path.Combine(path, $"{moduleName}.settings.json");

            if (File.Exists(fileName))
            {
                try
                {
                    string strSettings;
                    using (var reader = new StreamReader(fileName))
                    {
                        strSettings = await reader.ReadToEndAsync();
                    }
                    settings = JsonConvert.DeserializeObject(strSettings) as Settings<T>;
                    settings._fileName = fileName;
                }
                catch (JsonSerializationException)
                {
                    Log.Error($"Invalid settings for {moduleName} module.");
                }
                catch (Exception e)
                {
                    Log.Error($"Uncaught exception in {e.Source} for {e.TargetSite}:\n{e.Message}\n{e.StackTrace}\n{e.Data}");
                }
            }
            else
            {
                Log.Message($"Creating new settings for {moduleName} module.");
            }
            if (settings == null)
            {
                settings = new Settings<T>();
                settings.Default();
            }
            return settings;
        }

        public virtual void Default()
        {
        }

        public async Task SaveAsync()
        {
            var strSettings = JsonConvert.SerializeObject(this, Formatting.Indented);

            try
            {
                using (var file = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        await writer.WriteAsync(strSettings);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Uncaught exception in {e.Source} for {e.TargetSite}:\n{e.Message}\n{e.StackTrace}\n{e.Data}");
            }
        }
    }
}
