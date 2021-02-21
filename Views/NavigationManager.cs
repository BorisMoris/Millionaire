using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Millionaire.Models;

namespace Millionaire.Views
{
    //GC.collect
    
    public class NavigationManager : INotifyPropertyChanged
    {
        private UserControl currentUC;

        public UserControl CurrentUC
        {
            get { return currentUC; }
            set { currentUC = value; NotifyPropertyChanged(nameof(CurrentUC)); }
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void ShowQSetsUC()
        {      
            UserControl selectQSets = new SelectQSetsUC(this);
            CurrentUC = selectQSets;
        }

        public void ShowMainMenu()
        {
            UserControl mainMenu = new MainMenuUC(this);
            CurrentUC = mainMenu;
        }

        public void ShowGame(List<QSet> selectedQSets)
        {
            UserControl game = new GameUC(this, selectedQSets);
            CurrentUC = game;
        }

        public void ShowEndOfGame(GameManager gameManager)
        {
            UserControl endOfGame = new EndOfGame(this, gameManager);
            CurrentUC = endOfGame;
        }

        public void ShowVictory(GameManager gameManager)
        {
            UserControl victory = new VictoryUC(this, gameManager);
            CurrentUC = victory;
        }
    }
}
