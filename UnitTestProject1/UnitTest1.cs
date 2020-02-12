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
            Console.WriteLine(CSV.GetLengthFromCSV(@"\\nas72v2\vdi_data\USA\rparso2\Documents\Automation Anywhere Files\Automation Anywhere\Shaw Files\0066_InvoiceCDPackages\test.csv",","));
        }
    }
}
