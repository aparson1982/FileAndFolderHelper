using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAndFolderHelper
{
    internal class FilenameCompare : System.Collections.Generic.IEqualityComparer<System.IO.FileInfo>
    {
        public FilenameCompare()
        {
        }

        public bool Equals(FileInfo x, FileInfo y)
        {
            return (x.Name == y.Name);
        }

        public int GetHashCode(FileInfo obj)
        {
            string s = $"{obj.Name}{obj.Length}";
            return s.GetHashCode();
        }
    
    }
}
