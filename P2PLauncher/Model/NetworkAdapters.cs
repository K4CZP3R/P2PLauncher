using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace P2PLauncher.Model
{
    public class NetworkAdapters
    {
        /// <summary>
        /// Adapters containing the specific string should be ignored.
        /// WAN, Kernel, Bluetooth.
        /// </summary>
        private string[] adaptersToIgnore =
        {
            "WAN",
            "Kernel",
            "Bluetooth"
        };
        
        /// <summary>
        /// Detects all network adapters installed in the system
        /// (software and hardware)
        /// </summary>
        /// <returns>List of network adapters</returns>
        public List<NetworkAdapter> GetNetworkAdapters(string extraQueryContent = "")
        {
            List<NetworkAdapter> networkAdapters = new List<NetworkAdapter>();
            var query = new ObjectQuery($"SELECT * FROM Win32_NetworkAdapter {extraQueryContent}");
            using (var searcher = new ManagementObjectSearcher(query))
            {
                var queryCollection = searcher.Get();
                foreach (var m in queryCollection)
                {
                    var adapter = (new NetworkAdapter().FromWMI(m));
                    bool add = true;
                    foreach(string ignoreWord in adaptersToIgnore)
                    {
                        if(adapter.ToString().Contains(ignoreWord))
                        {
                            add = false;
                        }
                    }
                    if(add)
                        networkAdapters.Add(adapter);
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
        public List<NetworkAdapter> GetAdaptersToDisable()
        {
            string[] toDisable = GetAdapterNamesToDisable();
            List<NetworkAdapter> toDisableList = new List<NetworkAdapter>();
            foreach (NetworkAdapter w in GetNetworkAdapters())
            {
                if (toDisable.Contains(w.Name))
                {
                    toDisableList.Add(w);
                }
            }
            return toDisableList;

        }
    }
}
