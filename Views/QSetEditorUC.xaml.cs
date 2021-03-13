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
    /// Interakční logika pro QSetEditorUC.xaml
    /// </summary>
    public partial class QSetEditorUC : UserControl
    {
        QSet editedQSet;
        
        public QSetEditorUC(string path)
        {
            InitializeComponent();

            try
            {
                editedQSet = FileManager.LoadQSetFromFile(path);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Soubor se sadou se nepodařilo načíst.\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            easyQDataGrid.ItemsSource=editedQSet.EasyQuestions;
            //mediumQDataGrid.ItemsSource = editedQSet.MediumQuestions;
            //hardQDataGrid.ItemsSource = editedQSet.HardQuestions;
        }
    }
}
