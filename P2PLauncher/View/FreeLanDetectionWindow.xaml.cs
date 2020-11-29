using P2PLauncher.Services;
using P2PLauncher.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for FreeLanDetectionWindow.xaml
    /// </summary>
    public partial class FreeLanDetectionWindow : Window
    {
        public FreeLanDetectionWindow()
        {
            InitializeComponent();

            DataContext = new FreeLanDetectionViewModel(new WinDialogService(), new WinFileService());
        }
    }
}
