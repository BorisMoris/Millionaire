using Millionaire.Models;
using System.Windows.Controls;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro VictoryUC.xaml
    /// </summary>
    public partial class VictoryUC : UserControl
    {
        public NavigationManager NavigationManager { get; set; }
        public GameManager GameManager { get; set; }

        public VictoryUC(NavigationManager navigationManager, GameManager gameManager)
        {
            NavigationManager = navigationManager;
            GameManager = gameManager;
            InitializeComponent();
        }
    }
}