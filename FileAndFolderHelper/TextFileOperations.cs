using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAndFolderHelper
{
    public class TextFileOperations : FileAndFolderHelperProperties
    {
        /// <summary>
        /// Converts a .txt file to a string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ConvertTextFileToString(string path)
        {
            string str = string.Empty;
            try
            {
                str = File.ReadAllText(path);
                ReturnStatusCode = 0;
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                str = "Message:  " + e.Message + Environment.NewLine +
                    "Source:  " + e.Source + Environment.NewLine +
                    "StackTrace:  " + e.StackTrace + Environment.NewLine +
                    "Inner Exception:  " + e.InnerException + Environment.NewLine +
                    "Parameters:  path = " + path + Environment.NewLine;
            }
            return str;
        }
    }
}
