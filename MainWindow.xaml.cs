﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Millionaire.Models;
using Millionaire.Views;

namespace Millionaire
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NavigationManager navManager;


        public MainWindow()
        {
            InitializeComponent();

            //fileManager = new FileManager();
            //try
            //{
            //    fileManager.CopyDefault();                
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            navManager = new NavigationManager();
            DataContext = navManager;
            navManager.ShowMainMenu();
        }
    }
}
