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
    /// Interakční logika pro GameUC.xaml
    /// </summary>
    public partial class GameUC : UserControl
    {
        private GameManager gameManager;
        
        public GameUC(List<QSet> selectedQSets)
        {
            InitializeComponent();
            gameManager = new GameManager(selectedQSets);
            DataContext = gameManager;
            answerAButton.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        private void answerAButton_Click(object sender, RoutedEventArgs e)
        {
            gameManager.NewQuestion();
        }
    }
}
