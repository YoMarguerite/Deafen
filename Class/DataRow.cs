using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deafen.Class
{
    public class DataRow
    {
        public string Name;
        public byte[] arrayByte;
        public List<Frequency> frequencies;

        public DataRow(string n, List<Frequency> f)
        {
            Name = n;
            frequencies = f;

            MemoryStream mStrm;
            BinaryWriter writer;
            int samplesPerSecond = 44100;

            Beep.Encoding(samplesPerSecond, out mStrm, out writer);

            Beep.CreateBeep(mStrm, writer, samplesPerSecond, frequencies);

            mStrm.Seek(0, SeekOrigin.Begin);
            new System.Media.SoundPlayer(mStrm).Play();
            writer.Close();
            mStrm.Close();
            arrayByte =  mStrm.ToArray();
        }
    }
}
