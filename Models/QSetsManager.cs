using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class QSetsManager
    {
        public ObservableCollection<QSet> QuestionSets { get; set; }

        public void Sort()
        {
            List<QSet> sorted = QuestionSets.OrderBy(x => x.Name).ToList();
            QuestionSets = new ObservableCollection<QSet>(sorted);
        }

        public bool CheckName(string newName)
        {
            foreach(QSet qSet in QuestionSets)
            {
                if (qSet.Name == newName)
                {
                    return true;
                }
            }

            return false;
        }

        public (Question, string) CheckQuestions(HashSet<(Question, Difficulty)> questions)
        {
            string[] parts = new string[5];
            bool loop = true;
            Difficulty difficulty = Difficulty.Easy;
            //while (loop)
            //{
                foreach ((Question, Difficulty) question in questions)
                {
                    parts[0] = question.Item1.QuestionSentence;
                    parts[1] = question.Item1.RightAnswer;
                    parts[2] = question.Item1.WrongAnswer1;
                    parts[3] = question.Item1.WrongAnswer2;
                    parts[4] = question.Item1.WrongAnswer3;

                    if (parts[0].StartsWith("*"))
                    {
                        return (question.Item1, $"Otázka \"{parts[0]}\" začíná nedovoleným charakterem.");
                    }

                    foreach (string str in parts)
                    {
                        if (string.IsNullOrEmpty(str))
                        {
                            return (question.Item1, $"Otázka \"{parts[0]}\" obsahuje nevyplněné pole.");
                        }
                    }
                }

                //difficulty++;
                //if (difficulty == Difficulty.Medium)
                //{

                //}
            //}
            return (null, null);
        }
    }
}