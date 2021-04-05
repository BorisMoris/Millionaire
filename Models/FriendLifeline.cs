using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class FriendLifeline:LifelineBase
    {
        private string[] statements = new string[] {
            "Správná odpověď je určitě",
            "Řekni, že je to",
            "Myslím, že odpověď je",
            "Podle mě to bude",
            "Odpověď by mohla být",
            "Jsem si skoro jistý, že je to"
        };

        private string statement;
        public string Statement
        {
            get { return statement; }
            set {
                statement = value;
                NotifyPropertyChanged(nameof(Statement));
            }
        }

        private string answer;
        public string Answer
        {
            get { return answer; }
            set
            { 
                answer = value;
                NotifyPropertyChanged(nameof(Answer));
            }
        }

        public FriendLifeline(Random random):base (random)
        {
        }

        /// <summary>
        /// Create advice and set properties accordingly
        /// </summary>
        /// <param name="round">Current round of the game</param>
        /// <param name="rightAnswerIndex">Index of right answer in answers[]</param>
        /// <param name="answers">Answers of current question</param>
        public void GenerateAdvice(int round, int rightAnswerIndex, string[] answers)
        {
            bool rightAdvice = DecideIfAdviseCorrectly(round);
            int answerIndex;

            answerIndex = rightAdvice ? rightAnswerIndex : ChangeIndex(rightAnswerIndex);

            Statement = statements[random.Next(statements.Length)];
            Answer = answers[answerIndex];
        }
    }
}
