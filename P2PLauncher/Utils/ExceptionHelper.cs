using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PLauncher.Utils
{
    public static class ExceptionHelper
    {
        public static void ShowMessageBox(Exception ex)
        {
            MessageBox.Show($"The following error has occured:\n '{ex.Message}' ", "Something went wrong!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
