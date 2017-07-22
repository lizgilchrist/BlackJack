using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Game
    {
        private DealerPlayer _dealer;
        private HumanPlayer _player;
        private IDeck _deck;

        public Game(HumanPlayer player, IDeck deck)
        {
            _dealer = new DealerPlayer();
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
        public event Action<OnGameHandResultArgs> OnGameHandResult;
        public event Action<OnGameEndArgs> OnGameEnd;

        public void Start()
        {
            _player.Hand = new Hand();
            _player.Hand.AddCard(_deck.GetNextCard());
            _player.Hand.AddCard(_deck.GetNextCard());

            _dealer.Hand = new Hand();
            _dealer.Hand.AddCard(_deck.GetNextCard());

            OnGameStart(new OnGameStartArgs()
            {
                Player = _player,
                Dealer = _dealer
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

            bool isFirstHandAce = cards[0].Face == Face.Ace;
            List<Card> splitHandCards = _player.SplitHand.GetCards();
            bool isSplitHandAce = splitHandCards[0].Face == Face.Ace;

            if(!(isFirstHandAce && isSplitHandAce))
            {
                ResolvePlayerHand(_player.Hand);

                if (_player.IsSplit)
                {
                    ResolvePlayerHand(_player.SplitHand);
                }
            }

            if (_player.Hand.IsBust)
            {
                OnGameBust(new OnGameBustArgs()
                {
                    Player = _player,
                    BustHand = _player.Hand
                });
                return;
            }

            if (_player.SplitHand.IsBust)
            {
                OnGameBust(new OnGameBustArgs()
                {
                    Player = _player,
                    BustHand = _player.SplitHand
                });
                return;
            }

            Card holeCard = _deck.GetNextCard();
            _dealer.Hand.AddCard(holeCard);
            OnGameHoleCardReveal(new OnGameHoleCardRevealArgs()
            {
                Dealer = _dealer,
                HoleCard = holeCard
            });

            while (_dealer.Hand.Value < 17 && _dealer.Hand.Value < _player.Hand.Value)
            {
                _dealer.Hand.AddCard(_deck.GetNextCard());
                OnGameHit(new OnGameHitArgs()
                {
                    Player = _dealer
                });

            }

            if (!_dealer.Hand.IsBust)
            {
                OnGameStay(new OnGameStayArgs()
                {
                    Player = _dealer
                });
            }

            else
            {
                OnGameBust(new OnGameBustArgs()
                {
                    Dealer = _dealer,
                    BustHand = _dealer.Hand
                });
            }

            ResolveGameResult(_player.Hand);

            if(_player.IsSplit)
            {
                ResolveGameResult(_player.SplitHand);
            }

            OnGameEnd(new OnGameEndArgs());

        }

        private void ResolveGameResult(Hand hand)
        {
            HandResult result = HandResult.Unknown;

            if (_dealer.Hand.Value > hand.Value)
            {
                result = HandResult.Lose;
            }
            else if (_dealer.Hand.Value < hand.Value)
            {
                result = HandResult.Win;
            }
            else
            {
                result = HandResult.Tie;
            }

            OnGameHandResult(new OnGameHandResultArgs()
            {
                Result = result,
                Player = _player,
                Hand = hand

            });

        }

        /*private void ResolvePlayerGame(Hand hand)
        {
            Player winner = null;

            if (_dealer.Hand.Value > hand.Value)
            {
                winner = _dealer;
            }
            else if (_dealer.Hand.Value < hand.Value)
            {
                winner = _player;
            }

            OnGameEnd(new OnGameEndArgs()
            {
                Winner = winner
            });
        }
        */
        private void ResolvePlayerHand(Hand hand)
        {
            while (!hand.IsBust)
            {
                TurnAction turnAction = OnGameTurn(new OnGameTurnArgs()
                {
                    Player = _player
                });

                if (turnAction == TurnAction.Hit)
                {
                    hand.AddCard(_deck.GetNextCard());
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
        }
    }

    public class OnGameStartArgs
    {
        public Player Dealer { get; set; }

        public Player Player { get; set; }
    }

    public class OnGameSplitArgs
    {
        public HumanPlayer Player { get; set; }
    }

    public class OnGameTurnArgs
    {
        public HumanPlayer Player { get; set; }
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
        public DealerPlayer Dealer { get; set; }

        public HumanPlayer Player { get; set; }

        public Hand BustHand { get; set; }
    }

    public class OnGameHoleCardRevealArgs
    {
        public Player Dealer { get; set; }

        public Card HoleCard { get; set; }
    }

    public class OnGameHandResultArgs
    {
        public Hand Hand { get; set; }

        public HandResult Result { get; set; }

        public Player Player { get; set; }
    }

    public class OnGameEndArgs
    {
        
    }
}
