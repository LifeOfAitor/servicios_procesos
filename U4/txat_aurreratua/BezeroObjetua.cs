using System;
using System.Collections.Generic;
using System.IO;
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
        public NetworkStream ns = null;
        public StreamWriter sw = null;
        public StreamReader sr = null;

    public BezeroObjetua(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            ns = this.tcpClient.GetStream();
            sw = new StreamWriter(ns) { AutoFlush = true };
            sr = new StreamReader(ns);
        }
    public void setIzena(string izena)
        {
            this.izena = izena;
        }
    }
}
