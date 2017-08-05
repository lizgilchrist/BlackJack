using BlackJack.Events;
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
        public event Action<OnRoundIfSplitArgs> OnRoundIfSplit;
        public event Func<OnRoundDoubleArgs, DoubleAction> OnRoundDouble;
        public event Action<OnRoundIfDoubleArgs> OnRoundIfDouble;
        public event Action<OnRoundTurnStartArgs> OnRoundTurnStart;
        public event Func<OnRoundTurnDecisionArgs, TurnAction> OnRoundTurnDecision;
        public event Action<OnRoundDealArgs> OnRoundDeal;
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

            if (_player.Hand.Value == 21)
            {
                if(_dealer.Hand.Value < 10)
                {
                    OnRoundHandResult(new OnRoundHandResultArgs()
                    {
                        Hand = _player.Hand,
                        Player = _player,
                        Result = HandResult.BlackJack
                    });    
                }
            }

            List<Card> cards = _player.Hand.GetCards();
            if (cards[0].Face == cards[1].Face)
            {
                SplitAction splitAction = OnRoundSplit(new OnRoundSplitArgs()
                {
                    Player = _player
                });

                if (splitAction == SplitAction.Yes)
                {
                    OnRoundIfSplit(new OnRoundIfSplitArgs()
                    {
                        Player = _player
                    });

                    _player.SplitHand = _player.Hand.Split();
                    
                    _player.Hand.AddCard(_deck.GetNextCard());
                    OnRoundDeal(new OnRoundDealArgs()
                    {
                        Player = _player,
                        Hand = _player.Hand
                    });

                    _player.SplitHand.AddCard(_deck.GetNextCard());
                    OnRoundDeal(new OnRoundDealArgs()
                    {
                        Player = _player,
                        Hand = _player.SplitHand
                    });
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

            if(_dealer.Hand.Value == 21 && _player.Hand.Value == 21)
            {
                OnRoundHandResult(new OnRoundHandResultArgs()
                {
                    Hand = _player.Hand,
                    Player = _player,
                    Result = HandResult.Tie
                });
            }
            else if(_dealer.Hand.Value == 21)
            {
                OnRoundHandResult(new OnRoundHandResultArgs()
                {
                    Hand = _player.Hand,
                    Player = _player,
                    Result = HandResult.Lose
                });
            }
            
            while (_dealer.Hand.Value < 17 && _dealer.Hand.Value < _player.Hand.Value)
            {
                _dealer.Hand.AddCard(_deck.GetNextCard());
                OnRoundDeal(new OnRoundDealArgs()
                {
                    Dealer = _dealer,
                    Hand = _dealer.Hand
                });

            }

            if (!_dealer.Hand.IsBust)
            {
                OnRoundStay(new OnRoundStayArgs()
                {
                    Dealer = _dealer,
                    Hand = _dealer.Hand
                });
            }

            else
            {
                OnRoundBust(new OnRoundBustArgs()
                {
                    Dealer = _dealer,
                    BustHand = _dealer.Hand,
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

            if (hand.IsBust)
            {
                result = HandResult.Lose;
            }
            else if(_dealer.Hand.IsBust)
            {
                result = HandResult.Win;
            }

            if(!hand.IsBust && !_dealer.Hand.IsBust)
            {
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
            OnRoundTurnStart(new OnRoundTurnStartArgs()
            {
                Player = _player,
                Hand = hand 
            });

            bool isDouble = false;

            if (hand.Value <= 11)
            {
                DoubleAction doubleAction = OnRoundDouble(new OnRoundDoubleArgs()
                {
                    Player = _player
                });

                if (doubleAction == DoubleAction.Yes)
                {
                    OnRoundIfDouble(new OnRoundIfDoubleArgs()
                    {
                        Player = _player
                    });

                    isDouble = true;
                    _player.Hand.AddCard(_deck.GetNextCard());
                    OnRoundDeal(new OnRoundDealArgs()
                    {
                        Player = _player,
                        Hand = _player.Hand
                    });
                }
            }

            while (!hand.IsBust && !isDouble)
            {
                TurnAction turnAction = OnRoundTurnDecision(new OnRoundTurnDecisionArgs()
                {
                    Player = _player
                });

                if (turnAction == TurnAction.Hit)
                {
                    hand.AddCard(_deck.GetNextCard());
                    OnRoundDeal(new OnRoundDealArgs()
                    {
                        Player = _player,
                        Hand = hand
                    });
                }
                else if (turnAction == TurnAction.Stay)

                {
                    OnRoundStay(new OnRoundStayArgs()
                    {
                        Player = _player,
                        Hand = hand
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
}
