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
                new Card(Suit.Diamonds, Face.Ten),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Hearts, Face.Eight),
                new Card(Suit.Hearts, Face.Seven)

                ));

            game.Start();
        }

        public static Game CreateGame(IDeck deck)
        {
            Game game = new Game(new HumanPlayer("Player"), deck);
            game.OnGameStart += (ev) => { };
            game.OnGameSplit += (ev) => { return SplitAction.Yes; };
            game.OnGameHit += (ev) => { };
            game.OnGameStay += (ev) => { };
            game.OnGameBust += (ev) => { };
            game.OnGameHoleCardReveal += (ev) => { };
            game.OnGameEnd += (ev) => { };
            return game;
        }
    }
}
