using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    class AudienceLifeline
    {
        private GameManager gameManager;
        public int[] ResponsesRatio { get; set; }

        public AudienceLifeline(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void CalculateResponses()
        {

        }
    }
}
