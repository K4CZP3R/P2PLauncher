using P2PLauncher.Model;
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
    /// Interaction logic for WindowsServicesWindow.xaml
    /// </summary>
    public partial class WindowsServicesWindow : Window, IWindow
    {
        private readonly WindowsServices windowsServices;

        private readonly List<WindowsService> windowsServicesEnabled = new List<WindowsService>();
        private readonly List<WindowsService> windowsServicesDisabled= new List<WindowsService>();


        public WindowsServicesWindow()
        {
            InitializeComponent();
            windowsServices = new WindowsServices();

            UpdateWindow();
        }

        void MoveFromEnabledToDisabled(WindowsService item)
        {
            windowsServicesEnabled.Remove(item);
            windowsServicesDisabled.Add(item);
            UpdateAdaptersList();
        }
        void MoveFromDisabledToEnabled(WindowsService item)
        {
            windowsServicesDisabled.Remove(item);
            windowsServicesEnabled.Add(item);
            UpdateAdaptersList();
        }



        private void OnMoveToDisabledButton(object sender, RoutedEventArgs e)
        {
            int indexToMove = ListBoxWindowsServicesOn.SelectedIndex;
            if (indexToMove == -1)
                return;

            WindowsService item = windowsServicesEnabled[indexToMove];
            MoveFromEnabledToDisabled(item);
        }
        private void OnMoveToEnabledButton(object sender, RoutedEventArgs e)
        {
            int indexToMove = ListBoxWindowsServicesOff.SelectedIndex;
            if (indexToMove == -1)
                return;

            WindowsService item = windowsServicesDisabled[indexToMove];
            MoveFromDisabledToEnabled(item);

        }

        private void UpdateAdaptersList()
        {
            ListBoxWindowsServicesOff.ItemsSource = new List<Object>();
            ListBoxWindowsServicesOn.ItemsSource = new List<Object>();
            ListBoxWindowsServicesOn.ItemsSource = windowsServicesEnabled;
            ListBoxWindowsServicesOff.ItemsSource = windowsServicesDisabled;

            windowsServices.SaveServicesToDisable(windowsServicesDisabled);

        }

        public void UpdateWindow()
        {
            List<WindowsService> services = windowsServices.GetServices();
            string[] servicesToDisable = windowsServices.GetServiceNamesToDisable();
            
            windowsServicesEnabled.Clear();
            windowsServicesDisabled.Clear();
            foreach (WindowsService service in services)
            {
                if (servicesToDisable.Contains(service.Name))
                {
                    windowsServicesDisabled.Add(service);
                }
                else
                {
                    windowsServicesEnabled.Add(service);
                }
            }
            UpdateAdaptersList();


        }

    }
}
