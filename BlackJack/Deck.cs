using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Deck : IDeck
    {
        private static readonly Random RandomCard = new Random();

        public Card GetNextCard()
        {
            return new Card((Suit)RandomCard.Next(1, 4),(Face)RandomCard.Next(1, 13));
        }

    }
}
