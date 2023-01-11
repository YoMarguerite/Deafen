using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deafen.Class
{
    public class DataRow
    {
        public string Name { get; set; }
        public byte[] arrayByte { get; set; }

        public DataRow(string n, byte[] b)
        {
            Name = n;
            arrayByte = b;
        }
    }
}
