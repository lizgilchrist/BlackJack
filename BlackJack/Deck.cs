using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Deck
    {
        private static readonly Random RandomCard = new Random();

        public Card GetNextCard()
        {
            return new Card((Suit)RandomCard.Next(1, 4), RandomCard.Next(1, 13));
        }

    }
}
