using System.Windows;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro UniversalInputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public string Answer { get { return answerTextBox.Text; } }

        public InputDialog(string question, string defaultAnswer)
        {
            InitializeComponent();

            questionTextBlock.Text = question;
            answerTextBox.Text = defaultAnswer;
            answerTextBox.Focus();
            answerTextBox.SelectAll();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(answerTextBox.Text))
            {
                MessageBox.Show("Zadejte prosím text.", "Prázdná odpověď");
            }
            else
            {
                this.DialogResult = true;
            }
        }
    }
}
