using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class Score
    {
        public string PlayersName { get; set; }
        public int RightAnswers { get; set; }
        public PrizeMoney Prize { get; set; }
        public string QuestionSets { get; set; }

        public Score() { }
        
        public Score(string name, int rightAnswers, PrizeMoney prize, string questionSets)
        {
            PlayersName = name;
            RightAnswers = rightAnswers;
            Prize = prize;
            QuestionSets = questionSets;
        }
    }
}
