using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txat_aurreratua
{
    public class Txat
    {
        public string txat = "";

        public void gehitu(string mezua)
        {
            txat += $"{mezua}+ \n";
        }
    }
}
