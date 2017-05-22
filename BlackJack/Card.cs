using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Card
    {
        public Suit Suit { get; set; }

        public Face Face { get; set; }

        /*public string Value
        {
            get
            {
                return ValueId.ToString();
            }
        }
        */
        public Card(Suit suit, Face face)
        {
            if (face > Face.King || face < Face.Ace)
            {
                throw new ArgumentOutOfRangeException(nameof(face));
            }
            
            Suit = suit;
            Face = face;
        }

        public override string ToString()
        {
            // TODO: Return a nicely formatted version of card, e.g. if this card's Face is Ten and Suit is Diamonds, return "10 ♦"
            return base.ToString();
        }
    }
}

