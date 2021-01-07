using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Millionaire.Views
{
    public class NavigationManager : INotifyPropertyChanged
    {
        private UserControl _currentUC;

        public UserControl CurrentUC
        {
            get { return _currentUC; }
            set { _currentUC = value; NotifyPropertyChanged(nameof(CurrentUC)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ShowQSetsUC()
        {
            UserControl selectQSets = new SelectQSetsUC();
            CurrentUC = selectQSets;
        }

        public void ShowMainMenu()
        {
            UserControl mainMenu = new MainMenuUC(this);
            CurrentUC = mainMenu;
        }
    }
}
