using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class ScoresManager
    {
        public List<Score> Scores { get; set; }

        public ScoresManager()
        {
            Scores = new List<Score>();
        }

        public string AddPlayer(string playerName, int rightAnswers, PrizeMoney prize, List<string> questionSets)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for(int i= 0; i < questionSets.Count()-1; i++)
            {
                stringBuilder.Append(questionSets[i]);
                stringBuilder.Append(", ");
            }
            stringBuilder.Append(questionSets[questionSets.Count() - 1]);

            Scores.Add(new Score(playerName, rightAnswers, prize, stringBuilder.ToString()));
            Scores = Scores.OrderByDescending(x => x.RightAnswers).ToList();

            try
            {
                FileManager.SaveScores(Scores);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }
    }
}
