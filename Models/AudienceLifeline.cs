using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class AudienceLifeline : LifelineBase
    {   
        private int[] responsesRatio;
        public int[] ResponsesRatio {
            get
            {
                return responsesRatio;
            }
            set
            {
                responsesRatio = value;
            }
        }

        public AudienceLifeline(Random random) :base(random)
        {
            ResponsesRatio = new int[4];
        }

        /// <summary>
        /// Create advice and set properties accordingly
        /// </summary>
        /// <param name="round">Current round of the game</param>
        /// <param name="rightAnswerIndex">Index of right answer</param>
        public void GenerateAdvice(int round, int rightAnswerIndex)
        {
            bool rightAdvice = DecideIfAdviseCorrectly(round);
            if (!rightAdvice) //change rightAnswerIndex to give wrong advice
            {
                rightAnswerIndex = ChangeIndex(rightAnswerIndex);
            }

            int sum = 0;
            int rnd;
            for (int i = 0; i < 3; i++)
            {
                rnd = random.Next(80 - sum);
                sum += rnd;
                ResponsesRatio[i] = rnd;
            }
            ResponsesRatio[3] = 100 - sum;
            ResponsesRatio = ResponsesRatio.OrderBy(x => random.Next()).ToArray();

            int maxIndex = Array.IndexOf(ResponsesRatio, ResponsesRatio.Max());
            if (rightAnswerIndex != maxIndex)
            {
                int temp = ResponsesRatio[rightAnswerIndex];
                ResponsesRatio[rightAnswerIndex] = ResponsesRatio[maxIndex];
                ResponsesRatio[maxIndex] = temp;
            }

            NotifyPropertyChanged(nameof(ResponsesRatio));
        }

    }
}
