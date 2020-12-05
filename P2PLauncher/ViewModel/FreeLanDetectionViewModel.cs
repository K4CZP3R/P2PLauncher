using P2PLauncher.Model;
using P2PLauncher.Services;
using P2PLauncher.Services.Commands;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PLauncher.ViewModel
{
    public class FreeLanDetectionViewModel : INotifyPropertyChanged
    {
        public string SelectedPath { get; set; } = "Select path...";


        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        //private readonly FreeLanDetection freeLanDetection;


        private string installationInfo;
        public string InstallationInfo
        {
            get { return installationInfo; }
            set
            {
                installationInfo = value;
                OnPropertyChanged(EnvHelper.GetMemberName(() => InstallationInfo));
            }
        }

        private FreeLanInstallationStatus installationStatus;
        public FreeLanInstallationStatus InstallationStatus
        {
            get { return installationStatus; }
            set
            {
                installationStatus = value;
                InstallationStatusStr = EnumHelper.GetDescription(value);
                OnPropertyChanged(EnvHelper.GetMemberName(() => InstallationStatus));
                OnPropertyChanged(EnvHelper.GetMemberName(() => InstallationStatusStr));

            }
        }

        public string InstallationStatusStr { get; set; }

        private Visibility tabVisibility = Visibility.Hidden;
        public Visibility TabVisibility
        {
            get
            {
                return tabVisibility;
            }
            set
            {
                tabVisibility = value;
                OnPropertyChanged(EnvHelper.GetMemberName(() => TabVisibility));
            }
        }

        private Visibility autoSearchVisibility = Visibility.Hidden;
        public Visibility AutoSearchVisibility
        {
            get { return autoSearchVisibility; }
            set
            {
                autoSearchVisibility = value;
                OnPropertyChanged(EnvHelper.GetMemberName(() => AutoSearchVisibility));
            }
        }

        private string downloadHelp;
        public string DownloadHelp
        {
            get { return downloadHelp; }
            set
            {
                downloadHelp = value;
                OnPropertyChanged(EnvHelper.GetMemberName(() => DownloadHelp));
            }
        }


        public FreeLanDetectionViewModel(IDialogService dialogService, IFileService fileService)
        {
            this._dialogService = dialogService;
            this._fileService = fileService;
            //this.freeLanDetection = new FreeLanDetection(_fileService);

            QueryInstallationStatus();
            QueryUpdateHelp();



        }


        private RelayCommand _showDownloadSectionCommand;
        public RelayCommand ShowDownloadSectionCommand
        {
            get
            {
                return _showDownloadSectionCommand ??
                    (_showDownloadSectionCommand = new RelayCommand(obj =>
                    {
                        //System.Diagnostics.Process.Start(freeLanDetection.GetDownloadUrl());

                    }));
            }
        }

        private RelayCommand _findFreeLanCommand;
        public RelayCommand FindFreeLanCommand
        {
            get
            {
                return _findFreeLanCommand ??
                  (_findFreeLanCommand = new RelayCommand(obj =>
                  {
                      //if (!freeLanDetection.FindFreeLan())
                      //{
                      //    _dialogService.ShowMessage("Could not locate FreeLan, select it manually.", "FreeLan not found");
                      //}
                      QueryInstallationStatus();

                  }));
            }
        }

        private RelayCommand _selectFreeLanPath;
        public RelayCommand SelectFreeLanPath
        {
            get
            {
                return _selectFreeLanPath ??
                    (_selectFreeLanPath = new RelayCommand(obj =>
                    {
                        if (_dialogService.OpenFileDialog())
                        {
                        //    freeLanDetection.SetFreelanPath(_dialogService.FilePath);
                        }
                        QueryInstallationStatus();

                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            AdjustUserInterface(propertyName);
        }

        private void QueryInstallationStatus()
        {
            //InstallationStatus = freeLanDetection.GetInstallationStatus();
        }
        private void QueryUpdateHelp()
        {
            string bitValue = EnvHelper.Is64Bit() ? "x64-bit" : "x86-bit";
            DownloadHelp = $"You need to download {bitValue} version of FreeLan.";
        }

        public void AdjustUserInterface(string propertyName = null)
        {
            if (propertyName == null) return;

            switch (propertyName)
            {
                case "InstallationStatus":
                    AdjustInstallationStatus();
                    break;
            }

        }

        public void AdjustInstallationStatus()
        {
            switch (InstallationStatus)
            {
                case FreeLanInstallationStatus.CONFIG_NOT_SET:
                    TabVisibility = Visibility.Visible;
                    AutoSearchVisibility = Visibility.Visible;
                    InstallationInfo = "You can use automatic detection or locate it manually / download it";
                    _dialogService.ShowMessage("FreeLan path is unknown, please define it.", "Installation status");
                    break;
                case FreeLanInstallationStatus.INVALID_PATH:
                    AutoSearchVisibility = Visibility.Visible;
                    TabVisibility = Visibility.Visible;
                    InstallationInfo = "You can use automatic detection or locate it manually / download it";
                    _dialogService.ShowMessage("Current FreeLan path is invalid, please redefine it.", "Installation status");
                    break;
                case FreeLanInstallationStatus.OK:
                    TabVisibility = Visibility.Hidden;
                    AutoSearchVisibility = Visibility.Hidden;
                    InstallationInfo = "You are done here! You can close this window now.";
                    break;
            }

        }
    }
}
