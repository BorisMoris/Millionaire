using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class QSet /*: INotifyPropertyChanged*/
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int QuestionsCount { get { return EasyQuestions.Count() + MediumQuestions.Count() + HardQuestions.Count(); } }

        List<Question> easyQuestions = new List<Question>();
        List<Question> mediumQuestions = new List<Question>();
        List<Question> hardQuestions = new List<Question>();

        public List<Question> EasyQuestions { get { return easyQuestions; } }
        public List<Question> MediumQuestions { get { return mediumQuestions; } }
        public List<Question> HardQuestions { get { return hardQuestions; } }

        //#region PropertyChanged implementation
        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void NotifyPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    Console.WriteLine("property changed called");
        //}
        //#endregion

        public void AddQuestion(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    EasyQuestions.Add(new Question());
                    break;
                case Difficulty.Medium:
                    mediumQuestions.Add(new Question());
                    break;
                case Difficulty.Hard:
                    hardQuestions.Add(new Question());
                    break;
            }
        }
        
        public void AddQuestion(Difficulty difficulty, string question, string rightAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    EasyQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
                case Difficulty.Medium:
                    mediumQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
                case Difficulty.Hard:
                    hardQuestions.Add(new Question(question, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
            }
        }        
    }
}
