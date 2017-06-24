using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Tests
{
    public class MockDeck : IDeck
    {
        private List<Card> _cards;

        public MockDeck(params Card[] cards)
        {
            _cards = cards.ToList();
        }

        public Card GetNextCard()
        {
            var card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }
    }
}
