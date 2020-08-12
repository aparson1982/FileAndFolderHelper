using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAndFolderHelper
{
    public class CSV
    {
        public static int GetLengthFromTextOrCSV(string Path, string delimiter)
        {
            char delimit = delimiter[0];
            List<string> lines = new List<string>(File.ReadAllLines(Path));
            List<string> col = new List<string>();
            foreach (var line in lines)
            {
                col = new List<string>(line.Split(delimit));
            }
            return col.Count;
        }

        public static string ConvertCsvToXlsx(string FileName)
        {
            string str;
            try
            {
                Application app = new Application();
                Workbook wb = app.Workbooks.Open(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                string filePath = Path.GetDirectoryName(FileName);
                string fileName = Path.GetFileNameWithoutExtension(FileName);
                wb.SaveAs(filePath + @"\" + fileName + ".xlsx", XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                wb.Close();
                app.Quit();
                str = @filePath + @"\" + fileName + ".xlsx" + " created successfully.";
            }
            catch (Exception e)
            {
                str = "Message:  " + e.Message + Environment.NewLine +
                    "Source:  " + e.Source + Environment.NewLine +
                    "StackTrace:  " + e.StackTrace + Environment.NewLine +
                    "Inner Exception:  " + e.InnerException + Environment.NewLine +
                    "Parameters:  FileName = " + FileName + Environment.NewLine;
            }
            return str;
        }

        
    }
}
