using Newtonsoft.Json;
using P2PLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Modules
{
    class ConfigurationModule
    {
        /// <summary>
        /// Default Config path location.
        /// </summary>
        private readonly string ConfigPath = "config.json";

        /// <summary>
        /// Saves current config object to file, to be later retrived at any point in program.
        /// IMPORTANT: It is probably not async friendly (access is not controlled)
        /// </summary>
        /// <param name="configurationModel">Current config.</param>
        /// <returns>Current config.</returns>
        public ConfigurationModel Save(ConfigurationModel configurationModel)
        {
            SaveToFile(JsonConvert.SerializeObject(configurationModel), ConfigPath);
            return Load();
        }

        /// <summary>
        /// Loads config from config file
        /// </summary>
        /// <returns>Current config if exists, otherwise an empty config</returns>
        public ConfigurationModel Load()
        {
            ConfigurationModel fromFile = JsonConvert.DeserializeObject<ConfigurationModel>(ReadFromFile(ConfigPath));
            return fromFile == null ? new ConfigurationModel() : fromFile;

        }

        /// <summary>
        /// Saves any content to any file
        /// (REWORK): Should be moved to more generic class.
        /// </summary>
        /// <param name="content">Content to write</param>
        /// <param name="path">Location of the file</param>
        private void SaveToFile(string content, string path)
        {
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// Reads content from any file
        /// (REWORK): Should be moved to more generic class.
        /// </summary>
        /// <param name="path">Path to read</param>
        /// <returns>Content of file.</returns>
        private string ReadFromFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else
            {
                return "";
            }
        }
    }
}
