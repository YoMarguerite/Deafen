using Deafen.Class;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
            byte[] arrayByte = Beep.PlayBeep(ushort.Parse(text.Text), 1000);
            MyCollection.Add(new DataRow("OOOO", arrayByte));
        }
    }
}
