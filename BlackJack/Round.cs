using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Round
    {
        private DealerPlayer _dealer;
        private HumanPlayer _player;
        private IDeck _deck;

        public Round(HumanPlayer player, IDeck deck)
        {
            _dealer = new DealerPlayer();
            _player = player;
            _deck = deck;
        }

        public event Action<OnRoundStartArgs> OnRoundStart;
        public event Func<OnRoundSplitArgs, SplitAction> OnRoundSplit;
        public event Func<OnRoundTurnArgs, TurnAction> OnRoundTurn;
        public event Action<OnRoundHitArgs> OnRoundHit;
        public event Action<OnRoundStayArgs> OnRoundStay;
        public event Action<OnRoundBustArgs> OnRoundBust;
        public event Action<OnRoundHoleCardRevealArgs> OnRoundHoleCardReveal;
        public event Action<OnRoundHandResultArgs> OnRoundHandResult;

        public void Start()
        {
            _player.Hand = new Hand();
            _player.Hand.AddCard(_deck.GetNextCard());
            _player.Hand.AddCard(_deck.GetNextCard());

            _dealer.Hand = new Hand();
            _dealer.Hand.AddCard(_deck.GetNextCard());

            OnRoundStart(new OnRoundStartArgs()
            {
                Player = _player,
                Dealer = _dealer
            });

            List<Card> cards = _player.Hand.GetCards();
            if (cards[0].Face == cards[1].Face)
            {
                SplitAction splitAction = OnRoundSplit(new OnRoundSplitArgs()
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

            if(_player.IsSplit)
            {
                bool isFirstHandAce = cards[0].Face == Face.Ace;
                List<Card> splitHandCards = _player.SplitHand.GetCards();
                bool isSplitHandAce = splitHandCards[0].Face == Face.Ace;

                if (!isFirstHandAce || !isSplitHandAce)
                {
                    ResolvePlayerHand(_player.Hand);
                    ResolvePlayerHand(_player.SplitHand);
     
                }
            }
            else
            {
                ResolvePlayerHand(_player.Hand);
            }

            if (_player.Hand.IsBust)
            {
                OnRoundBust(new OnRoundBustArgs()
                {
                    Player = _player,
                    BustHand = _player.Hand
                });
                return;
            }

            if (_player.IsSplit && _player.SplitHand.IsBust)
            {
                OnRoundBust(new OnRoundBustArgs()
                {
                    Player = _player,
                    BustHand = _player.SplitHand
                });
                return;
            }

            Card holeCard = _deck.GetNextCard();
            _dealer.Hand.AddCard(holeCard);
            OnRoundHoleCardReveal(new OnRoundHoleCardRevealArgs()
            {
                Dealer = _dealer,
                HoleCard = holeCard
            });

            while (_dealer.Hand.Value < 17 && _dealer.Hand.Value < _player.Hand.Value)
            {
                _dealer.Hand.AddCard(_deck.GetNextCard());
                OnRoundHit(new OnRoundHitArgs()
                {
                    Player = _dealer
                });

            }

            if (!_dealer.Hand.IsBust)
            {
                OnRoundStay(new OnRoundStayArgs()
                {
                    Player = _dealer
                });
            }

            else
            {
                OnRoundBust(new OnRoundBustArgs()
                {
                    Dealer = _dealer,
                    BustHand = _dealer.Hand
                });
            }

            ResolveRoundResult(_player.Hand);

            if(_player.IsSplit)
            {
                ResolveRoundResult(_player.SplitHand);
            }

        }

        private void ResolveRoundResult(Hand hand)
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

            OnRoundHandResult(new OnRoundHandResultArgs()
            {
                Result = result,
                Player = _player,
                Hand = hand

            });

        }

        private void ResolvePlayerHand(Hand hand)
        {
            while (!hand.IsBust)
            {
                TurnAction turnAction = OnRoundTurn(new OnRoundTurnArgs()
                {
                    Player = _player
                });

                if (turnAction == TurnAction.Hit)
                {
                    hand.AddCard(_deck.GetNextCard());
                    OnRoundHit(new OnRoundHitArgs()
                    {
                        Player = _player
                    });
                }
                else if (turnAction == TurnAction.Stay)
                {
                    OnRoundStay(new OnRoundStayArgs()
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

    public class OnRoundStartArgs
    {
        public Player Dealer { get; set; }

        public Player Player { get; set; }
    }

    public class OnRoundSplitArgs
    {
        public HumanPlayer Player { get; set; }
    }

    public class OnRoundTurnArgs
    {
        public HumanPlayer Player { get; set; }
    }

    public class OnRoundHitArgs
    {
        public Player Player { get; set; } 
    }

    public class OnRoundStayArgs
    {
        public Player Player { get; set; }
    }

    public class OnRoundBustArgs
    {
        public DealerPlayer Dealer { get; set; }

        public HumanPlayer Player { get; set; }

        public Hand BustHand { get; set; }
    }

    public class OnRoundHoleCardRevealArgs
    {
        public Player Dealer { get; set; }

        public Card HoleCard { get; set; }
    }

    public class OnRoundHandResultArgs
    {
        public Hand Hand { get; set; }

        public HandResult Result { get; set; }

        public Player Player { get; set; }
    }

    public class OnRoundEndArgs
    {
        
    }
}
