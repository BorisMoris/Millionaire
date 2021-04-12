namespace Millionaire.Models
{
    public class Question
    {
        public string QuestionSentence { get; set; }
        public string RightAnswer { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public string WrongAnswer3 { get; set; }

        public Question()
        {
            QuestionSentence = string.Empty; //Musn't be null because of sorting
        }

        public Question(string question, string rightAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3)
        {
            QuestionSentence = question;
            RightAnswer = rightAnswer;
            WrongAnswer1 = wrongAnswer1;
            WrongAnswer2 = wrongAnswer2;
            WrongAnswer3 = wrongAnswer3;
        }
    }
}
