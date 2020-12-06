using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

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
    }
}
