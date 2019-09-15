using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
namespace PointOfSale
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static int InvoiceNumber = 1;

    }
    
    
    public sealed class Displayname
    {
        Displayname()
        {
        }
        private static readonly object padlock = new object();
        private static Displayname instance = null;
        public string Name = "";
        public static Displayname Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Displayname();
                    }
                    return instance;
                }
            }
        }
    }
     public class Product : INotifyPropertyChanged
    {
        public string PruductCatagory { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string QtyInStock { get; set; }
        

        public Product(string _catagory,string _price, string _name, string _id,string _stock)
        {
            QtyInStock = _stock;
            Price = _price;
            ProductName = _name;
            ProductID = _id;
            PruductCatagory = _catagory;
        }

        // Updates Qty in stock when selling/booking in stock
        public string qtyInStockChange
        {
            get { return QtyInStock; }
            set
            {
                QtyInStock = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("qtyInStockChange");
            }
        }

        protected void OnPropertyChanged(string newQty)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(newQty));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
       
    }

}
