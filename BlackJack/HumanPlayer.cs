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
        private int _account;

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public int Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
            }
        }

        public Hand SplitHand { get; set; }

        public bool IsSplit
        {
            get
            {
                return SplitHand != null;
            }
        }

        public override bool HasBlackjack
        {
            get
            {
                return !IsSplit && base.HasBlackjack;
            }
        }

        public HumanPlayer(string name, int account)
        {
            _name = name;
            _account = account;
        }
    }
}
