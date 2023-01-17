using Microsoft.Office.Interop.Excel;
using static System.Net.Mime.MediaTypeNames;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace TestCode
{
    internal class Program
    {
        static void Main(string[] args)
        {            
            
        }

    

        void GoExcecl()
        {
            //Create COM Objects.
            Application excelApp = new Application();


            if (excelApp == null)
            {
                Console.WriteLine("Excel is not installed!!");
                return;
            }

            Workbook excelBook = excelApp.Workbooks.Open(@"E:\readExample.xlsx");
            Worksheet excelSheet = excelBook.Sheets[1];
            Range excelRange = excelSheet.UsedRange;

            int rows = excelRange.Rows.Count;
            int cols = excelRange.Columns.Count;

            for (int i = 1; i <= rows; i++)
            {
                //create new line
                Console.Write("\r\n");
                for (int j = 1; j <= cols; j++)
                {

                    //write the console
                    if (excelRange.Cells[i, j] != null && excelRange.Cells[i, j].Value2 != null)
                        Console.Write(excelRange.Cells[i, j].Value2.ToString() + "\t");

                }
            }

            for (int i = 4; i <= 9; i++)
            {
                for (int j = 4; j < 9; j++)
                    excelSheet.Cells[i, j] = String.Format("{0} {1}", i, j);
            }
            excelApp.Application.ActiveWorkbook.Save();
            //after reading, relaase the excel project
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            Console.ReadLine();
        }
    }
}