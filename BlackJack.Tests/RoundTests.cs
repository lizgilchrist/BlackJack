using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlackJack.Tests
{
    public class RoundTests
    {
        [Fact]
        public void HitTriggersHitEvent()
        {
            Round round = CreateRound(new Deck());

            round.OnRoundTurn += (ev) =>
            {
                return TurnAction.Hit;
            };

            bool onRoundHitTriggered = false;
            round.OnRoundHit += (ev) =>
            {
                onRoundHitTriggered = true;
            };

            bool onRoundStayTriggered = false;
            round.OnRoundStay += (ev) =>
            {
                onRoundStayTriggered = true;
            };

            round.Start();
            Assert.True(onRoundHitTriggered);
            Assert.False(onRoundStayTriggered);
        }

        [Fact]
        public void StayTriggeredStayEvent()
        {
            Round round = CreateRound(new Deck());

            round.OnRoundTurn += (ev) =>
            {
                return TurnAction.Stay;
            };

            bool onRoundHitTriggered = false;
            round.OnRoundHit += (ev) =>
            {
                if (ev.Player.Name == "Player")
                {
                    onRoundHitTriggered = true;
                }
            };

            bool onRoundStayTriggered = false;
            round.OnRoundStay += (ev) =>
            {
                if (ev.Player.Name == "Player")
                {
                    onRoundStayTriggered = true;
                }
            };

            round.Start();
            Assert.False(onRoundHitTriggered);
            Assert.True(onRoundStayTriggered);
        }

        [Fact]
        public void TestPlayerBustWithThreeKings()
        {
            var round = CreateRound(new MockDeck(
                new Card(Suit.Clubs, Face.King),
                new Card(Suit.Clubs, Face.King),
                new Card(Suit.Clubs, Face.King),
                new Card(Suit.Clubs, Face.King)));

            round.OnRoundTurn += ev =>
            {
                return TurnAction.Hit;
            };

            bool onRoundBustTriggered = false;
            round.OnRoundBust += (ev) =>
            {
                if (ev.Player.Name == "Player")
                {
                    onRoundBustTriggered = true;
                }
            };

            round.Start();

            Assert.True(onRoundBustTriggered);
        }

        [Fact]
        public void TestDealerBust()
        {
            var round = CreateRound(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Six),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Five),
                new Card(Suit.Clubs, Face.King)
                ));

            round.OnRoundTurn += ev =>
            {
                return TurnAction.Stay;
            };

            bool onRoundBustTriggered = false;
            round.OnRoundBust += (ev) =>
            {
                if (ev.Dealer != null)
                {
                    onRoundBustTriggered = true;
                }
            };

            round.Start();

            Assert.True(onRoundBustTriggered);
        }

        public Round CreateRound(IDeck deck)
        {
            Round round = new Round(new HumanPlayer("Player"), deck);
            round.OnRoundStart += (ev) => { };
            round.OnRoundSplit += (ev) => { return SplitAction.No; };
            round.OnRoundHit += (ev) => { };
            round.OnRoundStay += (ev) => { };
            round.OnRoundBust += (ev) => { };
            round.OnRoundHoleCardReveal += (ev) => { };
            round.OnRoundHandResult += (ev) => { };
            round.OnRoundEnd += (ev) => { };
            return round;
        }

        [Fact]
        public void TestPlayerHandWins()
        {
            var round = CreateRound(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Nine),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight)
                ));

            round.OnRoundTurn += ev =>
            {
                return TurnAction.Stay;
            };

            bool onRoundEndPlayerHandWins = false;

            round.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.Win)
                {
                    onRoundEndPlayerHandWins = true;
                }
            };

            round.Start();

            Assert.True(onRoundEndPlayerHandWins);
        }


        [Fact]
        public void TestDealerWins()
        {
            var round = CreateRound(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Ten)
                ));

            round.OnRoundTurn += ev =>
            {
                return TurnAction.Stay;
            };

            bool onRoundEndDealerWins = false;

            round.OnRoundHandResult += (ev) =>
            {
                if (ev.Result == HandResult.Lose)
                {
                    onRoundEndDealerWins = true;
                }
            };

            round.Start();

            Assert.True(onRoundEndDealerWins);
        }

        [Fact]
        public void TestRoundHandIsTie()
        {
            var round = CreateRound(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight)
                ));

            round.OnRoundTurn += ev =>
            {
                return TurnAction.Stay;
            };

            bool onRoundEndHandTie = false;

            round.OnRoundHandResult += (ev) =>
            {
                if(ev.Result == HandResult.Tie)
                {
                    onRoundEndHandTie = true;
                }
                
            };

            round.Start();

            Assert.True(onRoundEndHandTie);
        }

        [Fact]
        public void TestPlayerOfferedSplit()
        {
            var round = CreateRound(new MockDeck(
                new Card(Suit.Diamonds, Face.Ten),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Eight),
                new Card(Suit.Clubs, Face.Ten)

                ));

            bool playerOfferedSplit = false;
            round.OnRoundSplit += (ev) =>
            {
                if(ev.Player.Name == "Player")
                {
                    playerOfferedSplit = true;
                };
                return SplitAction.No;
            };

            round.OnRoundTurn += (ev) => { return TurnAction.Stay;};

            round.Start();

            Assert.True(playerOfferedSplit);

        }

    }
}
