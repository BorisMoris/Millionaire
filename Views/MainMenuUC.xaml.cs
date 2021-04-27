using System.Windows;
using System.Windows.Controls;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro MainMenuUC.xaml
    /// </summary>
    public partial class MainMenuUC : UserControl
    {
        NavigationManager navManager;
        public MainMenuUC(NavigationManager navManager)
        {
            InitializeComponent();
            this.navManager = navManager;
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowQSetsUC(false);
        }

        private void highScoresButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowHighScores();
        }

        private void manageQSetsButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowManageQSets();
        }

        private void randomQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowQSetsUC(true);
        }
    }
}
