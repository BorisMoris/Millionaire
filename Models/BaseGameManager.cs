using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public abstract class BaseGameManager : INotifyPropertyChanged
    {
        protected List<Question> currentQList;

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

        protected Random random = new Random();

        private string[] randomizedAnswers;
        public string[] RandomizedAnswers
        {
            get { return randomizedAnswers; }
            set
            {
                randomizedAnswers = value;
                NotifyPropertyChanged(nameof(RandomizedAnswers));
            }
        }
        public int RightAnswerIndex { get; set; }

        public BaseGameManager(List<QSet> selectedQSets)
        {
            RandomizedAnswers = new string[4];

            LoadQuestions(selectedQSets);
            NewQuestion();
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public abstract void LoadQuestions(List<QSet> selectedQSets);

        public abstract void NewQuestion();

        public abstract void CheckAnswer(int index);

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
