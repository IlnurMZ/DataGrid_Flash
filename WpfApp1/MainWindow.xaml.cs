using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
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
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace FlashViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _path = ""; // путь открытия файла
        byte[] _bytesFile; // массив байт из файла
        int _id;
        List<string> _parametrs = new(); // массив для данных из конфигурационного файла

        public MainWindow()
        {
            InitializeComponent();           
        }

        public static DateTime ConvertTime(Byte[] array) // Вывод времени
        {
            string dateTime = Convert.ToHexString(array);
            int second = 0;
            int minute = 0;
            int hour = 0; 
            int day = 0; 
            int month = 0;
            int year = 0;
            DateTime date = new DateTime();
            //int[] intArray = dateTime.Select(x => x - '0').ToArray();

            //for (int i = 0, j = 0; i < intArray.Length; i = i + 2, j++)
            //{
            //    int value1 = intArray[i];
            //    int value2 = intArray[i + 1];
            //    intArray[j] = value1 * 10 + value2;
            //}
            try
            {
                second = int.Parse(dateTime[0].ToString() + dateTime[1].ToString());
                minute = int.Parse(dateTime[2].ToString() + dateTime[3].ToString());
                hour = int.Parse(dateTime[4].ToString() + dateTime[5].ToString());
                day = int.Parse(dateTime[6].ToString() + dateTime[7].ToString());
                month = int.Parse(dateTime[8].ToString() + dateTime[9].ToString());
                year = 2000 + int.Parse(dateTime[10].ToString() + dateTime[11].ToString());
                date = new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return date;
        }

        private void MenuItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();            

            if (openFileDialog.ShowDialog() == true)
                _path = openFileDialog.FileName;

            if (string.IsNullOrWhiteSpace(_path))
            {
                return;
            }

            ReadBytesFromFile(_path);

            _id = _bytesFile[1];

            LoadConfigFile(_id);

            LoadData();

            EditTable(_id);
        }

        private void EditTable(int id)
        {
            ObservableCollection<DataGridColumn> columns = datagrid1.Columns;

            foreach (DataGridColumn col in columns)
            {
                switch (col.Header.ToString())
                {
                    case "numPacket":
                        col.Header = "id пакета";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "time":
                        col.Header = "Дата";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "GK1":
                        col.Header = "ННК1/ННК1(вода)";
                        //col.Width = 180;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "GK2":
                        col.Header = "ННК2/ННК2(вода)";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "GK3":
                        col.Header = "НГК/НГК(вода)";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "GK4":
                        col.Header = "ННК1 [ед]";
                        //col.Width = 180;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "GK5":
                        col.Header = "ННК2 [ед]";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "GK6":
                        col.Header = "НГК [ед]";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "temperatura":
                        col.Header = "Температура НГК [°С]";
                        //col.Width = 180;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "periodSHIM":
                        col.Header = "Период ШИМ ННК";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "currentWorkTime":
                        col.Header = "Текущая наработка";
                        //col.Width = 180;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "totalWorkTime":
                        col.Header = "Общая наработка";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "timeNakop":
                        col.Header = "Время накопления [сек]";
                        //col.Width = 200;
                        col.Visibility = Visibility.Visible;
                        break;
                    case "error":
                        col.Header = "Ошибка I2C";
                        //col.Width = 180;
                        col.Visibility = Visibility.Visible;
                        break;
                    default:
                        col.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void LoadData()
        {
            List<NNGK> list = new();

            double gkcf = double.Parse(_parametrs[0]);
            double Ubatcf = double.Parse(_parametrs[1]);
            double tcf = double.Parse(_parametrs[2]);
            double timecf = double.Parse(_parametrs[3]);

            int rowCount = _bytesFile.Length / 34;
            int pos = 2; // позиция в массиве байтов. с 2 позиции начинается номер пачки

            for (int i = 0; i < rowCount; i++) // i<rowCount
            {
                NNGK line = new NNGK();

                line.numPacket = (_bytesFile[pos]) | (_bytesFile[pos + 1] << 8); // заполнение номера пачки
                pos = pos + 2;

                byte[] dateArray = new byte[6];
                Array.Copy(_bytesFile, pos, dateArray, 0, 6);
                line.time = ConvertTime(dateArray).ToString("HH:mm:ss dd.MM.yy"); // вывод даты
                pos = pos + 6;

                double gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                line.GK1 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                line.GK2 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                line.GK3 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                line.GK4 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                line.GK5 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                line.GK6 = (_bytesFile[pos]) | (_bytesFile[pos + 1] << 8); // вывод ГК ед                
                pos = pos + 2;

                line.temperatura = (double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / Ubatcf; // вывод Ubat
                pos = pos + 2;

                line.periodSHIM = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / tcf, 1); // вывод темп
                pos = pos + 2;

                line.currentWorkTime = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / timecf, 1); // вывод тек наработки
                pos = pos + 2;

                line.totalWorkTime = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / timecf, 1); // вывод общ наработки
                pos = pos + 2;

                line.timeNakop = _bytesFile[pos] * 5; // вывод времени накопления
                pos = pos + 1;

                line.error = _bytesFile[pos]; // вывод ошибка
                pos = pos + 5;
                list.Add(line);
                //datagrid1.Items.Add(line);
            }

            datagrid1.ItemsSource = list;

            
            
        }

        private void ReadBytesFromFile(string path)
        {
            try
            {
                _bytesFile = File.ReadAllBytes(path); // первые 384 байта лишние               
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла ошибка считывания файла. " + e.Message);
                return;
            }

            if (_bytesFile.Length > 386)
            {
                _bytesFile = _bytesFile.Skip(384).ToArray();
            }
            else
            {
                MessageBox.Show("Возникла ошибка с данными файла");
            }
        }

        private void LoadConfigFile(int id)
        {
            string path; // путь к файлу, который выбирается по id прибора
            if (id > 0 && id <= 9)
            {
                path = id switch
                {
                    9 => "Configurations\\mycongf.cfg",
                    6 => "Configurations\\mycongf.cfg",
                    _ => "error"
                };
            }
            else
            {
                return;
            }

            try
            {
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        
                        if (!string.IsNullOrWhiteSpace(row))
                        {
                            string[] line = row.Split(" ");
                            _parametrs.Add(line[0]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
    }

    //class Phone
    //{
    //    public string Name;
    //    int id;
    //    double Price;
    //}
    
}
