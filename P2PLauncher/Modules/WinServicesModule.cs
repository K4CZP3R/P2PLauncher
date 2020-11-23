using P2PLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Modules
{
    class WinServicesModule
    {
        /// <summary>
        /// Lists all services running on the local machine.
        /// </summary>
        /// <returns>List with WindowsServices</returns>
        public List<WindowsService> ListServices()
        {

            List<WindowsService> windowsServices = new List<WindowsService>();
            ServiceController[] services = ServiceController.GetServices();


            foreach(ServiceController service in services)
            {
                WindowsService windowsService = new WindowsService().FromServiceController(service);
                windowsServices.Add(windowsService);
                
            }

            return windowsServices;
        }

        /// <summary>
        /// Filters services.
        /// </summary>
        /// <param name="serviceType">Return only services with this type.</param>
        /// <returns>List of services with the given type.</returns>
        public List<WindowsService> FindServicesByType(ServiceType serviceType)
        {
            List<WindowsService> allServices = ListServices();

            foreach(WindowsService service in allServices)
            {
                if(service.Type != serviceType)
                {
                    allServices.Remove(service);
                }
            }

            return allServices;
        }
    }
}
