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
    public partial class NetworkAdaptersWindow : Window
    {
        public NetworkAdaptersWindow()
        {
            InitializeComponent();

            DataContext = new NetworkServicesViewModel();
        }
    }
}
