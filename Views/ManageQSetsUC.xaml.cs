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
                string file = saveFileDialog.FileName;
                
                try
                {
                    FileManager.ExportQSet(selected.Path, file);
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
                    
                    if (qSetsManager.CheckName(newQSet.Name))
                    {
                        MessageBox.Show("Sada otázek s názvem " + newQSet.Name + " už existuje. Pokud chcete přesto importovat, změňte název jedné ze sad.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    newQSet.Path = FileManager.GenerateFilePath(newQSet.Name);
                    if (qSetsManager.CheckPath(newQSet.Path))
                    {
                        MessageBox.Show("Pro název " + newQSet.Name + " nelze vygenerovat jedinečný název souboru.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    FileManager.ImportQSet(file);
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

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowQSetEditor(QSetsListBox.SelectedIndex);
        }

        private void newQSetButton_Click(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog = new InputDialog("Zadejte jméno nové sady:", string.Empty);
            if (inputDialog.ShowDialog() == true)
            {
                string name = inputDialog.Answer;

                if (FileManager.ContainsInvalidChars(name)) //check if the name doesn't contain invalid characters
                {
                    MessageBox.Show($"Zadaný název obsahuje nedovolené znaky.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (qSetsManager.CheckName(name)) //check if the name isn't in use already
                {
                    MessageBox.Show("Sada otázek s názvem " + name + " už existuje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string path = FileManager.GenerateFilePath(name);
                if (qSetsManager.CheckPath(path))
                {
                    MessageBox.Show("Pro název " + name + " nelze vygenerovat jedinečný název souboru.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                navManager.ShowQSetEditor(name, path);
            }            
        }
    }
}