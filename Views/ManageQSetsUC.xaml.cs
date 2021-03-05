using System;
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

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro ManageQSetsUC.xaml
    /// </summary>
    public partial class ManageQSetsUC : UserControl
    {
        NavigationManager navManager;
        QSetsManager qSetsManager;

        public ManageQSetsUC(NavigationManager navigationManager, QSetsManager qSetsManager )
        {
            InitializeComponent();

            navManager = navigationManager;
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

            QSetsListBox.ItemsSource = qSetsManager.QuestionSets;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowMainMenu();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            QSet selected = (QSet)QSetsListBox.SelectedItem;
            MessageBoxResult result = MessageBox.Show("Opravdu chcete smazat sadu " + selected.Name + "?", "Smazat sadu?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                FileManager.DeleteQSet(selected.Path);
                qSetsManager.QuestionSets.Remove(selected);
            }
        }
    }
}
