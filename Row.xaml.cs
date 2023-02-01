using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using Deafen.Class;
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
        private ChartValues<ObservablePoint> frequencyPoints = new ChartValues<ObservablePoint>();
        private ChartValues<ObservablePoint> volumePoints = new ChartValues<ObservablePoint>();
        public string Mute => "mute";

        private DataRow data;

        public Row()
        {
            InitializeComponent();
            data = MainWindow.MyCollection.Last();

            NameSequency.Text = data.Name;

            SequenceDetails.SetPointOnCartesianChart(data.frequencies, "frequency", frequencies, frequencyPoints);
            SequenceDetails.SetPointOnCartesianChart(data.frequencies, "volume", volumes, volumePoints);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            data.Play();
        }

        private void chart_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var chart = (CartesianChart)sender;
            ObservablePoint point = new ObservablePoint(chart.ConvertToChartValues(e.GetPosition(chart)).X, chart.ConvertToChartValues(e.GetPosition(chart)).Y);
            int index = frequencyPoints.ToList().FindIndex(value => value.X >= point.X);
            frequencyPoints.Insert(index, point);
            data.frequencies.Insert(index, new Frequency(point.X, point.Y, 100));
            data.CreateArrayByte();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SequenceDetails details = new SequenceDetails(data);
            details.Show();
        }
    }
}
