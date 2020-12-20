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

        private readonly List<NetworkAdapter> NetworkAdaptersEnabled = new List<NetworkAdapter>();
        private readonly List<NetworkAdapter> NetworkAdaptersDisabled = new List<NetworkAdapter>();

        void MoveFromEnabledToDisabled(NetworkAdapter item)
        {
            NetworkAdaptersEnabled.Remove(item);
            NetworkAdaptersDisabled.Add(item);
            UpdateAdaptersList();
        }
        void MoveFromDisabledToEnabled(NetworkAdapter item)
        {
            NetworkAdaptersDisabled.Remove(item);
            NetworkAdaptersEnabled.Add(item);
            UpdateAdaptersList();
        }


        public NetworkAdaptersWindow()
        {
            InitializeComponent();

            networkAdapters = new NetworkAdapters();

            UpdateWindow();


        }

        private void OnMoveToDisabledButton(object sender, RoutedEventArgs e)
        {
            int indexToMove = ListBoxNetworkAdaptersOn.SelectedIndex;
            if (indexToMove == -1)
                return;

            NetworkAdapter item = NetworkAdaptersEnabled[indexToMove];
            MoveFromEnabledToDisabled(item);
        }
        private void OnMoveToEnabledButton(object sender, RoutedEventArgs e)
        {
            int indexToMove = ListBoxNetworkAdaptersOff.SelectedIndex;
            if (indexToMove == -1)
                return;

            NetworkAdapter item = NetworkAdaptersDisabled[indexToMove];
            MoveFromDisabledToEnabled(item);

        }

        private void UpdateAdaptersList()
        {
            ListBoxNetworkAdaptersOff.ItemsSource = new List<Object>();
            ListBoxNetworkAdaptersOn.ItemsSource = new List<Object>();
            ListBoxNetworkAdaptersOn.ItemsSource = NetworkAdaptersEnabled;
            ListBoxNetworkAdaptersOff.ItemsSource = NetworkAdaptersDisabled;

            networkAdapters.SaveAdaptersToDisable(NetworkAdaptersDisabled);

        }

        public void UpdateWindow()
        {
            List<NetworkAdapter> adapters = networkAdapters.GetNetworkAdapters();
            string[] adaptersToDisable = networkAdapters.GetAdapterNamesToDisable();
            adapters.RemoveAt(new Random().Next(0, 3));

            NetworkAdaptersEnabled.Clear();
            foreach (NetworkAdapter adapter in adapters)
            {
                if (adaptersToDisable.Contains(adapter.Name))
                {
                    NetworkAdaptersDisabled.Add(adapter);
                }
                else
                {
                    NetworkAdaptersEnabled.Add(adapter);
                }
            }
            UpdateAdaptersList();


        }
    }
}
