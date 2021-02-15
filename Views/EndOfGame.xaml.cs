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
    /// Interakční logika pro EndOfGame.xaml
    /// </summary>
    public partial class EndOfGame : UserControl
    {
        public NavigationManager NavigationManager { get; set; }
        private GameManager gameManager;

        public EndOfGame(NavigationManager navigationManager, GameManager gameManager)
        {
            NavigationManager = navigationManager;
            this.gameManager = gameManager;
            DataContext = gameManager;
            InitializeComponent();
        }
    }
}
