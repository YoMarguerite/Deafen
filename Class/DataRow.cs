using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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

            CreateArrayByte();
        }

        public void CreateArrayByte()
        {
            MemoryStream mStrm;
            BinaryWriter writer;
            int samplesPerSecond = 44100;

            Beep.Encoding(samplesPerSecond, out mStrm, out writer);

            Beep.CreateBeep(mStrm, writer, samplesPerSecond, frequencies);

            mStrm.Seek(0, SeekOrigin.Begin);
            writer.Close();
            mStrm.Close();
            arrayByte = mStrm.ToArray();
        }

        public void Play()
        {
            MemoryStream stream = new MemoryStream(arrayByte);
            SoundPlayer player = new SoundPlayer(stream);
            player.Play();
        }
    }
}
