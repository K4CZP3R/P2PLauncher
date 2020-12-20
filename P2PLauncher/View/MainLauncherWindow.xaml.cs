using P2PLauncher.Model;
using P2PLauncher.Services;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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


        public void UpdateFreeLanStatus()
        {
            FreeLanInstallationStatus status = freeLanDetectionService.GetInstallationStatus();
            SetFreeLanStatusValueLabel(EnumHelper.GetDescription(status));
        }
        public void UpdateNumbers()
        {
            SetAdaptersToDisableValueLabel(networkAdapters.GetAdapterNamesToDisable().Length.ToString());
            SetServicesToDisableValueLabel(windowsServices.GetServiceNamesToDisable().Length.ToString());
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
            freeLanService.SetHostMode(true);
            freeLanService.SetPassphrase(TextBoxHostPassword.Text);
            if(freeLanService.StartFreeLan())
                dialogService.ShowMessage($"Tell your friends to enter {EnvHelper.GetPublicAddress()} as Host.", "Your public ip");

        }
        private void OnClientStart(object sender, RoutedEventArgs e)
        {
            freeLanService.SetHostMode(false);
            freeLanService.SetPassphrase(TextBoxPassword.Text);
            freeLanService.SetHostIp(TextBoxHost.Text);
            freeLanService.SetClientId(TextBoxId.Text);
            freeLanService.StartFreeLan();
        }

        private void OnFreeLanStop(object sender, RoutedEventArgs e)
        {
            freeLanService.StopFreeLan();

        }

        public void UpdateWindow()
        {
            UpdateFreeLanStatus();
            UpdateNumbers();

        }
    }
}
