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
        public int[] ResponsesRatio { get; set; }

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
            rightAnswerIndex = rightAdvice ? rightAnswerIndex : ChangeIndex(rightAnswerIndex);

            do
            {
                int sum = 0;
                int rnd;
                for (int i = 0; i < 3; i++) //fill 3 positions in ResponsesRatio[] with random values lower than 70
                {
                    rnd = random.Next(70 - sum);
                    sum += rnd;
                    ResponsesRatio[i] = rnd;
                }
                ResponsesRatio[3] = 100 - sum; //complete to 100
            } while (ResponsesRatio.Max() > 70); //if the max value is bigger than 70 (unlikely, but possible), repeat the cycle

            for(int i = 0; i < ResponsesRatio.Length; i++) //randomly shift values to add more accident
            {
                int rnd = random.Next(ResponsesRatio.Length);
                int temp = ResponsesRatio[i];
                ResponsesRatio[i] = ResponsesRatio[rnd];
                ResponsesRatio[rnd] = temp;
            }

            int maxIndex = Array.IndexOf(ResponsesRatio, ResponsesRatio.Max()); //match the highest value with the right answer
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
