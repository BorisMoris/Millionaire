using System;
using System.ComponentModel;

namespace Millionaire.Models
{
    public abstract class LifelineBase : INotifyPropertyChanged
    {
        protected Random random;

        public LifelineBase(Random random)
        {
            this.random = random;
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// Decides whether lifeline should be correct
        /// </summary>
        /// <param name="round">Current round of the game</param>
        /// <returns>Bool correct</returns>
        public bool DecideIfAdviseCorrectly(int round)
        {
            if (round > 10) //in difficult section the lifeline has 67% probability of being correct
            {
                return random.Next(3) < 2;
            }
            else if (round > 5) //in medium section the lifeline has 80% probability of being correct
            {
                return random.Next(5) < 4;
            }
            else //in easy section the lifeline will always be correct
            {                
                return true;
            }
        }

        /// <summary>
        /// Randomly change index
        /// </summary>
        /// <param name="index">Current index</param>
        /// <returns>Integer 0-3, other than inputted index</returns>
        public int ChangeIndex(int index)
        {
            return (index + random.Next(1, 4)) % 4; //add random number 1-3, if the result is bigger than 3, lower it by 4
        }
    }
}
