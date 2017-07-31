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
        //Set up each player with a bank account.
        //Multiple rounds
        //Round Payouts: BlackJack on first two cards = 3:2 win unless the Dealer also has a BlackJack then it's a tie, Win/Lose 1:1, Push/Tie - no money exchanged
        //Split becomes two separate bets half of the original bet. Each hand can win/lose or tie. Combination of the results will factor into player's total bank account.
        //NOTE:If the player's SplitHand has 21 but the dealer has a blackjack the player will still lose to the dealer in this case. 
        //End round: if yes - end game, if no repeat round.
        //End game: Total left in bank after all round/s completed - Player will be offered after each round whether to quit or continue
        
        private HumanPlayer _player;
        private IDeck _deck;

        public event Action<OnRoundStartArgs> OnRoundStart;
        public event Func<OnRoundSplitArgs, SplitAction> OnRoundSplit;
        public event Func<OnRoundTurnArgs, TurnAction> OnRoundTurn;
        public event Action<OnRoundHitArgs> OnRoundHit;
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

            while(true)
            {
                Round round = new Round(_player, _deck);

                round.OnRoundStart += (ev) =>
                {
                    OnRoundStart(ev);
                };

                round.OnRoundSplit += (ev) =>
                {
                    return OnRoundSplit(ev);
                };

                round.OnRoundTurn += (ev) =>
                {
                    return OnRoundTurn(ev);
                };

                round.OnRoundHit += (ev) =>
                {
                    OnRoundHit(ev);
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
                    OnRoundHandResult(ev);
                };

                round.Start();

                //Game.Payouts

                RoundEndAction roundEndAction = OnRoundEnd(new OnRoundEndArgs());

                if(roundEndAction == RoundEndAction.Quit)
                {
                    break;
                }
            }
        }
    }
}
