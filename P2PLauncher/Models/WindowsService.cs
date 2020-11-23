using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Models
{
    class WindowsService
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public ServiceType Type { get; set; }
        public ServiceControllerStatus Status { get; set; }

        /// <summary>
        /// Generates WindowsService object based on ServiceController class.
        /// </summary>
        /// <param name="serviceController">Object returned from ServiceController class.</param>
        /// <returns>WindowsService object.</returns>
        public WindowsService FromServiceController(ServiceController serviceController)
        {
            this.DisplayName = serviceController.DisplayName;
            this.Name = serviceController.ServiceName;
            this.Type = serviceController.ServiceType;
            this.Status = serviceController.Status;

            return this;
        }
    }
}
