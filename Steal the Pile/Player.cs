using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StealThePile
{
    public class Player
    {
        public string name { get; set; }
        public int cardsQuantity { get; set; }
        public int position { get; set; }
        public Queue<int> positionHistory { get; set; } = new Queue<int>();
        public Stack<Card> pile { get; set; } = new Stack<Card>();

        public Player(string name)
        {
            this.name = name;
            cardsQuantity = 0;
            position = 0;
        }

        public void ClearCards()
        {
            cardsQuantity = 0;
            pile.Clear();
        }

        public void UpdateCardCount()
        {
            cardsQuantity = pile.Count;
        }

        public void UpdatePosition(int position)
        {
            this.position = position;

            positionHistory.Enqueue(position);

            if (positionHistory.Count() > 5)
            {
                positionHistory.Dequeue();
            }
        }

        public void ShowPositionHistory()
        {
            Console.WriteLine($"\nHISTORY OF {name}:");

            foreach (var pos in positionHistory)
            {
                Console.WriteLine($"- {pos}° PLACE");
            }
        }
    }
}