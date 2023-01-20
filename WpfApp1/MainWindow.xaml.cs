using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
        byte[] FlashFile { get; set; } // массив байтов из файла 
        int ID_Device { get; set; }
        List<List<string>> DataConfig { get; set; } = new(); // массив для данных из конфигурационного файла
        List<List<string>> DeviceParam { get; set; } = new(); // переработанный массив для выгрузки данных
        List<Packet> PacketsSettings { get; set; } = new(); // параметры пакетов конфигурационн       
        DataTable Table { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();          
        }
        // Вывод времени
        private static string GetbdTime(Byte[] array)
        {
            string dateTime = Convert.ToHexString(array);
            string second = "";
            string minute = "";
            string hour = "";
            string day = "";
            string month = "";
            string year = "";
            string date;

            second = dateTime[0].ToString() + dateTime[1].ToString();
            minute = dateTime[2].ToString() + dateTime[3].ToString();
            hour = dateTime[4].ToString() + dateTime[5].ToString();
            day = dateTime[6].ToString() + dateTime[7].ToString();
            month = dateTime[8].ToString() + dateTime[9].ToString();
            year = dateTime[10].ToString() + dateTime[11].ToString();
            //date = new DateTime(year, month, day, hour, minute, second);
            date = $"{hour}:{minute}:{second} {day}.{month}.{year}";

            return date;
        }
        // нажатие кнопки открыть файл
        public async void MenuItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string pathFlash = ""; // путь до файла с данными флешки

            if (openFileDialog.ShowDialog() == true)
            {
                pathFlash = openFileDialog.FileName;
            }                
            // проверка корректности пути
            if (string.IsNullOrWhiteSpace(pathFlash))
            {
                MessageBox.Show("Ошибка. Не задан путь к файлу");
                return;
            }
            // читаем данные с флеш

            bool isReadFlash = ReadBytesFromFile(pathFlash); // проверка корректности флеш файла
            bool isReadConfig = false;                                            // 
            if (isReadFlash)
            {
                isReadConfig = LoadConfigFile(); // загрузка конф. файла
            }
            else
            {                
                return;
            }

            await Task.Run(() =>
            {
                if (isReadConfig && HandleDeviceParam())
                {
                    Table = LoadDataTable();
                }
                else
                {
                    return;
                }
            });

            //Dispatcher.Invoke(() =>
            //{
            //    datagrid1.ItemsSource = Table.DefaultView;               
            //    //var progress = new Progress<int>(value => progBar.Value = value);
            //    //((IProgress<int>)progress).Report(0);
            //    //progBar.Value = 0;
            //});
            datagrid1.ItemsSource = Table.DefaultView;
            EditTableColumn();
            progBar.Value = 0;

            //Task t2 = new Task(() =>
            //{
            //    progBar.Value = 0;
            //});
            //t2.Start();

        }

        // Изменение названия столбцов
        void EditTableColumn()
        {           
            // Меняем названия столбцов
            ObservableCollection<DataGridColumn> columns = datagrid1.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                columns[i].Header = PacketsSettings[0].headerColumn[i].ToString();
            }
            datagrid1.HorizontalAlignment = HorizontalAlignment.Left;
        }       

        string CalculateValueByType(string typeCalc, string value, double[] data) // по типу вычисления выдаем результат
        {
            string result = "";
            switch (typeCalc) // смотрим тип вычисления
            {
                case "нет":
                case "вр":
                    result = value;
                    break;
                case "лин":
                    if (double.TryParse(value, out double doubleValue))
                    {
                        result = (data[0] * doubleValue + data[1]).ToString();
                    }
                    else
                    {
                        throw new Exception("не удалось преобразовать данное при лин.вычислении");
                    }                    
                    break;                    
                default:
                    break;
            }
            return result;
        }

        string GetValueByType(string typeValue, byte[] value)
        {
            string result = "";
            switch (typeValue)
            {
                case "byteS":
                    result = ((sbyte)(value[0])).ToString();
                    break;
                case "byteUs":
                    result = ((byte)(value[0])).ToString();
                    break;
                case "shortS":
                    result = ((short)(value[0] | (value[1] << 8))).ToString();                    
                    break;
                case "shortUs":
                    result = ((ushort)(value[0] | (value[1] << 8))).ToString();
                    break;
                //case "intS":
                //    break;
                //case "intUs":
                //    break;
                case "bdTime":
                    result = GetbdTime(value);
                    break;
                default:
                    throw new FormatException("неизвестный тип данных");                 
            }
            return result;
        }
        DataTable LoadDataTable()
        {
            //создаем временную таблицу
            DataTable dt = new DataTable();
            int countColumn = PacketsSettings[0].headerColumn.Count;
            for (int i = 0; i < countColumn; i++)
            {
                dt.Columns.Add(i.ToString());                
            }           

            // вычисляем изначальные id пакета и устройства
            byte idPacketArray = FlashFile[0];
            byte idDeviceArray = FlashFile[1];           
            // выбираем нужный пакет исходя из id-шников 
            var myPacket = PacketsSettings[0];
            byte[] endLinePacket = myPacket.endLine;
            int countByteRow = myPacket.lengthLine; // количество байт на строку
            byte countParams = (byte)myPacket.typeParams.Count; // количество столбцов
            int countBadByte = 0;
            DataRow row;
            byte loadStatus = 0;
            byte tempVal = 0;            

            for (int i = 0; i < FlashFile.Length; i++)
            {
                // условие захода в начало строки
                bool isGoodStartLine = FlashFile[i] == idPacketArray && FlashFile[i + 1] == idDeviceArray; 

                if (i + countByteRow > FlashFile.Length) // проверка завершенности строки, чтобы исключить выход за пределы массива байт
                {
                    break;
                }
                // проверка двух байт на конец строки
                bool isGoodEndLine = FlashFile[i + countByteRow - 2] == endLinePacket[0] && FlashFile[i + countByteRow - 1] == endLinePacket[1]; 

                if (isGoodStartLine && isGoodEndLine) // проверка совпадения на начало строки
                {
                    row = dt.NewRow(); // создаем строку для таблицы
                    for (int j = 0; j < countParams; j++)
                    {
                        byte countByte = myPacket.lengthParams[j]; // определяем количество байт на параметр
                        byte[] values = new byte[countByte]; // берем необходимое количество байт                   
                        Array.Copy(FlashFile, i, values, 0, countByte); // копируем наш кусок
                        string valueA = GetValueByType(myPacket.typeParams[j], values); // вычисляем значение по типу данных
                        string valueB = CalculateValueByType(myPacket.typeCalculate[j], valueA, myPacket.dataCalculation[j]); // вычисляем пересчет данного по типу
                        row[j] = valueB;
                        i += countByte; // смещаем курсор по общему массиву байт                        
                    }
                    i--;
                    dt.Rows.Add(row);                    
                }
                else
                {
                    countBadByte++;
                }
                
                tempVal = (byte)(i*1.0 / FlashFile.Length * 100);
                if (tempVal >= loadStatus)
                {
                    loadStatus =(byte)(tempVal + 10);
                    Dispatcher.Invoke(() =>
                    {
                        //var progress = new Progress<int>(value => progBar.Value = value);
                        //((IProgress<int>)progress).Report(loadStatus);
                        progBar.Value = loadStatus;
                    });
                }
            }                   
            return dt;
        }        

        private bool ReadBytesFromFile(string path)
        {
            try
            {
                FlashFile = File.ReadAllBytes(path);               
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла ошибка считывания файла. " + e.Message);
                return false;
            }

            if (FlashFile.Length > 386)
            {
                FlashFile = FlashFile.Skip(384).ToArray(); // первые 384 байта лишние
                ID_Device = FlashFile[1]; // записываем id устройства
            }
            else
            {
                MessageBox.Show("Возникла ошибка с данными файла");
                return false;
            }
            return true;
        }

        private bool LoadConfigFile() // загрузка данных с файла, пересортировка данных для дальнейшей работы
        {
            string pathConfig; // путь к файлу, который выбирается по id устройства
            if (ID_Device > 0 && ID_Device <= 9) // id устройства
            {
                pathConfig = ID_Device switch
                {                    
                    6 => "Configurations\\flashRead_NNGK_v1_07-12-2022.cfg",
                    _ => "error"
                };
            }
            else
            {
                return false;
            }
            
            try
            {
                char[] separators = { ' ', '\t' };
                using (var reader = new StreamReader(pathConfig))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        
                        if (!string.IsNullOrWhiteSpace(row))
                        {
                            string[] line = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);                            
                            DataConfig.Add(new List<string>(line));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            // далее идет переработка файлов
            string idDevice = "";
            string idPacket = "";
            string version = DataConfig[0][0];

            for (int i = 1; i < DataConfig.Count; i++)
            {
                for (int j = 0; j < DataConfig[i].Count; j++)
                {

                    if (DataConfig[i][0][0] == '@' && DataConfig[i + 2][0][0] == '@') // вычисляем id устройства, обрамленное @
                    {
                        idDevice = DataConfig[i + 1][0];
                        i = i + 2;
                        break;
                    }

                    if (DataConfig[i][j][0] == '~' && idDevice != "") // вычисляем id пакета
                    {                        
                        idPacket = DataConfig[i][j].Trim('~');
                        do
                        {
                            i++;
                            List<string> data = new();
                            data.Add(idPacket);
                            data.Add(idDevice);
                            if (DataConfig[i][0][0] == '*')
                            {
                                DataConfig[i].RemoveAt(0);
                            }                            
                            else
                            {
                                //throw new Exception("Ошибка описания строки данных flash");
                                return false;
                            }
                            data.AddRange(DataConfig[i]);                            
                            DeviceParam.Add(new List<string>(data));
                            if (DataConfig[i + 1][0][0] == '#') // записываем конец строки, удаляем лишние символы
                            {
                                DataConfig[i + 1].RemoveAll(x => x == "#" || x == "H" || x == "h"); // удаляем лишние элементы из Листа
                                List<string> str = DataConfig[i + 1];
                                DeviceParam.Add(new List<string>(str));
                                break;
                            }
                        } while (true);
                        i++;
                    }
                }
            }
            return true;
        }     

        private bool HandleDeviceParam()
        {           
            for(int i = 0; i <DeviceParam.Count; i ++)
            {                
                var packet = new Packet();            

                try
                {
                    packet.idPacket = byte.Parse(DeviceParam[i][0]);
                    packet.idDevice = byte.Parse(DeviceParam[i][1]);
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                    return false;
                }
                do
                {
                    var list = DeviceParam[i];
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
                        MessageBox.Show("Неопознанное обозначение типа данных в конф.файле");
                        //throw new FormatException("Неопознанное обозначение типа данных в конф.файле");
                        return false;
                    }
                    packet.lengthLine += length;
                    packet.lengthParams.Add(length);
                    packet.typeParams.Add(list[2]);
                    if (list[5] == "[]")
                    {
                        packet.headerColumn.Add(list[4]);
                    }
                    else
                    {
                        packet.headerColumn.Add($"{list[4]} {list[5]}");
                    }

                    // пропускаем значение неопределенности
                    packet.typeCalculate.Add(list[7]);
                    double[] data = new double[4];

                    for (int j = 8; j <= 11; j++)
                    {
                        bool isParseDouble = double.TryParse(list[j], NumberStyles.Any, CultureInfo.InvariantCulture, out double value);
                        if (!isParseDouble)
                        {
                            //throw new Exception("Ошибка парсинга чисел для пересчета данного");
                            MessageBox.Show("Ошибка парсинга чисел для пересчета данного");
                            return false;
                        }
                        data[j - 8] = value;
                    }
                    packet.dataCalculation.Add(data); // загоняем коэффициенты для пересчета
                    bool isCountWidth = byte.TryParse(list[12], out byte resultCount);
                    bool isParseWidth = byte.TryParse(list[13], out byte resultWindth);

                    if (!isParseWidth && !isCountWidth)
                    {
                        //throw new Exception("Ошибка парсинга числа знаков после запятой или ширины столбца");
                        MessageBox.Show("Ошибка парсинга чисел для пересчета данного");
                        return false;
                    }
                    packet.countSign.Add(resultCount);
                    packet.widthColumn.Add(resultWindth);
                    i++;
                } while (DeviceParam[i].Count != 2 && DeviceParam[i].Count !=0);

                if (DeviceParam[i].Count == 2 )
                {
                    try
                    {
                        byte one = byte.Parse(DeviceParam[i][0]);
                        byte two = byte.Parse(DeviceParam[i][1]);
                        packet.endLine[0] = one;
                        packet.endLine[1] = two;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось сконвертировать конец строки в байты.\n" + ex.Message);
                        return false;
                    }
                    
                }
                else if (DeviceParam[i].Count == 0)
                {
                    packet.endLine[0] = 0;
                    packet.endLine[0] = 0;
                }
                //packet.endLine = int.Parse(_deviceParam[i][0]); // конвертирую в int (test-ый вариант)
                PacketsSettings.Add(packet);
            }
            return true;
        }

        private async void MenuItemCloseProgram_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
    
}
