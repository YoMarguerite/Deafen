using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisioForge.Libs.NAudio.Wave;

namespace Deafen.Class
{
    public class Beep
    {
        public static void Encoding(int samplesPerSecond, out MemoryStream mStrm ,out BinaryWriter writer)
        {
            mStrm = new MemoryStream();
            writer = new BinaryWriter(mStrm);

            int formatChunkSize = 16;
            int headerSize = 8;
            short formatType = 1;
            short tracks = 1;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int dataChunkSize = samplesPerSecond * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            // var encoding = new System.Text.UTF8Encoding();
            writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
            writer.Write(fileSize);
            writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
            writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(0x61746164); // = encoding.GetBytes("data")
            writer.Write(dataChunkSize);
        }

        public static void CreateBeep(MemoryStream mStrm, BinaryWriter writer, int samplesPerSecond, List<Frequency> frequencies)
        {
            for(int i = 0; i < frequencies.Count-1; i++)
            {
                int samples = (int)(samplesPerSecond * (frequencies[i+1].milliseconds - frequencies[i].milliseconds) / 1000);
                const double TAU = 2 * Math.PI;
                
                // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
                // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
                double amp = (int)(16383 * frequencies[i].volume /100) >> 2; // so we simply set amp = volume / 2
                //double theta = frequencies[i].frequency * TAU / (double)samplesPerSecond;
                for (int step = 0; step < samples; step++)
                {
                    double theta = frequencies[i].frequency + ((frequencies[i + 1].frequency - frequencies[i].frequency) * step / samplesPerSecond);
                    theta = theta * TAU / (double)samplesPerSecond;

                    short s = (short)(amp * Math.Sin(theta * (double)step));
                    writer.Write(s);
                }
            }
        }

        public static byte[] Concatenate(IEnumerable<byte[]> sourceData)
        {
            if(sourceData.Count() == 0)
            {
                return new byte[0];
            }

            var buffer = new byte[1024 * 4];
            WaveFileWriter waveFileWriter = null;

            using (var output = new MemoryStream())
            {
                try
                {
                    foreach (var binaryData in sourceData)
                    {
                        using (var audioStream = new MemoryStream(binaryData))
                        {
                            using (WaveFileReader reader = new WaveFileReader(audioStream))
                            {
                                if (waveFileWriter == null)
                                    waveFileWriter = new WaveFileWriter(output, reader.WaveFormat);
                                else
                                    AssertWaveFormat(reader, waveFileWriter);

                                WaveStreamWrite(reader, waveFileWriter, buffer);
                            }
                        }
                    }

                    waveFileWriter.Flush();

                    return output.ToArray();
                }
                finally
                {
                    waveFileWriter?.Dispose();
                }
            }
        }

        private static void AssertWaveFormat(WaveFileReader reader, WaveFileWriter writer)
        {
            if (!reader.WaveFormat.Equals(writer.WaveFormat))
            {
                throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format");
            }
        }

        private static void WaveStreamWrite(WaveFileReader reader, WaveFileWriter writer, byte[] buffer)
        {
            int read;
            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                writer.Write(buffer, 0, read);
            }
        }
    }
}
