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
        private int numberOfPlayers, cardsPerPlayer, round;
        private List<Player> playerList = new List<Player>();
        private Deck deck;

        private bool isGameRunning = true;

        public void Run()
        {
            while (isGameRunning)
            {
                Console.Clear();
                Header("Card Battle", "", ConsoleColor.Red);
                Console.WriteLine();
                Play();
            }
        }

        private void Play()
        {
            AskForNumberOfPlayers();

            playerList.Clear();

            //Add players
            for(int i = 1; i <= numberOfPlayers; i++)
                playerList.Add(new Player($"Player{i}"));

            deck = new Deck();
            deck.Shuffle();

            DealCards();

            round = 1;

            do
            {
                Console.WriteLine("Press any key to show the cards\n");
                Console.ReadKey();

                Showdown(playerList);
                round++;
            } while (round < cardsPerPlayer);

            Player winner = playerList.First();

            foreach (Player player in playerList)
            {
                if(player.Deck.GetNumberOfCards() > winner.Deck.GetNumberOfCards())
                    winner = player;
            }

            Console.WriteLine($"{winner} WON THE GAME!!!");

            isGameRunning = AskToContinue("Do you want to play again?");
        }

        private void DealCards()
        {
            cardsPerPlayer = deck.GetNumberOfCards() / numberOfPlayers;
            
            foreach(Player player in playerList)
            {
                List<Card> cards = new List<Card>();

                for(int i = 0; i < cardsPerPlayer; i++)
                    cards.Add(deck.Draw());

                player.Deck = new Deck(cards);
            }

            Console.WriteLine("Cards have been dealt");
        }

        private void Showdown(List<Player> playersToPlay, List<Card> cardsAdded = null)
        {
            Dictionary<Player, Card> cardPlayerDic = new Dictionary<Player, Card>();

            foreach (Player player in playersToPlay)
            {
                Card card = player.Deck.Draw();
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
            {
                Showdown(winners);
                round++;
            }
            else
            {
                Player winner = winners[0];
                winner.Deck.AddCards(cardsInGame);
                Console.WriteLine($"{winner} won this round\n");
            }
            
        }

        private void CheckAndShowPlayersCards()
        {
            foreach (Player player in playerList)
            {
                int numberOfCards = player.Deck.GetNumberOfCards();
                Console.WriteLine($"{player}: {numberOfCards} cards");
                if(numberOfCards == 0)
                    playerList.Remove(player);
            }
        }

        private void AskForNumberOfPlayers()
        {
            do
            {
                Console.WriteLine("Enter the number of players (between 2 and 5)");
            } while (!int.TryParse(Console.ReadLine(), out numberOfPlayers) || numberOfPlayers < 2 || numberOfPlayers > 5);
        }

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
        }

        private static string CenterText(string content, string decorationString = "", int windowWidthSize = 90)
        {
            int windowWidth = windowWidthSize - (2 * decorationString.Length);
            return String.Format(decorationString + "{0," + ((windowWidth / 2) + (content.Length / 2)) + "}{1," + (windowWidth - (windowWidth / 2) - (content.Length / 2) + decorationString.Length) + "}", content, decorationString);
        }
    }
}
