using System;
using System.Collections.Generic;

namespace KiemTra.MonsterArmyManagement;

public class Monster
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Species { get; set; } = "";
    public int Level { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int RewardExperience { get; set; }

    public int CalculatePower()
    {
        return Health + Damage * 2;
    }

    public void LevelUpOnce()
    {
        Level++;

        Health = (int)Math.Ceiling(
            Health * 1.10
        );

        Damage = (int)Math.Ceiling(
            Damage * 1.05
        );

        RewardExperience = (int)Math.Ceiling(
            RewardExperience * 1.08
        );
    }

    public void Display()
    {
        Console.WriteLine(
            $"{Id,-10} {Name,-18} {Species,-15} " +
            $"{Level,-8} {Health,-8} " +
            $"{Damage,-12} " +
            $"{RewardExperience,-12} " +
            $"{CalculatePower()}"
        );
    }
}

public class MonsterArmyManagement
{
    static List<Monster> monsterList
        = new List<Monster>();

    static Random random = new Random();

    public static void Run()
    {
        while (true)
        {
            DisplayMenu();

            int choice = ReadInteger(
                "Choose a function: ",
                0,
                9
            );

            switch (choice)
            {
                case 1:
                    AddMonster();
                    break;

                case 2:
                    DisplayMonsterList();
                    break;

                case 3:
                    FindMonsterById();
                    break;

                case 4:
                    FindMonsterBySpecies();
                    break;

                case 5:
                    UpdateMonsterLevel();
                    break;

                case 6:
                    RemoveDeadMonsters();
                    break;

                case 7:
                    SortByLevelDescending();
                    break;

                case 8:
                    DisplayStrongestMonster();
                    break;

                case 9:
                    SelectMonsterByPlayerLevel();
                    break;

                case 0:
                    return;
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine(
            "\n===== MONSTER ARMY MANAGEMENT ====="
        );

        Console.WriteLine("1. Add Monster");
        Console.WriteLine("2. Display Monster List");
        Console.WriteLine("3. Find Monster by ID");
        Console.WriteLine("4. Find Monster by Species");
        Console.WriteLine("5. Update Monster Level");
        Console.WriteLine("6. Remove Monsters with 0 Health");
        Console.WriteLine("7. Sort by Level (Descending)");
        Console.WriteLine("8. Display Strongest Monster");
        Console.WriteLine("9. Select Monster Based on Player Level");
        Console.WriteLine("0. Exit");
    }

    static void AddMonster()
    {
        string id = ReadString(
            "Enter monster ID: "
        );

        if (FindIndexById(id) != -1)
        {
            Console.WriteLine(
                "Monster ID already exists."
            );

            return;
        }

        Monster monster = new Monster();

        monster.Id = id;

        monster.Name = ReadString(
            "Enter monster name: "
        );

        monster.Species = ReadString(
            "Enter monster species: "
        );

        monster.Level = ReadInteger(
            "Enter level: ",
            1,
            int.MaxValue
        );

        monster.Health = ReadInteger(
            "Enter health: ",
            0,
            int.MaxValue
        );

        monster.Damage = ReadInteger(
            "Enter damage: ",
            0,
            int.MaxValue
        );

        monster.RewardExperience =
            ReadInteger(
                "Enter reward experience: ",
                0,
                int.MaxValue
            );

        monsterList.Add(monster);

        Console.WriteLine(
            "Monster added successfully."
        );
    }

    static void DisplayMonsterList()
    {
        if (monsterList.Count == 0)
        {
            Console.WriteLine(
                "The monster list is empty."
            );

            return;
        }

        Console.WriteLine(
            "\n===== MONSTER LIST ====="
        );

        PrintHeader();

        for (int i = 0; i < monsterList.Count; i++)
        {
            monsterList[i].Display();
        }
    }

    static void FindMonsterById()
    {
        string id = ReadString(
            "Enter monster ID to search: "
        );

        int index = FindIndexById(id);

        if (index == -1)
        {
            Console.WriteLine(
                "Monster not found."
            );

            return;
        }

        Console.WriteLine(
            "Monster found:"
        );

        PrintHeader();
        monsterList[index].Display();
    }

    static void FindMonsterBySpecies()
    {
        string keyword = ReadString(
            "Enter species to search: "
        );

        bool found = false;

        PrintHeader();

        for (int i = 0; i < monsterList.Count; i++)
        {
            if (monsterList[i].Species.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase))
            {
                monsterList[i].Display();
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine(
                "No monsters of this species found."
            );
        }
    }

    static void UpdateMonsterLevel()
    {
        string id = ReadString(
            "Enter monster ID: "
        );

        int index = FindIndexById(id);

        if (index == -1)
        {
            Console.WriteLine(
                "Monster not found."
            );

            return;
        }

        Monster monster = monsterList[index];

        Console.WriteLine(
            $"Current level: {monster.Level}"
        );

        int newLevel = ReadInteger(
            "Enter new level: ",
            monster.Level,
            int.MaxValue
        );

        int levelsGained =
            newLevel - monster.Level;

        if (levelsGained == 0)
        {
            Console.WriteLine(
                "Level unchanged."
            );

            return;
        }

        for (int i = 0; i < levelsGained; i++)
        {
            monster.LevelUpOnce();
        }

        Console.WriteLine(
            $"Increased by {levelsGained} level(s)."
        );

        Console.WriteLine(
            "Health increases by 10% per level."
        );

        Console.WriteLine(
            "Damage increases by 5% per level."
        );

        Console.WriteLine(
            "Reward experience increases by 8% per level."
        );

        PrintHeader();
        monster.Display();
    }

    static void RemoveDeadMonsters()
    {
        int removedCount = 0;

        for (int i = monsterList.Count - 1;
             i >= 0;
             i--)
        {
            if (monsterList[i].Health == 0)
            {
                monsterList.RemoveAt(i);
                removedCount++;
            }
        }

        Console.WriteLine(
            $"Removed {removedCount} monster(s)."
        );
    }

    static void SortByLevelDescending()
    {
        if (monsterList.Count == 0)
        {
            Console.WriteLine(
                "The monster list is empty."
            );

            return;
        }

        for (int i = 0;
             i < monsterList.Count - 1;
             i++)
        {
            for (int j = i + 1;
                 j < monsterList.Count;
                 j++)
            {
                if (monsterList[i].Level
                    < monsterList[j].Level)
                {
                    Monster temp = monsterList[i];
                    monsterList[i] = monsterList[j];
                    monsterList[j] = temp;
                }
            }
        }

        Console.WriteLine(
            "Sorted by level in descending order."
        );

        DisplayMonsterList();
    }

    static void DisplayStrongestMonster()
    {
        if (monsterList.Count == 0)
        {
            Console.WriteLine(
                "The monster list is empty."
            );

            return;
        }

        int highestPower =
            monsterList[0].CalculatePower();

        for (int i = 1;
             i < monsterList.Count;
             i++)
        {
            if (monsterList[i].CalculatePower()
                > highestPower)
            {
                highestPower =
                    monsterList[i].CalculatePower();
            }
        }

        Console.WriteLine(
            "\n===== STRONGEST MONSTER ====="
        );

        PrintHeader();

        for (int i = 0;
             i < monsterList.Count;
             i++)
        {
            if (monsterList[i].CalculatePower()
                == highestPower)
            {
                monsterList[i].Display();
            }
        }
    }

    static void SelectMonsterByPlayerLevel()
    {
        if (monsterList.Count == 0)
        {
            Console.WriteLine(
                "The monster list is empty."
            );

            return;
        }

        int playerLevel = ReadInteger(
            "Enter player level: ",
            1,
            int.MaxValue
        );

        int minLevel = playerLevel - 2;

        if (minLevel < 1)
        {
            minLevel = 1;
        }

        int maxLevel =
            playerLevel + 2;

        List<Monster> suitableMonsters =
            new List<Monster>();

        for (int i = 0;
             i < monsterList.Count;
             i++)
        {
            bool levelMatch =
                monsterList[i].Level >= minLevel
                &&
                monsterList[i].Level <= maxLevel;

            bool alive =
                monsterList[i].Health > 0;

            if (levelMatch && alive)
            {
                suitableMonsters.Add(
                    monsterList[i]
                );
            }
        }

        if (suitableMonsters.Count == 0)
        {
            Console.WriteLine(
                "No suitable monsters found."
            );

            return;
        }

        int randomIndex =
            random.Next(suitableMonsters.Count);

        Monster selectedMonster =
            suitableMonsters[randomIndex];

        Console.WriteLine(
            $"Suitable level range: " +
            $"{minLevel} to {maxLevel}"
        );

        Console.WriteLine(
            "Randomly selected monster:"
        );

        PrintHeader();
        selectedMonster.Display();
    }

    static int FindIndexById(string id)
    {
        for (int i = 0;
             i < monsterList.Count;
             i++)
        {
            if (monsterList[i].Id.Equals(
                    id,
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
            $"{"ID",-10} {"Name",-18} {"Species",-15} " +
            $"{"Level",-8} {"Health",-8} " +
            $"{"Damage",-12} {"Reward EXP",-12} " +
            $"{"Power"}"
        );
    }
}
