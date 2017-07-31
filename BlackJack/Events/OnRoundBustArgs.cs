using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Events
{
    public class OnRoundBustArgs
    {
        public DealerPlayer Dealer { get; set; }

        public HumanPlayer Player { get; set; }

        public Hand BustHand { get; set; }
    }
}
