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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Millionaire.Models;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro EndOfGameButtons.xaml
    /// </summary>
    public partial class EndOfGameButtons : UserControl
    {
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
            if (e.NewValue !=null)
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