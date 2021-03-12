using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class SandboxGameManager : BaseGameManager
    {        
        private int rightAnswersCount;
        public int RightAnswersCount
        {
            get { return rightAnswersCount; }
            set
            {
                rightAnswersCount = value;
                NotifyPropertyChanged(nameof(RightAnswersCount));
            }
        }

        private int wrongAnswersCount;
        public int WrongAnswersCount
        {
            get { return wrongAnswersCount; }
            set
            {
                wrongAnswersCount = value;
                NotifyPropertyChanged(nameof(WrongAnswersCount));
            }
        }

        public bool AnsweredRight { get; set; }

        public SandboxGameManager(List<QSet> selectedQSets) : base(selectedQSets)
        {
            RightAnswersCount = 0;
            WrongAnswersCount = 0;
        }

        public override void LoadQuestions(List<QSet> selectedQSets)
        {
            currentQList = new List<Question>();
            foreach (QSet qSet in selectedQSets) //load questions from selected qsets
            {
                currentQList.AddRange(qSet.EasyQuestions);
                currentQList.AddRange(qSet.MediumQuestions);
                currentQList.AddRange(qSet.HardQuestions);
            }
        }

        public override void CheckAnswer(int index)
        {
            if (index == RightAnswerIndex)
            {
                RightAnswersCount++;
                AnsweredRight = true;
            }
            else
            {
                WrongAnswersCount++;
                AnsweredRight = false;
            }
        }
    }
}