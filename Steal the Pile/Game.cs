using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StealThePile
{
    public class Game
    {
        static public string asciiArt = "  ██████ ▄▄▄█████▓▓█████ ▄▄▄       ██▓       ▄▄▄█████▓ ██░ ██ ▓█████     ██▓███   ██▓ ██▓    ▓█████ " +
                                    "\r\n▒██    ▒ ▓  ██▒ ▓▒▓█   ▀▒████▄    ▓██▒       ▓  ██▒ ▓▒▓██░ ██▒▓█   ▀    ▓██░  ██▒▓██▒▓██▒    ▓█   ▀ " +
                                    "\r\n░ ▓██▄   ▒ ▓██░ ▒░▒███  ▒██  ▀█▄  ▒██░       ▒ ▓██░ ▒░▒██▀▀██░▒███      ▓██░ ██▓▒▒██▒▒██░    ▒███   " +
                                    "\r\n  ▒   ██▒░ ▓██▓ ░ ▒▓█  ▄░██▄▄▄▄██ ▒██░       ░ ▓██▓ ░ ░▓█ ░██ ▒▓█  ▄    ▒██▄█▓▒ ▒░██░▒██░    ▒▓█  ▄ " +
                                    "\r\n▒██████▒▒  ▒██▒ ░ ░▒████▒▓█   ▓██▒░██████▒     ▒██▒ ░ ░▓█▒░██▓░▒████▒   ▒██▒ ░  ░░██░░██████▒░▒████▒" +
                                    "\r\n▒ ▒▓▒ ▒ ░  ▒ ░░   ░░ ▒░ ░▒▒   ▓▒█░░ ▒░▓  ░     ▒ ░░    ▒ ░░▒░▒░░ ▒░ ░   ▒▓▒░ ░  ░░▓  ░ ▒░▓  ░░░ ▒░ ░" +
                                    "\r\n░ ░▒  ░ ░    ░     ░ ░  ░ ▒   ▒▒ ░░ ░ ▒  ░       ░     ▒ ░▒░ ░ ░ ░  ░   ░▒ ░      ▒ ░░ ░ ▒  ░ ░ ░  ░" +
                                    "\r\n░  ░  ░    ░         ░    ░   ▒     ░ ░        ░       ░  ░░ ░   ░      ░░        ▒ ░  ░ ░      ░   ";

        static public string line = "____________________________________________________________________________________________________";
        static public string shortLine = "---";

        static public Player[] players { get; set; }
        static public Stack<Card> drawPile { get; set; } = new Stack<Card>();
        static public Dictionary<Card, int> discardArea { get; set; } = new Dictionary<Card, int>();
        static public int playersQuantity { get; set; }
        static public int decksQuantity { get; set; }

        public void Start()
        {
            StealThePileTag();

            do
            {
                Console.WriteLine("\nHOW MANY PLAYERS WILL PLAY? (2-4)");
                playersQuantity = int.Parse(Console.ReadLine());

                if (playersQuantity < 2 || playersQuantity > 4)
                {
                    Console.WriteLine("INVALID CHOICE, TRY AGAIN");
                }

            } while (playersQuantity < 2 || playersQuantity > 4);

            do
            {
                Console.WriteLine("\nHOW MANY DECKS WILL BE USED? (1-4)");
                decksQuantity = int.Parse(Console.ReadLine());

                if (decksQuantity < 1 || decksQuantity > 4)
                {
                    Console.WriteLine("INVALID CHOICE, TRY AGAIN");
                }

            } while (decksQuantity < 1 || decksQuantity > 4);

            players = new Player[playersQuantity];
            drawPile = new Stack<Card>(52 * decksQuantity);

            Logger.RegisterLog("the game was started w/ " + playersQuantity + " players");
            Logger.RegisterLog("the deck was created w/ " + (52 * decksQuantity) + " cards");

            Console.WriteLine();
            for (int i = 0; i < playersQuantity; i++)
            {
                Console.Write($"PLAYER N{i + 1}: ");
                string name = Console.ReadLine();

                players[i] = new Player(name);
            }

            string playersNames = "";
            for (int i = 0; i < players.Length; i++)
            {
                playersNames += players[i].name;

                if (i < players.Length - 1)
                {
                    playersNames += ", ";
                }
            }
            Logger.RegisterLog("MATCH PLAYERS: " + playersNames);

            Shuffle(decksQuantity);
            Play();
        }

        static public void Restart()
        {
            drawPile = new Stack<Card>();
            discardArea = new Dictionary<Card, int>();

            foreach (var player in players)
            {
                player.ClearCards();
            }
        }

        static public void Shuffle(int decksQuantity)
        {
            List<Card> deck = new List<Card>();

            string[] suits = { "HEARTS", "SPADES", "DIAMONDS", "CLUBS" };

            foreach (var suit in suits)
            {
                for (int i = 1; i <= 13; i++)
                {
                    string cardName;

                    switch (i)
                    {
                        case 1:
                            cardName = "ACE";
                            break;

                        case 2:
                            cardName = "TWO";
                            break;

                        case 3:
                            cardName = "THREE";
                            break;

                        case 4:
                            cardName = "FOUR";
                            break;

                        case 5:
                            cardName = "FIVE";
                            break;

                        case 6:
                            cardName = "SIX";
                            break;

                        case 7:
                            cardName = "SEVEN";
                            break;

                        case 8:
                            cardName = "EIGHT";
                            break;

                        case 9:
                            cardName = "NINE";
                            break;

                        case 10:
                            cardName = "TEN";
                            break;

                        case 11:
                            cardName = "JACK";
                            break;

                        case 12:
                            cardName = "QUEEN";
                            break;

                        case 13:
                            cardName = "KING";
                            break;

                        default:
                            cardName = i.ToString();
                            break;
                    }

                    deck.Add(new Card(i, $"{cardName} OF {suit}"));
                }
            }

            List<Card> multipliedDecks = new List<Card>();

            for (int i = 0; i < decksQuantity; i++)
            {
                foreach (var card in deck)
                {
                    multipliedDecks.Add(card);
                }
            }

            Random random = new Random();
            int n = multipliedDecks.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Card temp = multipliedDecks[i];
                multipliedDecks[i] = multipliedDecks[j];
                multipliedDecks[j] = temp;
            }

            foreach (var card in multipliedDecks)
            {
                drawPile.Push(card);
            }
        }

        static public void Play()
        {
            bool gameActive = true;

            while (gameActive)
            {
                int remainingCards = drawPile.Count;

                if (remainingCards == 0)
                {
                    Console.WriteLine("\nTHE DRAW PILE IS OVER! GAME OVER!!!");
                    Console.ReadLine();
                    gameActive = false;
                    break;
                }

                foreach (var player in players)
                {
                    PlayerTurn(player);

                    remainingCards = drawPile.Count;
                    if (remainingCards == 0)
                    {
                        Console.WriteLine("\nTHE DRAW PILE IS OVER! GAME OVER!!!");
                        Console.ReadLine();
                        gameActive = false;
                        break;
                    }
                }
            }

            EndMatch();
        }

        static public void EndMatch()
        {
            for (int i = 0; i < players.Count() - 1; i++)
            {
                for (int j = i + 1; j < players.Count(); j++)
                {
                    if (players[i].cardsQuantity < players[j].cardsQuantity)
                    {
                        var temp = players[i];
                        players[i] = players[j];
                        players[j] = temp;
                    }
                }
            }

            List<Player> winners = new List<Player>();
            int highestQuantity = players[0].cardsQuantity;
            foreach (var player in players)
            {
                if (player.cardsQuantity == highestQuantity)
                {
                    winners.Add(player);
                }
            }

            StealThePileTag();

            if (winners.Count == 1)
            {
                Console.WriteLine("\nAND THE WINNNER IS: {0}, W/ {1} CARDS!", winners[0].name, winners[0].cardsQuantity);
                Logger.RegisterLog($"the winner is: {winners[0].name} w/ {winners[0].cardsQuantity} cards");
            }

            else
            {
                Console.WriteLine("AND THE WINNERS ARE:");
                Logger.RegisterLog("the winners are:");
                foreach (var winner in winners)
                {
                    Console.WriteLine("{0}, W/ {1} CARDS!", winner.name, winners[0].cardsQuantity);
                    Logger.RegisterLog($"{winners[0].name} w/ {winners[0].cardsQuantity} cards");
                }
            }

            Console.ReadLine();
            Console.WriteLine(shortLine);
            Console.WriteLine("\nMATCH RESULT:\n");
            for (int i = 0; i < players.Count(); i++)
            {
                var player = players[i];
                player.UpdatePosition(i + 1);
                Console.WriteLine("POSITION: {0}° | PLAYER: {1} | CARDS IN PILE: {2}", i + 1, player.name, player.cardsQuantity);
                Logger.RegisterLog($"position: {i + 1}, player: {player.name}, cards in pile: {player.cardsQuantity}");
            }
            Logger.RegisterLog("\n");

            Console.ReadLine();
            Console.WriteLine(shortLine + "\n");
            Console.WriteLine("DO YOU WANT TO PLAY AGAIN? (YES/NO)");
            string resp = Console.ReadLine();

            if (resp.ToLower() == "yes")
            {
                StealThePileTag();

                do
                {
                    Console.WriteLine("\nHOW MANY DECK WILL BE USED? (1-4)");
                    decksQuantity = int.Parse(Console.ReadLine());

                    if (decksQuantity < 1 || decksQuantity > 4)
                    {
                        Console.WriteLine("INVALID CHOICE, TRY AGAIN");
                    }

                } while (decksQuantity < 1 || decksQuantity > 4);

                Restart();

                drawPile = new Stack<Card>(52 * decksQuantity);

                Shuffle(decksQuantity);
                Play();
            }

            else if (resp.ToLower() == "no")
            {
                string playerName = "";
                do
                {
                    Console.WriteLine("\nTYPE ANY PLAYER NAME TO SEE HIS HISTORY POSITION (TYPE 'NO' TO SKIP)");
                    playerName = Console.ReadLine();

                    if (playerName.ToLower() != "no")
                    {
                        StealThePileTag();

                        bool playerFound = false;

                        foreach (var player in players)
                        {
                            if (player.name.ToLower() == playerName.ToLower())
                            {
                                player.ShowPositionHistory();
                                playerFound = true;

                                Thread.Sleep(500);
                                break;
                            }
                        }

                        if (!playerFound)
                        {
                            Console.WriteLine("\nPLAYER NOT FOUND, TRY AGAIN");
                        }
                    }

                } while (playerName.ToLower() != "no");

                Console.WriteLine("\nGOODBYE! SEE YOU NEXT TIME!");
                Thread.Sleep(1000);
            }

            else
            {
                Console.WriteLine("INVALID ANSWER, TRY AGAIN");
            }
        }

        static public void PlayerTurn(Player player)
        {
            while (true)
            {
                int remainingCards = drawPile.Count;

                if (remainingCards == 0)
                {
                    break;
                }

                StealThePileTag();
                Console.WriteLine("\nCURRENT PLAYER: {0}", player.name);

                Card currentCard = drawPile.Pop();
                Console.WriteLine("CURRENT CARD: " + currentCard);
                Logger.RegisterLog($"{player.name} drew the card {currentCard}");

                Console.WriteLine("\nDRAW PILE (REMAINING CARDS: {0})", remainingCards);
                Console.WriteLine("\n" + shortLine);

                Card ownPileTopCard = null;
                if (player.pile.Count > 0)
                {
                    ownPileTopCard = player.pile.Peek();
                }

                Console.WriteLine("\n@ PILES AREA:");
                ShowPlayersPiles();
                Console.WriteLine("\n" + shortLine);

                Console.WriteLine("\n@ DISCARD AREA:");
                ShowDiscardArea();

                if (CheckOpponentsPiles(player, currentCard))
                {
                    Console.ReadLine();

                    Console.Write("W/ THIS, THE PLAYER CAN DRAW ANOTHER CARD!");
                    Logger.RegisterLog($"{player.name} stole a pile from an opponent");
                    ShowCurrentState();
                    Console.ReadLine();
                    continue;
                }

                if (CheckDiscardArea(player, currentCard))
                {
                    Console.ReadLine();

                    Console.Write("W/ THIS, THE PLAYER CAN DRAW ANOTHER CARD!");
                    Logger.RegisterLog($"{player.name} took a card from the discard area");
                    ShowCurrentState();
                    Console.ReadLine();
                    continue;
                }

                if (CheckOwnPile(player, currentCard, ownPileTopCard))
                {
                    Console.ReadLine();

                    Console.Write("W/ THIS, THE PLAYER CAN DRAW ANOTHER CARD!");
                    Logger.RegisterLog($"{player.name} placed the card {currentCard} on their own pile");
                    ShowCurrentState();
                    Console.ReadLine();
                    continue;
                }

                AddCardToDiscard(currentCard);
                Logger.RegisterLog($"{player.name} discarded the card {currentCard}");
                Console.ReadLine();

                Console.Write("THE PLAYER SKIPPED THEIR TURN!");
                Console.ReadLine();
                break;
            }
        }

        static public bool CheckOpponentsPiles(Player player, Card currentCard)
        {
            List<Player> candidates = new List<Player>();
            int largestSize = 0;

            foreach (var otherPlayer in players)
            {
                if (otherPlayer.name != player.name)
                {
                    Card pileTopCard = null;
                    if (otherPlayer.pile.Count > 0)
                    {
                        pileTopCard = otherPlayer.pile.Peek();
                    }

                    if (pileTopCard != null && currentCard.number == pileTopCard.number)
                    {
                        if (otherPlayer.pile.Count > largestSize)
                        {
                            candidates.Clear();
                            largestSize = otherPlayer.pile.Count;
                        }

                        if (otherPlayer.pile.Count == largestSize)
                        {
                            candidates.Add(otherPlayer);
                        }
                    }
                }
            }

            if (candidates.Count > 0)
            {
                Random random = new Random();

                int randomIndex = random.Next(candidates.Count);
                Player targetPlayer = candidates[randomIndex];

                Console.ReadLine();
                Console.WriteLine("SUMMARY:\n");
                Console.Write("YOU STOLE THE PILE FROM PLAYER {0}!", targetPlayer.name);

                StealPile(player, targetPlayer, currentCard);

                return true;
            }

            return false;
        }

        static public bool CheckDiscardArea(Player player, Card currentCard)
        {
            if (discardArea.ContainsValue(currentCard.number))
            {
                Card cardFromArea = null;
                foreach (var card in discardArea)
                {
                    if (card.Value == currentCard.number)
                    {
                        cardFromArea = new Card(card.Value, card.Key.suit);
                        discardArea.Remove(card.Key);

                        break;
                    }
                }

                player.pile.Push(cardFromArea);
                player.pile.Push(currentCard);
                player.UpdateCardCount();

                Console.ReadLine();
                Console.WriteLine("SUMARRY:\n");
                Console.Write("YOU TOOK A CARD FORM THE DISCARD AREA!");

                return true;
            }

            return false;
        }

        static public bool CheckOwnPile(Player player, Card currentCard, Card ownPileTopCard)
        {
            if (ownPileTopCard != null && currentCard.number == ownPileTopCard.number)
            {
                player.pile.Push(currentCard);
                player.UpdateCardCount();

                Console.ReadLine();
                Console.WriteLine("SUMARRY:\n");
                Console.Write("THE CURRENT CARD WAS ADDED TO YOUR OWN PILE!");

                return true;
            }

            return false;
        }

        static public void AddCardToDiscard(Card currentCard)
        {
            discardArea.Add(currentCard, currentCard.number);

            Console.ReadLine();
            Console.WriteLine("SUMMARY:\n");
            Console.Write("THE CURRENT CARD WAS DISCARDED!");
        }

        static public void ShowCurrentState()
        {
            Console.ReadLine();

            StealThePileTag();

            Console.WriteLine("\nCURRENT GAME STATE:");
            Console.WriteLine("\n" + shortLine);

            Console.WriteLine("\n@ PILES AREA:");
            ShowPlayersPiles();
            Console.WriteLine("\n" + shortLine);

            Console.WriteLine("\n@ DISCARD AREA:");
            ShowDiscardArea();

            Console.Write("\nPRESS 'ENTER' TO CONTINUE...");
        }

        static public void StealPile(Player player1, Player player2, Card currentCard)
        {
            Stack<Card> cards = new Stack<Card>();

            while (player2.pile.Count > 0)
            {
                cards.Push(player2.pile.Pop());
            }

            while (cards.Count > 0)
            {
                var card = cards.Pop();
                player1.pile.Push(card);
            }

            player1.pile.Push(currentCard);

            player1.UpdateCardCount();
            player2.UpdateCardCount();
        }

        static public void ShowDiscardArea()
        {
            Console.WriteLine();
            if (discardArea.Count > 0)
            {
                foreach (var card in discardArea)
                {
                    Console.WriteLine("> " + card.Key);
                }
            }

            else
            {
                Console.WriteLine("*EMPTY*");
            }

            Console.WriteLine("\n" + shortLine);
        }

        static public void ShowPlayersPiles()
        {
            Console.WriteLine();
            foreach (var player in players)
            {
                if (player.pile.Count > 0)
                {
                    Console.WriteLine("PLAYER: {0} | PILES TOP: {1} (SIZE: {2})", player.name, player.pile.Peek(), player.cardsQuantity);
                }

                else
                {
                    Console.WriteLine("PLAYER: {0} | PILES TOP: *EMPTY*", player.name);
                }
            }
        }

        static public void StealThePileTag()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(asciiArt);

            string credits = "CODED BY FELIPEOLV11";
            int position = 80;

            if (position > Console.WindowWidth - credits.Length)
            {
                position = Console.WindowWidth - credits.Length;
            }

            if (position < 0)
            {
                position = 0;
            }

            Console.SetCursorPosition(position, Console.CursorTop);
            Console.WriteLine(credits);

            Console.WriteLine(line);
        }
    }
}