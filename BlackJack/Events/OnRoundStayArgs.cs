﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Events
{
    public class OnRoundStayArgs
    {
        public HumanPlayer Player { get; set; }

        public Hand Hand { get; set; }

        public DealerPlayer Dealer { get; set; }
    }
}
