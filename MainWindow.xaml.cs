using Deafen.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;

namespace Deafen
{
    public partial class MainWindow : Window
    {
        public static ObservableCollection<DataRow> MyCollection = new ObservableCollection<DataRow>();

        public MainWindow()
        {
            InitializeComponent();

            List.ItemsSource = MyCollection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var window = new SequenceDetails();
            //window.Show();
            int freq1 = UInt16.Parse(text.Text);
            double vol1 = slider.Value;

            int freq2 = UInt16.Parse(text2.Text);
            double vol2 = slider2.Value;

            List<Frequency> frequencies = new List<Frequency>()
            {
                new Frequency(0,freq1,vol1),
                new Frequency(UInt16.Parse(milliseconds.Text),freq2,vol2)
            };

            var data = new DataRow(text.Text, frequencies);
            data.Play();
            MyCollection.Add(data);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<Frequency> frequencies = new List<Frequency>();
            MyCollection.ToList().ForEach((row) => {
                row.frequencies.ForEach((frequency) =>
                {
                    int index = frequencies.FindIndex(value => frequency.milliseconds >= value.milliseconds);
                    Console.WriteLine(index);
                    if (index >= 0 && index > frequencies.Count)
                    {
                        frequencies.Insert(index, frequency);
                    }
                    else
                    {
                        frequencies.Add(frequency);
                    }
                });
            });

            IEnumerable<IGrouping<double, Frequency>> query = frequencies.GroupBy(freq => freq.milliseconds, freq => freq);
            List<Frequency> globalFreq = new List<Frequency>();

            foreach (IGrouping<double, Frequency> freqGroup in query)
            {
                double num = 0;
                double denominateur = 0;
                foreach (Frequency freq in freqGroup)
                {
                    num += freq.frequency * freq.volume;
                    denominateur += freq.volume;
                }
                globalFreq.Add(new Frequency(freqGroup.Key, num / denominateur, denominateur / freqGroup.Count()));
            }

            DataRow final = new DataRow("final", globalFreq);
            MyCollection.Add(final);
        }
    }
}
