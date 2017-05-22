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
            hand.AddCard(new Card(Suit.Clubs, Face.Queen));
            Assert.Equal(10, hand.Value);
        }

        [Fact]
        public void TestHandValueNumberCard()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Hearts, Face.Six));
            Assert.Equal(6, hand.Value);
        }

        [Fact]
        public void TestHandValueMultiple()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Diamonds, Face.Six));
            hand.AddCard(new Card(Suit.Clubs, Face.Queen));
            Assert.Equal(16, hand.Value);
        }

        [Fact]
        public void TestHandVauleAce()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Clubs, Face.Ace));
            Assert.Equal(11, hand.Value);
        }

        [Fact]
        public void TestHandValueTwoAces()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Diamonds, Face.Ace));
            hand.AddCard(new Card(Suit.Hearts, Face.Ace));
            Assert.Equal(12, hand.Value);
        }

        [Fact]
        public void TestHandMultipleAndAce()
        {
            Hand hand = new Hand();
            hand.AddCard(new Card(Suit.Diamonds, Face.Ace));
            hand.AddCard(new Card(Suit.Hearts, Face.Six));
            hand.AddCard(new Card(Suit.Hearts, Face.Jack));
            Assert.Equal(17, hand.Value);
        }
    }
}
