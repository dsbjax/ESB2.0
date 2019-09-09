using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports
{
    public static class StatusReport
    {
        public static void PrintReport()
        {
            var statusReport = DatabaseAccess.GetStatusReport();
        }
    }
}
