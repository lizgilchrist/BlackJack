using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Game
    {
        //Events
        private HumanPlayer _player;
        private IDeck _deck;

        public Game(HumanPlayer player, IDeck deck)
        {
            _player = player;
            _deck = deck;
        }

        public event Action<OnGameStartArgs> OnGameStart;
        public event Func<OnGameSplitArgs, SplitAction> OnGameSplit;
        public event Func<OnGameTurnArgs, TurnAction> OnGameTurn;
        public event Action<OnGameHitArgs> OnGameHit;
        public event Action<OnGameStayArgs> OnGameStay;
        public event Action<OnGameBustArgs> OnGameBust;
        public event Action<OnGameHoleCardRevealArgs> OnGameHoleCardReveal;
        public event Action<OnGameEndArgs> OnGameEnd;

        public void Start()
        {
            _player.Hand = new Hand();
            _player.Hand.AddCard(_deck.GetNextCard());
            _player.Hand.AddCard(_deck.GetNextCard());

            DealerPlayer dealer = new DealerPlayer();
            dealer.Hand = new Hand();
            dealer.Hand.AddCard(_deck.GetNextCard());

            OnGameStart(new OnGameStartArgs()
            {
                Player = _player,
                Dealer = dealer
            });

            List<Card> cards = _player.Hand.GetCards();
            if (cards[0].Face == cards[1].Face)
            {
                SplitAction splitAction = OnGameSplit(new OnGameSplitArgs()
                {
                    Player = _player
                });

                if (splitAction == SplitAction.Yes)
                {
                    Hand splitHand = _player.Hand.Split();
                    _player.Hand.AddCard(_deck.GetNextCard());
                    splitHand.AddCard(_deck.GetNextCard());
                    _player.SplitHand = splitHand;

                }
            }

            while (!_player.Hand.IsBust)
            {
                TurnAction turnAction = OnGameTurn(new OnGameTurnArgs()
                {
                    Player = _player
                });

                if (turnAction == TurnAction.Hit)
                {
                    _player.Hand.AddCard(_deck.GetNextCard());
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

            Card holeCard = _deck.GetNextCard();
            dealer.Hand.AddCard(holeCard);
            OnGameHoleCardReveal(new OnGameHoleCardRevealArgs()
            {
                Dealer = dealer,
                HoleCard = holeCard
            });

            while (dealer.Hand.Value < 17 && dealer.Hand.Value < _player.Hand.Value)
            {
                dealer.Hand.AddCard(_deck.GetNextCard());
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

    public class OnGameSplitArgs
    {
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
