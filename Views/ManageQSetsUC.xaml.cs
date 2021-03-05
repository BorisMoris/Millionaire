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
        public QSetsManager qSetsManager { get; set; }
        List<Button> qSetActionButtons;
        QSet selected;

        public ManageQSetsUC(NavigationManager navigationManager, QSetsManager qSetsManager )
        {
            navManager = navigationManager;
            this.qSetsManager = qSetsManager;

            if (qSetsManager.QuestionSets == null)
            {
                List<Exception> exceptions;
                qSetsManager.QuestionSets = FileManager.LoadQuestionSets(out exceptions);
                foreach (Exception ex in exceptions)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                qSetsManager.Sort();
            }

            InitializeComponent();

            qSetActionButtons = new List<Button> { editButton, exportButton, deleteButton };
            QSetsListBox.ItemsSource = qSetsManager.QuestionSets;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowMainMenu();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show("Opravdu chcete smazat sadu " + selected.Name + "?", "Smazat sadu?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    FileManager.DeleteQSet(selected.Path);
                    qSetsManager.QuestionSets.Remove(selected);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Soubor se nepodařilo odstranit.\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.Filter = "Dokument CSV|*.csv";

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string dir = saveFileDialog.InitialDirectory;
                string file = saveFileDialog.FileName;
                Console.WriteLine("saved file " + dir + file);

                selected = (QSet)QSetsListBox.SelectedItem;
                try
                {
                    FileManager.ExportQSet(selected.Path, System.IO.Path.Combine(dir, file));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export se nezdařil.\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void QSetsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            foreach(Button button in qSetActionButtons)
            {
                button.IsEnabled = QSetsListBox.SelectedItem != null;
            }
            selected = (QSet)QSetsListBox.SelectedItem;
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = ".csv";
            openFileDialog.Filter = "Dokumenty CSV|*.csv";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string file = openFileDialog.FileName;
                QSet newQSet = null;

                try
                {
                    newQSet = FileManager.LoadQSetFromFile(file);

                    foreach (QSet qSet in qSetsManager.QuestionSets)
                    {
                        if (newQSet.Name == qSet.Name)
                        {
                            MessageBox.Show("Sada otázek s názvem " + newQSet.Name + " už existuje. Pokud chcete přesto importovat, změňte název jedné ze sad.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    newQSet.Path=FileManager.ImportQSet(file);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                qSetsManager.QuestionSets.Add(newQSet);
                qSetsManager.Sort();
                QSetsListBox.ItemsSource = qSetsManager.QuestionSets;
            }            
        }
    }
}