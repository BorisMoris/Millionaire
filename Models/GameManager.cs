using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class GameManager : BaseGameManager
    {
        public QSet GameQSet { get; set; }
        public List<string> QSetsNames { get; set; }
        public PrizeMoney Prize { get; set; }

        public GameStatus GameStatus { get; set; }
        public int Round { get; set; }        

        public GameManager(List<QSet> selectedQSets):base(selectedQSets)
        {            
            Prize = new PrizeMoney();
            Round = 0;

            LoadQuestions(selectedQSets);            
            NewQuestion();
        }

        public override void LoadQuestions(List<QSet> selectedQSets)
        {
            GameQSet = new QSet();
            QSetsNames = new List<string>();
            foreach (QSet qSet in selectedQSets) //load questions from selected qsets
            {
                GameQSet.EasyQuestions.AddRange(qSet.EasyQuestions);
                GameQSet.MediumQuestions.AddRange(qSet.MediumQuestions);
                GameQSet.HardQuestions.AddRange(qSet.HardQuestions);

                QSetsNames.Add(qSet.Name);
            }

            currentQList = GameQSet.EasyQuestions;
        }
                
        /// <summary>
        /// Set new question
        /// </summary>
        public override void NewQuestion()
        {
            Round++;
            if (Round == 6) //changing questions difficulty
            {
                currentQList = GameQSet.MediumQuestions;
            }
            else if (Round == 11)
            {
                currentQList = GameQSet.HardQuestions;
            }
            int position = random.Next(currentQList.Count);
            CurrentQuestion = currentQList[position];            
            currentQList.RemoveAt(position);
            
            RandomizedAnswers = RandomizeAnswers(CurrentQuestion, out int temp);
            RightAnswerIndex = temp;
        }

        public override void CheckAnswer(int index)
        {
            if (index == RightAnswerIndex && Round > 14)
            {
                GameStatus = GameStatus.Victory;
                Prize++;
            }
            else if (index == RightAnswerIndex)
            {
                GameStatus = GameStatus.InProgress;
                Prize++;
            }
            else
            {
                GameStatus = GameStatus.Loss;
            }
        }
    }
}