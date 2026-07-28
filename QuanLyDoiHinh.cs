using System;
using System.Collections.Generic;

namespace BattleTeamManagement;

public enum CharacterClass
{
    Warrior = 1,
    Archer = 2,
    Mage = 3,
    Tank = 4,
    Healer = 5
}

public class Warrior
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public CharacterClass Class { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }

    public int CalculatePower()
    {
        return Health + Attack * 2 + Defense + Speed;
    }

    public void Display()
    {
        Console.WriteLine(
            $"{Code,-10} {Name,-18} {Class,-10} " +
            $"{Health,-8} {Attack,-10} " +
            $"{Defense,-10} {Speed,-8} " +
            $"{CalculatePower()}"
        );
    }
}

public class BattleTeamManagement
{
    const int MAX_TEAM_MEMBERS = 5;

    static List<Warrior> warriorList
        = new List<Warrior>();

    static List<Warrior> team
        = new List<Warrior>();

    public static void Run()
    {
        while (true)
        {
            ShowMenu();

            int choice = ReadInteger(
                "Choose an option: ",
                0,
                10
            );

            switch (choice)
            {
                case 1:
                    AddWarrior();
                    break;

                case 2:
                    DisplayWarriors();
                    break;

                case 3:
                    AddToTeam();
                    break;

                case 4:
                    DisplayTeam();
                    break;

                case 5:
                    RemoveFromTeam();
                    break;

                case 6:
                    DisplayTeamStats();
                    break;

                case 7:
                    FindStrongestWarrior();
                    break;

                case 8:
                    SortTeamBySpeed();
                    break;

                case 9:
                    CheckBattleRequirements();
                    break;

                case 10:
                    CountWarriorsByClass();
                    break;

                case 0:
                    return;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine(
            "\n===== BATTLE TEAM MANAGEMENT ====="
        );

        Console.WriteLine("1. Add Warrior");
        Console.WriteLine("2. Display Warrior List");
        Console.WriteLine("3. Add Warrior to Team");
        Console.WriteLine("4. Display Team");
        Console.WriteLine("5. Remove Warrior from Team");
        Console.WriteLine("6. Display Team Statistics");
        Console.WriteLine("7. Find Strongest Warrior");
        Console.WriteLine("8. Sort Team by Speed (Descending)");
        Console.WriteLine("9. Check Battle Requirements");
        Console.WriteLine("10. Count Warriors by Class");
        Console.WriteLine("0. Exit");
    }

    static void AddWarrior()
    {
        string code = ReadString(
            "Enter warrior code: "
        );

        if (FindWarriorIndex(code) != -1)
        {
            Console.WriteLine(
                "Warrior code already exists."
            );

            return;
        }

        Warrior warrior = new Warrior();

        warrior.Code = code;

        warrior.Name = ReadString(
            "Enter warrior name: "
        );

        warrior.Class = ReadCharacterClass();

        warrior.Health = ReadInteger(
            "Enter health: ",
            0,
            int.MaxValue
        );

        warrior.Attack = ReadInteger(
            "Enter attack: ",
            0,
            int.MaxValue
        );

        warrior.Defense = ReadInteger(
            "Enter defense: ",
            0,
            int.MaxValue
        );

        warrior.Speed = ReadInteger(
            "Enter speed: ",
            0,
            int.MaxValue
        );

        warriorList.Add(warrior);

        Console.WriteLine(
            "Warrior added successfully."
        );
    }

    static void DisplayWarriors()
    {
        if (warriorList.Count == 0)
        {
            Console.WriteLine(
                "The warrior list is empty."
            );

            return;
        }

        Console.WriteLine(
            "\n===== WARRIOR LIST ====="
        );

        PrintHeader();

        for (int i = 0; i < warriorList.Count; i++)
        {
            warriorList[i].Display();
        }
    }

    static void AddToTeam()
    {
        if (team.Count >= MAX_TEAM_MEMBERS)
        {
            Console.WriteLine(
                "The team already has 5 warriors."
            );

            return;
        }

        string code = ReadString(
            "Enter warrior code: "
        );

        int index = FindWarriorIndex(code);

        if (index == -1)
        {
            Console.WriteLine(
                "Warrior not found."
            );

            return;
        }

        if (FindTeamIndex(code) != -1)
        {
            Console.WriteLine(
                "Warrior is already in the team."
            );

            return;
        }

        team.Add(warriorList[index]);

        Console.WriteLine(
            "Warrior added to the team."
        );
    }

    static void DisplayTeam()
    {
        if (team.Count == 0)
        {
            Console.WriteLine(
                "The team is empty."
            );

            return;
        }

        Console.WriteLine(
            $"\n===== TEAM {team.Count}/5 ====="
        );

        PrintHeader();

        for (int i = 0; i < team.Count; i++)
        {
            team[i].Display();
        }
    }

    static void RemoveFromTeam()
    {
        string code = ReadString(
            "Enter warrior code to remove: "
        );

        int index = FindTeamIndex(code);

        if (index == -1)
        {
            Console.WriteLine(
                "Warrior not found in the team."
            );

            return;
        }

        team.RemoveAt(index);

        Console.WriteLine(
            "Warrior removed from the team."
        );
    }

    static void DisplayTeamStats()
    {
        int totalHealth = 0;
        int totalAttack = 0;
        int totalDefense = 0;
        int totalSpeed = 0;

        for (int i = 0; i < team.Count; i++)
        {
            totalHealth += team[i].Health;
            totalAttack += team[i].Attack;
            totalDefense += team[i].Defense;
            totalSpeed += team[i].Speed;
        }

        Console.WriteLine(
            "\n===== TEAM STATISTICS ====="
        );

        Console.WriteLine(
            $"Members: {team.Count}/5"
        );

        Console.WriteLine($"Total Health: {totalHealth}");
        Console.WriteLine($"Total Attack: {totalAttack}");
        Console.WriteLine($"Total Defense: {totalDefense}");
        Console.WriteLine($"Total Speed: {totalSpeed}");
    }

    static void FindStrongestWarrior()
    {
        if (warriorList.Count == 0)
        {
            Console.WriteLine(
                "The warrior list is empty."
            );

            return;
        }

        int maxPower =
            warriorList[0].CalculatePower();

        for (int i = 1; i < warriorList.Count; i++)
        {
            if (warriorList[i].CalculatePower()
                > maxPower)
            {
                maxPower =
                    warriorList[i].CalculatePower();
            }
        }

        Console.WriteLine(
            "\n===== STRONGEST WARRIOR ====="
        );

        PrintHeader();

        for (int i = 0; i < warriorList.Count; i++)
        {
            if (warriorList[i].CalculatePower()
                == maxPower)
            {
                warriorList[i].Display();
            }
        }
    }

    static void SortTeamBySpeed()
    {
        if (team.Count == 0)
        {
            Console.WriteLine(
                "The team is empty."
            );

            return;
        }

        for (int i = 0; i < team.Count - 1; i++)
        {
            for (int j = i + 1;
                 j < team.Count;
                 j++)
            {
                if (team[i].Speed
                    < team[j].Speed)
                {
                    Warrior temp = team[i];
                    team[i] = team[j];
                    team[j] = temp;
                }
            }
        }

        Console.WriteLine(
            "Team sorted by descending speed."
        );

        DisplayTeam();
    }

    static void CheckBattleRequirements()
    {
        int totalHealth = 0;
        int totalAttack = 0;

        for (int i = 0; i < team.Count; i++)
        {
            totalHealth += team[i].Health;
            totalAttack += team[i].Attack;
        }

        bool enoughMembers = team.Count >= 3;
        bool enoughHealth = totalHealth >= 1000;
        bool enoughAttack = totalAttack >= 300;

        Console.WriteLine(
            "\n===== BATTLE REQUIREMENTS ====="
        );

        Console.WriteLine(
            $"Members: {team.Count}/3"
        );

        Console.WriteLine(
            $"Total Health: {totalHealth}/1000"
        );

        Console.WriteLine(
            $"Total Attack: {totalAttack}/300"
        );

        if (enoughMembers && enoughHealth && enoughAttack)
        {
            Console.WriteLine(
                "The team meets the battle requirements."
            );
        }
        else
        {
            Console.WriteLine(
                "The team does NOT meet the battle requirements."
            );
        }
    }

    static void CountWarriorsByClass()
    {
        Console.WriteLine(
            "\n===== WARRIORS BY CLASS ====="
        );

        for (int cls = 1; cls <= 5; cls++)
        {
            int count = 0;

            for (int i = 0; i < warriorList.Count; i++)
            {
                if ((int)warriorList[i].Class == cls)
                {
                    count++;
                }
            }

            Console.WriteLine(
                $"{(CharacterClass)cls}: {count}"
            );
        }
    }

    static int FindWarriorIndex(string code)
    {
        for (int i = 0; i < warriorList.Count; i++)
        {
            if (warriorList[i].Code.Equals(
                    code,
                    StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    static int FindTeamIndex(string code)
    {
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].Code.Equals(
                    code,
                    StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    static CharacterClass ReadCharacterClass()
    {
        Console.WriteLine("1. Warrior");
        Console.WriteLine("2. Archer");
        Console.WriteLine("3. Mage");
        Console.WriteLine("4. Tank");
        Console.WriteLine("5. Healer");

        int choice = ReadInteger(
            "Choose character class: ",
            1,
            5
        );

        return (CharacterClass)choice;
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

    static void Print()
    {
        Console.WriteLine(
            $"{"Code",-10} {"Name",-18} {"Class",-10} " +
            $"{"Health",-8} {"Attack",-10} " +
            $"{"Defense",-10} {"Speed",-8} " +
            $"{"Power"}"
        );
    }
}
