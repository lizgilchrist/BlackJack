using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    //Doubling down: Is allowed straight after the first two cards are dealt - The player will get one more card. They cannot ask for any more hits after this third card.
    //Split: If you two cards with the same value then you can split them into two separate hands - You will get hit once with both hands.
    //Surrender: Is when the dealers first card is either an Ace or a 10 value - A player who surrenders gives half their bet to the house. The round ends and a new round starts.
    //Insurance: When the dealers first card is an Ace - The player can 'take insurance' against the chance that the dealer has blackjack.
    //Push: It's a tie
    //Payouts:
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Game game = new Game(new HumanPlayer(Console.ReadLine()));

            game.OnGameStart += (ev) =>
            {
                Console.WriteLine("Let the game begin!");
                PrintHand(ev.Player);
                Console.WriteLine("The total for " + ev.Player.Name + "'s hand is " + ev.Player.Hand.Value);

                PrintHand(ev.Dealer);
                Console.WriteLine("The total for the " + ev.Dealer.Name + "'s hand is " + ev.Dealer.Hand.Value);
            };

            game.OnGameTurn += (ev) =>
            {
                Console.WriteLine("Please choose 'Hit' or 'Stay'? ");
                string userInput = Console.ReadLine();

                if (userInput == "Hit")
                {
                    return TurnAction.Hit;
                }
                else if (userInput == "Stay")
                {
                    return TurnAction.Stay;
                }

                throw new Exception("TODO: Need to handle bad input from user");
            };

            game.OnGameHit += (ev) =>
            {
                PrintHand(ev.Player);
                Console.WriteLine("The total for " + ev.Player.Name + "'s hand now is " + ev.Player.Hand.Value);
            };

            game.OnGameStay += (ev) =>
            {
                Console.WriteLine("The total for " + ev.Player.Name + "'s hand stays as " + ev.Player.Hand.Value);
            };

            game.OnGameEnd += (ev) =>
            {
                Console.WriteLine(ev.Player.Name + "'s hand is BUST!");
            };

            game.OnGameHoleCardReveal += (ev) =>
            {
                PrintHand(ev.Dealer);
                Console.WriteLine("The total for " + ev.Dealer.Name + "'s hand is " + ev.Dealer.Hand.Value);
            };

            game.Start();

            Deck deck = new Deck();

            HumanPlayer human = new HumanPlayer(Console.ReadLine());
            human.Hand = new Hand();
            human.Hand.AddCard(deck.GetNextCard());
            human.Hand.AddCard(deck.GetNextCard());
            PrintHand(human);
            Console.WriteLine("The total for " + human.Name + "'s hand is " + human.Hand.Value);

            DealerPlayer dealer = new DealerPlayer();
            dealer.Hand = new Hand();
            dealer.Hand.AddCard(deck.GetNextCard());
            PrintHand(dealer);
            Console.WriteLine("The total for the " + dealer.Name + "'s hand is " + dealer.Hand.Value);

            //Hit or Stay? - Player

            while (!human.Hand.IsBust)
            {
                Console.WriteLine("Please choose 'Hit' or 'Stay'? ");
                string userInput = Console.ReadLine();

                if (userInput == "Hit")
                {
                    human.Hand.AddCard(deck.GetNextCard());
                    PrintHand(human);
                    Console.WriteLine("The total for " + human.Name + "'s hand now is " + human.Hand.Value);
                }
                else if (userInput == "Stay")
                {
                    Console.WriteLine("The total for " + human.Name + "'s hand stays as " + human.Hand.Value);
                    break;
                }
            }

            if (human.Hand.IsBust)
            {
                Console.WriteLine(human.Name + "'s hand is BUST! Dealer wins");
                return;
            }

            //Dealers turn

            dealer.Hand.AddCard(deck.GetNextCard());
            PrintHand(dealer);
            Console.WriteLine("The total for " + dealer.Name + "'s hand is " + dealer.Hand.Value);

            if (dealer.Hand.Value >= 17)
            {
                Console.WriteLine("The total for the " + dealer.Name + "'s hand stays as " + dealer.Hand.Value);
            }

            while (dealer.Hand.Value < 17)
            {
                dealer.Hand.AddCard(deck.GetNextCard());
                PrintHand(dealer);
                Console.WriteLine("The total for the " + dealer.Name + "'s hand now is " + dealer.Hand.Value);
            }

            //NOTE: Dealer's hand can go bust. Use OnGameEnd 

            //Compare results

            if (!dealer.Hand.IsBust || !human.Hand.IsBust)
            {
                if (dealer.Hand.Value < human.Hand.Value)
                {
                    Console.WriteLine(human.Name + " wins!");
                }
                else if (dealer.Hand.Value == human.Hand.Value)
                {
                    Console.WriteLine("It's a tie!");
                }
                else
                {
                    Console.WriteLine(dealer.Name + " wins!");
                }
            }

        }

        private static void PrintHand(Player player)
        {
            string result = $"{player.Name}'s hand is: ";
            IEnumerable<Card> cards = player.Hand.GetCards();
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

        //"♣ ♦ ♥ ♠"
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
