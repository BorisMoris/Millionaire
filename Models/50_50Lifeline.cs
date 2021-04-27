using System;
using System.Collections.Generic;
using System.Linq;

namespace Millionaire.Models
{
    public class _50_50Lifeline : LifelineBase
    {
        public _50_50Lifeline(Random random) : base(random)
        {
        }

        /// <summary>
        /// Choose two wrong answers
        /// </summary>
        /// <param name="rightAnswerIndex">Index of right answer</param>
        /// <returns>List of indexes of two wrong answers</returns>
        public List<int> ChooseWrongAnswers(int rightAnswerIndex)
        {
            List<int> indexes = new List<int> { 0, 1, 2, 3 };
            indexes.Remove(rightAnswerIndex);
            indexes.RemoveAt(random.Next(indexes.Count()));

            return indexes;
        }
    }
}
