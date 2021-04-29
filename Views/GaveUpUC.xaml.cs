using Millionaire.Models;
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
    /// Interakční logika pro GaveUpUC.xaml
    /// </summary>
    public partial class GaveUpUC : UserControl
    {
        public NavigationManager NavigationManager { get; set; }
        public GameManager GameManager { get; set; }

        public GaveUpUC(NavigationManager navigationManager, GameManager gameManager)
        {
            NavigationManager = navigationManager;
            GameManager = gameManager;
            DataContext = GameManager;
            InitializeComponent();
        }
    }
}
