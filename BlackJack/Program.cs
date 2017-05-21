using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    //Must get to as close as possible to 21. If over 21 you're out. 
    //Start with 2 cards only
    //King, Queen and Jack are worth 10 points each and numbered cards retain their face value 2-10 Aces can be either 1 point or 11 points
    //Make a decision to 'Hit' or 'Stay'. Hit is to request another card from the dealer or stay by not picking up
    //Doubling down: Is allowed staright after the first two cards are dealt - The player will receive one more card and stay. They cannot ask for any more hits after this third card
    //Split: If you two cards have the same value then you can split them into two separate hands - You will get hit once both hands before staying.
    //Surrender: Is when the dealers upcard is either an Ace or a 10 value - A player who surrenders gives half their bet to the house. The round ends and a new round starts.
    //Insurance: When the dealers upcard is an Ace - A player can 'take insurance' against the chance that the dealer has blackjack.
    //Push: If the dealer and the player come up with the same total - it's a tie
    //Payouts:
    class Program
    {
        static void Main(string[] args)
        {
            //Create a new Deck and print out the result of GetNextCard
            Deck deck = new Deck();
            Card card = deck.GetNextCard();
            
            Hand lizHand = new Hand();
            lizHand.AddCard(card);
            int value = lizHand.Value;
            // Add card to hand here

            Stack<Card> cards = new Stack<Card>();
            Stack<Card> someOtherCards = new Stack<Card>();
            cards.Push(card);
           
        }
    }
}
