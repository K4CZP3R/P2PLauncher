using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    public class WindowsService
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public ServiceType Type { get; set; }
        public ServiceControllerStatus Status { get; set; }

        public WindowsService FromServiceController(ServiceController serviceController)
        {
            this.DisplayName = serviceController.DisplayName;
            this.Name = serviceController.ServiceName;
            this.Type = serviceController.ServiceType;
            this.Status = serviceController.Status;

            return this;
        }

        public override string ToString()
        {
            return $"{DisplayName} - {Name} - {Type} - {Status}";
        }

        public void Enable()
        {
            System.Diagnostics.ProcessStartInfo psi =
           new System.Diagnostics.ProcessStartInfo("net", "start \"" + Name + "\" /y");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            p.Start();  
        }
        public void Disable()
        {
            System.Diagnostics.ProcessStartInfo psi =
                        new System.Diagnostics.ProcessStartInfo("net", "stop \"" + Name + "\" /y");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            p.Start();
        }


    }
}
