using Millionaire.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro SelectQSetsUC.xaml
    /// </summary>
    public partial class SelectQSetsUC : UserControl
    {
        NavigationManager navManager;
        QSetsManager qSetsManager;
        bool isSandbox;

        public SelectQSetsUC(NavigationManager navManager, QSetsManager qSetsManager, bool isSandbox)
        {
            InitializeComponent();

            this.navManager = navManager;
            this.qSetsManager = qSetsManager;
            this.isSandbox = isSandbox;

            if (qSetsManager.QuestionSets == null)
            {
                List<string> errors;
                qSetsManager.QuestionSets = FileManager.LoadQuestionSets(out errors);
                foreach (string error in errors)
                {
                    MessageBox.Show(error, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                qSetsManager.Sort();
            }

            QSetsListBox.ItemsSource = qSetsManager.QuestionSets;
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
            foreach (QSet qSet in QSetsListBox.SelectedItems)
            {
                temp.Add(qSet);
            }
            if (!isSandbox)
            {
                navManager.ShowGame(temp);
            }
            else
            {
                navManager.ShowSandboxUC(temp);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowMainMenu();
        }

        private void QSetsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QSetsListBox.SelectedItems.Count > 0)
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
