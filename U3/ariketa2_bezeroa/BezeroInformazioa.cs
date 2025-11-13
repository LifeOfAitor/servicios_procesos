using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ariketa2_bezeroa
{
    internal class BezeroInformazioa
    {
        public TcpClient Socket { get; set; }
        public int sahiakerak { get; set; } = 0;
    }
}
