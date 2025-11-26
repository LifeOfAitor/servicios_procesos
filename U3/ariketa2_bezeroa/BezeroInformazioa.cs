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
//posible merjora
/*
 * internal class BezeroInformazioa
{
    public TcpClient Socket { get; private set; }
    public StreamReader Reader { get; private set; }
    public StreamWriter Writer { get; private set; }
    public int Sahiakerak { get; set; } = 0;

    public BezeroInformazioa(TcpClient socket)
    {
        Socket = socket;
        NetworkStream stream = socket.GetStream();
        Reader = new StreamReader(stream);
        Writer = new StreamWriter(stream) { AutoFlush = true };
    }

    public void Close()
    {
        Writer.Close();
        Reader.Close();
        Socket.Close();
    }
}
*/