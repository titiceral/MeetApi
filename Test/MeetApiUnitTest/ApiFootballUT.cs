using MeetApi.MeetAPiSpooler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MeetApiUnitTest
{
    [TestClass]
    public class ApiFootballUT
    {
        [TestMethod]
        public void TestApi()
        {
            MeetApiSpooler spooler = new MeetApiSpooler();
            var ocInputs = spooler.Initialize();
            spooler.GetData(ocInputs.Values.FirstOrDefault());
            
        }
    }
}
