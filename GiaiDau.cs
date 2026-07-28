using System;
using System.Collections.Generic;
using System.Linq;

class Team
{
    public string Code;
    public string Name;
    public int Wins;
    public int Draws;
    public int Losses;
    public int Points;

    public int TotalMatches => Wins + Draws + Losses;

    public void RecalculatePoints()
    {
        Points = Wins * 3 + Draws;
    }

    public void Display(int rank)
    {
        Console.WriteLine(
            $"{rank,-5} {Code,-10} {Name,-20} " +
            $"{TotalMatches,-8} {Wins,-8} {Draws,-8} {Losses,-8} {Points}"
        );
    }
}

class Program
{
    static List<Team> teams = new List<Team>();
    static Random rand = new Random();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n===== TOURNAMENT =====");
            Console.WriteLine("1. Add Team");
            Console.WriteLine("2. Display Teams");
            Console.WriteLine("3. Enter Match");
            Console.WriteLine("4. Standings");
            Console.WriteLine("5. Top Team");
            Console.WriteLine("6. Teams Without Wins");
            Console.WriteLine("7. Total Matches");
            Console.WriteLine("8. Random Results");
            Console.WriteLine("0. Exit");

            int c = ReadInt("Choose: ", 0, 8);

            switch (c)
            {
                case 1: AddTeam(); break;
                case 2: DisplayTeams(teams); break;
                case 3: EnterMatch(); break;
                case 4: ShowStandings(); break;
                case 5: ShowTop(); break;
                case 6: NoWinTeams(); break;
                case 7: TotalMatches(); break;
                case 8: RandomMatches(); break;
                case 0: return;
            }
        }
    }

    static void AddTeam()
    {
        string code;
        do
        {
            code = ReadString("Code: ");
        } while (teams.Any(t => t.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));

        Team t = new Team
        {
            Code = code,
            Name = ReadString("Name: ")
        };

        teams.Add(t);
    }

    static void DisplayTeams(List<Team> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("No teams.");
            return;
        }

        PrintHeader();

        for (int i = 0; i < list.Count; i++)
            list[i].Display(i + 1);
    }

    static void EnterMatch()
    {
        if (teams.Count < 2)
        {
            Console.WriteLine("Need at least 2 teams.");
            return;
        }

        int i1 = FindIndex(ReadString("Team 1 code: "));
        int i2 = FindIndex(ReadString("Team 2 code: "));

        if (i1 == -1 || i2 == -1 || i1 == i2)
        {
            Console.WriteLine("Invalid teams.");
            return;
        }

        Console.WriteLine("1. Team 1 wins");
        Console.WriteLine("2. Draw");
        Console.WriteLine("3. Team 2 wins");

        int r = ReadInt("Result: ", 1, 3);

        UpdateMatch(teams[i1], teams[i2], r);
    }

    static void UpdateMatch(Team a, Team b, int r)
    {
        if (r == 1)
        {
            a.Wins++; b.Losses++;
        }
        else if (r == 2)
        {
            a.Draws++; b.Draws++;
        }
        else
        {
            a.Losses++; b.Wins++;
        }

        a.RecalculatePoints();
        b.RecalculatePoints();
    }

    static List<Team> GetStandings()
    {
        return teams
            .OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.Wins)
            .ToList();
    }

    static void ShowStandings()
    {
        var list = GetStandings();
        DisplayTeams(list);
    }

    static void ShowTop()
    {
        if (teams.Count == 0) return;

        var top = GetStandings().First();
        PrintHeader();
        top.Display(1);
    }

    static void NoWinTeams()
    {
        var list = teams.Where(t => t.Wins == 0).ToList();

        if (list.Count == 0)
        {
            Console.WriteLine("All teams have wins.");
            return;
        }

        DisplayTeams(list);
    }

    static void TotalMatches()
    {
        int total = teams.Sum(t => t.TotalMatches) / 2;
        Console.WriteLine($"Total matches: {total}");
    }

    static void RandomMatches()
    {
        if (teams.Count < 2)
        {
            Console.WriteLine("Need at least 2 teams.");
            return;
        }

        int count = 0;

        for (int i = 0; i < teams.Count - 1; i++)
        {
            for (int j = i + 1; j < teams.Count; j++)
            {
                int r = rand.Next(1, 4);
                UpdateMatch(teams[i], teams[j], r);
                count++;
            }
        }

        Console.WriteLine($"Generated {count} matches.");
    }

    // ---------- Helpers ----------

    static int FindIndex(string code)
    {
        return teams.FindIndex(t =>
            t.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    static string ReadString(string msg)
    {
        Console.Write(msg);
        return (Console.ReadLine() ?? "").Trim();
    }

    static int ReadInt(string msg, int min, int max)
    {
        int x;
        while (true)
        {
            Console.Write(msg);
            if (int.TryParse(Console.ReadLine(), out x) && x >= min && x <= max)
                return x;
        }
    }

    static void PrintHeader()
    {
        Console.WriteLine(
            $"{"Rank",-5} {"Code",-10} {"Name",-20} " +
            $"{"Match",-8} {"Win",-8} {"Draw",-8} {"Loss",-8} {"Pts"}"
        );
    }
}
