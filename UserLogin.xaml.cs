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

namespace PointOfSale
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Page
    {
        public bool loginSuccess = false;
       
        public UserLogin()
        {
            InitializeComponent();
        }

        

        private void Button_PreviewMouseLeftButtonDownLoginButton(object sender, MouseButtonEventArgs e)
        {

            FileStream stream = File.Open(@"C:\Users\Stitchdirect\source\repos\PointOfSale\PointOfSale\bin\Debug\Users.xlsx", FileMode.Open, FileAccess.Read);

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
            for(int i = 0; i< result.Tables[0].Rows.Count; i++)
            {
                if((dt.Rows[i][0]).ToString() == UsernameBlock.Text && (dt.Rows[i][1]).ToString() == PasswordBlock.Text)
                {
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns.Navigate(new Uri("InOut.xaml", UriKind.Relative));
                    var displayname = Displayname.Instance;
                    displayname.Name = UsernameBlock.Text;
                    stream.Close();
                    loginSuccess = true;
                }
                
            }
            if (loginSuccess == false)
                MessageBox.Show("Login details were incorrect, please try again");
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
