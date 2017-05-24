﻿using System;
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
    //"♣ ♦ ♥ ♠"
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            //Create a new Deck and print out the result of GetNextCard
            Deck deck = new Deck();
            //Card card = deck.GetNextCard();

            Hand lizHand = new Hand();
            lizHand.AddCard(deck.GetNextCard());
            lizHand.AddCard(deck.GetNextCard());
            PrintHand("Liz", lizHand);

            Console.WriteLine("The total for Liz's hand is " + lizHand.Value);

            Hand dealer = new Hand();
            dealer.AddCard(deck.GetNextCard());
            //Show the player just one card from the dealer
            PrintHand("Dealer", dealer);

            Console.WriteLine("The total for Dealer's hand is " + dealer.Value);

            //Hit or Stay? - Player

            while (!lizHand.IsBust)
            {
                Console.WriteLine("Please choose 'Hit' or 'Stay'? ");
                string userInput = Console.ReadLine();

                if (userInput == "Hit")
                {
                    lizHand.AddCard(deck.GetNextCard());
                    PrintHand("Liz", lizHand);
                    Console.WriteLine("The total for Liz's hand now is " + lizHand.Value);
                }
                else if (userInput == "Stay")
                {
                    Console.WriteLine("The total for Liz's hand stays as " + lizHand.Value);
                    break;
                }
            }

            if (lizHand.IsBust)
            {
                Console.WriteLine("Liz's hand is BUST! Dealer wins");
                return;
            }

            //Dealers turn

            dealer.AddCard(deck.GetNextCard());
            PrintHand("Dealer", dealer);
            Console.WriteLine("The total for Dealer's hand is " + dealer.Value);

            if (dealer.Value >= 17)
            {
                Console.WriteLine("The total for the dealer's hand stays as " + dealer.Value);
            }

            while (dealer.Value < 17)
            {
                dealer.AddCard(deck.GetNextCard());
                PrintHand("Dealer", dealer);
                Console.WriteLine("The total for the Dealer's hand now is " + dealer.Value);
            }

            //Compare results

            if (!dealer.IsBust || !lizHand.IsBust)
            {
                if (dealer.Value < lizHand.Value)
                {
                    Console.WriteLine("Liz wins!");
                }
                else if (dealer.Value == lizHand.Value)
                {
                    Console.WriteLine("It's a tie!");
                }
                else
                {
                    Console.WriteLine("Dealer wins!");
                }
            }

            //Stack<Card> cards = new Stack<Card>();
            //Stack<Card> someOtherCards = new Stack<Card>();
            //cards.Push(card);

        }

        private static void PrintHand(string playerName, Hand hand)
        {
            string result = $"{playerName}'s hand is: ";
            IEnumerable<Card> cards = hand.GetCards();
            if (cards.Count() == 0)
            {
                result += "empty";
            }
            else
            {
                List<string> cardStrings = new List<string>();
                foreach (Card card in cards)
                {
                    cardStrings.Add(GetCardString(card));
                }
                result += string.Join(", ", cardStrings);
            }
            Console.WriteLine(result);
        }
       
        private static string GetCardString(Card card)
        {
            string suit = null;

            if (card.Suit == Suit.Clubs)
            {   
                suit = "♣";
            }
            else if (card.Suit == Suit.Diamonds)
            {
                suit = "♦";
            }   
            else if (card.Suit == Suit.Hearts)
            {
                suit = "♥";
            }
            else if (card.Suit == Suit.Spades)
            {
                suit = "♠";
            }

            string face = null;

            if (card.Face == Face.Ace || card.Face == Face.Jack || card.Face == Face.Queen || card.Face == Face.King)
            {
                face = card.Face.ToString().First().ToString();
            }
            else
            {
                face = ((int)card.Face).ToString();
            }

            return String.Format("{0} {1}", face, suit);
        }
    }
}
