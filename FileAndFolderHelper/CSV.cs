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

        
    }
}
