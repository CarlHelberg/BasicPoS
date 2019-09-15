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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointOfSale
{
    /// <summary>
    /// Interaction logic for InOut.xaml
    /// </summary>
    public partial class InOut : Page
    {
        string _displayname = Displayname.Instance.Name;
        public InOut()
        {
            InitializeComponent();
            UsernameBlock.Text = "Current user: " + Displayname.Instance.Name;
        }

        private void StockInBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService stockIn = NavigationService.GetNavigationService(this);
            stockIn.Navigate(new Uri("StockIn.xaml", UriKind.Relative));
        }
        private void StockOutBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService stockOut = NavigationService.GetNavigationService(this);
            stockOut.Navigate(new Uri("StockOut.xaml", UriKind.Relative));
        }
    }
}
