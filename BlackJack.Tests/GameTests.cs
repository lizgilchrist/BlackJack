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
        public void HitTriggersHitEvent()
        {
            Game game = CreateGame(new Deck());

            game.OnGameTurn += (ev) =>
            {
                return TurnAction.Hit;
            };

            bool onGameHitTriggered = false;
            game.OnGameHit += (ev) =>
            {
                onGameHitTriggered = true;
            };

            bool onGameStayTriggered = false;
            game.OnGameStay += (ev) =>
            {
                onGameStayTriggered = true;
            };

            game.Start();
            Assert.True(onGameHitTriggered);
            Assert.False(onGameStayTriggered);
        }

        [Fact]
        public void StayTriggeredStayEvent()
        {
            Game game = CreateGame(new Deck());

            game.OnGameTurn += (ev) =>
            {
                return TurnAction.Stay;
            };

            bool onGameHitTriggered = false;
            game.OnGameHit += (ev) =>
            {
                if (ev.Player.Name == "Player")
                {
                    onGameHitTriggered = true;
                }
            };

            bool onGameStayTriggered = false;
            game.OnGameStay += (ev) =>
            {
                if (ev.Player.Name == "Player")
                {
                    onGameStayTriggered = true;
                }
            };

            game.Start();
            Assert.False(onGameHitTriggered);
            Assert.True(onGameStayTriggered);
        }

        [Fact]
        public void TestPlayerBustWithThreeKings()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Clubs, Face.King),
                new Card(Suit.Clubs, Face.King),
                new Card(Suit.Clubs, Face.King),
                new Card(Suit.Clubs, Face.King)));

            game.OnGameTurn += ev =>
            {
                return TurnAction.Hit;
            };

            bool onGameBustTriggered = false;
            game.OnGameBust += (ev) =>
            {
                if (ev.Player.Name == "Player")
                {
                    onGameBustTriggered = true;
                }
            };

            game.Start();

            Assert.True(onGameBustTriggered);
        }

        [Fact]
        public void TestDealerBustWithThreeKings()
        {
            var game = CreateGame(new MockDeck(
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Six),
                new Card(Suit.Clubs, Face.Ten),
                new Card(Suit.Clubs, Face.Five),
                new Card(Suit.Clubs, Face.King)
                ));

            game.OnGameTurn += ev =>
            {
                return TurnAction.Stay;
            };
           
            bool onGameBustTriggered = false;
            game.OnGameBust += (ev) =>
            {
                if (ev.Player.Name == "Dealer")
                {
                    onGameBustTriggered = true;
                }
            };

            game.Start();

            Assert.True(onGameBustTriggered);
        }


        public Game CreateGame(IDeck deck)
        {
            Game game = new Game(new HumanPlayer("Player"), deck);
            game.OnGameStart += (ev) => { };
            game.OnGameHit += (ev) => { };
            game.OnGameStay += (ev) => { };
            game.OnGameBust += (ev) => { };
            game.OnGameHoleCardReveal += (ev) => { };
            game.OnGameEnd += (ev) => { };
            return game;
        }
    }
}
