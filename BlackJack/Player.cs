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

    }
}
