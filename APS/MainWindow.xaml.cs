using APS.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace APS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ObservableCollection<MyPicture> MyPictures { get; } = new ObservableCollection<MyPicture>();


        private void paste(IEnumerable<MyPicture> newPictures)
        {
            foreach (var item in newPictures)
            {
                MyPictures.Add(item);
            }
        }

        private void ListViewItem_MouseEnter_Folder(object sender, MouseEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog()
            {
                Title = "Select picture(s)",
                Filter = "All supported graphics|" +
                        "*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|" +
                        "*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)" +
                        "|*.png",
                InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyPictures),
                Multiselect = true
            };

            if (op.ShowDialog() == true)
            {
                paste(op.FileNames.Select(f => new MyPicture
                {
                    Url = new Uri(f, UriKind.Absolute),
                    Title = System.IO.Path.GetFileName(f)
                }));
            }
        }

        private void ListViewItem_MouseEnter_Sort(object sender, MouseEventArgs e)
        {
      
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
