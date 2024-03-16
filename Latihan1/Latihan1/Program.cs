using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("WELCOME TO DUEL MONSTER TEXT!!");
        Console.WriteLine("Silakan pilih kartu untuk membuat dek Anda.");

        // Membuat dek pemain
        string[] playerDeck = new string[5];
        for (int i = 0; i < playerDeck.Length; i++)
        {
            Console.WriteLine("Masukkan nama kartu (Blue-Eyes White Dragon/Dark Magician/Mirror Force):");
            string cardName = Console.ReadLine();
            playerDeck[i] = cardName;
        }

        // Dek pemain lawan
        string[] opponentDeck = new string[]
        {
            "Raigeki",
            "Slifer the Sky Dragon",
            "Monster Reborn"
        };

        // Membuat objek Player untuk pemain dan lawan
        Player player = new Player(playerDeck, 8000);
        Player opponent = new Player(opponentDeck, 8000);

        // Menampilkan dek pemain lawan
        Console.WriteLine("\nDek Pemain Lawan:");
        foreach (string card in opponent.Deck)
        {
            Console.WriteLine(card);
        }
        Console.WriteLine();

        // Membuat objek Game
        Game game = new Game(player, opponent);

        // Memulai permainan
        game.Start();

        // Menampilkan hasil permainan
        if (player.LifePoints <= 0)
        {
            Console.WriteLine("\nAnda kalah! Lawan memiliki poin kehidupan lebih dari Anda.");
        }
        else if (opponent.LifePoints <= 0)
        {
            Console.WriteLine("\nAnda menang! Lawan kehabisan poin kehidupan.");
        }
    }
}

public class Player
{
    private List<string> deck;
    private int lifePoints;
    private Dictionary<string, int> cardAttacks = new Dictionary<string, int>()
    {
        { "Blue-Eyes White Dragon", 3000 },
        { "Dark Magician", 2500 },
        { "Mirror Force", 0 } // Contoh, kartu trap tidak memiliki nilai serangan
    };

    public Player(string[] startingDeck, int startingLifePoints)
    {
        deck = new List<string>(startingDeck);
        lifePoints = startingLifePoints;
    }

    public List<string> Deck { get { return deck; } }

    public int LifePoints
    {
        get { return lifePoints; }
        set { lifePoints = value; }
    }

    public void Attack(Player opponent, string cardName)
    {
        int playerAttackPoints = GetAttackPoints(cardName); // Mendapatkan nilai serangan kartu pemain
        int opponentAttackPoints = opponent.GetAttackPoints(cardName); // Mendapatkan nilai serangan kartu lawan

        // Mengurangi poin kehidupan lawan berdasarkan perbedaan nilai serangan
        int damage = Math.Max(playerAttackPoints - opponentAttackPoints, 0);
        opponent.LifePoints -= damage;

        // Mengurangi poin kehidupan pemain jika kartu lawan memiliki serangan lebih besar dari 0
        if (opponentAttackPoints > 0)
        {
            this.LifePoints -= opponentAttackPoints;
        }
    }

    public void RemoveCard(string cardName)
    {
        deck.Remove(cardName);
    }

    public bool HasCards()
    {
        return deck.Count > 0;
    }

    private int GetAttackPoints(string cardName)
    {
        if (cardAttacks.ContainsKey(cardName))
        {
            return cardAttacks[cardName];
        }
        else
        {
            return 0;
        }
    }
}

public class Game
{
    private Player player;
    private Player opponent;
    private bool playerTurn;

    public Game(Player player, Player opponent)
    {
        this.player = player;
        this.opponent = opponent;
        this.playerTurn = true; // Mulai dengan giliran pemain pertama
    }

    public void Start()
    {
        // Implementasi logika pertarungan
        while (player.LifePoints > 0 && opponent.LifePoints > 0)
        {
            // Tampilkan status pemain
            Console.WriteLine("Poin Kehidupan Anda: " + player.LifePoints);
            Console.WriteLine("Poin Kehidupan Lawan: " + opponent.LifePoints);

            // Tentukan pemain yang sedang giliran
            Player currentPlayer = playerTurn ? player : opponent;

            // Menampilkan giliran pemain
            Console.WriteLine("Giliran " + (playerTurn ? "Anda!" : "Lawan!"));

            // Implementasi serangan pemain
            string attackCard = "";
            if (playerTurn)
            {
                Console.WriteLine("Masukkan serangan Anda (misalnya, nama kartu):");
                attackCard = Console.ReadLine();
                if (!currentPlayer.Deck.Contains(attackCard))
                {
                    Console.WriteLine("Kartu tidak valid.");
                    continue;
                }
                currentPlayer.RemoveCard(attackCard); // Contoh sederhana, implementasi sesuai kebutuhan
                opponent.Attack(player, attackCard); // Menyerang lawan
            }
            else
            {
                // Lawan menyerang pemain dengan kartu dari dek lawan secara acak
                Random rnd = new Random();
                int randomIndex = rnd.Next(opponent.Deck.Count);
                attackCard = opponent.Deck[randomIndex];
                opponent.RemoveCard(attackCard);
                player.Attack(opponent, attackCard); // Menyerang pemain
            }

            // Menampilkan serangan
            Console.WriteLine((playerTurn ? "Anda menyerang dengan kartu: " : "Lawan menyerang dengan kartu: ") + attackCard);

            // Ganti giliran pemain
            playerTurn = !playerTurn;

            // Pengecekan poin kehidupan dan pengurangan poin jika diperlukan
            if (player.LifePoints <= 0 || opponent.LifePoints <= 0)
            {
                break; // Keluar dari loop jika salah satu pemain kehabisan poin kehidupan
            }
        }

        // Tampilkan hasil permainan
        if (player.LifePoints <= 0)
        {
            Console.WriteLine("\nAnda kalah! Lawan memiliki poin kehidupan lebih dari Anda.");
        }
        else if (opponent.LifePoints <= 0)
        {
            Console.WriteLine("\nAnda menang! Lawan kehabisan poin kehidupan.");
        }
    }
}
