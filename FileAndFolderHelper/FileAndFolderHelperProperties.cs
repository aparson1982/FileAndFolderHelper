using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAndFolderHelper
{
    public class FileAndFolderHelperProperties : ErrorProperties
    {
        public static string ReturnStatusDescription { get; set; }
        public static int ReturnStatusCode { get; set; } = 0;
    }
}
