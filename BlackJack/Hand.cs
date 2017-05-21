using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Hand
    {
        private static List<Card> __allCards = new List<Card>();

        private List<Card> _cards;

        public Hand()
        {
            _cards = new List<Card>();
        }

        public int Value
        {
            get
            {
                int cardTotal = 0;
                foreach (Card card in _cards.OrderByDescending(c => c.ValueId)) //OrderBy descending to consider the Ace (1) last after all other cards.
                {
                    if (card.ValueId == 1)
                    {
                        if (cardTotal < 11)
                        {
                            cardTotal += 11;
                        }
                        else
                        {
                            cardTotal += 1;
                        } 
                    }
                    else if (card.ValueId == 11 || card.ValueId == 12 || card.ValueId == 13)
                    {
                        cardTotal += 10;
                    }
                    else
                    {
                        cardTotal += card.ValueId;
                    }
                }
                return cardTotal;
            }
        }

        public void AddCard(Card card)
        {
            if (__allCards.Contains(card))
            {
                throw new InvalidOperationException("Card has already been added to a hand");
            }

            __allCards.Add(card);
            _cards.Add(card);
        }
    }
    
}