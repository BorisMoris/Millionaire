using System.Collections.Generic;

namespace Millionaire.Models
{
    public class SandboxGameManager : BaseGameManager
    {
        public List<QSet> SelectedQSets { get; set; }

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
            SelectedQSets = selectedQSets;
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

        /// <summary>
        /// Set new question
        /// </summary>
        public override void NewQuestion()
        {
            int position = random.Next(currentQList.Count);
            CurrentQuestion = currentQList[position];
            currentQList.RemoveAt(position);

            if (currentQList.Count == 0)
            {
                LoadQuestions(SelectedQSets);
            }

            RandomizedAnswers = RandomizeAnswers(CurrentQuestion, out int temp);
            RightAnswerIndex = temp;
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