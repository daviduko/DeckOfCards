using System;

namespace DeckOfCards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            Console.WriteLine(deck.ToString());
            deck.Shuffle();
            Console.WriteLine(deck.ToString());
            Console.ReadKey();
        }
    }
}
