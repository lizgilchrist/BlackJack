﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Hand
    {

        private List<Card> _cards;

        public Hand()
        {
            _cards = new List<Card>();
        }

        public List<Card> GetCards()
        {
            return _cards; 
        }
        
        public int Value
        {
            get
            {
                int cardTotal = 0;
                foreach (Card card in _cards.OrderByDescending(c => c.Face)) //OrderBy descending to consider the Ace (1) last after all other cards.
                {
                    if (card.Face == Face.Ace)
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
                    else if (card.Face == Face.Jack || card.Face == Face.Queen || card.Face == Face.King)
                    {
                        cardTotal += 10;
                    }
                    else
                    {
                        cardTotal += (int)card.Face;
                    }
                }
                return cardTotal;
            }
        }

        public bool IsBust
        {
            get
            {
                return Value > 21;
            }
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public Hand Split()
        {
            var removedCard = _cards[1];
            // Remove card from list
             _cards.RemoveAt(1);
            // Create new hand
            Hand splitHand = new Hand();
            splitHand.AddCard(removedCard);

            return splitHand;
        }
    }
    
}