using P2PLauncher.Model;
using P2PLauncher.Services;
using P2PLauncher.Services.Commands;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
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
        private FreeLanDetection freeLanDetection;
        private string selectedPath;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;

        public string InstallationStatus { get; set; }

        public Visibility TabVisibility { get; set; } = Visibility.Hidden;
        public string SelectedPath
        {
            get { return selectedPath; }
            set
            {
                selectedPath = value;
                OnPropertyChanged("SelectedPath");
            }
        }


        public FreeLanDetection FreeLanDetection
        {
            get { return freeLanDetection; }
            set
            {
                freeLanDetection = value;
                OnPropertyChanged("FreeLanDetection");
            }
        }

        public FreeLanDetectionViewModel(IDialogService dialogService, IFileService fileService)
        {
            this._dialogService = dialogService;
            this._fileService = fileService;


            FreeLanDetection = new FreeLanDetection(_fileService);

            FreeLanDetection.GetInstallationStatus();


        }

        // Command to find freelan manually
        private RelayCommand _selectFreeLanCommand;
        public RelayCommand SelectFreeLanCommand
        {
            get
            {
                return _selectFreeLanCommand ??
                  (_selectFreeLanCommand = new RelayCommand(obj =>
                  {
                      this._dialogService.ShowMessage("Select freelan command!", "Relay!");
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
                            SelectedPath = _dialogService.FilePath;
                        }
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "FreeLanDetection")
                UpdateUI();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void UpdateUI()
        {

            if (EnumHelper.GetDescription(freeLanDetection.InstallationStatus).Equals(InstallationStatus))
                return;

            switch (freeLanDetection.InstallationStatus)
            {
                case FreeLanInstallationStatus.INVALID_EXECUTABLE:
                    _dialogService.ShowMessage("Selected FreeLan executable seems to be invalid, select a valid one.", "Installation status");
                    TabVisibility = Visibility.Visible;
                    break;
                case FreeLanInstallationStatus.INVALID_PATH:
                    _dialogService.ShowMessage("Selected FreeLan path is not valid, select a valid one.", "Installation status");
                    TabVisibility = Visibility.Visible;
                    break;
                case FreeLanInstallationStatus.CONFIG_NOT_SET:
                    _dialogService.ShowMessage("FreeLan path is not selected (yet). We will try to search for it.", "Installation status");
                    TabVisibility = Visibility.Visible;
                    break;
                case FreeLanInstallationStatus.OK:
                    TabVisibility = Visibility.Hidden;
                    break;
            }

            InstallationStatus = EnumHelper.GetDescription(freeLanDetection.InstallationStatus);


        }




    }
}
