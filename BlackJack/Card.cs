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

        public int ValueId { get; set; }

        /*public string Value
        {
            get
            {
                return ValueId.ToString();
            }
        }
        */
        public Card(Suit suit, int valueId)
        {
            if (valueId > 13 || valueId < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(valueId));
            }
            
            Suit = suit;
            ValueId = valueId;
        }
    }
}

