using P2PLauncher.Model;
using P2PLauncher.Services;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace P2PLauncher.View
{
    /// <summary>
    /// Interaction logic for MainLauncherWindow.xaml
    /// </summary>
    public partial class MainLauncherWindow : Window, IWindow
    {
        private readonly FreeLanDetectionService freeLanDetectionService;
        private readonly NetworkAdapters networkAdapters;
        private readonly WindowsServices windowsServices;
        private readonly IFileService fileService;
        private readonly IDialogService dialogService;
        private readonly FreeLanService freeLanService;
        private DispatcherTimer processCheck;
        private readonly string donators = "Striderstroke";

        public MainLauncherWindow()
        {
            InitializeComponent();

            fileService = new WinFileService();
            dialogService = new WinDialogService();
            freeLanDetectionService = new FreeLanDetectionService(fileService, dialogService);
            windowsServices = new WindowsServices();
            networkAdapters = new NetworkAdapters();
            freeLanService = new FreeLanService(
                freeLanDetectionService,
                dialogService);

            UpdateWindow();
        }

        private void SetFreeLanStatusValueLabel(string content)
        {
            LabelFreeLanStatusValue.Content = content;
        }
        private void SetServicesToDisableValueLabel(string content)
        {
            LabelServicesToDisableValue.Content = content;
        }
        private void SetAdaptersToDisableValueLabel(string content)
        {
            LabelAdaptersToDisableValue.Content = content;
        }
        private void SetPublicAddressValueLabel(string content)
        {
            LabelPublicAddress.Content = content;
        }
        private void SetStateValueLabel(string content)
        {
            LabelStateValue.Content = content;
        }
        private void SetDonatorsLabel(string content)
        {
            LabelDonators.Content = content;
        }

        public void UpdateFreeLanStatus()
        {
            FreeLanInstallationStatus status = freeLanDetectionService.GetInstallationStatus();
            SetFreeLanStatusValueLabel(EnumHelper.GetDescription(status));
        }
        public void UpdateNumbers()
        {
            SetAdaptersToDisableValueLabel(networkAdapters.GetAdapterNamesToDisable().Length.ToString());
            SetServicesToDisableValueLabel(windowsServices.GetServiceNamesToDisable().Length.ToString());
            SetPublicAddressValueLabel(EnvHelper.GetPublicAddress());

        }

        private void OnOpenFreeLanSettingsButton(object sender, RoutedEventArgs e)
        {
            FreeLanDetectionWindow window = new FreeLanDetectionWindow();
            window.ShowDialog();
            UpdateWindow();
        }
        private void OnOpenAdaptersSettingsButton(object sender, RoutedEventArgs e)
        {
            NetworkAdaptersWindow window = new NetworkAdaptersWindow();
            window.ShowDialog();
            UpdateWindow();
        }
        private void OnOpenServicesSettingsButton(object sender, RoutedEventArgs e)
        {
            WindowsServicesWindow window = new WindowsServicesWindow();
            window.ShowDialog();
            UpdateWindow();
        }
        private void OnHostStart(object sender, RoutedEventArgs e)
        {
            foreach (WindowsService w in windowsServices.GetServicesToDisable())
            {
                w.Disable();
            }
            foreach (NetworkAdapter a in networkAdapters.GetAdaptersToDisable())
            {
                a.Disable();
            }
            freeLanService.SetHostMode(true);
            freeLanService.SetPassphrase(TextBoxHostPassword.Text);
            freeLanService.SetRelayMode(CheckBoxHostRelay.IsChecked.Value);
            freeLanService.SetShowShell(CheckBoxDebug.IsChecked.Value);
            bool started = freeLanService.StartFreeLan();
            if (started)
            {
                SetStateValueLabel("Host - running.");
                StartProcessCheck();
            }
            
        }
        private void OnClientStart(object sender, RoutedEventArgs e)
        {
            foreach (WindowsService w in windowsServices.GetServicesToDisable())
            {
                w.Disable();
            }
            foreach (NetworkAdapter a in networkAdapters.GetAdaptersToDisable())
            {
                a.Disable();
            }

            freeLanService.SetHostMode(false);
            freeLanService.SetPassphrase(TextBoxPassword.Text);
            freeLanService.SetHostIp(TextBoxHost.Text);
            freeLanService.SetClientId(TextBoxId.Text);
            freeLanService.SetRelayMode(CheckBoxClientRelay.IsChecked.Value);
            freeLanService.SetShowShell(CheckBoxDebug.IsChecked.Value);
            bool started = freeLanService.StartFreeLan();
            if (started)
            {
                SetStateValueLabel("Client - running.");
                StartProcessCheck();
            }

        }

        private void OnFreeLanStop(object sender, RoutedEventArgs e)
        {
            OnFreeLanStop();
        }   
        private void OnFreeLanStop()
        {
            SetStateValueLabel("Not running.");
            freeLanService.StopFreeLan();
            foreach (WindowsService w in windowsServices.GetServicesToDisable())
            {
                w.Enable();
            }
            foreach (NetworkAdapter a in networkAdapters.GetAdaptersToDisable())
            {
                a.Enable();
            }
        }

        private void StartProcessCheck()
        {
            processCheck = new DispatcherTimer();
            processCheck.Tick += ProcessCheck_Tick;
            processCheck.Interval = TimeSpan.FromSeconds(1.00);
            processCheck.Start();
        }

        private void ProcessCheck_Tick(object sender, EventArgs e)
        {
            if (freeLanService.process.HasExited)
            {
                processCheck.Stop();
                OnFreeLanStop();
            }
        }

        public void UpdateWindow()
        {
            UpdateFreeLanStatus();
            UpdateNumbers();
            SetDonatorsLabel(donators);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(freeLanService.process != null && !freeLanService.process.HasExited)
            {
                OnFreeLanStop();
            }
        }
    }
}
