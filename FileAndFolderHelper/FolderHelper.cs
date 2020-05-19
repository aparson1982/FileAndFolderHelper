using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAndFolderHelper
{
    public class FolderHelper
    {
        public static string FolderEquality(string folderpathA, string folderpathB, string extensionA = null, string extensionB = null, bool CompareWithExtension = true)
        {
            string str = string.Empty;
            try
            {
                var dirA = new DirectoryInfo(folderpathA);
                var dirB = new DirectoryInfo(folderpathB);

                IEnumerable<System.IO.FileInfo> listA = dirA.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);
                IEnumerable<System.IO.FileInfo> listB = dirB.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);

                FileCompare fileCompare = new FileCompare();
                FilenameCompare filenameCompare = new FilenameCompare();

                bool areIdentical = listA.SequenceEqual(listB, fileCompare);

                if (areIdentical)
                {
                    return str = "Identical";
                }

                bool filenamesSame = listA.SequenceEqual(listB, filenameCompare);

                if (filenamesSame)
                {
                    return str = "Similar";
                }

                var missingFromListB = (from file in listA
                                        select file).Except(listB, filenameCompare).ToList();

                str = "Not Equal.  The following files are missing:  " + Environment.NewLine +
                          string.Join(", ", missingFromListB);

                if (CompareWithExtension == false)
                {
                    string filenamesA = String.Join(",", Directory.GetFiles(folderpathA, "*." + extensionA).Select(filename => Path.GetFileNameWithoutExtension(filename)));
                    string filenamesB = String.Join(",", Directory.GetFiles(folderpathB, "*." + extensionB).Select(filename => Path.GetFileNameWithoutExtension(filename)));

                    List<string> listOfFilenamesA = filenamesA.Split(',').ToList();
                    List<string> listOfFilenamesB = filenamesB.Split(',').ToList();

                    bool filenamesWithoutExtensionSame = listOfFilenamesA.All(listOfFilenamesB.Contains) && listOfFilenamesA.Count == listOfFilenamesB.Count;

                    if (filenamesWithoutExtensionSame)
                    {
                        return str = "Similar";
                    }

                    List<string> missingList = listOfFilenamesA.Except(listOfFilenamesB).ToList();

                    str = "Not Equal.  The following files are missing:  " + Environment.NewLine +
                          string.Join(", ", missingList);
                }
                

            }
            catch (Exception e)
            {
                str = "Message:  " + e.Message + Environment.NewLine +
                    "Source:  " + e.Source + Environment.NewLine +
                    "StackTrace:  " + e.StackTrace + Environment.NewLine +
                    "Inner Exception:  " + e.InnerException + Environment.NewLine +
                    "Parameters:  folderpathA = " + folderpathA + " | folderpathB = " + folderpathB + Environment.NewLine;
            }

            return str;
        }
    }
}
