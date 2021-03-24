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
using Path = System.IO.Path;

namespace APS
{
    /// <summary>
    /// Interaction logic for SortWindow.xaml
    /// </summary>
    public partial class SortWindow : Window
    {
        private string choosedFilePath;
        private readonly MainWindow _mainWindow;


        public SortWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
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
            IEnumerable<MyPicture> pictures = _mainWindow.MyPictures;
            string chosenPath;

            for (int i = 1; i < pictures.Count(); i++)
            {
                DateTime lastModified = File.GetLastWriteTime(pictures.ElementAt(i).Url.LocalPath);
                chosenPath = choosedFilePath + @"\" + lastModified.ToShortDateString();

                if (!Directory.Exists(chosenPath))
                {
                    Directory.CreateDirectory(chosenPath);
                }

                string lastFolderName = Path.GetFileName(chosenPath);


                if (lastModified.ToShortDateString().Equals(lastFolderName))
                {
                    MoveFile(pictures.ElementAt(i).Url.LocalPath, chosenPath + @"\" + pictures.ElementAt(i).Title);
                }

            }
        }

        private async void MoveFile(string source, string destination)
        {
            try
            {
                using (FileStream sourceStream = File.Open(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (FileStream destinationStream = File.Create(destination))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                        sourceStream.Close();

                    }
                }

                //if (File.Exists(destination))
                //{
                //    MessageBox.Show("The image already exists.");
                //}
                //else
                //{
                //    File.Move(source, destination);
                //}
            }
            catch (IOException ioex)
            {
                MessageBox.Show("An IOException occured during move, " + ioex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Exception occured during move, " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
