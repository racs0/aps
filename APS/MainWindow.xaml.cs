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
        private bool _disabled;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _disabled = false;
        }

        public ObservableCollection<MyPicture> MyPictures { get; set; } = new ObservableCollection<MyPicture>();
        public Dictionary<string, string> pictureLocations { get; set; }


        public ShutdownMode ShutdownMode { get; set; }

        private void paste(IEnumerable<MyPicture> newPictures)
        {
            MyPictures.Clear();
            pictureLocations = new Dictionary<string, string>();
            foreach (var item in newPictures)
            {
                MyPictures.Add(item);
                pictureLocations.Add(item.Title, item.Url.LocalPath);
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

                BG.Opacity = 0.35;
                _disabled = true;
                UpdateListViewItems();
            }
        }

        private void UpdateListViewItems()
        {
            Sort.IsEnabled = _disabled;
        }

        private void ListViewItem_MouseEnter_Sort(object sender, MouseEventArgs e)
        {
            UpdateListViewItems();
            MyPictures.Clear();
            SortWindow s = new SortWindow(this);
            s.Show();
            ShutdownMode = ShutdownMode.OnLastWindowClose;
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
