using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interakční logika pro QSetEditorUC.xaml
    /// </summary>
    public partial class QSetEditorUC : UserControl
    {
        public QSet EditedQSet { get; private set; }
        private HashSet<Question> editedQuestions; //HashSet to store only unique items
        private bool saved;

        private NavigationManager navManager;
        private QSetsManager qSetsManager;        

        public ICollectionView EasyCollectionView { get; private set; }
        public ICollectionView MediumCollectionView { get; private set; }
        public ICollectionView HardCollectionView { get; private set; }

        private string filterKey = string.Empty;
        public string FilterKey
        {
            get
            {
                return filterKey;
            }
            set
            {
                filterKey = value;
                Refresh();
            }
        }

        private List<TextBox> questionTextBoxes;
        private Difficulty selectedDifficulty
        {
            get
            {
                switch (difficultyComboBox.SelectedIndex)
                {
                    case 0:
                        return Difficulty.Easy;
                    case 1:
                        return Difficulty.Medium;
                    case 2:
                        return Difficulty.Hard;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public QSetEditorUC(NavigationManager navManager, QSetsManager qSetsManager, bool isNew, int index)
        {
            this.navManager = navManager;
            this.qSetsManager = qSetsManager;

            editedQuestions = new HashSet<Question>();
            saved = true;

            if (isNew)
            {
                EditedQSet = new QSet();
            }
            else
            {
                EditedQSet = qSetsManager.QuestionSets[index];
            }            

            InitializeComponent();
            DataContext = this;

            questionTextBoxes = new List<TextBox> { questionTextBox, rightAnswerTextBox, wrongAnswer1TextBox, wrongAnswer2TextBox, wrongAnswer3TextBox };

            EasyCollectionView = CollectionViewSource.GetDefaultView(EditedQSet.EasyQuestions);
            MediumCollectionView = CollectionViewSource.GetDefaultView(EditedQSet.MediumQuestions);
            HardCollectionView = CollectionViewSource.GetDefaultView(EditedQSet.HardQuestions);

            questionsListBox.Items.SortDescriptions.Add(new SortDescription("QuestionSentence", ListSortDirection.Ascending));
            questionsListBox.Items.IsLiveSorting = true;
            questionsListBox.Items.LiveSortingProperties.Add("QuestionSentence");

            questionsListBox.Items.Filter = FiterQuestions;
            questionsListBox.Items.IsLiveFiltering = true;
            questionsListBox.Items.LiveFilteringProperties.Add("QuestionSentence");
        }        

        private bool FiterQuestions(object obj)
        {
            if(obj is Question question)
            {
                return question.QuestionSentence.IndexOf(FilterKey, StringComparison.InvariantCultureIgnoreCase)>=0;
            }
            else
            {
                return false;
            }            
        }

        private void questionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isSelected = questionsListBox.SelectedItem != null;
            foreach(TextBox textBox in questionTextBoxes)
            {
                textBox.IsEnabled = isSelected;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (questionsListBox.SelectedItem != null)
            {
                switch (selectedDifficulty)
                {
                    case Difficulty.Easy:
                        EditedQSet.EasyQuestions.Remove((Question)questionsListBox.SelectedItem);
                        break;
                    case Difficulty.Medium:
                        EditedQSet.MediumQuestions.Remove((Question)questionsListBox.SelectedItem);
                        break;
                    case Difficulty.Hard:
                        EditedQSet.HardQuestions.Remove((Question)questionsListBox.SelectedItem);
                        break;
                }
            }            

            Refresh();
            saved = false;
        }

        private void newQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            FilterKey = string.Empty;
            filterTextBox.Text = string.Empty;
            
            editedQuestions.Add(EditedQSet.AddQuestion(selectedDifficulty));
            saved = false;
            
            Refresh();
            questionsListBox.SelectedIndex = 0;
            questionTextBox.Focus();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!qSetsManager.CheckName(EditedQSet.Name))
            {
                MessageBox.Show($"Chyba při ukládání otázek:\nSada otázek s názvem \"{EditedQSet.Name}\" už existuje. Nelze vytvořit novou sadu se stejn", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!saved)
            {
                MessageBoxResult result = MessageBox.Show("Chystáte se zavřít editor. Přejete si uložit změny?", "Uložit změny", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    if (!Save())
                    {
                        return;
                    }
                }
            }
            navManager.ShowManageQSets();
        }

        private void textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (saved)
            {
                saved = false;
            }
            editedQuestions.Add((Question)questionsListBox.SelectedItem);
        }

        /// <summary>
        /// Refresh currently displayed collection view
        /// </summary>
        private void Refresh()
        {
            ICollectionView collectionView = (ICollectionView)questionsListBox.ItemsSource;
            collectionView.Refresh();
        }

        /// <summary>
        /// Check and save edited question set
        /// </summary>
        /// <returns>True if the qSet was saved successfuly, otherwise false </returns>
        private bool Save()
        {
            string error = qSetsManager.CheckQSet(EditedQSet);
            if (error != null)
            {
                MessageBox.Show($"Chyba při ukládání otázek:\n{error}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
            var result = qSetsManager.CheckQuestions(editedQuestions);
            if (result.Item1 != null && result.Item2 != null)
            {
                MessageBox.Show($"Chyba při ukládání otázek:\n{result.Item2}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                SelectQuestion(result.Item1);
                return false;
            }

            try
            {
                FileManager.SaveQSet(EditedQSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ukládání otázek:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            saved = true;
            editedQuestions.Clear();
            return true;
        }       
        
        private void SelectQuestion(Question wantedQuestion)
        {            
            int index;
            index = IndexOfQuestion(EasyCollectionView, wantedQuestion);
            if (index >= 0)
            {
                difficultyComboBox.SelectedIndex = 0;
                questionsListBox.SelectedIndex = index;
                
                return;
            }
            else
            {
                index = IndexOfQuestion(MediumCollectionView, wantedQuestion);
                if (index >= 0)
                {
                    difficultyComboBox.SelectedIndex = 1;
                    questionsListBox.SelectedIndex = index;
                    
                    return;
                }
                else
                {
                    index = IndexOfQuestion(HardCollectionView, wantedQuestion);
                    if (index >= 0)
                    {
                        difficultyComboBox.SelectedIndex = 2;
                        questionsListBox.SelectedIndex = index;
                        
                        return;
                    }
                }
            }
        }

        private int IndexOfQuestion(ICollectionView collection, Question wantedQuestion)
        {
            int counter = 0;
            foreach (Question question in collection)
            {
                if (question == wantedQuestion)
                {
                    Console.WriteLine(counter);
                    return counter;
                }
                counter++;
            }
            return -1;
        }
    }
}
