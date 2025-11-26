using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace txat_aurreratua
{
    public class BezeroObjetua
    {
        public string izena;
        public TcpClient tcpClient;

        public BezeroObjetua(string izena, TcpClient tcpClient)
        {
            this.izena = izena;
            this.tcpClient = tcpClient;
        }
    }
}
