using P2PLauncher.Model;
using P2PLauncher.Services;
using P2PLauncher.Services.Commands;
using P2PLauncher.Utils;
using P2PLauncher.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PLauncher.ViewModel
{
    public class FreeLanStatusViewModel : INotifyPropertyChanged
    {
        private readonly FreeLanDetection _freeLanDetection;
        private readonly IFileService _fileService;

        public FreeLanStatusViewModel(IFileService fileService)
        {
            this._fileService = fileService;
            this._freeLanDetection = new FreeLanDetection(_fileService);

            QueryInstallationStatus();
        }

        private Visibility openDetectionWindowVisibility = Visibility.Hidden;
        public Visibility OpenDetectionWindowVisibility
        {
            get { return openDetectionWindowVisibility; }
            set
            {
                openDetectionWindowVisibility = value;
                OnPropertyChanged(EnvHelper.GetMemberName(() => OpenDetectionWindowVisibility));
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


        private void QueryInstallationStatus()
        {
            InstallationStatus = _freeLanDetection.GetInstallationStatus();
        }

        public string InstallationStatusStr { get; set; }


        private RelayCommand _showDetectionWindowCommand;
        public RelayCommand ShowDetectionWindowCommand
        {
            get
            {
                return _showDetectionWindowCommand ??
                    (_showDetectionWindowCommand = new RelayCommand(obj =>
                    {
                        (new FreeLanDetectionWindow()).ShowDialog();
                        QueryInstallationStatus(); //Happens after close of window.

                    }));
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            AdjustUserInterface(propertyName);
        }

        private void AdjustUserInterface(string propertyName)
        {
            if (propertyName == EnvHelper.GetMemberName(() => InstallationStatus))
            {
                if (InstallationStatus != FreeLanInstallationStatus.OK)
                {
                    OpenDetectionWindowVisibility = Visibility.Visible;
                }
                else
                {
                    OpenDetectionWindowVisibility = Visibility.Hidden;
                }
            }
        }
    }
}
