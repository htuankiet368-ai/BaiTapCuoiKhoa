using System;
using System.Collections.Generic;

namespace KiemTra.PlayerScoreManagement;

public enum PlayerRank
{
    Bronze = 1,
    Silver = 2,
    Gold = 3,
    Diamond = 4
}

public class Player
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public int Score { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int PlayTime { get; set; }

    public int TotalMatches()
    {
        return Wins + Losses;
    }

    public double CalculateWinRate()
    {
        if (TotalMatches() == 0)
        {
            return 0;
        }

        return (double)Wins /
               TotalMatches() * 100;
    }

    public PlayerRank GetRank()
    {
        if (Score >= 5000)
        {
            return PlayerRank.Diamond;
        }

        if (Score >= 3000)
        {
            return PlayerRank.Gold;
        }

        if (Score >= 1000)
        {
            return PlayerRank.Silver;
        }

        return PlayerRank.Bronze;
    }

    public void Display()
    {
        Console.WriteLine(
            $"{Code,-10} {Name,-18} {Score,-8} " +
            $"{Wins,-8} {Losses,-8} " +
            $"{PlayTime,-12} " +
            $"{CalculateWinRate(),-10:F2}% " +
            $"{GetRank()}"
        );
    }
}

public class PlayerScoreManager
{
    static List<Player> players
        = new List<Player>();

    public static void Run()
    {
        while (true)
        {
            DisplayMenu();

            int choice = ReadInteger(
                "Choose an option: ",
                0,
                9
            );

            switch (choice)
            {
                case 1:
                    AddPlayer();
                    break;

                case 2:
                    DisplayPlayers();
                    break;

                case 3:
                    UpdateScore();
                    break;

                case 4:
                    SearchPlayerByName();
                    break;

                case 5:
                    DisplayTop5();
                    break;

                case 6:
                    DisplayHighestWinRate();
                    break;

                case 7:
                    StatisticsByRank();
                    break;

                case 8:
                    RemovePlayersWithNoMatches();
                    break;

                case 9:
                    SortPlayers();
                    break;

                case 0:
                    return;
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine(
            "\n===== PLAYER SCORE MANAGEMENT ====="
        );

        Console.WriteLine("1. Add Player");
        Console.WriteLine("2. Display Player List");
        Console.WriteLine("3. Update Score by Code");
        Console.WriteLine("4. Search Player by Name");
        Console.WriteLine("5. Top 5 Highest Scores");
        Console.WriteLine("6. Player with Highest Win Rate");
        Console.WriteLine("7. Statistics by Rank");
        Console.WriteLine("8. Remove Players with No Matches");
        Console.WriteLine("9. Sort Player List");
        Console.WriteLine("0. Back");
    }

    static void AddPlayer()
    {
        string code = ReadString(
            "Enter player code: "
        );

        if (FindPlayerIndex(code) != -1)
        {
            Console.WriteLine(
                "Player code already exists."
            );

            return;
        }

        Player player = new Player();

        player.Code = code;

        player.Name = ReadString(
            "Enter player name: "
        );

        player.Score = ReadInteger(
            "Enter score: ",
            0,
            int.MaxValue
        );

        player.Wins = ReadInteger(
            "Enter number of wins: ",
            0,
            int.MaxValue
        );

        player.Losses = ReadInteger(
            "Enter number of losses: ",
            0,
            int.MaxValue
        );

        player.PlayTime = ReadInteger(
            "Enter play time (minutes): ",
            0,
            int.MaxValue
        );

        players.Add(player);

        Console.WriteLine(
            "Player added successfully."
        );
    }

    static void DisplayPlayers()
    {
        if (players.Count == 0)
        {
            Console.WriteLine(
                "Player list is empty."
            );

            return;
        }

        PrintHeader();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].Display();
        }
    }

    static void UpdateScore()
    {
        string code = ReadString(
            "Enter player code: "
        );

        int index = FindPlayerIndex(code);

        if (index == -1)
        {
            Console.WriteLine(
                "Player not found."
            );

            return;
        }

        int newScore = ReadInteger(
            "Enter new score: ",
            0,
            int.MaxValue
        );

        players[index].Score = newScore;

        Console.WriteLine(
            "Score updated successfully."
        );
    }

