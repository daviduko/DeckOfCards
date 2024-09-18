using System;
using System.Collections.Generic;
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

            //Add players
            for(int i = 1; i <= numberOfPlayers; i++)
                playerList.Add(new Player($"Player{i}"));

            deck = new Deck();
            deck.Shuffle();

            DealCards();

            round = 1;

            do
            {
                Console.WriteLine("Press any key to show the cards");
                Console.ReadKey();

                Showdown(playerList);
            } while (round < cardsPerPlayer);
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

        private void Showdown(List<Player> playersLeftList)
        {
            Dictionary<Player, Card> cardPlayerDic = new Dictionary<Player, Card>();

            foreach (Player player in playersLeftList)
            {
                Card card = player.Deck.Draw();
                cardPlayerDic.Add(player, card);
                Console.WriteLine($"{player.Name}: {card}");
            }
        }

        private void AskForNumberOfPlayers()
        {
            do
            {
                Console.WriteLine("Enter the number of players (between 2 and 5)");
            } while (!int.TryParse(Console.ReadLine(), out numberOfPlayers) || numberOfPlayers < 2 || numberOfPlayers > 5);
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
