﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Events
{
    public class OnRoundHoleCardRevealArgs
    {
        public Player Dealer { get; set; }

        public Card HoleCard { get; set; }
    }
}
