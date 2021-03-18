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
                ICollectionView collectionView = (ICollectionView)questionsListBox.ItemsSource;
                collectionView.Refresh();
            }
        }

        private List<TextBox> questionTextBoxes;

        public QSetEditorUC(string path)
        {
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

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
           
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
    }
}
