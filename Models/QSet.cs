using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Millionaire.Models
{
    public class QSet : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }
        public string Path { get; set; }
        public int QuestionsCount { get { return EasyQuestions.Count() + MediumQuestions.Count() + HardQuestions.Count(); } }

        List<Question> easyQuestions = new List<Question>();
        List<Question> mediumQuestions = new List<Question>();
        List<Question> hardQuestions = new List<Question>();

        public List<Question> EasyQuestions { get { return easyQuestions; } }
        public List<Question> MediumQuestions { get { return mediumQuestions; } }
        public List<Question> HardQuestions { get { return hardQuestions; } }

        public QSet() { }

        public QSet(string name, string path)
        {
            Name = name;
            Path = path;
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// Adds empty question
        /// </summary>
        /// <param name="difficulty">Difficulty of the new question</param>
        /// <returns>Added question</returns>
        public Question AddQuestion(Difficulty difficulty)
        {
            Question question = new Question();
            switch (difficulty)
            {
                case Difficulty.Easy:
                    EasyQuestions.Add(question);
                    break;
                case Difficulty.Medium:
                    MediumQuestions.Add(question);
                    break;
                case Difficulty.Hard:
                    HardQuestions.Add(question);
                    break;
            }
            return question;
        }

        /// <summary>
        /// Adds question
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="questionSentence"></param>
        /// <param name="rightAnswer"></param>
        /// <param name="wrongAnswer1"></param>
        /// <param name="wrongAnswer2"></param>
        /// <param name="wrongAnswer3"></param>
        public void AddQuestion(Difficulty difficulty, string questionSentence, string rightAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    EasyQuestions.Add(new Question(questionSentence, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
                case Difficulty.Medium:
                    MediumQuestions.Add(new Question(questionSentence, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
                case Difficulty.Hard:
                    HardQuestions.Add(new Question(questionSentence, rightAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3));
                    break;
            }
        }
    }
}
