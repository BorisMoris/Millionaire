using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class GameManager : INotifyPropertyChanged
    {
        public QSet GameQSet { get; set; }

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
        private List<Question> currentQList;
        private int round = 1;

        public string[] RandomizedAnswers { get; set;}
        public int RightAnswerIndex { get; set; }

        public GameManager(List<QSet> selectedQSets)
        {
            GameQSet = new QSet();
            RandomizedAnswers = new string[4];

            foreach (QSet qSet in selectedQSets)
            {
                GameQSet.EasyQuestions.AddRange(qSet.EasyQuestions);
                GameQSet.MediumQuestions.AddRange(qSet.MediumQuestions);
                GameQSet.HardQuestions.AddRange(qSet.HardQuestions);
            }

            currentQList = GameQSet.EasyQuestions;

            foreach(Question question in GameQSet.EasyQuestions)
            {
                Console.WriteLine(question.RightAnswer);
            }
            NewQuestion();
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void NewQuestion()
        {
            round++;
            if (round == 5)
            {
                currentQList = GameQSet.MediumQuestions;
            }
            else if (round == 10)
            {
                currentQList = GameQSet.HardQuestions;
            }
            int position = random.Next(currentQList.Count);
            CurrentQuestion = currentQList[position];            
            currentQList.RemoveAt(position);

            RandomizeAnswers();  

        }

        public void RandomizeAnswers()
        {
            RightAnswerIndex = random.Next(4);
            RandomizedAnswers[RightAnswerIndex] = currentQuestion.RightAnswer;
            List<string> wrongAnswers = new List<string> { currentQuestion.WrongAnswer1, currentQuestion.WrongAnswer2, currentQuestion.WrongAnswer3 };            
            for(int i=0; i<4; i++)
            {
                if (i != RightAnswerIndex)
                {
                    int randomAnswerIndex = random.Next(wrongAnswers.Count());
                    RandomizedAnswers[i] = wrongAnswers[randomAnswerIndex];
                    wrongAnswers.RemoveAt(randomAnswerIndex);
                }
            }

            NotifyPropertyChanged(nameof(RandomizedAnswers));
        }
    }
}