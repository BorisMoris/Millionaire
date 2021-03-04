using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro SelectQSetsUC.xaml
    /// </summary>
    public partial class SelectQSetsUC : UserControl
    {
        public List<QSet> avalaibleQSets;
        NavigationManager navManager;
        QSetsManager qSetsManager;
        
        public SelectQSetsUC(NavigationManager navManager, QSetsManager qSetsManager)
        {
            InitializeComponent();

            this.navManager = navManager;
            this.qSetsManager = qSetsManager;

            if (qSetsManager.QuestionSets == null)
            {
                Console.WriteLine("loading dictionary");
                
                List<Exception> exceptions;
                qSetsManager.QuestionSets = FileManager.LoadQuestionSets(out exceptions);
                foreach (Exception ex in exceptions)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            avalaibleQSets = qSetsManager.QuestionSets.Values.ToList();
            QSetsListBox.ItemsSource = avalaibleQSets;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            QSetsListBox.SelectAll();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            QSetsListBox.UnselectAll();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            
            List<QSet> temp = new List<QSet>();
            foreach(QSet qSet in QSetsListBox.SelectedItems)
            {
                temp.Add(qSet);
            }
            navManager.ShowGame(temp);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowMainMenu();
        }

        private void QSetsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QSetsListBox.SelectedItems.Count>0)
            {
                continueButton.IsEnabled = true;
            }
            else
            {
                continueButton.IsEnabled = false;
            }
        }
    }
}
