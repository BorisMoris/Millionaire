using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    public class PrizeMoney : INotifyPropertyChanged
    {
        private int[] values = new int[] { 0, 1000, 2000, 3000, 5000, 10000, 20000, 40000, 80000, 160000,320000, 640000, 1250000, 2500000, 5000000, 10000000};

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
                return values[Value].ToString("### ### ##0") + " Kč";
            }
        }

        #region PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
