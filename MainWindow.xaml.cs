using Millionaire.Views;
using System.Windows;

namespace Millionaire
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NavigationManager navManager;


        public MainWindow()
        {
            InitializeComponent();

            navManager = new NavigationManager();
            DataContext = navManager;
            navManager.ShowMainMenu();
        }
    }
}
