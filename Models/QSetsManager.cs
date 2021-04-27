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

        public string CheckQSet(QSet qSet)
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

        public (Question, string) CheckQuestions(HashSet<Question> questions)
        {
            string[] parts = new string[5];
            foreach (Question question in questions)
            {
                parts[0] = question.QuestionSentence;
                parts[1] = question.RightAnswer;
                parts[2] = question.WrongAnswer1;
                parts[3] = question.WrongAnswer2;
                parts[4] = question.WrongAnswer3;

                if (parts[0].StartsWith("*"))
                {
                    return (question, $"Otázka \"{parts[0]}\" začíná nedovoleným charakterem.");
                }

                foreach (string str in parts)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        return (question, $"Otázka \"{parts[0]}\" obsahuje nevyplněné pole.");
                    }
                }
            }
            return (null, null);
        }
    }
}