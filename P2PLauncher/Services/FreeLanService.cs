using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Thread thread;
        private Process process;
        private ThreadStart threadStart;

        private string passphrase;
        private string hostip;
        private string clientid;
        private bool hostMode;

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

        
        public FreeLanService(FreeLanDetectionService freeLanDetectionService, IDialogService dialogService)
        {
            this.freeLanDetectionService = freeLanDetectionService;
            this.dialogService = dialogService;
        }

        public void StopFreeLan()
        {
            if(thread != null)
            {
                process.Kill();
            }
        }
        public bool StartFreeLan()
        {
            if(freeLanDetectionService.GetInstallationStatus() != Model.FreeLanInstallationStatus.OK)
            {
                dialogService.ShowMessage("FreeLan is not confiured! do it", "JUST DO IT!");
                return false;
            }
            /*
             * //"freelan.exe" --security.passphrase %quoted% --tap_adapter.ipv4_address_prefix_length 9.0.0.1/24 --switch.relay_mode_enabled yes --tap_adapter.metric 1 --debug
             * //"freelan.exe" --security.passphrase %quoted% --fscp.contact %hostip%:12000 --tap_adapter.ipv4_address_prefix_length 9.0.0.%clientid%/24 --tap_adapter.metric 1 --debug
             */

            process = new Process();
            process.StartInfo.FileName = freeLanDetectionService.GetFreeLanExecutableLocation();
            if(hostMode)
            {
                process.StartInfo.Arguments = $"--security.passphrase {passphrase} --tap_adapter.ipv4_address_prefix_length 9.0.0.1/24 --switch.relay_mode_enabled yes --tap_adapter.metric 1 --debug";
            }
            else
            {
                process.StartInfo.Arguments = $"--security.passphrase {passphrase} --fscp.contact {hostip}:12000 --tap_adapter.ipv4_address_prefix_length 9.0.0.{clientid}/24 --tap_adapter.metric 1 --debug";
            }
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;

            ThreadStart ths = new ThreadStart(() => process.Start());
            thread = new Thread(ths);
            thread.Start();
            return true;
        }




    }
}
