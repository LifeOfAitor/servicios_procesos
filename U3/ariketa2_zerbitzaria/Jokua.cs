using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ariketa2_zerbitzaria
{
    internal class Jokua
    {
        public const int JOKALARIAK = 10;
        public int zenbakia;
        public Jokua()
        {
            Random rand = new Random();
            zenbakia = rand.Next(1, 101);
        }  
    }
}
