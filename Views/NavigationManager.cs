using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Millionaire.Models;

namespace Millionaire.Views
{
    
    public class NavigationManager : INotifyPropertyChanged
    {
        private UserControl currentUC;
        public UserControl CurrentUC
        {
            get { return currentUC; }
            set { currentUC = value; NotifyPropertyChanged(nameof(CurrentUC)); }
        }

        public ScoresManager ScoresManager { get; set; }
        public QSetsManager QSetsManager{ get; set; }

        public NavigationManager()
        {
            ScoresManager = new ScoresManager();
            QSetsManager = new QSetsManager();
            try
            {
                ScoresManager.Scores = FileManager.LoadScores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nepodařilo se načíst uložená skóre: " + ex.Message, "Nelze načíst skóre", MessageBoxButton.OK, MessageBoxImage.Error);
                FileManager.safeToSaveScores = false;
            }
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void ShowQSetsUC(bool isSandbox)
        {      
            UserControl selectQSets = new SelectQSetsUC(this, QSetsManager, isSandbox);
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

        public void ShowEnterNickname(GameManager gameManager)
        {
            EnterNicknameWindow enterNicknameWindow = new EnterNicknameWindow(ScoresManager, gameManager, this);
            enterNicknameWindow.ShowDialog();
        }

        public void ShowHighScores()
        {
            UserControl highScores = new HighScoresUC(ScoresManager, this);
            CurrentUC = highScores;
        }

        public void ShowManageQSets()
        {
            UserControl manageQSets = new ManageQSetsUC(this, QSetsManager);
            CurrentUC = manageQSets;
        }

        public void ShowSandboxUC(List<QSet> selectedQSets)
        {
            UserControl sandbox = new SandboxUC(this, selectedQSets);
            CurrentUC = sandbox;
        }

        public void ShowQSetEditor(string path)
        {
            UserControl editor = new QSetEditorUC(path);
            CurrentUC = editor;
        }
    }
}