    static void SearchPlayerByName()
    {
        string keyword = ReadString(
            "Enter player name or part of the name: "
        );

        bool found = false;

        PrintHeader();

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Name.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase))
            {
                players[i].Display();
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine(
                "Player not found."
            );
        }
    }

    static void DisplayTop5()
    {
        if (players.Count == 0)
        {
            Console.WriteLine(
                "Player list is empty."
            );

            return;
        }

        List<Player> tempList
            = new List<Player>(players);

        SortDescending(tempList);

        int displayCount = 5;

        if (tempList.Count < 5)
        {
            displayCount = tempList.Count;
        }

        Console.WriteLine(
            "\n===== TOP 5 PLAYERS ====="
        );

        PrintHeader();

        for (int i = 0; i < displayCount; i++)
        {
            tempList[i].Display();
        }
    }

    static void DisplayHighestWinRate()
    {
        if (players.Count == 0)
        {
            Console.WriteLine(
                "Player list is empty."
            );

            return;
        }

        double highestWinRate = -1;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].TotalMatches() > 0)
            {
                double rate =
                    players[i].CalculateWinRate();

                if (rate > highestWinRate)
                {
                    highestWinRate = rate;
                }
            }
        }

        if (highestWinRate == -1)
        {
            Console.WriteLine(
                "No player has played any matches."
            );

            return;
        }

        Console.WriteLine(
            "\nPlayer(s) with the highest win rate:"
        );

        PrintHeader();

        for (int i = 0; i < players.Count; i++)
        {
            double difference = Math.Abs(
                players[i].CalculateWinRate()
                - highestWinRate
            );

            if (players[i].TotalMatches() > 0 &&
                difference < 0.0001)
            {
                players[i].Display();
            }
        }
    }

    static void StatisticsByRank()
    {
        Console.WriteLine(
            "\n===== RANK STATISTICS ====="
        );

        for (int rank = 1; rank <= 4; rank++)
        {
            int count = 0;

            for (int i = 0; i < players.Count; i++)
            {
                if ((int)players[i].GetRank() == rank)
                {
                    count++;
                }
            }

            Console.WriteLine(
                $"{(PlayerRank)rank}: {count}"
            );
        }
    }

    static void RemovePlayersWithNoMatches()
    {
        int removedCount = 0;

        for (int i = players.Count - 1;
             i >= 0;
             i--)
        {
            if (players[i].TotalMatches() == 0)
            {
                players.RemoveAt(i);
                removedCount++;
            }
        }

        Console.WriteLine(
            $"Removed {removedCount} player(s)."
        );
    }

    static void SortPlayers()
    {
        if (players.Count == 0)
        {
            Console.WriteLine(
                "Player list is empty."
            );

            return;
        }

        SortDescending(players);

        Console.WriteLine(
            "Player list sorted successfully."
        );

        DisplayPlayers();
    }

    static void SortDescending(
        List<Player> listToSort)
    {
        for (int i = 0;
             i < listToSort.Count - 1;
             i++)
        {
            for (int j = i + 1;
                 j < listToSort.Count;
                 j++)
            {
                bool shouldSwap = false;

                if (listToSort[i].Score
                    < listToSort[j].Score)
                {
                    shouldSwap = true;
                }
                else if (
                    listToSort[i].Score
                    == listToSort[j].Score
                    &&
                    listToSort[i].Wins
                    < listToSort[j].Wins)
                {
                    shouldSwap = true;
                }

                if (shouldSwap)
                {
                    Player temp =
                        listToSort[i];

                    listToSort[i] =
                        listToSort[j];

                    listToSort[j] = temp;
                }
            }
        }
    }

    static int FindPlayerIndex(string code)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Code.Equals(
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
            $"{"Code",-10} {"Name",-18} {"Score",-8} " +
            $"{"Wins",-8} {"Losses",-8} " +
            $"{"Play Time",-12} {"Win Rate",-11} {"Rank"}"
        );
    }
}
