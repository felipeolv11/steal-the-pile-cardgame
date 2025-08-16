using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StealThePile
{
    public class Card
    {
        public int number { get; set; }
        public string suit { get; set; }

        public Card(int number, string suit)
        {
            this.number = number;
            this.suit = suit;
        }

        public override string ToString()
        {
            return $"{suit}";
        }
    }
}