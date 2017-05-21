using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlackJack.Tests
{
    public class HandValueTests
    {

        [Fact]
        public void TestHandValue()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Clubs, 12));
            Assert.Equal(10, hand.Value);
        }

        [Fact]
        public void TestHandValueNumberCard()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Hearts, 6));
            Assert.Equal(6, hand.Value);
        }

        [Fact]
        public void TestHandValueMultiple()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Diamonds, 6));
            hand.AddCard(new Card(Suit.Clubs, 12));
            Assert.Equal(16, hand.Value);
        }

        [Fact]
        public void TestHandVauleAce()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Clubs, 1));
            Assert.Equal(11, hand.Value);
        }

        [Fact]
        public void TestHandValueTwoAces()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Diamonds, 1));
            hand.AddCard(new Card(Suit.Hearts, 1));
            Assert.Equal(12, hand.Value);
        }

        [Fact]
        public void TestHandMultipleAndAce()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Diamonds, 1));
            hand.AddCard(new Card(Suit.Hearts, 6));
            hand.AddCard(new Card(Suit.Hearts, 11));
            Assert.Equal(17, hand.Value);
        }
    }
}
