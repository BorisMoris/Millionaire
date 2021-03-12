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
    /// Interakční logika pro SandboxUC.xaml
    /// </summary>
    public partial class SandboxUC : UserControl
    {

        private SandboxGameManager gameManager;
        private NavigationManager navigationManager;

        private DispatcherTimer dispatcherTimer;
        private Button selectedButton;
        private Style rightAnswerStyle;
        private Style wrongAnswerStyle;

        private List<Button> answerButtons;

        public SandboxUC(NavigationManager navigationManager, List<QSet> selectedQSets)
        {
            InitializeComponent();

            gameManager = new SandboxGameManager(selectedQSets);
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
            if (gameManager.AnsweredRight)
            {
                style = rightAnswerStyle;
            }
            else
            {
                style = wrongAnswerStyle;
                answerButtons[gameManager.RightAnswerIndex].Style = rightAnswerStyle;
            }
            selectedButton.Style = style;

            dispatcherTimer.Start(); //start timer to stop highlighting answer
        }

        // This method is executed when the DispatcherTimer interval occurs
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            EnableAnswerButtons(true);            

            selectedButton.Style = default;
            if (!gameManager.AnsweredRight)
            {
                answerButtons[gameManager.RightAnswerIndex].Style = default;
            }

            gameManager.NewQuestion();

            dispatcherTimer.IsEnabled = false;
        }

        /// <summary>
        /// Enable or disables all answer buttons
        /// </summary>
        /// <param name="isEnabled"></param>
        private void EnableAnswerButtons(bool isEnabled)
        {
            foreach (Button button in answerButtons)
            {
                button.IsEnabled = isEnabled;
            }
        }

        private void endGameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Chceš opustit procvičování náhodných otázek?", "Opustit", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                navigationManager.ShowMainMenu();
            }
        }
    }
}
