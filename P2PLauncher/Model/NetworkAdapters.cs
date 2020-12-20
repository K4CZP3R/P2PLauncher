using System.Collections.Generic;
using System.Management;

namespace P2PLauncher.Model
{
    public class NetworkAdapters
    {
        /// <summary>
        /// Detects all network adapters installed in the system
        /// (software and hardware)
        /// </summary>
        /// <returns>List of network adapters</returns>
        public List<NetworkAdapter> GetNetworkAdapters()
        {
            List<NetworkAdapter> networkAdapters = new List<NetworkAdapter>();
            var query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");
            using (var searcher = new ManagementObjectSearcher(query))
            {
                var queryCollection = searcher.Get();
                foreach (var m in queryCollection)
                {
                    networkAdapters.Add(new NetworkAdapter().FromWMI(m));
                }
            }
            return networkAdapters;

        }

        public void SaveAdaptersToDisable(List<NetworkAdapter> adapters)
        {
            string toSave = "";
            for(int i =0; i< adapters.Count; i++)
            {

                toSave += adapters[i].Name;
                if(i != adapters.Count - 1)
                {
                    toSave += ",";
                }

            }
            Properties.Settings.Default.AdaptersToDisable = toSave;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Reload();
        }

        public string[] GetAdapterNamesToDisable()
        {
            List<string> _temp = new List<string>();
            string saved = Properties.Settings.Default.AdaptersToDisable;
            if(saved == null)
            {
                return new string[0];
            }

            return saved.Split(',');
        }
    }
}
