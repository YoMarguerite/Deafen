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
            int freq1 = UInt16.Parse(text.Text);
            int freq2 = UInt16.Parse(text2.Text);
            List<Frequency> frequencies = new List<Frequency>()
            {
                new Frequency(0,freq1,0),
                new Frequency(200,freq1,0),
                new Frequency(200,0,0),
                new Frequency(400,0,0),
                new Frequency(400,freq1 + (freq2 - freq1),0),
                new Frequency(600,freq1 + (freq2 - freq1),0),
                new Frequency(600,0,0),
                new Frequency(800,0,0),
                new Frequency(800,freq2,0),
                new Frequency(1000,freq2,0)
            };
            MyCollection.Add(new DataRow(text.Text, frequencies));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<byte[]> bytes = new List<byte[]>();
            MyCollection.ToList().ForEach((value) =>{
                bytes.Add(value.arrayByte);
                Play(value.arrayByte);
            });
            //byte[] finalByte = Beep.Concatenate(bytes);

            //MemoryStream stream = new MemoryStream(finalByte);
            //SoundPlayer player = new SoundPlayer(stream);
            //player.Play();
        }

        private void Play(byte[] array)
        {
            MemoryStream stream = new MemoryStream(array);
            SoundPlayer player = new SoundPlayer(stream);
            player.Play();
        }
    }
}
