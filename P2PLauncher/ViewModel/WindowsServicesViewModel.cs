using P2PLauncher.Model;
using P2PLauncher.Services.Commands;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.ViewModel
{
    public class WindowsServicesViewModel : INotifyPropertyChanged
    {
        private readonly WindowsServices _windowsServices;
        public WindowsServicesViewModel()
        {
            this._windowsServices = new WindowsServices();

            QueryWindowsServices();

        }

        private void QueryWindowsServices()
        {
            WindowsServicesListItem = new List<WindowsServiceListItem>();
            foreach(WindowsService service in this._windowsServices.GetServices())
            {
                WindowsServicesListItem.Add(new WindowsServiceListItem(service));
                
            }
        }

        private List<WindowsServiceListItem> windowsServicesListItem;
        public List<WindowsServiceListItem> WindowsServicesListItem
        {
            get { return windowsServicesListItem; }
            set
            {
                windowsServicesListItem = value;
                OnPropertyChanged(EnvHelper.GetMemberName(() => WindowsServicesListItem));
            }
        }

        private WindowsServiceListItem selectedWindowsServiceListItem;
        public WindowsServiceListItem SelectedWindowsServicesListItem
        {
            get { return selectedWindowsServiceListItem; }
            set
            {
                selectedWindowsServiceListItem = value;
                OnPropertyChanged(EnvHelper.GetMemberName(() => SelectedWindowsServicesListItem));
            }
        }

        private RelayCommand _flipStatusOfCurrentCommand;
        public RelayCommand FlipStatusOfCurrentCommand
        {
            get
            {
                return _flipStatusOfCurrentCommand ??
                    (_flipStatusOfCurrentCommand = new RelayCommand(obj =>
                    {
                        SelectedWindowsServicesListItem.FlipStatus();
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //AdjustUserInterface(propertyName);
        }
    }
}
