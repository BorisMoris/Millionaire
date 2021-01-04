using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class QSet
    {
        public string Name { get; set; }

        List<Question> easyQuestions = new List<Question>();
        List<Question> mediumQuestions = new List<Question>();
        List<Question> hardQuestions = new List<Question>();

        public List<Question> EasyQuestions { get { return easyQuestions; } }
        public List<Question> MediumQuestions { get { return mediumQuestions; } }
        public List<Question> HardQuestions { get { return hardQuestions; } }
        

        public void AddQuestion(Difficulty difficulty, string question, string rightAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3)
        {
            switch (difficulty)
            {
                case Difficulty.easy:
                    easyQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
                case Difficulty.medium:
                    mediumQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
                case Difficulty.hard:
                    hardQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
            }
        }

        
    }

    public enum Difficulty
    {
        easy,
        medium,
        hard
    }
}
