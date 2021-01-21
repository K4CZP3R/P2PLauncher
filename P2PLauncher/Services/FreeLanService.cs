using P2PLauncher.Exceptions;
using P2PLauncher.Model;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PLauncher.Services
{
    public class FreeLanService
    {
        private readonly FreeLanDetectionService freeLanDetectionService;
        private readonly IDialogService dialogService;
        private readonly WindowsServices windowsServices;

        public Process process;

        private string passphrase;
        private string hostIp;
        private AddressType hostIpType;
        private string clientId;
        private FreeLanMode mode;
        private string relayMode;
        private bool showShell;
        private StreamWriter debugWrite;

        public void SetPassphrase(string content)
        {
            passphrase = content;
        }
        public void SetHostIp(string content)
        {
            hostIpType = AddressHelper.GetAddressType(content);
            switch (hostIpType)
            {
                case AddressType.UNKNOWN:
                    throw new InvalidInput("Invalid host/hub address!");
                case AddressType.IPV4:
                    hostIp = content;
                    break;
            }
        }
        public void SetClientId(string content)
        {
            if (!int.TryParse(content, out int parsed))
            {
                throw new InvalidInput("ID should be an number.");
            }

            if (parsed < 2 || parsed > 253)
            {
                throw new InvalidInput("ID should be in range between 2-253.");
            }

            clientId = content;
        }
        public void SetMode(FreeLanMode c)
        {
            mode = c;
        }

        public void SetRelayMode(bool c)
        {
            relayMode = c ? "yes" : "no";
        }
        public void SetShowShell(bool c)
        {
            showShell = c;
        }

        public bool IsThisValidIPForTheCurrentMode(string ip)
        {
            switch (mode)
            {
                case FreeLanMode.CLIENT:
                    return ip.StartsWith("9.0.0") && !ip.Equals("9.0.0.1");
                case FreeLanMode.CLIENT_HUB:
                    return ip.StartsWith("9.0.0") && !ip.Equals("9.0.0.1");
                case FreeLanMode.HOST:
                    return ip.StartsWith("9.0.0");
                default:
                    return false;
    
            }
        }



        public FreeLanService(WindowsServices windowsServices,
            FreeLanDetectionService freeLanDetectionService,
            IDialogService dialogService)
        {
            this.freeLanDetectionService = freeLanDetectionService;
            this.dialogService = dialogService;
            this.windowsServices = windowsServices;
        }


        public bool GetFreeLanServiceStatus()
        {
            WindowsService freeLanService = windowsServices.GetServiceByName("FreeLAN Service");
            if (freeLanService == null)
                return false;
            return freeLanService.Status == ServiceControllerStatus.Running;

        }
        public void SetFreeLanServiceStatus(bool start)
        {
            WindowsService freeLanService = windowsServices.GetServiceByName("FreeLAN Service");
            if (freeLanService == null)
                return;
            if (start)
                freeLanService.Enable();
            else
                freeLanService.Disable();

        }
        public void StopFreeLan()
        {
            if (!process.HasExited)
                process.Kill();
            debugWrite.Close();
        }

        public bool GetStrangeFreeLansRunning()
        {
            foreach (var process in Process.GetProcessesByName("freelan"))
            {
                return true;
            }
            return false;
        }

        public bool KillStrangeFreeLan()
        {
            bool killed = false;
            foreach (var process in Process.GetProcessesByName("freelan"))
            {
                killed = true;
                process.Kill();
            }
            return killed;
        }

        public bool StartFreeLan()
        {
            if(process != null && !process.HasExited)
            {
                throw new AlreadyRunning("Please stop it first.");
            }
            if (freeLanDetectionService.GetInstallationStatus() != FreeLanInstallationStatus.OK)
            {
                dialogService.ShowMessage("FreeLan is not confiured! do it", "JUST DO IT!");
                return false;
            }

            if (GetFreeLanServiceStatus())
            {
                SetFreeLanServiceStatus(false);
            }

            if(GetStrangeFreeLansRunning())
            {
                KillStrangeFreeLan();
            }


            debugWrite = new StreamWriter("debug.txt");

            /*
             * //"freelan.exe" --security.passphrase %quoted% --tap_adapter.ipv4_address_prefix_length 9.0.0.1/24 --switch.relay_mode_enabled yes --tap_adapter.metric 1 --debug
             * //"freelan.exe" --security.passphrase %quoted% --fscp.contact %hostip%:12000 --tap_adapter.ipv4_address_prefix_length 9.0.0.%clientid%/24 --tap_adapter.metric 1 --debug
             * freelan.exe --security.passphrase "[INSERT_HERE]" --fscp.contact [HOSTS_IP]:12000 --tap_adapter.dhcp_proxy_enabled no --tap_adapter.ipv4_dhcp true --tap_adapter.metric 1 --debug
             * freelan.exe" --security.passphrase "[INSERT_HERE]" --fscp.contact [[IPV6_IP]]:12000 --tap_adapter.ipv4_address_prefix_length 9.0.0.[CLIENTID]/24 --tap_adapter.metric 1 --debug
             */

            process = new Process();
            process.StartInfo.FileName = freeLanDetectionService.GetFreeLanExecutableLocation();

            switch (mode)
            {
                case FreeLanMode.CLIENT:
                    process.StartInfo.Arguments =
                        $"--security.passphrase {passphrase} --fscp.contact {hostIp}:12000 --switch.relay_mode_enabled {relayMode} --tap_adapter.ipv4_address_prefix_length 9.0.0.{clientId}/24 --tap_adapter.metric 1 --debug";
                    break;
                case FreeLanMode.HOST:
                    process.StartInfo.Arguments =
                        $"--security.passphrase {passphrase} --tap_adapter.ipv4_address_prefix_length 9.0.01/24 --switch.relay_mode_enabled {relayMode} --tap_adapter.metric 1 --debug";
                    break;
                case FreeLanMode.CLIENT_HUB:
                    process.StartInfo.Arguments =
                        $"--security.passphrase {passphrase} --fscp.contact {hostIp}:12000 --tap_adapter.dhcp_proxy_enabled 0 --tap_adapter.ipv4_dhcp 1 --tap_adapter.metric 1 --debug";
                    break;
            };

            process.StartInfo.CreateNoWindow = !showShell;
            process.StartInfo.WindowStyle = showShell ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;

            if (!showShell)
            {
                // Logging to file
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        debugWrite.WriteLine(e.Data);
                    }
                });
            }


            process.Start();
            if (!showShell)
                process.BeginOutputReadLine();
            return !process.HasExited;
        }
    }
}
