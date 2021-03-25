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
        private HashSet<(Question, Difficulty)> editedQuestions; //HashSet to store only unique items
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

        public QSetEditorUC(NavigationManager navManager, QSetsManager qSetsManager, string path)
        {
            this.navManager = navManager;
            this.qSetsManager = qSetsManager;

            editedQuestions = new HashSet<(Question, Difficulty)>();
            saved = true;
            
            try
            {
                EditedQSet = FileManager.LoadQSetFromFile(path);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Soubor se sadou se nepodařilo načíst.\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
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
        }

        private void newQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            FilterKey = string.Empty;
            filterTextBox.Text = string.Empty;
            
            EditedQSet.AddQuestion(selectedDifficulty);            
            
            Refresh();
            questionsListBox.SelectedIndex = 0;
            questionTextBox.Focus();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            //FileManager.SaveQSet(EditedQSet);

            var result = qSetsManager.CheckQuestions(editedQuestions);
            if (result.Item1 != null && result.Item2!=null)
            {
                MessageBox.Show($"Chyba při ukládání otázek:\n {result.Item2}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                //     Console.WriteLine(EasyCollectionView.Contains(result.Item2));
                //Console.WriteLine(FindQuestion(result.Item1));
                //questionsListBox.SelectedIndex = FindQuestion(result.Item1);
                SelectQuestion(result.Item1);
                return;
            }

            Save();
            saved = true;
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {

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
                    Save();
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
            Console.WriteLine(editedQuestions.Add(((Question)questionsListBox.SelectedItem, selectedDifficulty)));
        }

        /// <summary>
        /// Refresh currently displayed collection view
        /// </summary>
        private void Refresh()
        {
            ICollectionView collectionView = (ICollectionView)questionsListBox.ItemsSource;
            collectionView.Refresh();
        }

        private void Save() { }       
        
        private void SelectQuestion(Question wantedQuestion)
        {
            Difficulty difficulty = Difficulty.Easy;
            int index = 0;

            int counter = 0;
            foreach (Question question in EasyCollectionView)
            {
                if (question == wantedQuestion)
                {
                    difficulty = Difficulty.Easy;
                    index = counter;
                }
                counter++;
            }
            counter = 0;
            foreach (Question question in MediumCollectionView)
            {
                if (question == wantedQuestion)
                {
                    difficulty = Difficulty.Medium;
                    index = counter;
                }
                counter++;
            }
            counter = 0;
            foreach (Question question in HardCollectionView)
            {
                if (question == wantedQuestion)
                {
                    difficulty = Difficulty.Hard;
                    index = counter;
                }
                counter++;
            }

            difficultyComboBox.SelectedIndex = (int)difficulty;
            questionsListBox.SelectedIndex = index;

            //int counter = 0;
            //foreach (Question question in EasyCollectionView)
            //{
            //    if(question == wantedQuestion)
            //    {
            //        difficultyComboBox.SelectedIndex = 0;
            //        questionsListBox.SelectedIndex = counter;
            //    }
            //    counter++;
            //}
            //counter = 0;
            //foreach (Question question in MediumCollectionView)
            //{
            //    if (question == wantedQuestion)
            //    {
            //        difficultyComboBox.SelectedIndex = 1;
            //        questionsListBox.SelectedIndex = counter;
            //    }
            //    counter++;
            //}
            //counter = 0;
            //foreach (Question question in HardCollectionView)
            //{
            //    if (question == wantedQuestion)
            //    {
            //        difficultyComboBox.SelectedIndex = 2;
            //        questionsListBox.SelectedIndex = counter;
            //    }
            //    counter++;
            //}
        }
    }
}
