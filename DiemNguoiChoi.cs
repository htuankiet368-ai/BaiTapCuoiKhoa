using System;
using System.Collections.Generic;
using System.Linq;

class Player
{
    
    public string Code;
    public string Name;
    public int Score;
    public int Wins;
    public int Losses;

    public int TotalMatches => Wins + Losses;

    public double WinRate =>
        TotalMatches == 0 ? 0 : (double)Wins / TotalMatches;

    public string Rank
    {
        get
        {
            if (Score >= 5000) return "Diamond";
            if (Score >= 3000) return "Platinum";
            if (Score >= 1000) return "Gold";
            return "Silver";
        }
    }

    public void Display()
    {
        Console.WriteLine($"{Code} | {Name} | Score: {Score} | W/L: {Wins}/{Losses} | WR: {WinRate:F2} | Rank: {Rank}");
    }
}

class Program
{
    static List<Player> players = new List<Player>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Add Player");
            Console.WriteLine("2. Display Players");
            Console.WriteLine("3. Search Player");
            Console.WriteLine("4. Top 5 Players");
            Console.WriteLine("5. Highest Win Rate");
            Console.WriteLine("6. Remove Inactive");
            Console.WriteLine("7. Rank Statistics");
            Console.WriteLine("0. Exit");

            int choice = ReadInt("Choose: ");

            switch (choice)
            {
                case 1: AddPlayer(); break;
                case 2: DisplayPlayers(players); break;
                case 3: SearchPlayer(); break;
                case 4: ShowTop5(); break;
                case 5: HighestWinRate(); break;
                case 6: RemoveInactive(); break;
                case 7: RankStats(); break;
                case 0: return;
            }
        }
    }

    static void AddPlayer()
    {
        string code;
        do
        {
            code = ReadString("Code: ");
        } while (players.Any(p => p.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));

        Player p = new Player
        {
            Code = code,
            Name = ReadString("Name: "),
            Score = ReadInt("Score: "),
            Wins = ReadInt("Wins: "),
            Losses = ReadInt("Losses: ")
        };

        players.Add(p);
    }

    static void DisplayPlayers(List<Player> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("No players.");
            return;
        }

        foreach (var p in list)
            p.Display();
    }

    static void SearchPlayer()
    {
        string keyword = ReadString("Enter name: ");

        var result = players
            .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();

        DisplayPlayers(result);
    }

    static void ShowTop5()
    {
        var top = players
            .OrderByDescending(p => p.Score)
            .ThenByDescending(p => p.Wins)
            .Take(5)
            .ToList();

        DisplayPlayers(top);
    }

    static void HighestWinRate()
    {
        if (players.Count == 0) return;

        double max = players.Max(p => p.WinRate);

        var result = players
            .Where(p => Math.Abs(p.WinRate - max) < 0.0001)
            .ToList();

        DisplayPlayers(result);
    }

    static void RemoveInactive()
    {
        players.RemoveAll(p => p.TotalMatches == 0);
        Console.WriteLine("Removed inactive players.");
    }

    static void RankStats()
    {
        var groups = players.GroupBy(p => p.Rank);

        foreach (var g in groups)
        {
            Console.WriteLine($"{g.Key}: {g.Count()} players");
        }
    }

    // ---------- Helpers ----------
    static int ReadInt(string msg)
    {
        int value;
        while (true)
        {
            Console.Write(msg);
            if (int.TryParse(Console.ReadLine(), out value))
                return value;
        }
    }

    static string ReadString(string msg)
    {
        Console.Write(msg);
        return Console.ReadLine();
    }
}
