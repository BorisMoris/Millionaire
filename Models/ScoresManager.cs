using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Millionaire.Models
{
    public class ScoresManager
    {
        public List<Score> Scores { get; set; }

        public ScoresManager()
        {
            Scores = new List<Score>();
        }

        /// <summary>
        /// Adds new instance of Score to the list of scores and saves the list
        /// </summary>
        /// <param name="playersName"></param>
        /// <param name="rightAnswers"></param>
        /// <param name="prize"></param>
        /// <param name="questionSets"></param>
        /// <returns></returns>
        public string AddScore(string playersName, int rightAnswers, PrizeMoney prize, List<string> questionSets)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < questionSets.Count() - 1; i++)
            {
                stringBuilder.Append(questionSets[i]);
                stringBuilder.Append(", ");
            }
            stringBuilder.Append(questionSets[questionSets.Count() - 1]);

            Scores.Add(new Score(playersName, rightAnswers, prize, stringBuilder.ToString()));
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
