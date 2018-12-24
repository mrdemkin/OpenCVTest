using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openCVTest
{
    public static class CommonTools
    {
        private const string LOG_LINE_SEPARATOR = "===========";

        public static void WriteLog(string log)
        {
            Console.WriteLine(log);
            Console.WriteLine(LOG_LINE_SEPARATOR);
        }

        public static void WaitUser()
        {
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
