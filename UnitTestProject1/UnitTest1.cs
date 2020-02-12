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
    }
}
