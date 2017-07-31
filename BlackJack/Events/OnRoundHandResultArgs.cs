using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Events
{
    public class OnRoundHandResultArgs
    {
        public Hand Hand { get; set; }

        public HandResult Result { get; set; }

        public Player Player { get; set; }
    }
}
