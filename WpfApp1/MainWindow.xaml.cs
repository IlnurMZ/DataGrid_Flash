using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using FlashViewer2;
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
        static byte[] _bytesFile; // массив байт из файла
        int _id;
        List<List<string>> _dataConfig = new(); // массив для данных из конфигурационного файла
        static List<List<string>> _deviceParam = new(); // переработанный массив для выгрузки данных
        static List<Packet> _packetsSettings = new();

        public MainWindow()
        {
            InitializeComponent();           
        }

        private static DateTime GetbdTime(Byte[] array) // Вывод времени
        {
            string dateTime = Convert.ToHexString(array);
            int second = 0;
            int minute = 0;
            int hour = 0; 
            int day = 0; 
            int month = 0;
            int year = 0;
            DateTime date;
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
            catch 
            {
                date = new DateTime();
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
            HandleDeviceParam();

            //LoadData();

            //EditTable(_id);
        }

        private void EditTable(int id)
        {
            ObservableCollection<DataGridColumn> columns = datagrid1.Columns;

            foreach (DataGridColumn col in columns)
            {
                switch (col.Header.ToString())
                {
                    case "numPacket":
                        col.Header = "ID строки";
                        col.Width = 200;
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

        private static void LoadData2()
        {
            // вычисляем изначальные id пакета и устройства
            byte idPacketArray = _bytesFile[0];
            byte idDeviceArray = _bytesFile[1];
            // определяем длину пакета
            int countByteRow = _packetsSettings[0].lengthLine;
            byte countParams = (byte)_packetsSettings[0].typeParams.Count;

            for (int i = 0; i < countByteRow; i++)
            {
                List<NNGK> list = new();
            }

        }

        private void LoadData()
        {
            List<NNGK> list = new();

            //double gkcf = 0;//double.Parse(_parametrs[0]);
            //double Ubatcf = 0;// double.Parse(_parametrs[1]);
            //double tcf = 0;// double.Parse(_parametrs[2]);
            //double timecf = 0;// double.Parse(_parametrs[3]);

            // вычисляем изначальные id пакета и устройства
            //byte idPacketArray = _bytesFile[0]; 
            //byte idDeviceArray = _bytesFile[1];

            // выполняем сравнение с массивом данных из конфига _packetsSettings
            //int rowCount = _bytesFile.Length / 34;
            int rowCount = _packetsSettings[0].lengthLine;
            int pos = 2; // позиция в массиве байтов. с 2 позиции начинается номер пачки

            for (int i = 0; i < rowCount; i++) // i<rowCount
            {
                NNGK line = new NNGK();

                line.numPacket = (_bytesFile[pos]) | (_bytesFile[pos + 1] << 8); // заполнение номера пачки
                pos = pos + 2;

                byte[] dateArray = new byte[6];
                Array.Copy(_bytesFile, pos, dateArray, 0, 6);
                DateTime time = GetbdTime(dateArray);
                if (time.Year < 2000 || time.Year > 2100 ) // отбраковка странных значений по году
                {
                    continue;
                }
                line.time = time.ToString("HH:mm:ss dd.MM.yy"); // вывод даты
                pos = pos + 6;

                double gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                //line.GK1 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                //line.GK2 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                //line.GK3 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                //line.GK4 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
                //line.GK5 = Math.Round(gk / gkcf, 1);
                pos = pos + 2;

                line.GK6 = (_bytesFile[pos]) | (_bytesFile[pos + 1] << 8); // вывод ГК ед                
                pos = pos + 2;

                //line.temperatura = (double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / Ubatcf; // вывод Ubat
                pos = pos + 2;

               // line.periodSHIM = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / tcf, 1); // вывод темп
                pos = pos + 2;

                //line.currentWorkTime = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / timecf, 1); // вывод тек наработки
                pos = pos + 2;

                //line.totalWorkTime = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / timecf, 1); // вывод общ наработки
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
                _bytesFile = File.ReadAllBytes(path);               
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла ошибка считывания файла. " + e.Message);
                return;
            }

            if (_bytesFile.Length > 386)
            {
                _bytesFile = _bytesFile.Skip(384).ToArray(); // первые 384 байта лишние 
            }
            else
            {
                MessageBox.Show("Возникла ошибка с данными файла");
            }
        }

        private void LoadConfigFile(int id) // загрузка данных с файла, пересортировка данных для дальнейшей работы
        {
            string path; // путь к файлу, который выбирается по id устройства
            //if (id > 0 && id <= 9) // id устройства
            //{
            //    path = id switch
            //    {
            //        //9 => "Configurations\\mycongf.cfg",
            //        6 => "Configurations\\flashRead_NNGK_v1_07-12-2022.cfg",
            //        _ => "error"
            //    };
            //}
            //else
            //{
            //    return;
            //}
            path = "Configurations\\flashRead_NNGK_v1_07-12-2022.cfg";
            try
            {
                char[] separators = { ' ', '\t' };
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        
                        if (!string.IsNullOrWhiteSpace(row))
                        {
                            string[] line = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                          
                            //_parametrs.Add(line[0]);
                            _dataConfig.Add(new List<string>(line));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            // далее идет переработка файлов
            string idDevice = "";
            string idPacket = "";
            string version = _dataConfig[0][0];

            for (int i = 1; i < _dataConfig.Count; i++)
            {
                for (int j = 0; j < _dataConfig[i].Count; j++)
                {

                    if (_dataConfig[i][0][0] == '@' && _dataConfig[i + 2][0][0] == '@') // вычисляем id устройства, обрамленное @
                    {
                        idDevice = _dataConfig[i + 1][0];
                        i = i + 2;
                        break;
                    }

                    if (_dataConfig[i][j][0] == '~' && idDevice != "") // вычисляем id пакета
                    {                        
                        idPacket = _dataConfig[i][j].Trim('~');
                        do
                        {
                            i++;
                            List<string> data = new();
                            data.Add(idPacket);
                            data.Add(idDevice);
                            if (_dataConfig[i][0][0] == '*')
                            {
                                _dataConfig[i].RemoveAt(0);
                            }                            
                            else
                            {
                                throw new Exception("Ошибка описания строки данных flash");
                            }
                            data.AddRange(_dataConfig[i]);                            
                            _deviceParam.Add(new List<string>(data));
                            if (_dataConfig[i + 1][0][0] == '#') // записываем конец строки, удаляем лишние символы
                            {
                                _deviceParam.Add(new List<string>() { _dataConfig[i + 1][0].Trim('#','h','H') });                                
                            }
                        } while (_dataConfig[i+1][0][0] != '#');
                        i++;
                    }
                }
            }
            
        } 

        private static ushort GetShortUsValue(byte a, byte b)
        {
            return (ushort)(a | (b << 8));
        }

        private static short GetShortSValue(byte a, byte b)
        {
            return (short)(a | (b << 8));
        }     

        private static void HandleDeviceParam()
        {
            //List<Packet> packetsSettings = new();
            for(int i = 0; i <_deviceParam.Count; i ++)
            {                
                var packet = new Packet();            

                try
                {
                    packet.idPacket = byte.Parse(_deviceParam[i][0]);
                    packet.idDevice = byte.Parse(_deviceParam[i][1]);
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                    return;
                }
                do
                {
                    var list = _deviceParam[i];
                    byte length = list[2] switch
                    {
                        "byteUs" => 1,
                        "byteS" => 1,
                        "shortUs" => 2,
                        "shortS" => 2,
                        "intS" => 4,
                        "intUs" => 4,
                        "bdTime" => 6,
                        _ => 0
                    };
                    if (length == 0)
                    {
                        throw new Exception("Неопознанное обозначение типа данных");
                    }
                    packet.lengthLine += length;
                    packet.lengthParams.Add(length);
                    packet.typeParams.Add(list[2]);
                    if (list[5] == "[]")
                    {
                        packet.titleColumn.Add(list[4]);
                    }
                    else
                    {
                        packet.titleColumn.Add($"{list[4]} {list[5]}");
                    }

                    // пропускаем значение неопределенности
                    packet.typeCalculate.Add(list[7]);
                    double[] data = new double[4];

                    for (int j = 8; j <= 11; j++)
                    {
                        bool isParseDouble = double.TryParse(list[j], NumberStyles.Any, CultureInfo.InvariantCulture, out double value);
                        if (!isParseDouble)
                        {
                            throw new Exception("Ошибка парсинга чисел для пересчета данного");
                            MessageBox.Show("Ошибка парсинга чисел для пересчета данного");
                            return;
                        }
                        data[j - 8] = value;
                    }
                    packet.dataCalculation.Add(data); // загоняем коэффициенты для пересчета
                    bool isCountWidth = byte.TryParse(list[12], out byte resultCount);
                    bool isParseWidth = byte.TryParse(list[13], out byte resultWindth);

                    if (!isParseWidth && !isCountWidth)
                    {
                        throw new Exception("Ошибка парсинга числа знаков после запятой или ширины столбца");
                        //MessageBox.Show("Ошибка парсинга чисел для пересчета данного");
                        return;
                    }
                    packet.countSign.Add(resultCount);
                    packet.widthColumn.Add(resultWindth);
                    i++;
                } while (_deviceParam[i].Count != 1);
                packet.endLine = _deviceParam[i][0];
                _packetsSettings.Add(packet);
            }
        }
        
    }
    
}
