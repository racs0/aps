using APS.Entities;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<string> duplicates;
        private List<string> imgList;
        private Dictionary<string,string> pictures; 


        public SortWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            duplicates = new List<string>();
            imgList = new List<string>();
            pictures =_mainWindow.pictureLocations; 
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
            string chosenPath;

            for (int i = 1; i < pictures.Count(); i++)
            {
                DateTime lastModified = File.GetLastWriteTime(pictures.Values.ElementAt(i));
                chosenPath = choosedFilePath + @"\" + lastModified.ToShortDateString();

                if (!Directory.Exists(chosenPath))
                {
                    Directory.CreateDirectory(chosenPath);
                }

                string lastFolderName = Path.GetFileName(chosenPath);


                if (lastModified.ToShortDateString().Equals(lastFolderName))
                {
                    MoveFile(pictures.Values.ElementAt(i), chosenPath +  @"\" + pictures.Keys.ElementAt(i));
                    //imgList.Add(pictures..ElementAt(i));
                }

                //if (i == pictures.Count() - 1 && duplicates.Count > 0)
                //{
                //    MessageBox.Show($"{duplicates.Count} duplicates detected and couldn't be sorted!");

                //    for (int j = 0; j < duplicates.Count; j++)
                //    {
                //        pictures.Remove(duplicates[j]);
                //    }

                //    duplicates.Clear();
                //}

                //if (i == pictures.Count()-1 && duplicates.Count == 0)
                //{
                //    MessageBox.Show($"{pictures.Count()} Pictures are successfully sorted.");
                //    removeOldOnes(chosenPath);
                //    this.Close();
                //}


                if (i == pictures.Count() - 1)
                {
                    MessageBox.Show($"{pictures.Count()} Pictures are successfully sorted.");
                    removeOldOnes(chosenPath);
                    this.Close();
                }

            }
        }

        private async void removeOldOnes(string source)
        {
            _mainWindow.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            for (int i = 0; i < pictures.Keys.Count; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(pictures.Values.ElementAt(i));
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
                        if (File.Exists(destination))
                        {
                            duplicates.Add(destination);
                        }
                        await sourceStream.CopyToAsync(destinationStream);
                        sourceStream.Close();

                    }
                }

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
