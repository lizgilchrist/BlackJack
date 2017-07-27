using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    //Doubling down: Is allowed straight after the first two cards are dealt - The player will get one more card. They cannot ask for any more hits after this third card.
    //Surrender: Is when the dealers first card is either an Ace or a 10 value - A player who surrenders gives half their bet to the house. The round ends and a new round starts.
    //Insurance: When the dealers first card is an Ace - The player can 'take insurance' against the chance that the dealer has blackjack.
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Game game = new Game(
                new HumanPlayer(Console.ReadLine()),
                new Deck());

            game.OnRoundStart += (ev) =>
            {
                Console.WriteLine("The round has started!");
                PrintHand(ev.Player);
                Console.WriteLine("The total for " + ev.Player.Name + "'s hand is " + ev.Player.Hand.Value);

                PrintHand(ev.Dealer);
                Console.WriteLine("The total for the " + ev.Dealer.Name + "'s hand is " + ev.Dealer.Hand.Value);
            };

            game.OnRoundSplit += (ev) =>
            {
                Console.WriteLine("Would you like to split?");
                string userInput = Console.ReadLine();

                if (userInput == "Yes")
                {
                    return SplitAction.Yes;
                }
                else if (userInput == "No")
                {
                    return SplitAction.No;
                }

                throw new Exception("TODO: Need to handle bad input from user");
            };

            game.OnRoundTurn += (ev) =>
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

            game.OnRoundHit += (ev) =>
            {
                PrintHand(ev.Player);
                Console.WriteLine("The total for " + ev.Player.Name + "'s hand now is " + ev.Player.Hand.Value);
            };

            game.OnRoundStay += (ev) =>
            {
                Console.WriteLine("The total for " + ev.Player.Name + "'s hand stays as " + ev.Player.Hand.Value);
            };

            game.OnRoundBust += (ev) =>
            {
                if (ev.Player == null)
                {
                    Console.WriteLine(ev.Dealer + "'s hand is BUST!");
                }
                if (ev.Dealer == null)
                {
                    if(!ev.Player.IsSplit)
                    {
                        Console.WriteLine(ev.Player.Name + "'s hand is BUST!");
                    }
                    else
                    {
                        if(ev.BustHand == ev.Player.Hand)
                        {
                            Console.WriteLine(ev.Player.Name + "'s first hand is BUST!");
                        }
                        else
                        {
                            Console.WriteLine(ev.Player.Name + "'s second hand is BUST!");
                        }
                    }
                }
                
            };

            game.OnRoundHoleCardReveal += (ev) =>
            {
                PrintHand(ev.Dealer);
                Console.WriteLine("The total for " + ev.Dealer.Name + "'s hand is " + ev.Dealer.Hand.Value);
            };

            game.OnRoundHandResult += (ev) =>
            {
                if(ev.Result == HandResult.Tie)
                {
                    Console.WriteLine("It's a tie!");
                }
                else if(ev.Result == HandResult.Win)
                {
                    Console.WriteLine(ev.Player.Name + "Win's");
                }
                else if(ev.Result == HandResult.Lose)
                {
                    Console.WriteLine(ev.Player.Name + "Lost");
                }

                throw new Exception("TODO: Need to handle bad input from user");

            };

            game.OnRoundEnd += (ev) =>
            {
                Console.WriteLine("Round is over");
            };

            game.Start();
           
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
