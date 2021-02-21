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

        // Using a DependencyProperty as the backing store for navigationManager.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty navigationManagerProperty =
            DependencyProperty.Register("navigationManager", typeof(NavigationManager), typeof(EndOfGameButtons), new PropertyMetadata(null));

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
            navigationManager.ShowQSetsUC();
        }
    }
}
