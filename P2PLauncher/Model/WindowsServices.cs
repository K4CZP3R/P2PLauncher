using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    public class WindowsServices
    {
        public List<WindowsService> GetServices()
        {
            List<WindowsService> windowsServices = new List<WindowsService>();

            ServiceController[] services = ServiceController.GetServices();

            foreach(ServiceController service in services)
            {
                windowsServices.Add(new WindowsService().FromServiceController(service));
            }
            return windowsServices;
        }

        public List<WindowsService> GetServicesWithType(ServiceType serviceType)
        {
            List<WindowsService> withType = new List<WindowsService>();

            foreach(WindowsService service in GetServices())
            {
                if(service.Type == serviceType)
                {
                    withType.Add(service);
                }
            }
            return withType;
        }

    }
}
