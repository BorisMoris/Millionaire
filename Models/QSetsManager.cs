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

        public void Sort()
        {
            List<QSet> sorted = QuestionSets.OrderBy(x => x.Name).ToList();
            QuestionSets = new ObservableCollection<QSet>(sorted);
        }
    }
}