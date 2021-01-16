using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PLauncher.Services
{
    public class FreeLanService
    {
        private readonly FreeLanDetectionService freeLanDetectionService;
        private readonly IDialogService dialogService;

        public Process process;
        
        private string passphrase;
        private string hostip;
        private string clientid;
        private bool hostMode;
        private string relayMode;
        private bool showShell;
        private StreamWriter debugWrite;

        public void SetPassphrase(string content)
        {
            passphrase = content;
        }
        public void SetHostIp(string content)
        {
            hostip = content;
        }
        public void SetClientId(string content)
        {
            clientid = content;
        }
        public void SetHostMode(bool c)
        {
            hostMode = c;
        }

        public void SetRelayMode(bool c)
        {
            relayMode = c ? "yes" : "no";
        }
        public void SetShowShell(bool c)
        {
            showShell = c;
        }
        
        public FreeLanService(FreeLanDetectionService freeLanDetectionService, IDialogService dialogService)
        {
            this.freeLanDetectionService = freeLanDetectionService;
            this.dialogService = dialogService;
        }

        public void StopFreeLan()
        {
            if(!process.HasExited)
                process.Kill();
            debugWrite.Close();
        }
        public bool StartFreeLan()
        {
            if(freeLanDetectionService.GetInstallationStatus() != Model.FreeLanInstallationStatus.OK)
            {
                dialogService.ShowMessage("FreeLan is not confiured! do it", "JUST DO IT!");
                return false;
            }


            debugWrite = new StreamWriter("debug.txt");

            /*
             * //"freelan.exe" --security.passphrase %quoted% --tap_adapter.ipv4_address_prefix_length 9.0.0.1/24 --switch.relay_mode_enabled yes --tap_adapter.metric 1 --debug
             * //"freelan.exe" --security.passphrase %quoted% --fscp.contact %hostip%:12000 --tap_adapter.ipv4_address_prefix_length 9.0.0.%clientid%/24 --tap_adapter.metric 1 --debug
             */

            process = new Process();
            process.StartInfo.FileName = freeLanDetectionService.GetFreeLanExecutableLocation();
            if(hostMode)
            {
                process.StartInfo.Arguments = $"--security.passphrase {passphrase} --tap_adapter.ipv4_address_prefix_length 9.0.0.1/24 --switch.relay_mode_enabled {relayMode} --tap_adapter.metric 1 --debug";
            }
            else
            {
                process.StartInfo.Arguments = $"--security.passphrase {passphrase} --fscp.contact {hostip}:12000 --switch.relay_mode_enabled {relayMode} --tap_adapter.ipv4_address_prefix_length 9.0.0.{clientid}/24 --tap_adapter.metric 1 --debug";
            }
            process.StartInfo.CreateNoWindow = !showShell;
            process.StartInfo.WindowStyle = showShell ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;

            if(!showShell)
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
            if(!showShell)
                process.BeginOutputReadLine();
            return !process.HasExited;
        }
    }
}
