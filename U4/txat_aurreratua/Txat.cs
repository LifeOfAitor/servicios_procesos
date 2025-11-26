using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txat_aurreratua
{
    public static class Txat
    {
        public static string txat = "";

        public static void gehitu(string mezua)
        {
            txat += $"{mezua}+ \n";
        }
    }
}
