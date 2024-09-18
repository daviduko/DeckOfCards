﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards
{
    internal class Player
    {
        public string Name {  get; set; }
        public Card Card { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}: {Card.ToString()}";
        }
    }
}
