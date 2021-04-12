using Millionaire.Models;
using System.Windows;
using System.Windows.Controls;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro HighScoresUC.xaml
    /// </summary>
    public partial class HighScoresUC : UserControl
    {
        private NavigationManager navigationManager;

        public HighScoresUC(ScoresManager scoresManager, NavigationManager navigationManager)
        {
            InitializeComponent();
            this.navigationManager = navigationManager;
            DataContext = scoresManager;
        }

        private void mainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            navigationManager.ShowMainMenu();
        }
    }
}
