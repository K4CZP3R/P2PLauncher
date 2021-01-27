using P2PLauncher.Exceptions;
using P2PLauncher.Model;
using P2PLauncher.Services;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private DispatcherTimer freeLanAddressCheck;
        private readonly string donators = "Striderstroke";
        private readonly double freeLanAddressCheckInterval = 3.00;
        private int freeLanAddressCheckRenewTimeout = 0;
        private int freeLanAddressCheckLoops = 0;


        public MainLauncherWindow()
        {
            InitializeComponent();

            fileService = new WinFileService();
            dialogService = new WinDialogService();
            freeLanDetectionService = new FreeLanDetectionService(fileService, dialogService);
            windowsServices = new WindowsServices();
            networkAdapters = new NetworkAdapters();
            freeLanService = new FreeLanService(
                windowsServices,
                freeLanDetectionService,
                dialogService
                );



            if (!EnvHelper.IsAdministrator())
            {
                ConsoleHelper.Print("User is not administrator, will exit.");
                MessageBox.Show("To use this application you will need administrator privileges!");
                System.Environment.Exit(0);
            }
            ConsoleHelper.Print("User is admin!");

            UpdateWindow();

        }

        #region UI setters
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
        private void SetFreeLANAddressValueLabel(string content)
        {
            LabelFreeLANAddress.Content = content;
        }
        private void SetVisibilityFreeLANAddressTipLabel(bool visible)
        {
            LabelFreeLANAddressTip.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }
        private void SetStateValueLabel(string content)
        {
            LabelStateValue.Content = content;
        }
        private void SetDonatorsLabel(string content)
        {
            LabelDonators.Content = content;
        }
        #endregion

        #region UI
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            ConsoleHelper.Print($"Validating input: {e.Text}");
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool UpdateFreeLANAddress(List<NetworkAdapter> adapters)
        {
            List<string> tapIps = new List<string>();
            foreach (NetworkAdapter adapter in adapters)
            {
                foreach (string ip in adapter.GetCurrentIP())
                {
                    if (freeLanService.IsThisValidIPForTheCurrentMode(ip))
                    {
                        ConsoleHelper.Print($"'{ip}' is valid ip for FreeLan.");
                        tapIps.Add(ip);
                    }
                    else
                    {
                        ConsoleHelper.Print($"'{ip}' is not valid ip for FreeLan.");
                    }
                }
            }

            if (tapIps.Count == 0)
            {
                SetFreeLANAddressValueLabel("Unknown.");
                SetVisibilityFreeLANAddressTipLabel(false);
                return false;
            }
            else if (tapIps.Count > 1)
            {
                SetFreeLANAddressValueLabel(String.Join(",", tapIps.ToArray()));
                SetVisibilityFreeLANAddressTipLabel(true);
            }
            else
            {
                SetFreeLANAddressValueLabel(tapIps[0]);
                SetVisibilityFreeLANAddressTipLabel(false);
            }
            return true;
        }

        public void UpdateWindow()
        {
            UpdateFreeLanStatus();
            UpdateNumbers();
            SetDonatorsLabel(donators);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (freeLanService.process != null && !freeLanService.process.HasExited)
            {
                OnFreeLanStop();
            }
        }

        #endregion

        #region Button actions (all)
            
        private void OnOpenConsoleClick(object sender, RoutedEventArgs e)
        {
            ConsoleHelper.Show();
        }
        private void OnOpenLogsClick(object sender, RoutedEventArgs e)
        {
            UpdateWindow();
            if (EnvHelper.FileExists("debug.txt"))
            {
                EnvHelper.OpenNotepadWithFile("debug.txt");
            }
            else
            {
                MessageBox.Show("There are no logs yet");
            }
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
        private void OnCopyPublicAddressClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(LabelPublicAddress.Content.ToString());
        }

        #endregion

        #region Button actions Tabs
        private void OnCommonStart()
        {
            if (freeLanService.GetFreeLanServiceStatus())
            {
                MessageBox.Show("FreeLAN is already running in the background. To prevent this app from failing, it will be disabled.");
            }
            foreach (WindowsService w in windowsServices.GetServicesToDisable())
            {
                w.Disable();
            }
            foreach (NetworkAdapter a in networkAdapters.GetAdaptersToDisable())
            {
                a.Disable();
            }
            
            StartFreeLANAddressCheck();
        }

        private void OnHubStartClick(object sender, RoutedEventArgs e)
        {
            try
            {
                freeLanService.SetMode(FreeLanMode.CLIENT_HUB);
                freeLanService.SetPassphrase(TextBoxHubPassword.Text);
                freeLanService.SetHostIp(TextBoxHubHost.Text);
                freeLanService.SetShowShell(CheckBoxDebug.IsChecked.Value);

                OnCommonStart();

                bool started = freeLanService.StartFreeLan();
                if (started)
                {
                    SetStateValueLabel("Hub - running.");
                    StartProcessCheck();
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidInput || ex is AlreadyRunning)
                {
                    ExceptionHelper.ShowMessageBox(ex);
                    return;
                }
                throw;
            }
        }
        private void OnHostStartClick(object sender, RoutedEventArgs e)
        {
            try
            {
                freeLanService.SetMode(FreeLanMode.HOST);
                freeLanService.SetPassphrase(TextBoxHostPassword.Text);
                freeLanService.SetRelayMode(CheckBoxHostRelay.IsChecked.Value);
                freeLanService.SetShowShell(CheckBoxDebug.IsChecked.Value);
                freeLanService.GetFreeLanServiceStatus();

                OnCommonStart();

                bool started = freeLanService.StartFreeLan();
                if (started)
                {
                    SetStateValueLabel("Host - running.");
                    StartProcessCheck();
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidInput || ex is AlreadyRunning)
                {
                    ExceptionHelper.ShowMessageBox(ex);
                    return;
                }
                throw;
            }


        }
        private void OnClientStartClick(object sender, RoutedEventArgs e)
        {
            try
            {
                freeLanService.SetMode(FreeLanMode.CLIENT);
                freeLanService.SetPassphrase(TextBoxClientPassword.Text);
                freeLanService.SetHostIp(TextBoxClientHost.Text);
                freeLanService.SetClientId(TextBoxId.Text);
                freeLanService.SetRelayMode(CheckBoxClientRelay.IsChecked.Value);
                freeLanService.SetShowShell(CheckBoxDebug.IsChecked.Value);

                OnCommonStart();

                bool started = freeLanService.StartFreeLan();
                if (started)
                {
                    SetStateValueLabel("Client - running.");
                    StartProcessCheck();
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidInput || ex is AlreadyRunning)
                {
                    ExceptionHelper.ShowMessageBox(ex);
                    return;
                }
                throw;
            }



        }

        private void OnFreeLanStopClick(object sender, RoutedEventArgs e)
        {
            OnFreeLanStop();
        }

        #endregion
        private void OnFreeLanStop()
        {
            SetStateValueLabel("Not running.");
            SetFreeLANAddressValueLabel("None");
            freeLanService.StopFreeLan();
            if(freeLanAddressCheck != null)
                freeLanAddressCheck.Stop();
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
            processCheck.Tick += OnProcessCheck;
            processCheck.Interval = TimeSpan.FromSeconds(1.00);
            processCheck.Start();
        }

        private void StartFreeLANAddressCheck()
        {
            freeLanAddressCheckRenewTimeout = int.Parse(TextBoxUATimeout.Text);
            freeLanAddressCheck = new DispatcherTimer();
            freeLanAddressCheck.Tick += OnFreeLanAddressCheck;
            freeLanAddressCheck.Interval = TimeSpan.FromSeconds(freeLanAddressCheckInterval);
            freeLanAddressCheckLoops = 0;
            freeLanAddressCheck.Start();
        }

        private void OnFreeLanAddressCheck(object sender, EventArgs e)
        {
            List<NetworkAdapter> adapters = networkAdapters.GetTAPAdapters();
            UpdateFreeLANAddress(adapters);



            foreach (NetworkAdapter adapter in adapters)
            {
                foreach (string ip in adapter.GetCurrentIP())
                {
                    if (ip.StartsWith("169."))
                    {
                        if (freeLanAddressCheckInterval * freeLanAddressCheckLoops > freeLanAddressCheckRenewTimeout)
                        {
                            ConsoleHelper.Print($"IP starts with 169. and the timeout is reached, resetting DHCP.");
                            freeLanAddressCheckLoops = 0;
                            adapter.SetDefaultMode();
                            adapter.SetDHCPMode();
                        }
                    }
                    if (freeLanService.GetMode() == FreeLanMode.CLIENT_HUB)
                    {
                        if (ip == "9.0.0.1")
                        {
                            ConsoleHelper.Print($"IP looks like host ip when in hub mode (bug), resetting DHCP.");
                            adapter.SetDHCPMode();
                        }
                    }
                }

            }
            int timeLeft = freeLanAddressCheckRenewTimeout - (int)(freeLanAddressCheckInterval * freeLanAddressCheckLoops);
            ConsoleHelper.Print($"{timeLeft}s for the timeout.");
            freeLanAddressCheckLoops += 1;

        }

        private void OnProcessCheck(object sender, EventArgs e)
        {
            if (freeLanService.process.HasExited)
            {
                processCheck.Stop();
                OnFreeLanStop();
            }
        }


    }
}
