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
         
        //NOTE:If the player's SplitHand has 21 but the dealer has a blackjack the player will still lose to the dealer in this case. 
        
        private HumanPlayer _player;
        private IDeck _deck;
        
       
        public event Func<OnRoundBetArgs, Int32> OnRoundBet;
        public event Action<OnRoundStartArgs> OnRoundStart;
        public event Func<OnRoundSplitArgs, SplitAction> OnRoundSplit;
        public event Action<OnRoundIfSplitArgs> OnRoundIfSplit;
        public event Func<OnRoundDoubleArgs, DoubleAction> OnRoundDouble;
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
            int playerBet = OnRoundBet(new OnRoundBetArgs()
            {
                Player = _player
            });
            _player.Account =_player.Account - playerBet;

            while (true)
            {
                Round round = new Round(_player, _deck);

                round.OnRoundStart += (ev) =>
                {
                    OnRoundStart(ev);
                };

                round.OnRoundDouble += (ev) =>
                {
                    _player.Account = _player.Account - playerBet;
                    return OnRoundDouble(ev);
                };

                round.OnRoundSplit += (ev) =>
                {
                    return OnRoundSplit(ev);
                };

                round.OnRoundIfSplit += (ev) =>
                {
                    _player.Account = _player.Account - playerBet;
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
                    if(ev.Result == HandResult.BlackJack)
                    {
                        _player.Account = _player.Account + (int)(playerBet * 2.5); 
                    }
                    else if (ev.Result == HandResult.Tie)
                    {
                        _player.Account = _player.Account + playerBet;
                    }
                    else if (ev.Result == HandResult.Win)
                    {
                         _player.Account = _player.Account + playerBet * 2;
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
