using Millionaire.Models;
using System.Windows.Controls;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro EndOfGame.xaml
    /// </summary>
    public partial class EndOfGame : UserControl
    {
        public NavigationManager NavigationManager { get; set; }
        public GameManager GameManager { get; set; }

        public EndOfGame(NavigationManager navigationManager, GameManager gameManager)
        {
            NavigationManager = navigationManager;
            GameManager = gameManager;
            DataContext = GameManager;
            InitializeComponent();
        }
    }
}
