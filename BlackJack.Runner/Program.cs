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
            var game = CreateGame(new MockDeck(
                new Card(Suit.Diamonds, Face.Eight),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Ace),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Hearts, Face.Eight),
                new Card(Suit.Hearts, Face.Ten)

                ));

            game.Start();
        }

        public static Game CreateGame(IDeck deck)
        {
            Game game = new Game(new HumanPlayer("Player", 500), deck);
            game.OnRoundBet += (ev) => { return 500; };
            game.OnRoundStart += (ev) => { };
            game.OnRoundInsurance += (ev) => { return InsuranceAction.No; };
            game.OnRoundIfInsurance += (ev) => { };
            game.OnRoundSplit += (ev) => { return SplitAction.No; };
            game.OnRoundDeal += (ev) => { };
            game.OnRoundStay += (ev) => { };
            game.OnRoundBust += (ev) => { };
            game.OnRoundTurnDecision += (ev) => { return TurnAction.Stay;};
            game.OnRoundTurnStart += (ev) => { };
            game.OnRoundHoleCardReveal += (ev) => { };
            game.OnRoundHandResult += (ev) => { };
            game.OnRoundEnd += (ev) => { return RoundEndAction.Quit; };
            return game;
        }
    }
}
