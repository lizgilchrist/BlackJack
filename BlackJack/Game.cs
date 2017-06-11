﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Game
    {
        //Events
        private Player _player;

        public Game(Player player)
        {
            _player = player;
        }

        public event Action<OnGameStartArgs> OnGameStart;
        public event Func<OnGameTurnArgs, TurnAction> OnGameTurn;
        public event Action<OnGameHitArgs> OnGameHit;
        public event Action<OnGameStayArgs> OnGameStay;
        public event Action<OnGameBustArgs> OnGameBust;
        public event Action<OnGameHoleCardRevealArgs> OnGameHoleCardReveal;
        public event Action<OnGameEndArgs> OnGameEnd;

        public void Start()
        {
            Deck deck = new Deck();

            _player.Hand = new Hand();
            _player.Hand.AddCard(deck.GetNextCard());
            _player.Hand.AddCard(deck.GetNextCard());

            DealerPlayer dealer = new DealerPlayer();
            dealer.Hand = new Hand();
            dealer.Hand.AddCard(deck.GetNextCard());

            OnGameStart(new OnGameStartArgs()
            {
                Player = _player,
                Dealer = dealer
            });

            while (!_player.Hand.IsBust)
            {
                TurnAction turnAction = OnGameTurn(new OnGameTurnArgs()
                {
                    Player = _player
                });

                if (turnAction == TurnAction.Hit)
                {
                    _player.Hand.AddCard(deck.GetNextCard());
                    OnGameHit(new OnGameHitArgs()
                    {
                        Player = _player
                    });
                }
                else if (turnAction == TurnAction.Stay)
                {
                    OnGameStay(new OnGameStayArgs()
                    {
                        Player = _player
                    });
                    break;
                }
                else
                {
                    throw new InvalidOperationException("Invalid TurnAction");
                }
            }

            if (_player.Hand.IsBust)
            {
                OnGameBust(new OnGameBustArgs()
                {
                    Player = _player
                });
                return;
            }

            Card holeCard = deck.GetNextCard();
            dealer.Hand.AddCard(holeCard);
            OnGameHoleCardReveal(new OnGameHoleCardRevealArgs()
            {
                Dealer = dealer,
                HoleCard = holeCard
            });

            while (dealer.Hand.Value < 17 && dealer.Hand.Value < _player.Hand.Value)
            {
                dealer.Hand.AddCard(deck.GetNextCard());
                OnGameHit(new OnGameHitArgs()
                {
                    Player = dealer
                });

            }

            if (!dealer.Hand.IsBust)
            {
                OnGameStay(new OnGameStayArgs()
                {
                    Player = dealer
                });
            }

            else
            {
                OnGameBust(new OnGameBustArgs()
                {
                    Player = dealer
                });
            }

            Player winner = null;

            if (dealer.Hand.Value > _player.Hand.Value)
            {
                winner = dealer;
            }
            else if (dealer.Hand.Value < _player.Hand.Value)
            {
                winner = _player;
            }

            OnGameEnd(new OnGameEndArgs()
            {
                Winner = winner
            });
        }
    }

    public class OnGameStartArgs
    {
        public Player Dealer { get; set; }

        public Player Player { get; set; }
    }

    public class OnGameTurnArgs
    {
        public Player Player { get; set; }
    }

    public class OnGameHitArgs
    {
        public Player Player { get; set; } 
    }

    public class OnGameStayArgs
    {
        public Player Player { get; set; }
    }

    public class OnGameBustArgs
    {
        public Player Dealer { get; set; }

        public Player Player { get; set; }
    }

    public class OnGameHoleCardRevealArgs
    {
        public Player Dealer { get; set; }

        public Card HoleCard { get; set; }
    }

    public class OnGameEndArgs
    {
        public Player Winner { get; set; }
    }
}
