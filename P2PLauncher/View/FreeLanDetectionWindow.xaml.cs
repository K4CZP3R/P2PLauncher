using P2PLauncher.Model;
using P2PLauncher.Services;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
    /// Interaction logic for FreeLanDetectionWindow.xaml
    /// </summary>
    public partial class FreeLanDetectionWindow : Window, IWindow
    {
        private readonly FreeLanDetectionService freeLanDetectionService;
        private readonly IFileService fileService;
        private readonly IDialogService dialogService;
        private bool FreeLanAutoDetectFailed = false;
        private FreeLanInstallationStatus FreeLanInstallationStatus;

        private void SetStatus(string statusContent)
        {
            LabelStatus.Content = $"Status: {statusContent}";

        }

        private void SetFindFreeLanButtonVisibility(bool visible)
        {
            ButtonFindFreeLan.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }
        private void SetFreeLanLocationTabControlVisibility(bool visible)
        {
            TabControlFreeLanLocation.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }
        private void OnFindFreeLanButton(object sender, RoutedEventArgs e)
        {
            bool result = freeLanDetectionService.FindFreeLan();
            FreeLanAutoDetectFailed = !result;
            UpdateWindow();
            if (result)
                MessageBox.Show("FreeLan found! You can close this window now!", "Located!");
            else
            {
                MessageBox.Show("You need to find FreeLan by locating it manually!", "Could not locate.");
            }
        }
        private void OnSelectFreeLanPathButton(object sender, RoutedEventArgs e)
        {
            bool result = freeLanDetectionService.SelectPath();
            UpdateWindow();
            if (!result)
                MessageBox.Show("Something went wrong while selecting file!", "Try again!");


            
        }
        private void OnDownloadFreeLanButton(object sender, RoutedEventArgs e)
        {
            Process.Start(freeLanDetectionService.GetDownloadUrl());
        }

        private void SetDownloadFreeLanHintLabel(string content)
        {
            LabelDownloadFreeLanHint.Content = content;
        }




        public FreeLanDetectionWindow()
        {
            InitializeComponent();

            fileService = new WinFileService();
            dialogService = new WinDialogService();
            freeLanDetectionService = new FreeLanDetectionService(fileService, dialogService);

            UpdateWindow();

        }

        public void UpdateWindow()
        {
            FreeLanInstallationStatus currentStatus = freeLanDetectionService.GetInstallationStatus();
            SetStatus(EnumHelper.GetDescription(currentStatus));
            SetFreeLanLocationTabControlVisibility(FreeLanAutoDetectFailed);
            SetDownloadFreeLanHintLabel(EnvHelper.Is64Bit() ? "You need to download x64 version" :
                "You need to download x86 (32-bit) version.");

            if (currentStatus != FreeLanInstallationStatus)
                UpdateWindowAcknowledgeChange(currentStatus);


            switch(currentStatus)
            {
                case FreeLanInstallationStatus.OK:
                    SetFindFreeLanButtonVisibility(false);
                    SetFreeLanLocationTabControlVisibility(false);
                    break;
            }

            FreeLanInstallationStatus = currentStatus;
            
        }

        private void UpdateWindowAcknowledgeChange(FreeLanInstallationStatus newStatus)
        {
            switch (newStatus)
            {
                case FreeLanInstallationStatus.OK:
                    MessageBox.Show("FreeLan found! You can close this window now.", "Ok!");
                    break;
            }

        }

        

    }
}
