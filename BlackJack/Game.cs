﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Events;

namespace BlackJack
{
    public class Game
    {
        
        private HumanPlayer _player;
        private IDeck _deck;
        
       
        public event Func<OnRoundBetArgs, Int32> OnRoundBet;
        public event Action<OnRoundStartArgs> OnRoundStart;
        public event Func<OnRoundInsuranceArgs, InsuranceAction> OnRoundInsurance;
        public event Action<OnRoundIfInsuranceArgs> OnRoundIfInsurance;
        public event Func<OnRoundSplitArgs, SplitAction> OnRoundSplit;
        public event Action<OnRoundIfSplitArgs> OnRoundIfSplit;
        public event Func<OnRoundDoubleArgs, DoubleAction> OnRoundDouble;
        public event Action<OnRoundIfDoubleArgs> OnRoundIfDouble;
        public event Func<OnRoundTurnDecisionArgs, TurnAction> OnRoundTurnDecision;
        public event Action<OnRoundTurnStartArgs> OnRoundTurnStart;
        public event Action<OnRoundDealArgs> OnRoundDeal;
        public event Action<OnRoundStayArgs> OnRoundStay;
        public event Action<OnRoundBustArgs> OnRoundBust;
        public event Action<OnRoundHoleCardRevealArgs> OnRoundHoleCardReveal;
        public event Action<OnRoundHandResultArgs> OnRoundHandResult;
        public event Func<OnRoundEndArgs, RoundEndAction> OnRoundEnd;

        public Game(HumanPlayer player, IDeck deck)
        {
            _player = player;
            _deck = deck;
        }

        public void Start()
        {

            while (true)
            {
                int originalBet = OnRoundBet(new OnRoundBetArgs()
                {
                    Player = _player
                });
                int handBet = originalBet;
                int splitHandBet = originalBet;
                _player.Account = _player.Account - originalBet;

                Round round = new Round(_player, _deck);

                round.OnRoundStart += (ev) =>
                {
                    OnRoundStart(ev);
                };

                round.OnRoundInsurance += (ev) =>
                {
                    return OnRoundInsurance(ev);
                };

                round.OnRoundIfInsurance += (ev) =>
                {
                    OnRoundIfInsurance?.Invoke(ev);
                };

                round.OnRoundDouble += (ev) =>
                {
                    if(originalBet < _player.Account)
                    {
                        return OnRoundDouble(ev);
                    }
                    else
                    {
                        return DoubleAction.No;
                    }
                };

                round.OnRoundIfDouble += (ev) =>
                {
                    _player.Account = _player.Account - originalBet;

                    if (ev.Hand.IsSplit)
                    {
                        splitHandBet = splitHandBet * 2;
                    }
                    else
                    {
                        handBet = originalBet * 2;
                    }
                    
                    OnRoundIfDouble?.Invoke(ev);
                };

                round.OnRoundSplit += (ev) =>
                {
                    if (originalBet < _player.Account)
                    {
                        return OnRoundSplit(ev);
                    }
                    else
                    {
                        return SplitAction.No;
                    }
                 
                };

                round.OnRoundIfSplit += (ev) =>
                {
                    _player.Account = _player.Account - originalBet;
                    OnRoundIfSplit?.Invoke(ev);
                };

                round.OnRoundTurnDecision += (ev) =>
                {
                    return OnRoundTurnDecision(ev);
                };

                round.OnRoundTurnStart += (ev) =>
                {
                    OnRoundTurnStart(ev);
                };

                round.OnRoundDeal += (ev) =>
                {
                    OnRoundDeal(ev);
                };

                round.OnRoundStay += (ev) =>
                {
                    OnRoundStay(ev);
                };

                round.OnRoundBust += (ev) =>
                {
                    OnRoundBust(ev);
                };

                round.OnRoundHoleCardReveal += (ev) =>
                {
                    OnRoundHoleCardReveal(ev);
                };

                round.OnRoundHandResult += (ev) =>
                {
                    int bet = 0;
                    if(ev.Hand.IsSplit)
                    {
                        bet = splitHandBet;
                    }
                    else
                    {
                        bet = handBet;
                    }

                    if(ev.Result == HandResult.BlackJack)
                    {
                        _player.Account = _player.Account + (int)(bet * 2.5); 
                    }
                    else if (ev.Result == HandResult.Tie)
                    {
                        _player.Account = _player.Account + bet;
                    }
                    else if (ev.Result == HandResult.Win)
                    {
                         _player.Account = _player.Account + bet * 2;
                    }
                    else if (ev.Result == HandResult.InsuranceBlackJack)
                    {
                        _player.Account = bet / 2 + _player.Account;
                    }

                    OnRoundHandResult(ev);

                };

                round.Start();

                RoundEndAction roundEndAction = OnRoundEnd(new OnRoundEndArgs());

                if(roundEndAction == RoundEndAction.Quit)
                {
                    break;
                }
            }
        }
    }
}
