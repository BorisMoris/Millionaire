using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    class _50_50Lifeline
    {
        private GameManager gameManager;
        
        public _50_50Lifeline(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public List<int> WrongAnswers()
        {
            List<int> indexes = new List<int> { 0, 1, 2, 3 };
            Random random = new Random();
            indexes.Remove(gameManager.RightAnswerIndex);
            indexes.RemoveAt(random.Next(indexes.Count()));

            return indexes;
        }
    }
}
