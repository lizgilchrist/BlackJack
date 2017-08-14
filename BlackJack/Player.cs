using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public abstract class Player
    {
        public Hand Hand { get; set; }

        public abstract string Name { get; }

        public virtual bool HasBlackjack
        {
            get
            {
                return Hand.Value == 21 && Hand.GetCards().Count() == 2;
            }
        }
    }
}
