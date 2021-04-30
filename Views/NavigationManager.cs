using Millionaire.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Millionaire.Views
{

    public class NavigationManager : INotifyPropertyChanged
    {
        //Idea of binding UserControls comes from a comment below this video: https://www.youtube.com/watch?v=1_cUgpWqS0Y

        private UserControl currentUC;
        public UserControl CurrentUC
        {
            get { return currentUC; }
            set { currentUC = value; NotifyPropertyChanged(nameof(CurrentUC)); }
        }

        public ScoresManager ScoresManager { get; set; }
        public QSetsManager QSetsManager { get; set; }

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
                MessageBox.Show("Nepodařilo se načíst uložená skóre: " + ex.Message + "\nNebude možné ukládat skóre. Pokud chcete umožnit ukládání, restartujte aplikaci.", "Nelze načíst skóre", MessageBoxButton.OK, MessageBoxImage.Error);
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

        /// <summary>
        /// Displays UserControl for selecting QSets
        /// </summary>
        /// <param name="isSandbox">True if the user wants to practice random questions</param>
        public void ShowQSetsUC(bool isSandbox)
        {
            UserControl selectQSets = new SelectQSetsUC(this, QSetsManager, isSandbox);
            CurrentUC = selectQSets;
        }

        /// <summary>
        /// Displays main menu UserControl
        /// </summary>
        public void ShowMainMenu()
        {
            UserControl mainMenu = new MainMenuUC(this);
            CurrentUC = mainMenu;
        }

        /// <summary>
        /// Displays game UserControl
        /// </summary>
        /// <param name="selectedQSets"></param>
        public void ShowGame(List<QSet> selectedQSets)
        {
            UserControl game = new GameUC(this, selectedQSets);
            CurrentUC = game;
        }

        /// <summary>
        /// Displays UserControl with end-of-game message
        /// </summary>
        /// <param name="gameManager"></param>
        public void ShowEndOfGame(GameManager gameManager)
        {
            UserControl endOfGame = new EndOfGame(this, gameManager);
            CurrentUC = endOfGame;
        }

        /// <summary>
        /// Displays UserControl with victory message
        /// </summary>
        /// <param name="gameManager"></param>
        public void ShowVictory(GameManager gameManager)
        {
            UserControl victory = new VictoryUC(this, gameManager);
            CurrentUC = victory;
        }

        public void ShowGaveUp(GameManager gameManager)
        {
            UserControl gaveUp = new GaveUpUC(this, gameManager);
            CurrentUC = gaveUp;
        }

        /// <summary>
        /// Shows a window allowing to enter nickname and save score
        /// </summary>
        /// <param name="gameManager"></param>
        public void ShowEnterNickname(GameManager gameManager)
        {
            EnterNicknameWindow enterNicknameWindow = new EnterNicknameWindow(ScoresManager, gameManager, this);
            enterNicknameWindow.ShowDialog();
        }

        /// <summary>
        /// Displays UserControl with saved scores
        /// </summary>
        public void ShowHighScores()
        {
            UserControl highScores = new HighScoresUC(ScoresManager, this);
            CurrentUC = highScores;
        }

        /// <summary>
        /// Displays UserControl for managing QSets
        /// </summary>
        public void ShowManageQSets()
        {
            UserControl manageQSets = new ManageQSetsUC(this, QSetsManager);
            CurrentUC = manageQSets;
        }

        /// <summary>
        /// Displays UserControl for sandbox game
        /// </summary>
        /// <param name="selectedQSets"></param>
        public void ShowSandboxUC(List<QSet> selectedQSets)
        {
            UserControl sandbox = new SandboxUC(this, selectedQSets);
            CurrentUC = sandbox;
        }

        /// <summary>
        /// Displays UserControl with QSet editor (editing existing QSet)
        /// </summary>
        /// <param name="qSet">QSet to edit</param>
        /// <param name="index">Index of QSet in original collection</param>
        public void ShowQSetEditor(QSet qSet, int index)
        {
            UserControl editor = new QSetEditorUC(this, QSetsManager, qSet, index);
            CurrentUC = editor;
        }

        /// <summary>
        /// Displays UserControl with QSet editor (brand new QSet)
        /// </summary>
        /// <param name="name">Name of the new QSet</param>
        /// <param name="path">Path to the file with the new QSet</param>
        public void ShowQSetEditor(string name, string path)
        {
            UserControl editor = new QSetEditorUC(this, QSetsManager, name, path);
            CurrentUC = editor;
        }
    }
}
