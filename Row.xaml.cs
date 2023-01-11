using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Deafen
{
    /// <summary>
    /// Logique d'interaction pour Row.xaml
    /// </summary>
    public partial class Row : UserControl
    {
        private ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> Points => points;
        public string Mute => "mute";

        private Class.DataRow data;

        public Row()
        {
            InitializeComponent();
            data = MainWindow.MyCollection.Last();

            Title.Text = data.Name;

            if (data.arrayByte.Length == 0)
            {
                GenerateGraphicsData();
            }
            else
            {
                for (int i = 0; i < data.arrayByte.Length; i++)
                {
                    points.Add(new ObservablePoint(i, data.arrayByte[i]));
                }
            }

            chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = points ,
                    PointGeometrySize = 1
                } 
            };

        }

        private void GenerateGraphicsData()
        {
            Random rand = new Random();

            for (int i = 0; i < 1000000; i++)
            {
                int ord = rand.Next(200, 3000);
                points.Add(new ObservablePoint(i, 1000));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //var wav = new WavePcmFormat(buffer, numChannels: 1, sampleRate: 44100, bitsPerSample: 16);
            //var rawDataWithHeader = wav.ToBytesArray();

            MemoryStream stream = new MemoryStream(data.arrayByte);
            SoundPlayer player = new SoundPlayer(stream);
            player.Play();
        }
    }
}
