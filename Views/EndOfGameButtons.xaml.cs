using Millionaire.Models;
using System.Windows;
using System.Windows.Controls;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro EndOfGameButtons.xaml
    /// </summary>
    public partial class EndOfGameButtons : UserControl
    {
        //instances of manager classes are passed through DependencyProperties
        public NavigationManager navigationManager
        {
            get { return (NavigationManager)GetValue(navigationManagerProperty); }
            set { SetValue(navigationManagerProperty, value); }
        }

        public static readonly DependencyProperty navigationManagerProperty =
            DependencyProperty.Register("navigationManager", typeof(NavigationManager), typeof(EndOfGameButtons), new PropertyMetadata(null));

        public GameManager gameManager
        {
            get { return (GameManager)GetValue(gameManagerProperty); }
            set { SetValue(gameManagerProperty, value); }
        }

        public static readonly DependencyProperty gameManagerProperty =
            DependencyProperty.Register("gameManager", typeof(GameManager), typeof(EndOfGameButtons), new PropertyMetadata(new PropertyChangedCallback(OnPropertySet)));

        private static void OnPropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null) //decide wheter saveScoreButton should be enabled
                ((EndOfGameButtons)d).DisableButton();
        }

        public EndOfGameButtons()
        {
            InitializeComponent();
        }

        private void backToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            navigationManager.ShowMainMenu();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            navigationManager.ShowQSetsUC(false);
        }

        private void saveScoreButton_Click(object sender, RoutedEventArgs e)
        {
            navigationManager.ShowEnterNickname(gameManager);
        }

        private void DisableButton()
        {
            if (gameManager.Round <= 5)
            {
                saveScoreButton.IsEnabled = false;
            }
        }
    }
}