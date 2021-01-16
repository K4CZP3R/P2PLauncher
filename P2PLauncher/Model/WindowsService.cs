using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("net", "start \"" + Name + "\" /y");
            p.StartInfo = psi;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();  
        }
        public void Disable()
        {
            ProcessStartInfo psi =
                        new ProcessStartInfo("net", "stop \"" + Name + "\" /y");
            Process p = new Process();
            p.StartInfo = psi;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
        }


    }
}
