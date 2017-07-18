using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class HumanPlayer : Player
    {
        private string _name;

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public Hand SplitHand { get; set; }

        public HumanPlayer(string name)
        {
            _name = name;
        }
    }
}
