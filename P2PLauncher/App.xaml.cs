using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace P2PLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }


        /// <summary>
        /// Catch any unhandled exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMsg = String.Format("An unhandled exception occurred: {0}", e.Exception);
            MessageBox.Show(errorMsg, "Unhandled Exception!", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;

        }
    }
}
