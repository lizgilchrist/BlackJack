using BlackJack.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var round = CreateRound(new MockDeck(
                new Card(Suit.Diamonds, Face.Eight),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Hearts, Face.Eight),
                new Card(Suit.Hearts, Face.Ten)

                ));

            round.Start();
        }

        public static Round CreateRound(IDeck deck)
        {
            Round round = new Round(new HumanPlayer("Player"), deck);
            round.OnRoundStart += (ev) => { };
            round.OnRoundSplit += (ev) => { return SplitAction.Yes; };
            round.OnRoundHit += (ev) => { };
            round.OnRoundStay += (ev) => { };
            round.OnRoundBust += (ev) => { };
            round.OnRoundTurn += (ev) => { return TurnAction.Stay;};
            round.OnRoundHoleCardReveal += (ev) => { };
            round.OnRoundHandResult += (ev) => { };
            return round;
        }
    }
}
