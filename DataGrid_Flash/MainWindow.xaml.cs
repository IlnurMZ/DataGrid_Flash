using System;
using System.Collections.Generic;
using System.Data;
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

namespace DataGrid_Flash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var dt = new DataTable();

            for (int i = 0; i < 3; i++)
                dt.Columns.Add("col" + i.ToString());

            for (int i = 0; i < 3; i++)
            {
                DataRow r = items.NewRow();
                r[0] = "a" + i.ToString();
                r[1] = "b" + i.ToString();
                r[2] = "c" + i.ToString();
                dt.Rows.Add(r);
            }

            myGrid.ItemsSource = dt;
            //https://stackoverflow.com/questions/15655271/dynamically-add-columns-to-datagrid-in-wpf
        }



    }
 
}
