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

            data.frequencies.ForEach((value) =>
            {
                points.Add(new ObservablePoint(value.milliseconds, value.frequency));
            });

            chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = points ,
                    PointGeometrySize = 5,
                    LineSmoothness = 0
                } 
            };
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
