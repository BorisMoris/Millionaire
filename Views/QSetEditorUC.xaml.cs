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
        public QSet EditedQSet { get; set; }
        private NavigationManager navManager;

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

        public QSetEditorUC(NavigationManager navigationManager, string path)
        {
            navManager = navigationManager;
            
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
            //foreach (Question question in EditedQSet.EasyQuestions)
            //{
            //    Console.WriteLine(question.QuestionSentence + " " + question.RightAnswer);
            //}
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowManageQSets();
        }

        /// <summary>
        /// Refresh currently displayed collection view
        /// </summary>
        private void Refresh()
        {
            ICollectionView collectionView = (ICollectionView)questionsListBox.ItemsSource;
            collectionView.Refresh();
        }        
    }
}
