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
using System.Windows.Threading;
using Millionaire.Models;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro GameUC.xaml
    /// </summary>
    public partial class GameUC : UserControl
    {
        private GameManager gameManager;
        private NavigationManager navigationManager;

        private DispatcherTimer dispatcherTimer;
        private Button selectedButton;
        private Style rightAnswerStyle;
        private Style wrongAnswerStyle;

        private List<Button> answerButtons;

        public GameUC(NavigationManager navigationManager, List<QSet> selectedQSets)
        {
            InitializeComponent();

            gameManager = new GameManager(selectedQSets);
            DataContext = gameManager;
            this.navigationManager = navigationManager;

            rightAnswerStyle = FindResource("rightAnswer") as Style;
            wrongAnswerStyle = FindResource("wrongAnswer") as Style;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            answerButtons = new List<Button> { answerAButton, answerBButton, answerCButton, answerDButton };
        }        

        private void answerButton_Click(object sender, RoutedEventArgs e)
        {
            selectedButton = ((Button)sender);
            switch (selectedButton.Name)
            {
                case "answerAButton":
                    gameManager.CheckAnswer(0);
                    break;
                case "answerBButton":
                    gameManager.CheckAnswer(1);
                    break;
                case "answerCButton":
                    gameManager.CheckAnswer(2);
                    break;
                case "answerDButton":
                    gameManager.CheckAnswer(3);
                    break;
            }
            HighlightAnswer();
        }

        /// <summary>
        /// Highlight selected button with a style 
        /// </summary>
        /// <param name="style"></param>
        private void HighlightAnswer()
        {
            Style style;
            EnableAnswerButtons(false);
            if(gameManager.GameStatus==GameStatus.InProgress || gameManager.GameStatus == GameStatus.Victory)
            {
                style = rightAnswerStyle;
            }
            else
            {
                style = wrongAnswerStyle;
            }
            selectedButton.Style = style;

            dispatcherTimer.Start(); //start timer to stop highlighting answer
        }

        // This method is executed when the DispatcherTimer interval occurs
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {            
            EvaluateRound();

            dispatcherTimer.IsEnabled = false;
        }

        /// <summary>
        /// Decide what to do at the end of the round
        /// </summary>
        private void EvaluateRound()
        {
            if(gameManager.GameStatus==GameStatus.InProgress)
            {
                selectedButton.Style = default;
                EnableAnswerButtons(true);
                foreach (UserControl userControl in lifelinesStackPanel.Children)
                {
                    userControl.Visibility = Visibility.Collapsed;
                }
                lifelineButtonsStackPanel.Visibility = Visibility.Visible;

                gameManager.NewQuestion();
            }
            else if (gameManager.GameStatus==GameStatus.Victory)
            {
                navigationManager.ShowVictory(gameManager);
            }
            else
            {
                navigationManager.ShowEndOfGame(gameManager);
            }
        }

        /// <summary>
        /// Enable or disables all answer buttons
        /// </summary>
        /// <param name="isEnabled"></param>
        private void EnableAnswerButtons(bool isEnabled)
        {
            foreach(Button button in answerButtons)
            {
                button.IsEnabled = isEnabled;
            }
        }

        private void endGameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result= MessageBox.Show("Opravdu chceš ukončit hru? Tvůj postup bude ztracen.", "Ukončit hru", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result==MessageBoxResult.Yes)
            {
                navigationManager.ShowMainMenu();
            }
        }

        private void fiftyLifelineButton_Click(object sender, RoutedEventArgs e)
        {
            lifelineButtonsStackPanel.Visibility = Visibility.Collapsed;
            fiftyLifelineUC.Visibility = Visibility.Visible;
            fiftyLifelineButton.IsEnabled = false;

            List<int> wrongIndexes = gameManager._50_50Lifeline.ChooseWrongAnswers(gameManager.RightAnswerIndex);
            foreach (int index in wrongIndexes)
            {
                answerButtons[index].IsEnabled = false;
            }
        }

        private void audienceLifelineButton_Click(object sender, RoutedEventArgs e)
        {
            gameManager.AudienceLifeline.GenerateAdvice(gameManager.Round, gameManager.RightAnswerIndex);

            lifelineButtonsStackPanel.Visibility = Visibility.Collapsed;
            audienceLifelineUC.Visibility = Visibility.Visible;
           // audienceLifelineButton.IsEnabled = false;
        }

        private void friendLifelineButton_Click(object sender, RoutedEventArgs e)
        {
            gameManager.FriendLifeline.GenerateAdvice(gameManager.Round, gameManager.RightAnswerIndex, gameManager.RandomizedAnswers);

            lifelineButtonsStackPanel.Visibility = Visibility.Collapsed;
            friendLifelineUC.Visibility = Visibility.Visible;
          //  friendLifelineButton.IsEnabled = false;
        }
    }
}
