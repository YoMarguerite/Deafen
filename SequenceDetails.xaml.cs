using Deafen.Class;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Deafen
{
    public partial class SequenceDetails : Window
    {
        public DataRow data = new DataRow("", new List<Frequency>());
        private ChartValues<ObservablePoint> frequencyPoints = new ChartValues<ObservablePoint>();
        private ChartValues<ObservablePoint> volumePoints = new ChartValues<ObservablePoint>();

        public SequenceDetails(DataRow row = null)
        {
            InitializeComponent();

            if (row != null)
            {
                data = row;
            }

            NameSequency.Text = data.Name;

            SetPointOnCartesianChart(data.frequencies, "frequency", Frequencies, frequencyPoints);
            SetPointOnCartesianChart(data.frequencies, "volume", Volumes, volumePoints);
        }

        public static void SetPointOnCartesianChart(List<Frequency> frequencies, string propertyName, CartesianChart chart, ChartValues<ObservablePoint> points)
        {
            if(points == null)
            {
                points = new ChartValues<ObservablePoint>();
            }

            frequencies.ForEach((value) =>
            {
                var prop = typeof(Frequency).GetField(propertyName);
                var y = (double)prop.GetValue(value);
                points.Add(new ObservablePoint(value.milliseconds, y));
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

        public static void chart_MouseDoubleClick(object sender, MouseButtonEventArgs e, ChartValues<ObservablePoint> points, DataRow data)
        {
            var chart = (CartesianChart)sender;
            ObservablePoint point = new ObservablePoint(chart.ConvertToChartValues(e.GetPosition(chart)).X, chart.ConvertToChartValues(e.GetPosition(chart)).Y);
            int index = points.ToList().FindIndex(value => value.X >= point.X);
            points.Insert(index, point);
            data.frequencies.Insert(index, new Frequency(point.X, point.Y, 100));
            data.CreateArrayByte();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
