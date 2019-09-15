using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Bytescout.Spreadsheet;

namespace PointOfSale
{
    /// <summary>
    /// Interaction logic for StockIn.xaml
    /// </summary>
    public partial class StockIn : Page
    {
        public StockIn()
        {
            
            InitializeComponent();
            var displayname = Displayname.Instance;
            UsernameBlock.Text = "";
            UsernameBlock.Text = "Current User: " + displayname.Name ;
            
            StockInListBox.ItemsSource = CreateInventory();
            
        }

        private void MouseEnterSeaerchBox(object sender, MouseEventArgs e)
        {
            if(SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
            }
        }

        private void MouseLeaveSearchBox(object sender, MouseEventArgs e)
        {
            if (SearchBox.Text == "")
            {
                SearchBox.Text = "Search...";
            }
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            string searchInput = SearchBox.Text;
            List<Product> searchResult = new List<Product>();
            if (searchInput != null) { 
                foreach (Product item in CreateInventory())
                {
                    if (searchInput.ToLower() == item.ProductName.ToLower() || searchInput.ToLower() == item.ProductID.ToLower()
                        || searchInput.ToLower() == item.PruductCatagory.ToLower())
                    {
                        searchResult.Add(item);
                    }
                }
                if (searchResult.Count == 0)
                {
                    MessageBox.Show("Sorry! No results were found that matches your search query");
                    StockInListBox.ItemsSource = CreateInventory();
                }
                else if (searchResult.Count > 0)
                {
                    StockInListBox.ItemsSource = searchResult;
                }
            }
        }

        private void MouseEnterQtyAddBox(object sender, MouseEventArgs e)
        {
            TextBox lbi = sender as TextBox;
            
            if (lbi.Text == "0")
            {
                lbi.Text = "";
            }
        }

        private void MouseLeaveQtyAddBox(object sender, MouseEventArgs e)
        {
            TextBox lbi = sender as TextBox;

            if (lbi.Text == "")
            {
                lbi.Text = "0";
            }
        }

        // create List for inventory
        public List<Product> CreateInventory ()
        {
            List<Product> Products = new List<Product>();
            FileStream stream = File.Open(@"C:\Users\Stitchdirect\source\repos\PointOfSale\PointOfSale\bin\Debug\ProductList.xlsx", FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader;
            if (System.IO.Path.GetExtension(@"ProductList.xlsx").ToUpper() == ".XLS")
            {
                //1.1 Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                //1.2 Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            DataSet result = excelReader.AsDataSet();
            DataTable dt = result.Tables[0];

            string _id = "";
            string _name = "";
            string _catagory = "";
            string _price = "";
            string _inStock = "";

            for (int i = 1; i < result.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < result.Tables[0].Columns.Count; j++)
                {

                    if (j == 0)
                        _id = (dt.Rows[i][j]).ToString();
                    else if (j == 1)
                        _name = (dt.Rows[i][j]).ToString();
                    else if (j == 2)
                        _catagory = (dt.Rows[i][j]).ToString();
                    else if (j == 3)
                        _price = (dt.Rows[i][j]).ToString();
                    else if (j == 4)
                        _inStock = (dt.Rows[i][j]).ToString();
                }
                Products.Add(new Product(_catagory, _price, _name, _id, _inStock));
            }
            stream.Close();
            return Products;
        }

        //Add stock to inventory
        private void StockBookinBtn(object sender, RoutedEventArgs e)
        {

            ListBoxItem myListBoxItem = (ListBoxItem)(StockInListBox.ItemContainerGenerator.ContainerFromItem(StockInListBox.Items.CurrentItem));
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            TextBlock itemID = (TextBlock)myDataTemplate.FindName("idBox", myContentPresenter);
            TextBox qtyToAdd = (TextBox)myDataTemplate.FindName("QtyAddBox", myContentPresenter);
            TextBox qtyinstock = (TextBox)myDataTemplate.FindName("QtyInStock", myContentPresenter);
            string qtyAdd = qtyToAdd.Text;
            
            Spreadsheet document = new Spreadsheet();
            document.LoadFromFile("ProductList.xlsx");
            Worksheet worksheet = document.Workbook.Worksheets.ByName("Sheet1");
            int maxRows = worksheet.UsedRangeRowMax;
            for (int i = 1; i < maxRows ; i++)
            {
                    if (itemID.Text == (worksheet.Cell(i , 0).Value.ToString()))
                    {
                        int toAdd = Int32.Parse(qtyAdd);
                        try
                        {
                            int ParsedValue = Int32.Parse(worksheet.Cell(i, 4).Value.ToString());
                            worksheet.Cell(i, 4).Value = (ParsedValue + toAdd).ToString();
                            document.SaveAs("ProductList.xlsx");
                            document.Close();
                            StockInListBox.ItemsSource = CreateInventory(); ;
                    }
                        catch
                        {
                            MessageBox.Show("the value entered is NaN");
                        }
                    
                }
            }

        }
    

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void LogoutButtonclk(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("UserLogin.xaml", UriKind.Relative));
        }

        private void NavigateToSell(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("StockOut.xaml", UriKind.Relative));
        }

        private void NavigateToBookIn(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("StockIn.xaml", UriKind.Relative));
        }
    }
}
