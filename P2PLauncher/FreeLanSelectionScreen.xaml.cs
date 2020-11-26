using P2PLauncher.Exceptions;
using P2PLauncher.Modules;
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

namespace P2PLauncher
{
    /// <summary>
    /// Interaction logic for FreeLanSelectionScreen.xaml
    /// </summary>
    public partial class FreeLanSelectionScreen : Window
    {

        private FreeLanModule FreeLanModule;
        public FreeLanSelectionScreen()
        {
            InitializeComponent();
            FreeLanModule = new FreeLanModule();
        }

        private void DefaultControlsState()
        {
            freeLanTabs.Visibility = Visibility.Hidden;
            StatusDescription.Content = "FreeLan location check...";
        }

        private void LocateFreeLanControlsState()
        {
            freeLanTabs.Visibility = Visibility.Visible;
            StatusDescription.Content = "FreeLan not found, you can either find it manually or download it.";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultControlsState();

            switch (FreeLanModule.GetStatus())
            {
                case Models.FreeLanStatus.READY_TO_START:
                    StatusDescription.Content = "FreeLan location found! You can close this window!";
                    break;
                case Models.FreeLanStatus.EXECUTABLE_UNKNOWN:
                    StatusDescription.Content = "FreeLan location is not known, searching known locations...";
                    TryToFindFreeLan();
                    break;
            }
        }

        private void TryToFindFreeLan()
        {
            try
            {
                FreeLanModule.TryFindFreeLanInstallation();
                StatusDescription.Content = $"Found FreeLan!";
            }
            catch(FreeLanInstallationNotFoundException)
            {
                LocateFreeLanControlsState();
            }
        }

    }
}
