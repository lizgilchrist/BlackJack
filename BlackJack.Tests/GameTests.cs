﻿using System;
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
            Game game = CreateGame();

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
            Game game = CreateGame();

            game.OnGameTurn += (ev) =>
            {
                return TurnAction.Stay;
            };

            bool onGameHitTriggered = false;
            game.OnGameHit += (ev) =>
            {
                if(ev.Player.Name == "Player")
                {
                    onGameHitTriggered = true;
                }
            };

            bool onGameStayTriggered = false;
            game.OnGameStay += (ev) =>
            {
                if(ev.Player.Name == "Player")
                {
                    onGameStayTriggered = true;
                }
            };

            game.Start();
            Assert.False(onGameHitTriggered);
            Assert.True(onGameStayTriggered);
        }

        public Game CreateGame()
        {
            Game game = new Game(new HumanPlayer("Player"));
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
