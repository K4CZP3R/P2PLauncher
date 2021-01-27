using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    public class NetworkAdapter
    {
        public string Description { get; set; }
        public string ID { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public bool Enabled { get; set; }
        public string ProductName { get; set; }
        public string ServiceName { get; set; }
        public IPInterfaceProperties IPInterfaceProperties { get; set; }



        /// <summary>
        /// Generates NetworkAdapter object based on WMI data
        /// </summary>
        /// <param name="managementBaseObject">WMI object</param>
        /// <returns>Filled in NetworkAdapter object.</returns>
        public NetworkAdapter FromWMI(ManagementBaseObject managementBaseObject)
        {
            this.Description = (string)managementBaseObject["Description"];
            this.ID = (string)managementBaseObject["DeviceID"];
            this.Manufacturer = (string)managementBaseObject["Manufacturer"];
            this.Name = (string)managementBaseObject["Name"];
            this.ConnectionId = (string)managementBaseObject["NetConnectionID"];
            //this.Enabled = (bool)managementBaseObject["NetEnabled"];
            this.ProductName = (string)managementBaseObject["ProductName"];
            this.ServiceName = (string)managementBaseObject["ServiceName"];
            this.IPInterfaceProperties = null;
            return this;
        }

        public NetworkAdapter FromInterface(NetworkInterface networkInterface)
        {
            this.ConnectionId = networkInterface.Name;
            this.Description = networkInterface.Description;
            this.ID = null;
            this.Manufacturer = null;
            this.Name = null;
            this.ProductName = null;
            this.ServiceName = null;
            this.IPInterfaceProperties = networkInterface.GetIPProperties();

            return this;

        }

        public List<string> GetCurrentIP()
        {
            List<string> currentAddresses = new List<string>();
            if (IPInterfaceProperties == null)
                return currentAddresses;

            foreach (UnicastIPAddressInformation ip in IPInterfaceProperties.UnicastAddresses)
            {
                if(ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    currentAddresses.Add(ip.Address.ToString());
                }
            }
            return currentAddresses;
        }

        /// <summary>
        /// Returns representation of this object in string.
        /// </summary>
        /// <returns>String containing all data stored in this object.</returns>
        public override string ToString()
        {
            return $"{Name}";
        }

        public void Enable()
        {
            ProcessStartInfo psi =
           new ProcessStartInfo("netsh", "interface set interface \"" + ConnectionId + "\" enable");
            Process p = new Process();
            p.StartInfo = psi;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
        }

        public void SetDefaultMode()
        {
            ProcessStartInfo psi =
         new ProcessStartInfo("netsh", "interface ip set address \"" + ConnectionId + "\" static 9.0.0.1 255.255.255.0 9.0.0.1");
            Process p = new Process();
            p.StartInfo = psi;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
        }
        public void SetDHCPMode()
        {
            ProcessStartInfo psi =
          new ProcessStartInfo("netsh", "interface ip set address \"" + ConnectionId + "\" DHCP");
            Process p = new Process();
            p.StartInfo = psi;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
        }

        public void Disable()
        {
            ProcessStartInfo psi =
                        new ProcessStartInfo("netsh", "interface set interface \"" + ConnectionId + "\" disable");
            Process p = new Process();
            p.StartInfo = psi;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
        }

    }
}
