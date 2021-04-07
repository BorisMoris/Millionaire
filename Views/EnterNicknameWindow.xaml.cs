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
using System.Windows.Shapes;
using Millionaire.Models;

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
                string error = scoresManager.AddPlayer(nickNameTextBox.Text, gameManager.Round - 1, gameManager.Prize, gameManager.QSetsNames);
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
