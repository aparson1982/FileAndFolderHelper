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
        public static int GetLengthFromCSV(string Path, string delimiter)
        {
            char delimit = delimiter[0];
            List<string> values = File.ReadAllLines(Path).Select(v => ReadFromCSV(v, delimit).ToString()).ToList();
            return values.Count;
        }

        internal static string[] ReadFromCSV(string CsvLine, char delimiter)
        {
            string[] values = CsvLine.Split(delimiter);
            return values;
        }
    }
}
