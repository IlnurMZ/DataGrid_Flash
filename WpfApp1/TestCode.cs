using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace FlashViewer2
{
    internal class TestCode
    {
        //void CreateTable()
        //{
        //    int countColumn = _packetsSettings[0].headerColumn.Count;
        //    for (int i = 0; i < countColumn; i++)
        //    {
        //        //string titleColumn = _packetsSettings[0].headerColumn[i];
        //        Table.Columns.Add("col" + i.ToString());

        //        //DataColumn col = new DataColumn(titleColumn);
        //        //Table.Columns.Add(col);                
        //    }
        //    // Изменяем ширину столбцов
        //    //for (int i = 0; i < countColumn; i ++)
        //    //{
        //    //    datagrid1.Columns[i].Width = _packetsSettings[0].widthColumn[i];
        //    //}
        //}

        byte a;

        //void CreateTable()
        //{
        //    int countColumn = PacketsSettings[0].headerColumn.Count;
        //    for (int i = 0; i < countColumn; i++)
        //    {
        //        Table.Columns.Add("col" + i.ToString());
        //    }
        //}

        byte b;

        //private void LoadData()
        //{
        //    List<NNGK> list = new();

        //    //double gkcf = 0;//double.Parse(_parametrs[0]);
        //    //double Ubatcf = 0;// double.Parse(_parametrs[1]);
        //    //double tcf = 0;// double.Parse(_parametrs[2]);
        //    //double timecf = 0;// double.Parse(_parametrs[3]);

        //    // вычисляем изначальные id пакета и устройства
        //    //byte idPacketArray = _bytesFile[0]; 
        //    //byte idDeviceArray = _bytesFile[1];

        //    // выполняем сравнение с массивом данных из конфига _packetsSettings
        //    //int rowCount = _bytesFile.Length / 34;
        //    int rowCount = _packetsSettings[0].lengthLine;
        //    int pos = 2; // позиция в массиве байтов. с 2 позиции начинается номер пачки

        //    for (int i = 0; i < rowCount; i++) // i<rowCount
        //    {
        //        NNGK line = new NNGK();

        //        line.numPacket = (_bytesFile[pos]) | (_bytesFile[pos + 1] << 8); // заполнение номера пачки
        //        pos = pos + 2;

        //        byte[] dateArray = new byte[6];
        //        Array.Copy(_bytesFile, pos, dateArray, 0, 6);
        //        DateTime time = GetbdTime(dateArray);
        //        if (time.Year < 2000 || time.Year > 2100) // отбраковка странных значений по году
        //        {
        //            continue;
        //        }
        //        line.time = time.ToString("HH:mm:ss dd.MM.yy"); // вывод даты
        //        pos = pos + 6;

        //        double gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
        //        //line.GK1 = Math.Round(gk / gkcf, 1);
        //        pos = pos + 2;

        //        gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
        //        //line.GK2 = Math.Round(gk / gkcf, 1);
        //        pos = pos + 2;

        //        gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
        //        //line.GK3 = Math.Round(gk / gkcf, 1);
        //        pos = pos + 2;

        //        gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
        //        //line.GK4 = Math.Round(gk / gkcf, 1);
        //        pos = pos + 2;

        //        gk = ((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8));
        //        //line.GK5 = Math.Round(gk / gkcf, 1);
        //        pos = pos + 2;

        //        line.GK6 = (_bytesFile[pos]) | (_bytesFile[pos + 1] << 8); // вывод ГК ед                
        //        pos = pos + 2;

        //        //line.temperatura = (double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / Ubatcf; // вывод Ubat
        //        pos = pos + 2;

        //        // line.periodSHIM = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / tcf, 1); // вывод темп
        //        pos = pos + 2;

        //        //line.currentWorkTime = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / timecf, 1); // вывод тек наработки
        //        pos = pos + 2;

        //        //line.totalWorkTime = Math.Round((double)((_bytesFile[pos]) | (_bytesFile[pos + 1] << 8)) / timecf, 1); // вывод общ наработки
        //        pos = pos + 2;

        //        line.timeNakop = _bytesFile[pos] * 5; // вывод времени накопления
        //        pos = pos + 1;

        //        line.error = _bytesFile[pos]; // вывод ошибка
        //        pos = pos + 5;
        //        list.Add(line);
        //        //datagrid1.Items.Add(line);
        //    }

        //    datagrid1.ItemsSource = list;



        //}

        byte c;
        //BackgroundWorker worker = new BackgroundWorker();
        //worker.WorkerReportsProgress = true;
        //worker.DoWork += worker_DoWork;
        //worker.ProgressChanged += worker_ProgressChanged;
        //worker.RunWorkerAsync();
        //var progress = new Progress<int>(value => progBar.Value = value);
        //await Task.Run(() =>
        //{
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        ((IProgress<int>)progress).Report(i);
        //        Thread.Sleep(100);
        //    }
        //});
        //void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        (sender as BackgroundWorker).ReportProgress(i);                
        //    }
        //}

        //void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    progBar.Value = e.ProgressPercentage;
        //}

        byte d;

        //void EditTable(int id)
        //{
        //    ObservableCollection<DataGridColumn> columns = datagrid1.Columns;

        //    foreach (DataGridColumn col in columns)
        //    {
        //        switch (col.Header.ToString())
        //        {
        //            case "numPacket":
        //                col.Header = "ID строки";
        //                col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "time":
        //                col.Header = "Дата";
        //                //col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "GK1":
        //                col.Header = "ННК1/ННК1(вода)";
        //                //col.Width = 180;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "GK2":
        //                col.Header = "ННК2/ННК2(вода)";
        //                //col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "GK3":
        //                col.Header = "НГК/НГК(вода)";
        //                //col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "GK4":
        //                col.Header = "ННК1 [ед]";
        //                //col.Width = 180;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "GK5":
        //                col.Header = "ННК2 [ед]";
        //                //col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "GK6":
        //                col.Header = "НГК [ед]";
        //                //col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "temperatura":
        //                col.Header = "Температура НГК [°С]";
        //                //col.Width = 180;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "periodSHIM":
        //                col.Header = "Период ШИМ ННК";
        //                //col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "currentWorkTime":
        //                col.Header = "Текущая наработка";
        //                //col.Width = 180;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "totalWorkTime":
        //                col.Header = "Общая наработка";
        //                //col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "timeNakop":
        //                col.Header = "Время накопления [сек]";
        //                //col.Width = 200;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            case "error":
        //                col.Header = "Ошибка I2C";
        //                //col.Width = 180;
        //                col.Visibility = Visibility.Visible;
        //                break;
        //            default:
        //                col.Visibility = Visibility.Collapsed;
        //                break;
        //        }
        //    }
        //}

        byte e;


    }
}
