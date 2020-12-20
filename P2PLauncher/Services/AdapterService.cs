using P2PLauncher.Model;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Services
{
    public class AdapterService
    {
        private readonly NetworkAdapters _networkAdapters;

        public AdapterService(NetworkAdapters networkAdapters)
        {
            _networkAdapters = networkAdapters;
        }

        /// <summary>
        /// Save marked adapters to config file.
        /// </summary>
        /// <param name="adapters">Adapters which should be disabled on FreeLan start.</param>
        public void SaveAdaptersToDisable(List<NetworkAdapter> adapters)
        {
            List<string> ids = new List<string>();
            foreach (NetworkAdapter adapter in adapters)
            {
                ids.Add(adapter.ID);
            }
            Save(String.Join(",", ids));
        }

        /// <summary>
        /// Gets adapters which should be disabled on FreeLan start.
        /// </summary>
        /// <returns>List of NetworkAdapters</returns>
        public List<NetworkAdapter> GetAdaptersToDisable()
        {
            List<NetworkAdapter> toReturn = new List<NetworkAdapter>();

            String savedContent = Read();
            if (savedContent == null)
            {
                return toReturn;
            }

            List<NetworkAdapter> allAdapters = _networkAdapters.GetNetworkAdapters();
            string[] splitted = savedContent.Split(',');
            foreach (string s in splitted)
            {
                var ret = allAdapters.Where(x => x.ID.Equals(s)).ToArray();
                if (ret.Length > 0)
                {
                    toReturn.Add(ret[0]);
                }

            }

            return toReturn;
        }




        /// <summary>
        /// Saves content (IDs, comma separated) to config file and reloads config.
        /// </summary>
        /// <param name="content">Network Adapter ID's comma separated.</param>
        private void Save(string content)
        {
            Properties.Settings.Default.AdaptersToTurnOff = content;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Reload();
        }

        
        /// <summary>
        /// Reads ID's comma separated from config
        /// </summary>
        /// <returns>String containing comma's and IDs</returns>
        private String Read()
        {
            return Properties.Settings.Default.AdaptersToTurnOff;
        }
    }
}