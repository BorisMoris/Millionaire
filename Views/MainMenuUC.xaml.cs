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
using Millionaire;

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
            navManager.ShowQSetsUC();            
        }

        private void highScoresButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowHighScores();
        }

        private void manageQSetsButton_Click(object sender, RoutedEventArgs e)
        {
            navManager.ShowManageQSets();
        }
    }
}
