using P2PLauncher.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using P2PLauncher.Services;

namespace P2PLauncher.View
{
    /// <summary>
    /// Interaction logic for NetworkAdaptersWindow.xaml
    /// </summary>
    public partial class NetworkAdaptersWindow : Window, IWindow
    {
        private readonly NetworkAdapters _networkAdapters;
        private readonly AdapterService _adapterService;
        private readonly List<NetworkAdapterListItem> _networkAdaptersList = new List<NetworkAdapterListItem>();
        public NetworkAdaptersWindow()
        {
            InitializeComponent();

            _networkAdapters = new NetworkAdapters();
            _adapterService = new AdapterService(_networkAdapters);
            
            UpdateWindow();

            
        }

        /// <summary>
        /// Action to be performed on Save button click
        /// </summary>
        /// <param name="sender">.NET Framework related object</param>
        /// <param name="e">.NET Framework related object</param>
        private void OnSaveButton(object sender, RoutedEventArgs e)
        {
            UpdateAdaptersList();
        }
        /// <summary>
        /// Action to be performed on Change state button click
        /// </summary>
        /// <param name="sender">.NET Framework related object.</param>
        /// <param name="e">.NET Framework related object.</param>
        private void OnFlipCurrentAdapterButton(object sender, RoutedEventArgs e)
        {
            int indexToFlip = ListBoxNetworkAdapters.SelectedIndex;
            if (indexToFlip == -1)
                return;
            _networkAdaptersList[indexToFlip].FlipStatus();
            UpdateAdaptersList();

        }

        /// <summary>
        /// Updates list of adapters to disable while running FreeLan.
        /// (Saves ID of connections which are marked)
        /// Then (re)loads adapter list with the changes.
        /// </summary>
        private void UpdateAdaptersList()
        {
            List<NetworkAdapter> toSave = new List<NetworkAdapter>();
            foreach (NetworkAdapterListItem item in _networkAdaptersList)
            {
                if(item.Marked)
                    toSave.Add(item.NetworkAdapter);
            }
            
            _adapterService.SaveAdaptersToDisable(toSave);
            
            LoadAdaptersList();
            



        }

        /// <summary>
        /// Loads adapters list and shows it in the window.
        /// Gets all adapters and adapters which are marked as disabled.
        /// 
        /// </summary>
        public void LoadAdaptersList()
        {
            List<NetworkAdapter> allAdapters = _networkAdapters.GetNetworkAdapters();
            List<NetworkAdapter> toDisableAdapters = _adapterService.GetAdaptersToDisable();
            
            
            _networkAdaptersList.Clear();
            foreach (NetworkAdapter networkAdapter in allAdapters)
            {
                bool isDisabled = false;
                for(int i = 0; i < toDisableAdapters.Count; i++)
                {
                    if (networkAdapter.ID.Equals(toDisableAdapters[i].ID))
                        isDisabled = true;

                }
                
                _networkAdaptersList.Add(new NetworkAdapterListItem(networkAdapter, isDisabled));
            }
            
            ListBoxNetworkAdapters.ItemsSource = new List<Object>();
            ListBoxNetworkAdapters.ItemsSource = _networkAdaptersList;   
        }

        /// <summary>
        /// Action performed on window show up.
        /// </summary>
        public void UpdateWindow()
        {
            LoadAdaptersList();
        }
    }
}