using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interakční logika pro QSetEditorUC.xaml
    /// </summary>
    public partial class QSetEditorUC : UserControl
    {
        public QSet EditedQSet { get; private set; }
        private HashSet<Question> editedQuestions; //HashSet to store only unique items

        private bool saved = true;
        public bool Saved
        {
            get { return saved; }
            set {
                saved = value;
                savedInfoStackPanel.Visibility = saved ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private bool isNew;
        private bool nameChanged;
        private string actualPath;
        private int index;

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

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="navManager"></param>
        /// <param name="qSetsManager"></param>
        public QSetEditorUC(NavigationManager navManager, QSetsManager qSetsManager)
        {
            InitializeComponent();
            DataContext = this;

            this.navManager = navManager;
            this.qSetsManager = qSetsManager;

            editedQuestions = new HashSet<Question>();
            nameChanged = false;

            questionTextBoxes = new List<TextBox> { questionTextBox, rightAnswerTextBox, wrongAnswer1TextBox, wrongAnswer2TextBox, wrongAnswer3TextBox };            
        }

        /// <summary>
        /// Constructor for editing existing QSet
        /// </summary>
        /// <param name="navManager"></param>
        /// <param name="qSetsManager"></param>
        /// <param name="index"></param>
        public QSetEditorUC(NavigationManager navManager, QSetsManager qSetsManager, QSet qSet, int index) : this(navManager, qSetsManager)
        {
            EditedQSet = qSet;
            isNew = false;
            this.index = index;
            actualPath = EditedQSet.Path;
            SetCollectionViews();
        }

        /// <summary>
        /// Constructor for creating new QSet
        /// </summary>
        /// <param name="navManager"></param>
        /// <param name="qSetsManager"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public QSetEditorUC(NavigationManager navManager, QSetsManager qSetsManager, string name, string path) : this (navManager, qSetsManager)
        {            
            EditedQSet = new QSet(name, path);
            isNew = true;
            SetCollectionViews();
        }

        /// <summary>
        /// Set sorting and filtering
        /// </summary>
        private void SetCollectionViews()
        {
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
                Question toRemove = (Question)questionsListBox.SelectedItem;
                switch (selectedDifficulty)
                {
                    case Difficulty.Easy:
                        EditedQSet.EasyQuestions.Remove(toRemove);
                        break;
                    case Difficulty.Medium:
                        EditedQSet.MediumQuestions.Remove(toRemove);
                        break;
                    case Difficulty.Hard:
                        EditedQSet.HardQuestions.Remove(toRemove);
                        break;
                }

                Refresh();
                Saved = false;
                editedQuestions.Remove(toRemove);
            }
        }

        private void newQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            FilterKey = string.Empty;
            filterTextBox.Text = string.Empty;
            
            editedQuestions.Add(EditedQSet.AddQuestion(selectedDifficulty));
            Saved = false;
            
            Refresh();
            questionsListBox.SelectedIndex = 0;
            questionTextBox.Focus();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Saved)
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

            QSet savedQSet;
            try
            {
                savedQSet = FileManager.LoadQSetFromFile(actualPath);
                if (isNew)
                {
                    qSetsManager.QuestionSets.Add(savedQSet);
                }
                else
                {
                    qSetsManager.QuestionSets[index] = savedQSet;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nelze načíst změny v sadě otázek:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            navManager.ShowManageQSets();
        }

        /// <summary>
        /// Mark current question as edited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Saved)
            {
                Saved = false;
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
            if (nameChanged && actualPath!=null)
            {
                try
                {
                    File.Move(actualPath, EditedQSet.Path);
                    actualPath = EditedQSet.Path;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba při ukládání otázek:\nPřejmenování souboru se nezdařilo: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

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
                actualPath = EditedQSet.Path;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ukládání otázek:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }          

            Saved = true;
            nameChanged = false;
            editedQuestions.Clear();
            return true;
        }       
        
        /// <summary>
        /// Selects given Question in questionListBox
        /// </summary>
        /// <param name="wantedQuestion"></param>
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

        /// <summary>
        /// Finds index of Question in ICollectionView
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="wantedQuestion"></param>
        /// <returns>Index of Question, -1 if ICollectionView does not contain Question</returns>
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

        /// <summary>
        /// Rename QSet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameButton_Click(object sender, RoutedEventArgs e)
        {
            bool loop = true;
            string name = EditedQSet.Name;
            while (loop)
            {
                InputDialog inputDialog = new InputDialog("Zadejte nové jméno sady", name);
                if (inputDialog.ShowDialog() == true)
                {
                    name = inputDialog.Answer;

                    if (FileManager.ContainsInvalidChars(name)) //check if the name doesn't contain invalid characters
                    {
                        MessageBox.Show($"Zadaný název obsahuje nedovolené znaky.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        continue;
                    }

                    if (qSetsManager.CheckName(name)) //check if the name isn't used already
                    {
                        MessageBox.Show("Sada otázek s názvem \"" + name + "\" už existuje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        continue;
                    }

                    string path = FileManager.GenerateFilePath(name);
                    if (qSetsManager.CheckPath(path)) //check if generated file path isn't used already
                    {
                        MessageBox.Show("Pro název \"" + name + "\" nelze vygenerovat jedinečný název souboru.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        continue;
                    }
                 
                    EditedQSet.Path = path;
                    EditedQSet.Name = name;                    
                    Saved = false;
                    nameChanged = true;
                    loop = false;
                }
                else
                {
                    loop = false;
                }
            }
        }
    }
}