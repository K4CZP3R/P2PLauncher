using P2PLauncher.Model;
using P2PLauncher.ViewModel;
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
    /// Interaction logic for NetworkAdaptersWindow.xaml
    /// </summary>
    public partial class NetworkAdaptersWindow : Window, IWindow
    {
        private readonly NetworkAdapters networkAdapters;
        private readonly List<NetworkAdapterListItem> NetworkAdaptersList = new List<NetworkAdapterListItem>();
        public NetworkAdaptersWindow()
        {
            InitializeComponent();

            networkAdapters = new NetworkAdapters();

            UpdateWindow();

            
        }

        private void OnFlipCurrentAdapterButton(object sender, RoutedEventArgs e)
        {
            int indexToFlip = ListBoxNetworkAdapters.SelectedIndex;
            if (indexToFlip == -1)
                return;
            NetworkAdaptersList[indexToFlip].FlipStatus();
            UpdateAdaptersList();

        }

        private void UpdateAdaptersList()
        {
            ListBoxNetworkAdapters.ItemsSource = new List<Object>();
            ListBoxNetworkAdapters.ItemsSource = NetworkAdaptersList;

        }

        public void UpdateWindow()
        {
            List<NetworkAdapter> adapters = networkAdapters.GetNetworkAdapters();
            adapters.RemoveAt(new Random().Next(0, 3));
            
            NetworkAdaptersList.Clear();
            foreach (NetworkAdapter adapter in adapters)
            {
                NetworkAdaptersList.Add(new NetworkAdapterListItem(adapter));

            }


            UpdateAdaptersList();

            
        }
    }
}
