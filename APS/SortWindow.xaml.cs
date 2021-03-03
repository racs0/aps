using APS.Entities;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
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


namespace APS
{
    /// <summary>
    /// Interaction logic for SortWindow.xaml
    /// </summary>
    public partial class SortWindow : Window
    {
        private string choosedFilePath;

        public SortWindow()
        {
            InitializeComponent();
        }

        private void BtnFilePath_Click(object sender, RoutedEventArgs e)
        {
            var ookiiDialog = new VistaFolderBrowserDialog();
            if (ookiiDialog.ShowDialog() == true)
            {
                choosedFilePath = ookiiDialog.SelectedPath;
                TxtBoxPath.Text = choosedFilePath;
                MessageBox.Show(ookiiDialog.SelectedPath);
            }
        }

        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            IEnumerable<MyPicture> pictures = mw.MyPictures;
            string chosenPath;

            for (int i = 0; i < pictures.Count()-1; i++)
            {
                DateTime lastModified = System.IO.File.GetLastWriteTime(pictures.ElementAt(i).Url.ToString());
                chosenPath = "choosedFilePath" + lastModified.ToString();
                System.IO.Directory.CreateDirectory(chosenPath);

                if (lastModified.ToString().Equals(chosenPath))
                {
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
