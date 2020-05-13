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
        public static string CompareFolders(string folderpathA, string folderpathB, string extensionA = null, string extensionB = null)
        {
            string str = string.Empty;
            try
            {
                var dirA = new DirectoryInfo(folderpathA);
                var dirB = new DirectoryInfo(folderpathB);

                IEnumerable<System.IO.FileInfo> listA = dirA.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                IEnumerable<System.IO.FileInfo> listB = dirB.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                FileCompare fileCompare = new FileCompare();
                FilenameCompare filenameCompare = new FilenameCompare();

                bool areIdentical = listA.SequenceEqual(listB, fileCompare);

                if (areIdentical)
                {
                    return str = "Identical.  The two folders are identical.";
                }

                bool filenamesSame = listA.SequenceEqual(listB, filenameCompare);

                if (filenamesSame)
                {
                    return str = "Equal.  The two folders aren't identical but contain the same filenames.";
                }

                string filenamesA = String.Join(",", Directory.GetFiles(folderpathA, "*." + extensionA).Select(filename => Path.GetFileNameWithoutExtension(filename)));
                string filenamesB = String.Join(",", Directory.GetFiles(folderpathB, "*." + extensionB).Select(filename => Path.GetFileNameWithoutExtension(filename)));

                List<string> listOfFilenamesA = filenamesA.Split(',').ToList();
                List<string> listOfFilenamesB = filenamesB.Split(',').ToList();

                bool filenamesWithoutExtensionSame = listOfFilenamesA.All(listOfFilenamesB.Contains) && listOfFilenamesA.Count == listOfFilenamesB.Count;

                if (filenamesWithoutExtensionSame)
                {
                    return str = "Equal.  The folder contains filenames (without extensions) that are the same.";
                }

                List<string> missingList = listOfFilenamesA.Except(listOfFilenamesB).ToList();

                str = "Not Equal.  The following files are missing:  " + Environment.NewLine +
                      string.Join(", ", missingList);

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
