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

        private DispatcherTimer dispatcherTimer;
        private Button selectedButton;
        private Style rightAnswerStyle;
        private Style wrongAnswerStyle;

        private List<Button> answerButtons;

        public GameUC(List<QSet> selectedQSets)
        {
            InitializeComponent();

            gameManager = new GameManager(selectedQSets);
            DataContext = gameManager;

            rightAnswerStyle = FindResource("rightAnswer") as Style;
            wrongAnswerStyle = FindResource("wrongAnswer") as Style;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            answerButtons = new List<Button>();
            answerButtons.Add(answerAButton);
            answerButtons.Add(answerBButton);
            answerButtons.Add(answerCButton);
            answerButtons.Add(answerDButton);
        }

        private void answerAButton_Click(object sender, RoutedEventArgs e)
        {
            selectedButton = ((Button)sender);
            if (gameManager.RightAnswerIndex == 0)
            {
                HighlightAnswer(rightAnswerStyle);
                gameManager.Prize.Value++;
                Console.WriteLine(gameManager.Prize);
            }
            else
            {
                HighlightAnswer(wrongAnswerStyle);
            }
        }

        private void answerBButton_Click(object sender, RoutedEventArgs e)
        {
            selectedButton = ((Button)sender);
            if (gameManager.RightAnswerIndex == 1)
            {
                HighlightAnswer(rightAnswerStyle);
                gameManager.Prize.Value++;
                Console.WriteLine(gameManager.Prize);
            }
            else
            {
                HighlightAnswer(wrongAnswerStyle);
            }
        }

        private void answerCButton_Click(object sender, RoutedEventArgs e)
        {
            selectedButton = ((Button)sender);
            if (gameManager.RightAnswerIndex == 2)
            {
                HighlightAnswer(rightAnswerStyle);
                gameManager.Prize.Value++;
                Console.WriteLine(gameManager.Prize);
            }
            else
            {
                HighlightAnswer(wrongAnswerStyle);
            }
        }

        private void answerDButton_Click(object sender, RoutedEventArgs e)
        {
            selectedButton = ((Button)sender);
            if (gameManager.RightAnswerIndex == 3)
            {
                HighlightAnswer(rightAnswerStyle);
                gameManager.Prize.Value++;
                Console.WriteLine(gameManager.Prize);
            }
            else
            {
                HighlightAnswer(wrongAnswerStyle);
            }
        }

        // This method is executed when the DispatcherTimer interval occurs
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {            
            selectedButton.Style = default;
            EnableAnswerButtons(true);
            gameManager.NewQuestion();

            dispatcherTimer.IsEnabled = false;
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

        /// <summary>
        /// Highlight selected button with a style 
        /// </summary>
        /// <param name="style"></param>
        private void HighlightAnswer(Style style)
        {
            selectedButton.Style = style;
            EnableAnswerButtons(false);
            dispatcherTimer.Start();
        }
    }
}
