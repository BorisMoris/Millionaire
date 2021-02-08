using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class GameManager : INotifyPropertyChanged
    {
        public QSet GameQSet { get; set; }
        private List<Question> currentQList;

        private Question currentQuestion;
        public Question CurrentQuestion
        {
            get { return currentQuestion; }
            set
            {
                currentQuestion = value;
                NotifyPropertyChanged(nameof(CurrentQuestion));
            }
        }

        private Random random = new Random();        
        private int round = 0;

        private string[] randomizedAnswers;
        public string[] RandomizedAnswers {
            get { return randomizedAnswers; }
            set
            {
                randomizedAnswers = value;
                NotifyPropertyChanged(nameof(RandomizedAnswers));
            }
        }

        public int RightAnswerIndex { get; set; }

        public PrizeMoney Prize { get; set; }

        public GameManager(List<QSet> selectedQSets)
        {
            GameQSet = new QSet();
            Prize = new PrizeMoney();
            RandomizedAnswers = new string[4];

            foreach (QSet qSet in selectedQSets) //load questions from selected qsets
            {
                GameQSet.EasyQuestions.AddRange(qSet.EasyQuestions);
                GameQSet.MediumQuestions.AddRange(qSet.MediumQuestions);
                GameQSet.HardQuestions.AddRange(qSet.HardQuestions);
            }

            currentQList = GameQSet.EasyQuestions;
            NewQuestion();
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// Set new question
        /// </summary>
        public void NewQuestion()
        {
            round++;
            if (round == 6) //changing questions difficulty
            {
                currentQList = GameQSet.MediumQuestions;
            }
            else if (round == 11)
            {
                currentQList = GameQSet.HardQuestions;
            }
            int position = random.Next(currentQList.Count);
            CurrentQuestion = currentQList[position];            
            currentQList.RemoveAt(position);
            
            RandomizedAnswers = RandomizeAnswers(CurrentQuestion, out int temp);
            RightAnswerIndex = temp;
            Console.WriteLine(RightAnswerIndex);
        }

        /// <summary>
        /// Randomize order of answers in question
        /// </summary>
        /// <param name="question">Contains answers to randomize</param>
        /// <param name="rightAnswerIndex">Index of right answer in randomizedAnswers[]</param>
        /// <returns>String[] of randomized answers</returns>
        public string[] RandomizeAnswers(Question question, out int rightAnswerIndex)
        {
            string[] randomizedAnswers = new string[4];

            rightAnswerIndex = random.Next(4); //Randomly place right answer in the array
            randomizedAnswers[rightAnswerIndex] = question.RightAnswer;

            List<string> wrongAnswers = new List<string> { question.WrongAnswer1, question.WrongAnswer2, question.WrongAnswer3 };
            for (int i = 0; i < 4; i++) //Randomly populate the rest of the array with wrong answers
            {
                if (i != rightAnswerIndex)
                {
                    int randomAnswerIndex = random.Next(wrongAnswers.Count());
                    randomizedAnswers[i] = wrongAnswers[randomAnswerIndex];
                    wrongAnswers.RemoveAt(randomAnswerIndex);
                }
            }

            return randomizedAnswers;
        }
    }
}