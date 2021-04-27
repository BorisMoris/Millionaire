using System.ComponentModel;

namespace Millionaire.Models
{
    public class PrizeMoney : INotifyPropertyChanged
    {
        private int[] values = new int[] { 0, 1000, 2000, 3000, 5000, 10000, 20000, 40000, 80000, 160000, 320000, 640000, 1250000, 2500000, 5000000, 10000000 };

        private int value;
        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                NotifyPropertyChanged(nameof(StrValue));
            }
        }

        public string StrValue
        {
            get
            {
                return values[value].ToString("N0") + " Kč";
            }
        }

        /// <summary>
        /// Prize of the last checkpoint
        /// </summary>
        public string GuaranteedPrize
        {
            get
            {
                int lastCheckpoint = Value - (Value % 5);
                return values[lastCheckpoint].ToString("N0") + " Kč";
            }
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public static PrizeMoney operator ++(PrizeMoney prizeMoney)
        {
            prizeMoney.Value++;
            return prizeMoney;
        }
    }
}
