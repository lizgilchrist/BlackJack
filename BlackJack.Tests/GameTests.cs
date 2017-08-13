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

        public void TestPlayerWinsBlackJack()
        {
            Game game = CreateGame(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Ace),
                new Card(Suit.Diamonds, Face.Six),
                new Card(Suit.Hearts, Face.Queen),
                new Card(Suit.Hearts, Face.Two)
                ));

            bool onRoundEndPlayerWinsBlackJack = false;
            int? account = null;

            game.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.BlackJack)
                {
                    onRoundEndPlayerWinsBlackJack = true;
                    account = ev.Player.Account;
                }
            };

            game.Start();

            Assert.True(onRoundEndPlayerWinsBlackJack);
            Assert.Equal(650, account);
        }

        [Fact]
        public void TestPlayerSplitHandWinsOtherHandLoses()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Diamonds, Face.Eight),
                new Card(Suit.Diamonds, Face.Eight),
                new Card(Suit.Diamonds, Face.Seven),
                new Card(Suit.Diamonds, Face.Ten),
                new Card(Suit.Diamonds, Face.Six),
                new Card(Suit.Hearts, Face.Ten)
                ));

            game.OnRoundSplit += ev =>
            {
                return SplitAction.Yes;
            };

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            int? account = null;
            int numberOfWins = 0;

            game.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.Win)
                {
                    account = ev.Player.Account;
                    numberOfWins++;
                }
            };

            game.Start();

            Assert.Equal(500, account);
            Assert.Equal(1, numberOfWins);
        }

        [Fact]
        public void TestPlayerSplitHandWinsOtherHandTies()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Diamonds, Face.Eight),
                new Card(Suit.Diamonds, Face.Eight),
                new Card(Suit.Diamonds, Face.Seven),
                new Card(Suit.Diamonds, Face.Ten),
                new Card(Suit.Diamonds, Face.Nine),
                new Card(Suit.Hearts, Face.Ten)
                ));

            game.OnRoundSplit += ev =>
            {
                return SplitAction.Yes;
            };

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            int? account = null;

            game.OnRoundHandResult += (ev) =>
            {
                account = ev.Player.Account;
            };

            game.Start();

            Assert.Equal(600, account);
        }

        [Fact]
        public void TestPlayerSplitHandsBothWin()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Diamonds, Face.Eight),
                new Card(Suit.Diamonds, Face.Seven),
                new Card(Suit.Hearts, Face.Ten),
                new Card(Suit.Hearts, Face.Jack),
                new Card(Suit.Spades, Face.Ten)
                ));

            game.OnRoundSplit += ev =>
            {
                return SplitAction.Yes;
            };

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            int? account = null;
            int numberOfWins = 0;

            game.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.Win)
                {
                    account = ev.Player.Account;
                    numberOfWins++;
                }
            };

            game.Start();

            Assert.Equal(700, account);
            Assert.Equal(2, numberOfWins);
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
        public void TestDealerWinsBlackJack()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Diamonds, Face.King),
                new Card(Suit.Diamonds, Face.Ace),
                new Card(Suit.Diamonds, Face.Ten)
                ));

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            bool onRoundEndDealerWinsBlackJack = false;
            int? account = null;

            game.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.Lose)
                {
                    onRoundEndDealerWinsBlackJack = true;
                    account = ev.Player.Account;
                }
            };

            game.Start();

            Assert.True(onRoundEndDealerWinsBlackJack);
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

        [Fact]
        public void TestPlayerDoubleDownWin()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Spades, Face.Eight),
                new Card(Suit.Hearts, Face.Three),
                new Card(Suit.Hearts, Face.Ten),
                new Card(Suit.Hearts, Face.Ten),
                new Card(Suit.Hearts, Face.Seven)
                ));

            game.OnRoundDouble += ev =>
            {
                return DoubleAction.Yes;
            };

            bool onRoundPlayerWinsOnDouble = false;
            int? account = null;

            game.OnRoundHandResult += ev =>
            {
                if (ev.Result == HandResult.Win)
                {
                    onRoundPlayerWinsOnDouble = true;
                    account = ev.Player.Account;
                }
            };

            game.Start();

            Assert.True(onRoundPlayerWinsOnDouble);
            Assert.Equal(700, account);
        }

        [Fact]
        public void TestPlayerSplitDoubleWin()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Spades, Face.Eight),
                new Card(Suit.Hearts, Face.Eight),
                new Card(Suit.Hearts, Face.Ten),
                new Card(Suit.Hearts, Face.Three),
                new Card(Suit.Hearts, Face.Nine),
                new Card(Suit.Diamonds, Face.Jack),
                new Card(Suit.Hearts, Face.Nine)
                ));

            game.OnRoundSplit += ev =>
            {
                return SplitAction.Yes;
            };

            game.OnRoundDouble += ev =>
            {
                return DoubleAction.Yes;
            };

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            int? account = null;

            game.OnRoundHandResult += ev =>
            {
                account = ev.Player.Account;
            };

            game.Start();

            Assert.Equal(600, account);
        }

        [Fact]
        public void TestPlayerSplitDoubleWinOtherHandTies()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Spades, Face.Eight),
                new Card(Suit.Hearts, Face.Eight),
                new Card(Suit.Hearts, Face.Ten),
                new Card(Suit.Hearts, Face.Three),
                new Card(Suit.Hearts, Face.Ten),
                new Card(Suit.Diamonds, Face.Jack),
                new Card(Suit.Hearts, Face.Eight)
                ));

            game.OnRoundSplit += ev =>
            {
                return SplitAction.Yes;
            };

            game.OnRoundDouble += ev =>
            {
                return DoubleAction.Yes;
            };

            game.OnRoundTurnDecision += ev =>
            {
                return TurnAction.Stay;
            };

            int? account = null;

            game.OnRoundHandResult += ev =>
            {
                account = ev.Player.Account;
            };

            game.Start();

            Assert.Equal(700, account);
        }

        private Game CreateGame(IDeck deck)
        {
            Game game = new Game(new HumanPlayer("Player", 500), deck);
            game.OnRoundStart += (ev) => { };
            game.OnRoundBet += (ev) => { return 100; };
            game.OnRoundSplit += (ev) => { return SplitAction.No; };
            game.OnRoundIfSplit += (ev) => { };
            game.OnRoundDouble += (ev) => { return DoubleAction.No; };
            game.OnRoundIfDouble += (ev) => { };
            game.OnRoundTurnStart += (ev) => { };
            game.OnRoundTurnDecision += (ev) => { return TurnAction.Stay; };
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
