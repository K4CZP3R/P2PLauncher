using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
            return this;
        }

        /// <summary>
        /// Returns representation of this object in string.
        /// </summary>
        /// <returns>String containing all data stored in this object.</returns>
        public override string ToString()
        {
            return $"{Description}/{ID}/{Manufacturer}/{Name}/{ConnectionId}/{ProductName}/{ServiceName}";
        }
        
    }
}
