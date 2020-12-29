using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    class QSet
    {
        public string Name { get; set; }

        List<Question> easyQuestions = new List<Question>();
        List<Question> mediumQuestions = new List<Question>();
        List<Question> hardQuestions = new List<Question>();


        public List<Question> EasyQuestions { get { return easyQuestions; } }
        public List<Question> MediumQuestions { get { return mediumQuestions; } }
        public List<Question> HardQuestions { get { return hardQuestions; } }

        //public QSet(string name)
        //{
        //    Name = name;
        //}

        public void AddEasyQ(string question, string rightAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3)
        {
            easyQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
        }

        public void AddMediumQ(string question, string rightAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3)
        {
            mediumQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
        }

        public void AddHardQ(string question, string rightAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3)
        {
            hardQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
        }
    }
}
