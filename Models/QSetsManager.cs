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
        private const int MINIMUM_OF_QUESTIONS = 5;

        /// <summary>
        /// Sorts the collection of QSets
        /// </summary>
        public void Sort()
        {
            List<QSet> sorted = QuestionSets.OrderBy(x => x.Name).ToList();
            QuestionSets = new ObservableCollection<QSet>(sorted);
        }

        /// <summary>
        /// Checks if the name is not used already
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the path is not used already
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public bool CheckPath(string newPath)
        {
            foreach (QSet qSet in QuestionSets)
            {
                if (qSet.Path == newPath)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks number of questions in QSet
        /// </summary>
        /// <param name="qSet"></param>
        /// <returns>Error message; null if QSet is ok</returns>
        public static string CheckQSet(QSet qSet)
        {
            if (qSet.EasyQuestions.Count() < MINIMUM_OF_QUESTIONS)
            {
                return $"Nedostatek jednoduchých otázek. Musí jich být alespoň {MINIMUM_OF_QUESTIONS}.";
            }
            if (qSet.MediumQuestions.Count() < MINIMUM_OF_QUESTIONS)
            {
                return $"Nedostatek středně těžkých otázek. Musí jich být alespoň {MINIMUM_OF_QUESTIONS}.";
            }
            if (qSet.HardQuestions.Count() < MINIMUM_OF_QUESTIONS)
            {
                return $"Nedostatek těžkých otázek. Musí jich být alespoň {MINIMUM_OF_QUESTIONS}.";
            }

            return null;
        }

        /// <summary>
        /// Checks individual questions of nulls and invalid characters
        /// </summary>
        /// <param name="questions"></param>
        /// <returns>Tuple with invalid question and an error message; (null, null) if all questions are ok</returns>
        public (Question, string) CheckQuestions(HashSet<Question> questions)
        {
            string[] parts = new string[5];
            string error = string.Empty;
            foreach (Question question in questions)
            {
                parts[0] = question.QuestionSentence;
                parts[1] = question.RightAnswer;
                parts[2] = question.WrongAnswer1;
                parts[3] = question.WrongAnswer2;
                parts[4] = question.WrongAnswer3;

                if (parts[0].StartsWith(FileManager.TAG_FIRST_CHAR))
                {
                    error = $"Otázka \"{parts[0]}\" začíná nedovoleným charakterem.";
                }

                foreach (string str in parts)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        return (question, $"Otázka \"{parts[0]}\" obsahuje nevyplněné pole.");
                    }
                    if (str.Contains(";"))
                    {
                        return (question, $"Otázka \"{parts[0]}\" obsahuje nepovolený znak: ;");
                    }
                }
                return (question, error);
            }
            return (null, null);
        }
    }
}