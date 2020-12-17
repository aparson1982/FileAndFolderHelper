using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;

namespace FileAndFolderHelper
{
    public class FileNameHelper : FileAndFolderHelperProperties
    {
        public static string GenerateUniqueNumericFileName(string folderpath, string filename = null, int padLength = 0, bool appendName = false)
        {
            string validname = string.Empty;
            string extension = string.Empty;
            List<KeyValuePair<int, string>> keyValPairList = new List<KeyValuePair<int, string>>();
            try
            {
                
                int numericSequence = 1;
                if (!string.IsNullOrEmpty(filename?.Trim()))
                {
                    validname = Path.GetFileNameWithoutExtension(filename.Trim());
                    extension = Path.GetExtension(filename);

                    string numericPattern = @"(\d+$)";
                    //string nonNumericPattern = @"\d+(\.\w+$)";
                    string fileExtensionPattern = @"[\w-]+(\.)[\w]+";  //matches file with extension  adam.txt or adam005.txt
                    string filepathPattern = @"[\w-]+(?=\.)";  //matches file name only with numbers  adam  or adam005
                    string numberExtraction = @"[\d-]+(?=\.)";
                    Regex regexNumeric = new Regex(numericPattern);
                    Regex regexExtensionPattern = new Regex(fileExtensionPattern);
                    Regex regexFilePathPattern = new Regex(filepathPattern);
                    Regex regexNumberExtractionPattern = new Regex(numberExtraction);


                    if (appendName == true && !IsDigitsOnly(validname))
                    {
                        
                        string validnameRemovedNumber = regexNumeric.Replace(validname, string.Empty);
                

                        IOrderedEnumerable<string> sortedList = Directory.GetFiles(folderpath).OrderBy(f => f);
                        
                        string tempItem;
                        foreach(var item in sortedList)
                        {
                            tempItem = regexFilePathPattern.Match(item).Value;
                            string tempItemRemovedNumber = regexNumeric.Replace(tempItem,string.Empty);
                            
                            //tempItem = regexFilePathPattern.Match(tempItem).Value;
                            string filenameAndExtension = regexExtensionPattern.Match(item).Value;
                        
                            keyValPairList.Add(new KeyValuePair<int, string>(CalcLevenshteinDistance(tempItemRemovedNumber, validnameRemovedNumber), filenameAndExtension));
                        }
                        keyValPairList.Sort((x, y) => (y.Key.CompareTo(x.Key)));
                        var a = keyValPairList.Count();

                        filename = keyValPairList[a-1].Value;
                        validname = Path.GetFileNameWithoutExtension(filename);
                    }
                    else if (appendName == true && IsDigitsOnly(validname))
                    {
                        IOrderedEnumerable<string> sortedList = Directory.GetFiles(folderpath).OrderBy(f => f);
                        foreach(var item in sortedList)
                        {
                            string itemNumbers = regexNumberExtractionPattern.Match(item).Value.Trim();
                            int? itemNumber = Int32.TryParse(itemNumbers, out var temp) ? temp : (int?)null;
                            string filenameAndExtension = regexExtensionPattern.Match(item).Value;
                            if (itemNumbers.Length == padLength && itemNumber != null)
                            {
                                keyValPairList.Add(new KeyValuePair<int, string>(Int32.Parse(itemNumbers), filenameAndExtension));
                            }
                        }

                        if (keyValPairList.Any())
                        {
                            keyValPairList.Sort((x, y) => (y.Value.CompareTo(x.Value)));

                            var a = keyValPairList.Count();
                            //keyValPairList.OrderBy(o => o.Key);
                            filename = keyValPairList[0].Value;
                            validname = Path.GetFileNameWithoutExtension(filename);
                        }
                        
                        
                    }
                }

                if (string.IsNullOrEmpty(validname))
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

                    if (padLength > 0)
                    {
                        validname = string.Format("{0}", val.ToString().PadLeft(padLength, '0'));
                    }
                    
                    while (File.Exists(Path.Combine(folderpath, validname.PadLeft(padLength,'0') + extension)))
                    {
                        validname = string.Format("{0}", val++.ToString().PadLeft(padLength, '0'));
                    }
                }
                else
                {
                    //string numericPattern = @"\d+(?=\.\w+$)";
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

                ReturnStatusCode = 0; 
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
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

        private static int CalcLevenshteinDistance(string a, string b)
        {
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (String.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (String.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }


    }
}
