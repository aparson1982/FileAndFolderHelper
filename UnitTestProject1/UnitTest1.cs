using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileAndFolderHelper;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine(CSV.GetLengthFromTextOrCSV(@"C:\Users\Robert Parson\Documents\New folder\test.csv", ","));
        }


        [TestMethod]
        public void TestFileName()
        {
            //Console.WriteLine(FileNameHelper.GenerateUniqueNumericFileName(@"C:\Users\rparso2\Downloads\New folder","Adam001.txt",3));
            Console.WriteLine(FileNameHelper.GenerateUniqueNumericFileName(@"C:\Users\rparso2\Downloads\New folder", "0000001.txt", 1, true));
        }


        [TestMethod]
        public void TestCompare()
        {
            Console.WriteLine(FolderHelper.FolderEquality(@"\\nas72v2\vdi_data\USA\rparso2\Documents\Automation Anywhere Files\Automation Anywhere\Shaw Files\0067_RDCCarpetReplenishment\EDI Files\0001996", @"\\nas72v2\vdi_data\USA\rparso2\Documents\Automation Anywhere Files\Automation Anywhere\Shaw Files\0067_RDCCarpetReplenishment\EDI Files\0001996\Outbox", "850","855",false));
        }
    }
}
