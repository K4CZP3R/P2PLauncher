using P2PLauncher.Model;
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
    public class NetworkServicesViewModel : INotifyPropertyChanged
    {
        private readonly NetworkAdapters _networkAdapters;
        public NetworkServicesViewModel()
        {
            this._networkAdapters = new NetworkAdapters();
            QueryNetworkAdapters();
        }

        private void QueryNetworkAdapters()
        {
            NetworkAdapterListItems = new List<NetworkAdapterListItem>();
            foreach(NetworkAdapter adapter in this._networkAdapters.GetNetworkAdapters())
            {
                NetworkAdapterListItems.Add(new NetworkAdapterListItem(adapter));
            }

        }

        private List<NetworkAdapterListItem> networkAdapterListItems;
        public List<NetworkAdapterListItem> NetworkAdapterListItems
        {
            get { return networkAdapterListItems; }
            set
            {
                networkAdapterListItems = value;
                OnPropertyChanged(EnvHelper.GetMemberName(() => NetworkAdapterListItems));
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
