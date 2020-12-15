using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAndFolderHelper
{
    public class FolderHelper : FileAndFolderHelperProperties
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

                    ReturnStatusCode = 0;
                }
                

            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                str = "Message:  " + e.Message + Environment.NewLine +
                    "Source:  " + e.Source + Environment.NewLine +
                    "StackTrace:  " + e.StackTrace + Environment.NewLine +
                    "Inner Exception:  " + e.InnerException + Environment.NewLine +
                    "Parameters:  folderpathA = " + folderpathA + " | folderpathB = " + folderpathB + Environment.NewLine;
            }

            return str;
        }


        /// <summary>
        /// Deletes files older than x number of days.  Please ensure that the the filename and extension are written
        /// in this manner.  
        /// Example:  filename = somefilename  or  * 
        ///           ext      = .doc
        /// </summary>
        /// <param name="path"></param>
        /// <param name="days"></param>
        /// <param name="filename"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string DeleteFilesOlderThanXDays(string path, int days, string filename = null, string ext = null)
        {
            string str = string.Empty;
            try
            {
                int fileCount = 0;
                string fileDeleted = null;
                string directory;

                FileInfo[] files = new FileInfo[] { };
                DirectoryInfo di = new DirectoryInfo(path);

                if (!string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(ext))
                {
                    files = di.GetFiles(filename + ext).Where(p => p.Extension == ext).ToArray();
                }
                else
                {
                    files = di.GetFiles();
                }

                directory = di.FullName;

                foreach (FileInfo file in files)
                {

                    if (file.LastAccessTime < DateTime.Now.AddDays(-days) || file.LastWriteTime < DateTime.Now.AddDays(-days))
                    {
                        fileDeleted += file.Name + Environment.NewLine;
                        file.Delete();
                        fileCount++;
                    }
                    
                }

                str = fileCount.ToString() + " files were deleted from directory " + directory + ".  " + Environment.NewLine
                    + "The following files were deleted:  " + fileDeleted;

                ReturnStatusCode = 0;
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                str = "Message:  " + e.Message + Environment.NewLine +
                    "Source:  " + e.Source + Environment.NewLine +
                    "StackTrace:  " + e.StackTrace + Environment.NewLine +
                    "Inner Exception:  " + e.InnerException + Environment.NewLine +
                    "Parameters:  path = " + path + " days " + days + " filename = " + filename + " ext = " + ext + Environment.NewLine;
            }
            return str;
        }


        /// <summary>
        /// Checks if a directory contains a file or files of a certain extension.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string FileExists(string dirPath, string filename)
        {
            string str = string.Empty;
            try
            {
                if (Directory.GetFiles(dirPath, filename).Length == 0)
                {
                    str = "False";
                }
                else
                {
                    str = "True";
                }

                ReturnStatusCode = 0;
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                str = "Message:  " + e.Message + Environment.NewLine +
                    "Source:  " + e.Source + Environment.NewLine +
                    "StackTrace:  " + e.StackTrace + Environment.NewLine +
                    "Inner Exception:  " + e.InnerException + Environment.NewLine +
                    "Parameters:  path = " + dirPath + " filename = " + filename + Environment.NewLine;
            }
            return str;
        }


        /// <summary>
        /// Will delete duplicate file(s) in folderpathB.  Use *.extension to delete multiple duplicates.
        /// </summary>
        /// <param name="folderpathA"></param>
        /// <param name="folderpathB"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string DeleteDuplicate(string folderpathA, string folderpathB, string filename)
        {
            string str = string.Empty;
            try
            {
                var dirA = new DirectoryInfo(folderpathA);
                var dirB = new DirectoryInfo(folderpathB);
                int count = 0;
                string filenames = string.Empty;

                IEnumerable<System.IO.FileInfo> listA = dirA.GetFiles(filename, System.IO.SearchOption.TopDirectoryOnly);
                IEnumerable<System.IO.FileInfo> listB = dirB.GetFiles(filename, System.IO.SearchOption.TopDirectoryOnly);

                foreach (FileInfo item in listA)
                {
                    foreach (FileInfo file in listB)
                    {
                        if (item.Name == file.Name)
                        {
                            
                            filenames += file.Name + ", ";
                            file.Delete();
                            count++;
                        }
                    }
                }

                if (count > 0)
                {
                    str = count.ToString() + " files were deleted from " + folderpathB + ".  " + Environment.NewLine
                        + "The following files were deleted:  " + Environment.NewLine + filenames;
                }
                else
                {
                    str = count.ToString() + " files were deleted from " + folderpathB;
                }
                ReturnStatusCode = 0;
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                str = "Message:  " + e.Message + Environment.NewLine +
                    "Source:  " + e.Source + Environment.NewLine +
                    "StackTrace:  " + e.StackTrace + Environment.NewLine +
                    "Inner Exception:  " + e.InnerException + Environment.NewLine +
                    "Parameters:  folderpathA = " + folderpathA + " | folderpathB = " + folderpathB + Environment.NewLine;
            }

            return str;
        }


        public static string CreateDir(string path)
        {
            string str = string.Empty;
            try
            {
                path = path.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
                str = $"{path} created successfully.";
                ReturnStatusCode = 0;
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                str = $"{ErrorIntro} Message:  {e.Message}{Environment.NewLine} " +
                    $"Source:  {e.Source}{Environment.NewLine} " +
                    $"StackTrace:  {e.StackTrace}{Environment.NewLine}";
                
            }
            ReturnStatusDescription = str;
            return str;

        }
    }
}
