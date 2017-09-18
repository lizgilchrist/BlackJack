﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    //Surrender: Is when the dealers first card is either an Ace or a 10 value - A player who surrenders gives half their bet to the house. The round ends and a new round starts.
    //Insurance: When the dealers first card is an Ace - The player can 'take insurance' if the dealer has blackjack they will only lose half their bet.
    
    class Program
    {
        public class MockDeck : IDeck
        {
            private List<Card> _cards;

            public MockDeck(params Card[] cards)
            {
                _cards = cards.ToList();
            }

            public Card GetNextCard()
            {
                var card = _cards[0];
                _cards.RemoveAt(0);
                return card;
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;


            Game game = new Game(
                new HumanPlayer(Console.ReadLine(), 500),
                new MockDeck(
                    new Card(Suit.Diamonds, Face.Eight),
                    new Card(Suit.Clubs, Face.Eight),
                    new Card(Suit.Clubs, Face.Ace),
                    new Card(Suit.Clubs, Face.Queen),
                    new Card(Suit.Hearts, Face.Ten),
                    new Card(Suit.Hearts, Face.Jack),
                    new Card(Suit.Diamonds, Face.Eight)
                    ));

            game.OnRoundBet += (ev) =>
            {
                Console.WriteLine("How much would you like to bet? (1 - " + ev.Player.Account + ")");
                int playerBet = 0;
                try
                {
                    playerBet = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    
                }
                
                while(playerBet > ev.Player.Account || playerBet < 1)
                {
                    Console.WriteLine("Sorry that's an invalid entry. Please try again.");
                    try
                    {
                        playerBet = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (FormatException)
                    {

                    }
                }

                return playerBet;

            };
            
            game.OnRoundStart += (ev) =>
            {
                Console.WriteLine("The round has started!");
                Console.WriteLine();
                PrintHand(ev.Player, ev.Player.Hand);
                Console.WriteLine("The total is " + ev.Player.Hand.Value);
                Console.WriteLine();
                PrintHand(ev.Dealer, ev.Dealer.Hand);
                Console.WriteLine("The total is " + ev.Dealer.Hand.Value);
            };

            game.OnRoundInsurance += (ev) =>
            {
                Console.WriteLine();
                Console.WriteLine("Would you like to take insurance?");
                string userInput = Console.ReadLine().ToLower();

                while (userInput != "yes" &&userInput != "no")
                {
                    Console.WriteLine("Sorry, that's not a valid answer. Please try again.");
                    userInput = Console.ReadLine().ToLower();
                }

                if(userInput == "yes")
                {
                    return InsuranceAction.Yes;
                }
                else
                {
                    return InsuranceAction.No;
                }
            };

            game.OnRoundDouble += (ev) =>
            {
                Console.WriteLine();
                Console.WriteLine("Would you like to double down?");
                string userInput = Console.ReadLine().ToLower();

                while (userInput != "yes" && userInput != "no")
                {
                    Console.WriteLine("Sorry, that's not a valid answer. Please try again.");
                    userInput = Console.ReadLine().ToLower();
                }

                if (userInput == "yes")
                {
                    return DoubleAction.Yes;
                }
                else
                {
                    return DoubleAction.No;
                }
                
            };

            game.OnRoundIfInsurance += (ev) =>
            {
                Console.WriteLine("You've taken insurance! This means you've forfeited half of your original bet! Lets hope the dealer has BlackJack!");
            };

            game.OnRoundIfDouble += (ev) =>
            {
                Console.WriteLine("You doubled your bet! Your account balance is now: " + ev.Player.Account);
            };


            game.OnRoundSplit += (ev) =>
            {
                Console.WriteLine();
                Console.WriteLine("Would you like to split?");
                string userInput = Console.ReadLine().ToLower();

                while (userInput != "yes" && userInput != "no")
                {
                    Console.WriteLine("Sorry, that's not a valid answer. Please try again.");
                    userInput = Console.ReadLine().ToLower();
                }

                if (userInput == "yes")
                {
                    return SplitAction.Yes;
                }
                else
                {
                    return SplitAction.No;
                }

            };

            game.OnRoundIfSplit += (ev) =>
            {
                Console.WriteLine("You doubled your bet! Your account balance is now: " + ev.Player.Account);
            };

            game.OnRoundTurnStart += (ev) =>
            {
                string handName = null;
                if (ev.Hand.IsSplit)
                {
                    handName = "-- == SECOND HAND == --";
                }
                else
                {
                    handName = "-- == FIRST HAND == --";
                }

                if (ev.Player.IsSplit)
                {
                    Console.WriteLine();
                    Console.WriteLine(handName);
                }

            };

            game.OnRoundTurnDecision += (ev) =>
            {
                Console.WriteLine();
                Console.WriteLine("Please choose 'Hit' or 'Stay'? ");
                string userInput = Console.ReadLine().ToLower();
                
                while (userInput != "hit" && userInput != "stay")
                {
                    Console.WriteLine("Sorry, that's not a valid answer. Please try again.");
                    userInput = Console.ReadLine().ToLower();
                }

                if (userInput == "hit")
                {
                    return TurnAction.Hit;
                }
                else
                {
                    return TurnAction.Stay;
                }
            };

            game.OnRoundDeal += (ev) =>
            {
                if (ev.Player != null)
                {
                    Console.WriteLine();
                    PrintHand(ev.Player, ev.Hand);
                    string handName = null;
                    if (!ev.Player.IsSplit)
                    {
                        handName = "hand";
                    }

                    else if (ev.Hand.IsSplit)
                    {
                        handName = "second hand";
                    }
                    else
                    {
                        handName = "first hand";
                    }
                    Console.WriteLine("The total for " + ev.Player.Name + "'s " + handName + " is now " + ev.Hand.Value);
                }
                else
                {
                    PrintHand(ev.Dealer, ev.Hand);
                    Console.WriteLine("The total for " + ev.Dealer.Name + "'s hand stays as " + ev.Hand.Value);
                }
            };

            game.OnRoundStay += (ev) =>
            {
                if (ev.Player != null)
                {
                    string handName = null;
                    if (!ev.Player.IsSplit)
                    {
                        handName = "hand";
                    }
                
                    else if (ev.Hand.IsSplit)
                    {
                        handName = "second hand";
                    }
                    else
                    {
                        handName = "first hand";
                        
                    }
                    Console.WriteLine("The total for " + ev.Player.Name + "'s " + handName + " stays as " + ev.Hand.Value);
                }
                else
                {
                    Console.WriteLine("The total for " + ev.Dealer.Name + "'s hand stays as " + ev.Hand.Value);
                }
                    
            };

            game.OnRoundBust += (ev) =>
            {
                if (ev.Player == null)
                {
                    Console.WriteLine(ev.Dealer.Name + "'s hand is BUST!");
                    
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
                Console.WriteLine();
                PrintHand(ev.Dealer, ev.Dealer.Hand);
                
            };

            game.OnRoundHandResult += (ev) =>
            {
                if(!ev.Player.IsSplit)
                {
                    Console.WriteLine();
                    Console.WriteLine("The result for your hand is...");
                }
                else
                {
                    string handName = null;
                    if (ev.Hand.IsSplit)
                    {
                        handName = "second hand";
                    }
                    else
                    {
                        handName = "first hand";
                    }
                    Console.WriteLine();
                    Console.WriteLine("The result for your " + handName + " is...");
                }

                if (ev.Result == HandResult.Tie)
                {
                    Console.WriteLine("It's a tie! Your account balance is now " + ev.Player.Account);
                }
                else if(ev.Result == HandResult.Win)
                {
                    Console.WriteLine(ev.Player.Name + " Win's! Your account balance is now " + ev.Player.Account);
                }
                else if(ev.Result == HandResult.Lose)
                {
                    Console.WriteLine(ev.Player.Name + " Lost. Your account balance is now " + ev.Player.Account);
                }
                else if(ev.Result == HandResult.BlackJack)
                {
                    Console.WriteLine(ev.Player.Name + " WIN'S BLACKJACK!! Your account balance is now " + ev.Player.Account);
                }
                else if(ev.Result == HandResult.InsuranceBlackJack)
                {
                    Console.WriteLine(ev.Player.Name + ", the dealer did have BlackJack!! Your account balance is now " + ev.Player.Account);
                }

            };

            game.OnRoundEnd += (ev) =>
            {
                Console.WriteLine();
                Console.WriteLine("Do you wish to start a new round?");
                string userInput = Console.ReadLine().ToLower();
                

                while (userInput != "yes" && userInput != "no")
                {
                    Console.WriteLine("Sorry, that's not a valid answer. Please try again.");
                    userInput = Console.ReadLine().ToLower();
                }

                if (userInput == "yes")
                {
                   return RoundEndAction.Continue;
                }
                else
                {
                    return RoundEndAction.Quit;
                }
            };

            game.Start();
           
        }

        private static void PrintHand(Player player, Hand hand)
        {
            string handName = null;
            if (player is HumanPlayer)
            {
                HumanPlayer humanPlayer = (HumanPlayer)player;

                if (hand.IsSplit)
                {
                    handName = "other hand";
                }
                else
                {
                    handName = "hand";
                }
            }
            else
            {
                handName = "hand";
            }

            string result = $"{player.Name}'s " + handName + " is: ";
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
