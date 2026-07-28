using System;
using System.Collections.Generic;
using System.Linq;

class Monster
{
    public string Id;
    public string Name;
    public string Species;
    public int Level;
    public int Health;
    public int Damage;
    public int RewardExp;

    public int Power => Health + Damage * 2;

    public void LevelUp()
    {
        Level++;
        Health = (int)Math.Ceiling(Health * 1.10);
        Damage = (int)Math.Ceiling(Damage * 1.05);
        RewardExp = (int)Math.Ceiling(RewardExp * 1.08);
    }

    public void Display()
    {
        Console.WriteLine(
            $"{Id,-10} {Name,-18} {Species,-15} " +
            $"{Level,-8} {Health,-8} {Damage,-10} {RewardExp,-12} {Power}"
        );
    }
}

class Program
{
    static List<Monster> monsters = new List<Monster>();
    static Random rand = new Random();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n===== MONSTER =====");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Display");
            Console.WriteLine("3. Find by ID");
            Console.WriteLine("4. Find by Species");
            Console.WriteLine("5. Level Up");
            Console.WriteLine("6. Remove Dead");
            Console.WriteLine("7. Sort by Level");
            Console.WriteLine("8. Strongest");
            Console.WriteLine("9. Select by Player Level");
            Console.WriteLine("0. Exit");

            int c = ReadInt("Choose: ", 0, 9);

            switch (c)
            {
                case 1: Add(); break;
                case 2: Show(monsters); break;
                case 3: FindById(); break;
                case 4: FindBySpecies(); break;
                case 5: LevelUpMonster(); break;
                case 6: RemoveDead(); break;
                case 7: SortByLevel(); break;
                case 8: Strongest(); break;
                case 9: SelectByPlayerLevel(); break;
                case 0: return;
            }
        }
    }

    static void Add()
    {
        string id;
        do
        {
            id = ReadString("ID: ");
        } while (monsters.Any(m => m.Id.Equals(id, StringComparison.OrdinalIgnoreCase)));

        Monster m = new Monster
        {
            Id = id,
            Name = ReadString("Name: "),
            Species = ReadString("Species: "),
            Level = ReadInt("Level: ", 1, int.MaxValue),
            Health = ReadInt("Health: ", 0, int.MaxValue),
            Damage = ReadInt("Damage: ", 0, int.MaxValue),
            RewardExp = ReadInt("Reward EXP: ", 0, int.MaxValue)
        };

        monsters.Add(m);
    }

    static void Show(List<Monster> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("No monsters.");
            return;
        }

        Header();
        foreach (var m in list) m.Display();
    }

    static void FindById()
    {
        string id = ReadString("ID: ");

        var m = monsters.FirstOrDefault(x =>
            x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

        if (m == null)
        {
            Console.WriteLine("Not found.");
            return;
        }

        Header();
        m.Display();
    }

    static void FindBySpecies()
    {
        string key = ReadString("Species: ");

        var list = monsters
            .Where(m => m.Species.Contains(key, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (list.Count == 0)
        {
            Console.WriteLine("No match.");
            return;
        }

        Show(list);
    }

    static void LevelUpMonster()
    {
        string id = ReadString("ID: ");

        var m = monsters.FirstOrDefault(x =>
            x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

        if (m == null)
        {
            Console.WriteLine("Not found.");
            return;
        }

        int newLv = ReadInt("New level: ", m.Level, int.MaxValue);
        int diff = newLv - m.Level;

        for (int i = 0; i < diff; i++)
            m.LevelUp();

        Console.WriteLine($"+{diff} level(s)");
        Header();
        m.Display();
    }

    static void RemoveDead()
    {
        int removed = monsters.RemoveAll(m => m.Health == 0);
        Console.WriteLine($"Removed {removed}");
    }

    static void SortByLevel()
    {
        monsters = monsters
            .OrderByDescending(m => m.Level)
            .ToList();

        Console.WriteLine("Sorted.");
        Show(monsters);
    }

    static void Strongest()
    {
        if (monsters.Count == 0) return;

        int max = monsters.Max(m => m.Power);

        var list = monsters
            .Where(m => m.Power == max)
            .ToList();

        Header();
        foreach (var m in list) m.Display();
    }

    static void SelectByPlayerLevel()
    {
        if (monsters.Count == 0) return;

        int p = ReadInt("Player level: ", 1, int.MaxValue);

        int min = Math.Max(1, p - 2);
        int max = p + 2;

        var list = monsters
            .Where(m => m.Level >= min && m.Level <= max && m.Health > 0)
            .ToList();

        if (list.Count == 0)
        {
            Console.WriteLine("No suitable monsters.");
            return;
        }

        var chosen = list[rand.Next(list.Count)];

        Console.WriteLine($"Range: {min}-{max}");
        Header();
        chosen.Display();
    }

    // ---------- Helpers ----------

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

    static void Header()
    {
        Console.WriteLine(
            $"{"ID",-10} {"Name",-18} {"Species",-15} " +
            $"{"Level",-8} {"HP",-8} {"DMG",-10} {"EXP",-12} {"Power"}"
        );
    }
}
