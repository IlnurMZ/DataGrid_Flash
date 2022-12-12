using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DataView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Phone> catalog = new List<Phone>();
        public MainWindow()
        {
            InitializeComponent();

            //DataGrid data2 = new DataGrid();

            //DataGridTextColumn c1 = new DataGridTextColumn();
            //c1.Header = "Заголовок1";            
            //c1.Width = 140;
            //data2.Columns.Add(c1);

            //DataGridTextColumn c2 = new DataGridTextColumn();
            //c2.Header = "Заголовок2";
            //c2.Width = 140;            
            //data2.Columns.Add(c2);

            //DataGridTextColumn c3 = new DataGridTextColumn();
            //c3.Header = "Заголовок3";
            //c3.Width = 140;           
            //data2.Columns.Add(c3);

            //grid1.Children.Add(data2);

            //grid1.ite

            catalog.Add(new Phone { Name = "S22", Compania = new Company { Title = "Samsung" }, Price = 1000 });
            catalog.Add(new Phone { Name = "S23", Compania = new Company { Title = "Samsung" }, Price = 2000 });
            catalog.Add(new Phone { Name = "S24", Compania = new Company { Title = "Samsung" }, Price = 3000 });
            catalog.Add(new Phone { Name = "S25", Compania = new Company { Title = "Samsung" }, Price = 4000 });
            catalog.Add(new Phone { Name = "S26", Compania = new Company { Title = "Samsung" }, Price = 5000 });            

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            phonesGrid.ItemsSource = catalog;

            ObservableCollection<DataGridColumn> columns = phonesGrid.Columns;
            foreach (DataGridColumn col in columns)
            {
                switch (col.Header.ToString())
                {
                    case "Name":
                        col.Header = "Название";
                        //col.Ca
                        col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "Compania":
                        col.Header = "Компания";
                        col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "Price":
                        col.Header = "Цена";
                        col.Width = 180;
                        col.Visibility = Visibility.Visible;
                        break;                   
                    default:
                        col.Visibility = Visibility.Collapsed;
                        break;
                }
            }




        }
    }
}
