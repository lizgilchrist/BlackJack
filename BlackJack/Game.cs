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
        //
        private Player _player;

        public Game(Player player)
        {
            _player = player;
        }

        public event Action<OnGameStartArgs> OnGameStart;
        public event Action OnGameTurn;

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
                Human = _player,
                Dealer = dealer
            });

            

        }

    }

    public class OnGameStartArgs
    {
        public Player Dealer { get; set; }

        public Player Human { get; set; }
    }

    
}
