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
