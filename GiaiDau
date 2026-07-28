using System;
using System.Collections.Generic;

namespace TournamentSimulation;

public class Team
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int Points { get; set; }

    public int TotalMatches()
    {
        return Wins +
               Draws +
               Losses;
    }

    public void RecalculatePoints()
    {
        Points = Wins * 3 +
                 Draws;
    }

    public void Display(int rank)
    {
        Console.WriteLine(
            $"{rank,-5} {Code,-10} {Name,-20} " +
            $"{TotalMatches(),-8} {Wins,-8} " +
            $"{Draws,-8} {Losses,-8} {Points}"
        );
    }
}

public class TournamentSimulation
{
    static List<Team> teamList = new List<Team>();

    static Random random = new Random();

    public static void Run()
    {
        while (true)
        {
            ShowMenu();

            int choice = ReadInteger(
                "Choose an option: ",
                0,
                8
            );

            switch (choice)
            {
                case 1:
                    AddTeam();
                    break;

                case 2:
                    DisplayTeams();
                    break;

                case 3:
                    EnterMatchResult();
                    break;

                case 4:
                    DisplayStandings();
                    break;

                case 5:
                    DisplayTopTeam();
                    break;

                case 6:
                    DisplayTeamsWithoutWins();
                    break;

                case 7:
                    ShowTotalMatches();
                    break;

                case 8:
                    GenerateRandomResults();
                    break;

                case 0:
                    return;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine(
            "\n===== TOURNAMENT SIMULATION ====="
        );

        Console.WriteLine("1. Add Team");
        Console.WriteLine("2. Display Team List");
        Console.WriteLine("3. Enter Match Result");
        Console.WriteLine("4. Display Standings");
        Console.WriteLine("5. Display Top Team");
        Console.WriteLine("6. Display Teams Without Wins");
        Console.WriteLine("7. Show Total Matches");
        Console.WriteLine("8. Generate Random Results");
        Console.WriteLine("0. Exit");
    }

    static void AddTeam()
    {
        string code = ReadString(
            "Enter team code: "
        );

        if (FindTeamIndex(code) != -1)
        {
            Console.WriteLine(
                "Team code already exists."
            );

            return;
        }

        Team team = new Team();

        team.Code = code;

        team.Name = ReadString(
            "Enter team name: "
        );

        team.Wins = 0;
        team.Draws = 0;
        team.Losses = 0;
        team.Points = 0;

        teamList.Add(team);

        Console.WriteLine(
            "Team added successfully."
        );
    }

    static void DisplayTeams()
    {
        if (teamList.Count == 0)
        {
            Console.WriteLine(
                "The team list is empty."
            );

            return;
        }

        Console.WriteLine(
            "\n===== TEAM LIST ====="
        );

        PrintHeader();

        for (int i = 0; i < teamList.Count; i++)
        {
            teamList[i].Display(i + 1);
        }
    }

    static void EnterMatchResult()
    {
        if (teamList.Count < 2)
        {
            Console.WriteLine(
                "At least two teams are required."
            );

            return;
        }

        string code1 = ReadString(
            "Enter first team code: "
        );

        int index1 = FindTeamIndex(code1);

        if (index1 == -1)
        {
            Console.WriteLine(
                "First team not found."
            );

            return;
        }

        string code2 = ReadString(
            "Enter second team code: "
        );

        int index2 = FindTeamIndex(code2);

        if (index2 == -1)
        {
            Console.WriteLine(
                "Second team not found."
            );

            return;
        }

        if (index1 == index2)
        {
            Console.WriteLine(
                "A team cannot play against itself."
            );

            return;
        }

        Team team1 = teamList[index1];
        Team team2 = teamList[index2];

        Console.WriteLine(
            $"1. {team1.Name} wins"
        );

        Console.WriteLine("2. Draw");

        Console.WriteLine(
            $"3. {team2.Name} wins"
        );

        int result = ReadInteger(
            "Enter result: ",
            1,
            3
        );

        UpdateMatchResult(team1, team2, result);

        Console.WriteLine(
            "Match result updated successfully."
        );
    }

    static void UpdateMatchResult(
        Team team1,
        Team team2,
        int result)
    {
        if (result == 1)
        {
            team1.Wins++;
            team2.Losses++;
        }
        else if (result == 2)
        {
            team1.Draws++;
            team2.Draws++;
        }
        else
        {
            team1.Losses++;
            team2.Wins++;
        }

        team1.RecalculatePoints();
        team2.RecalculatePoints();
    }

    static void DisplayStandings()
    {
        if (teamList.Count == 0)
        {
            Console.WriteLine(
                "The team list is empty."
            );

            return;
        }

        List<Team> standings =
            CreateStandings();

        Console.WriteLine(
            "\n===== STANDINGS ====="
        );

        PrintHeader();

        for (int i = 0; i < standings.Count; i++)
        {
            standings[i].Display(i + 1);
        }
    }

    static List<Team> CreateStandings()
    {
        List<Team> standings =
            new List<Team>(teamList);

        for (int i = 0; i < standings.Count - 1; i++)
        {
            for (int j = i + 1;
                 j < standings.Count;
                 j++)
            {
                bool shouldSwap = false;

                // Higher points first
                if (standings[i].Points < standings[j].Points)
                {
                    shouldSwap = true;
                }
                // If tied on points, more wins first
                else if (
                    standings[i].Points == standings[j].Points
                    &&
                    standings[i].Wins < standings[j].Wins)
                {
                    shouldSwap = true;
                }

                if (shouldSwap)
                {
                    Team temp = standings[i];
                    standings[i] = standings[j];
                    standings[j] = temp;
                }
            }
        }

        return standings;
    }

    static void DisplayTopTeam()
    {
        if (teamList.Count == 0)
        {
            Console.WriteLine(
                "The team list is empty."
            );

            return;
        }

        List<Team> standings =
            CreateStandings();

        Console.WriteLine(
            "\n===== TOP TEAM ====="
        );

        PrintHeader();
        standings[0].Display(1);
    }

    static void DisplayTeamsWithoutWins()
    {
        if (teamList.Count == 0)
        {
            Console.WriteLine(
                "The team list is empty."
            );

            return;
        }

        bool found = false;

        Console.WriteLine(
            "\n===== TEAMS WITHOUT A WIN ====="
        );

        PrintHeader();

        int rank = 1;

        for (int i = 0; i < teamList.Count; i++)
        {
            if (teamList[i].Wins == 0)
            {
                teamList[i].Display(rank);
                rank++;
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine(
                "Every team has won at least one match."
            );
        }
    }

    static void ShowTotalMatches()
    {
        int totalGamesPlayed = 0;

        for (int i = 0; i < teamList.Count; i++)
        {
            totalGamesPlayed +=
                teamList[i].TotalMatches();
        }

        // Each match is counted twice (once per team)
        int totalMatches = totalGamesPlayed / 2;

        Console.WriteLine(
            $"Total matches played: {totalMatches}"
        );
    }

    static void GenerateRandomResults()
    {
        if (teamList.Count < 2)
        {
            Console.WriteLine(
                "At least two teams are required."
            );

            return;
        }

        int matchesCreated = 0;

        for (int i = 0;
             i < teamList.Count - 1;
             i++)
        {
            for (int j = i + 1;
                 j < teamList.Count;
                 j++)
            {
                int result = random.Next(1, 4);

                UpdateMatchResult(
                    teamList[i],
                    teamList[j],
                    result
                );

                Console.Write(
                    $"{teamList[i].Name} - " +
                    $"{teamList[j].Name}: "
                );

                if (result == 1)
                {
                    Console.WriteLine(
                        $"{teamList[i].Name} wins"
                    );
                }
                else if (result == 2)
                {
                    Console.WriteLine("Draw");
                }
                else
                {
                    Console.WriteLine(
                        $"{teamList[j].Name} wins"
                    );
                }

                matchesCreated++;
            }
        }

        Console.WriteLine(
            $"Generated {matchesCreated} random matches."
        );
    }

    static int FindTeamIndex(string code)
    {
        for (int i = 0; i < teamList.Count; i++)
        {
            if (teamList[i].Code.Equals(
                    code,
                    StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    static string ReadString(string message)
    {
        while (true)
        {
            Console.Write(message);

            string input =
                (Console.ReadLine() ?? "").Trim();

            if (input != "")
            {
                return input;
            }

            Console.WriteLine(
                "Input cannot be empty."
            );
        }
    }

    static int ReadInteger(
        string message,
        int minValue,
        int maxValue)
    {
        while (true)
        {
            Console.Write(message);

            int value;

            if (int.TryParse(
                    Console.ReadLine(),
                    out value)
                &&
                value >= minValue
                &&
                value <= maxValue)
            {
                return value;
            }

            Console.WriteLine(
                "Invalid input."
            );
        }
    }

    static void PrintHeader()
    {
        Console.WriteLine(
            $"{"Rank",-5} {"Code",-10} {"Team Name",-20} " +
            $"{"Matches",-8} {"Wins",-8} " +
            $"{"Draws",-8} {"Losses",-8} {"Points"}"
        );
    }
}
