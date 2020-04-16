using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FileAndFolderHelper
{
    public class FileNameHelper
    {
        public static string GenerateUniqueNumericFileName(string folderpath, string filename = null, int padLength = 0)
        {
            string validname = string.Empty;
            string extension = string.Empty;
            try
            {
                
                int numericSequence = 1;
                if (!string.IsNullOrEmpty(filename.Trim()))
                {
                    validname = Path.GetFileNameWithoutExtension(filename.Trim());
                    extension = Path.GetExtension(filename);
                }
                

                
                if(string.IsNullOrEmpty(validname))
                {
                    validname = string.Format("{0}{1}", validname, numericSequence++.ToString().PadLeft(padLength, '0'));
                    while (File.Exists(Path.Combine(folderpath, validname + extension)))
                    {
                        validname = string.Format("{0}{1}", validname, numericSequence++.ToString().PadLeft(padLength, '0'));
                    }
                }
                else if (IsDigitsOnly(validname))
                {
                    int? val = Int32.TryParse(validname, out var tempVal) ? tempVal : (int?)null;

                    while (File.Exists(Path.Combine(folderpath, validname.PadLeft(padLength,'0') + extension)))
                    {
                        validname = string.Format("{0}", val++.ToString().PadLeft(padLength, '0'));
                    }
                }
                else
                {
                    string numericPattern = @"\d+(?=\.\w+$)";
                    Regex regexNumeric = new Regex(numericPattern);
                    string validNameNumber = regexNumeric.Match(filename).Value;
                    int? fileNumber = Int32.TryParse(validNameNumber, out var tempVal) ? tempVal : (int?)null;

                    string nonNumericPattern = @"\d+(\.\w+$)";
                    Regex regexNonNumeric = new Regex(nonNumericPattern);
                    string [] splitFileNameArray = regexNonNumeric.Split(filename);
                    string splitFileName = splitFileNameArray[0];

                    if (fileNumber != null)
                    {
                        while (File.Exists(Path.Combine(folderpath, validname + extension)))
                        {
                            validname = string.Format("{0}{1}", splitFileName, fileNumber++.ToString().PadLeft(padLength, '0'));
                        }
                    }
                    else
                    {
                        string numericPatternNoExt = @"(\d+$)";
                        Regex numericNoExtension = new Regex(numericPatternNoExt);
                        int? rgxNbr = null;
                        while (File.Exists(Path.Combine(folderpath, validname + extension)))
                        {
                            string[] splitValidNameArray = numericNoExtension.Split(validname);
                            validname = splitValidNameArray[0];
                            validname = string.Format("{0}{1}", validname, (rgxNbr.HasValue ? ++rgxNbr : rgxNbr = 1).ToString().PadLeft(padLength, '0'));
                            string extractedNumber = numericNoExtension.Match(validname).Value;
                            rgxNbr = Int32.TryParse(extractedNumber, out var temp) ? temp : (int?)null;
                        }
                    }
                        
                }

               validname = Path.Combine(validname.Trim() + extension);

                
            }
            catch (Exception e)
            {
                validname = "Message:  " + e.Message + Environment.NewLine +
                    "Source:  " + e.Source + Environment.NewLine +
                    "StackTrace:  " + e.StackTrace + Environment.NewLine +
                    "Inner Exception:  " + e.InnerException + Environment.NewLine +
                    "Parameters:  folderpath = " + folderpath + " | filename = " + filename + Environment.NewLine;
            }
            return validname;
        }

        public static bool IsDigitsOnly(string str)
        {
            return str.All(ch => ch >= '0' && ch <= '9');
        }

        
    }
}
