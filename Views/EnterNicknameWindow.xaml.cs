using Millionaire.Models;
using System.Windows;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro EnterNicknameWindow.xaml
    /// </summary>
    public partial class EnterNicknameWindow : Window
    {
        private ScoresManager scoresManager;
        private GameManager gameManager;
        private NavigationManager navigationManager;

        public EnterNicknameWindow(ScoresManager scoresManager, GameManager gameManager, NavigationManager navigationManager)
        {
            InitializeComponent();
            this.scoresManager = scoresManager;
            this.gameManager = gameManager;
            this.navigationManager = navigationManager;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveScoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nickNameTextBox.Text))
            {
                string error = scoresManager.AddScore(nickNameTextBox.Text, gameManager.Round - 1, gameManager.Prize, gameManager.QSetsNames);
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show("Skóre se nepodařilo uložit: " + error, "Chyba při ukládání", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Close();
                navigationManager.ShowHighScores();
            }
            else
            {
                MessageBox.Show("Zadejte přezdívku", "Prázdná odpověď", MessageBoxButton.OK);
            }
        }
    }
}
