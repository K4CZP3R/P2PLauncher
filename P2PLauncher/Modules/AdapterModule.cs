using P2PLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Modules
{
    class AdapterModule
    {

        /// <summary>
        /// Detects all network adapters installed in the system
        /// (software and hardware)
        /// </summary>
        /// <returns>List of network adapters</returns>
        public List<NetworkAdapter> ListAdapters()
        {
            List<NetworkAdapter> networkAdapters = new List<NetworkAdapter>();
            var query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");
            using(var searcher = new ManagementObjectSearcher(query))
            {
                var queryCollection = searcher.Get();
                foreach(var m in queryCollection)
                {
                    NetworkAdapter adapter = new NetworkAdapter().FromWMI(m);
                    networkAdapters.Add(adapter);
                }
            }
            return networkAdapters;

        }
                
    }
}                                                   
