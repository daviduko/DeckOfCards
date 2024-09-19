using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards
{
    internal class Game
    {
        private int numberOfPlayers, cardsPerPlayer;
        private List<Player> playerList = new List<Player>();
        private Deck deck;

        public void Run()
        {
            do
            {
                Console.Clear();
                Header("Card Battle", "", ConsoleColor.Red);
            } while (Play());
        }

        /// <summary>
        ///╔=====================================================================================================╗
        ///║                                              Title                                                  ║
        ///║                                             Subtitle                                                ║
        ///╚=====================================================================================================╝
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subtitle"></param>
        /// <param name="foreGroundColor"></param>
        private static void Header(string title, string subtitle = "", ConsoleColor foreGroundColor = ConsoleColor.White)
        {
            int windowWidthSize = Console.WindowWidth;
            Console.Title = title + (subtitle != "" ? " - " + subtitle : "");
            string titleContent = CenterText(title, "║", windowWidthSize);
            string subtitleContent = CenterText(subtitle, "║", windowWidthSize);
            string borderLine = new String('═', windowWidthSize - 2);

            Console.ForegroundColor = foreGroundColor;
            Console.WriteLine($"╔{borderLine}╗");
            Console.WriteLine(titleContent);
            if (!string.IsNullOrEmpty(subtitle))
            {
                Console.WriteLine(subtitleContent);
            }
            Console.WriteLine($"╚{borderLine}╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static string CenterText(string content, string decorationString = "", int windowWidthSize = 90)
        {
            int windowWidth = windowWidthSize - (2 * decorationString.Length);
            return String.Format(decorationString + "{0," + ((windowWidth / 2) + (content.Length / 2)) + "}{1," + (windowWidth - (windowWidth / 2) - (content.Length / 2) + decorationString.Length) + "}", content, decorationString);
        }

        /// <summary>
        /// Method that sets up the game and loops every round 
        /// </summary>
        /// <returns>Boolean to play again</returns>
        private bool Play()
        {
            InitGame();

            do
            {
                Console.WriteLine("\nPress any key to show the cards\n");
                Console.ReadKey();

                Showdown(playerList);
                CheckAndShowPlayersCards();

            } while (playerList.Count > 1);

            Player winner = playerList.First();

            Console.WriteLine($"{winner} WON THE GAME!!!");

            return AskToContinue("Do you want to play again?");
        }

        /// <summary>
        /// Creates players list and the deck, then deals cards to the players
        /// </summary>
        private void InitGame()
        {
            AskForNumberOfPlayers();

            playerList.Clear();

            //Add players
            for (int i = 1; i <= numberOfPlayers; i++)
                playerList.Add(new Player($"Player{i}"));

            deck = new Deck();
            deck.Shuffle();

            DealCards();
        }

        /// <summary>
        /// Creates a deck for each player and adds the cards to it
        /// </summary>
        private void DealCards()
        {
            cardsPerPlayer = deck.GetNumberOfCards() / numberOfPlayers;
            
            foreach(Player player in playerList)
            {
                List<Card> cards = new List<Card>();

                for(int i = 0; i < cardsPerPlayer; i++)
                    cards.Add(deck.DrawCard());

                player.Deck = new Deck(cards);
            }

            Console.WriteLine("Cards have been dealt");
        }

        /// <summary>
        /// Draws the cards of each player that plays that round
        /// </summary>
        /// <param name="playersToPlay">List of players of the round, in case there's a tie, there's and exgtra round for the players 
        /// that won the round with the same card number</param>
        /// <param name="cardsAdded">Cards that the winner will add to his deck</param>
        private void Showdown(List<Player> playersToPlay, List<Card> cardsAdded = null)
        {
            Dictionary<Player, Card> cardPlayerDic = new Dictionary<Player, Card>();

            foreach (Player player in playersToPlay)
            {
                Card card = player.Deck.DrawCard();
                cardPlayerDic.Add(player, card);
                Console.WriteLine($"{player.Name}: {card}");
            }

            Console.WriteLine("-------------");

            if(cardsAdded != null)
            {
                List<Card> totalCards = cardPlayerDic.Values.ToList();
                totalCards.AddRange(cardsAdded);
                CheckWinner(cardPlayerDic, totalCards);
            }
            else
                CheckWinner(cardPlayerDic, cardPlayerDic.Values.ToList());
        }

        /// <summary>
        /// Checks the winner, if there's more than one, calls ShowDown() again
        /// </summary>
        /// <param name="cardPlayerDic">Dictionary with the player and card</param>
        /// <param name="cardsInGame">cards that will be added to the winners deck</param>
        private void CheckWinner(Dictionary<Player, Card> cardPlayerDic, List<Card> cardsInGame)
        {
            int maxValue = 1;
            List<Player> winners = new List<Player>();

            foreach (KeyValuePair<Player, Card> kvp in cardPlayerDic)
            {
                if(winners.Count == 0)
                {
                    maxValue = kvp.Value.Number;
                    winners.Add(kvp.Key);
                }
                else
                {
                    if(kvp.Value.Number > maxValue)
                    {
                        maxValue = kvp.Value.Number;
                        winners.Clear();
                        winners.Add(kvp.Key);
                    }
                    else if(kvp.Value.Number == maxValue) 
                        winners.Add(kvp.Key);
                }
            }
            
            if(winners.Count > 1)
                Showdown(winners, cardsInGame);
            else
            {
                Player winner = winners[0];
                winner.Deck.AddCards(cardsInGame);
                Console.WriteLine($"{winner} won this round\n");
            }
        }

        /// <summary>
        /// Shows at console players cards and checks if a player has no cards and must be removed from playerList
        /// </summary>
        private void CheckAndShowPlayersCards()
        {
            List<Player> playersToRemove = new List<Player>();

            Console.WriteLine("Cards:");

            foreach (Player player in playerList)
            {
                int numberOfCards = player.Deck.GetNumberOfCards();
                Console.WriteLine($" {player}: {numberOfCards} cards");
                if(numberOfCards == 0)
                    playersToRemove.Add(player);
            }

            playerList = playerList.Except(playersToRemove).ToList();
        }

        /// <summary>
        /// Asks for number of players making sure its not above 5 or under 2
        /// </summary>
        private void AskForNumberOfPlayers()
        {
            do
            {
                Console.WriteLine("Enter the number of players (between 2 and 5)");
            } while (!int.TryParse(Console.ReadLine(), out numberOfPlayers) || numberOfPlayers < 2 || numberOfPlayers > 5);
        }

        /// <summary>
        /// Asks if you want to continue regarding the question
        /// </summary>
        /// <param name="question">Question that is made</param>
        /// <returns>Boolean: true => yes, false => no</returns>
        private bool AskToContinue(string question)
        {
            string answer;
            bool keep = true;
            do
            {
                Console.WriteLine($"{question} (y/n)");
                answer = Console.ReadLine();
            } while (answer != "y" && answer != "n");

            if (answer == "n")
                keep = false;

            return keep;
        }
        
    }
}
