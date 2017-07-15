using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public class Settings<T> where T : Settings<T>, new()
    {
        private string _fileName;

        public static async Task<T> OpenAsync(Project project, string moduleName)
        {
            return await OpenAsync(Path.Combine(project.ProjectPath, ".settings/modules/"), moduleName);
        }
        public static async Task<T> OpenAsync(string path, string moduleName)
        {
            T settings = null;
            var _fileName = Path.Combine(path, $"{moduleName}.settings.json");

            if (File.Exists(_fileName))
            {
                try
                {
                    string strSettings;
                    using (var reader = new StreamReader(_fileName))
                    {
                        strSettings = await reader.ReadToEndAsync();
                    }
                    settings = JsonConvert.DeserializeObject<T>(strSettings) as T;
                    settings._fileName = _fileName;
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
                settings = new T();
                settings._fileName = _fileName;
                settings.Default();
            }
            return settings as T;
        }

        public virtual void Default()
        {
        }

        public async Task SaveAsync()
        {
            var strSettings = JsonConvert.SerializeObject(this, Formatting.Indented);

            try
            {
                var path = Path.GetDirectoryName(_fileName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
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
