using System;
using System.Collections;
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
        DataTable table = new DataTable();
        public MainWindow()
        {
            InitializeComponent();

            //grid1.Datas = dt;
            
            //table = MakeNamesTable();
            

            // Once a table has been created, use the
            // NewRow to create a DataRow.


            // Then add the new row to the collection.
            //row["fName"] = "John";
            //row["lName"] = "Smith";


            //foreach (DataColumn column in table.Columns)
            //    Console.WriteLine(column.ColumnName);

            Push();

        }

        public void Push()
        {
            DataRow row;
            for (int i = 0; i < 3; i++)
                table.Columns.Add("col" + i.ToString());

            for (int i = 0; i < 4; i++)
            {
                row = table.NewRow();
                row[0] = "A";
                row[1] = "B";
                row[2] = "C";
                table.Rows.Add(row);
            }
            datagrid1.DataContext = table;
        }


        private DataTable MakeNamesTable()
        {
            // Create a new DataTable titled 'Names.'
            DataTable namesTable = new DataTable("Names");

            // Add three column objects to the table.
            DataColumn idColumn = new DataColumn();
            idColumn.DataType = System.Type.GetType("System.Int32");
            idColumn.ColumnName = "id";
            idColumn.AutoIncrement = true;
            namesTable.Columns.Add(idColumn);

            DataColumn fNameColumn = new DataColumn();
            fNameColumn.DataType = System.Type.GetType("System.String");
            fNameColumn.ColumnName = "Fname";
            fNameColumn.DefaultValue = "Fname";
            namesTable.Columns.Add(fNameColumn);

            DataColumn lNameColumn = new DataColumn();
            lNameColumn.DataType = System.Type.GetType("System.String");
            lNameColumn.ColumnName = "LName";
            namesTable.Columns.Add(lNameColumn);

            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = idColumn;
            namesTable.PrimaryKey = keys;

            // Return the new DataTable.
            return namesTable;
        }


    }


}
