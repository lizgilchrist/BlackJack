using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlackJack.Tests
{
    public class GameTests
    {
        [Fact]

        public void TestPlayerHandWins()
        {
            Game game = CreateGame(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Nine),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight)
                ));

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            bool onRoundEndPlayerHandWins = false;
            int? account = null;

            game.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.Win)
                {
                    onRoundEndPlayerHandWins = true;
                    account = ev.Player.Account;
                }
            };

            game.Start();

            Assert.True(onRoundEndPlayerHandWins);
            Assert.Equal(600, account);
        }

        [Fact]
        public void TestDealerWins()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Ten)
                ));

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            bool onRoundEndDealerWins = false;
            int? account = null;

            game.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.Lose)
                {
                    onRoundEndDealerWins = true;
                    account = ev.Player.Account;
                }
            };

            game.Start();

            Assert.True(onRoundEndDealerWins);
            Assert.Equal(400, account);
        }

        [Fact]
        public void TestRoundHandIsTie()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight)
                ));

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            bool onRoundEndHandTie = false;
            int? account = null;

            game.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.Tie)
                {
                    onRoundEndHandTie = true;
                    account = ev.Player.Account;
                }

            };

            game.Start();

            Assert.True(onRoundEndHandTie);
            Assert.Equal(500, account);
        }

        private Game CreateGame(IDeck deck)
        {
            Game game = new Game(new HumanPlayer("Player", 500), deck);
            game.OnRoundStart += (ev) => { };
            game.OnRoundBet += (ev) => { return 100; };
            game.OnRoundSplit += (ev) => { return SplitAction.No; };
            game.OnRoundDouble += (ev) => { return DoubleAction.No; };
            game.OnRoundIfDouble += (ev) => { };
            game.OnRoundTurnStart += (ev) => { };
            game.OnRoundDeal += (ev) => { };
            game.OnRoundStay += (ev) => { };
            game.OnRoundBust += (ev) => { };
            game.OnRoundHoleCardReveal += (ev) => { };
            game.OnRoundHandResult += (ev) => { };
            game.OnRoundEnd += (ev) => { return RoundEndAction.Quit; };
            return game;
        }
    }
}
