using System;
using System.Collections.Generic;
using System.Linq;


enum CharacterClass
{
    Warrior = 1,
    Archer,
    Mage,
    Tank,
    Healer
}

class Warrior
{
    public string Code;
    public string Name;
    public CharacterClass Class;
    public int Health;
    public int Attack;
    public int Defense;
    public int Speed;

    public int Power => Health + Attack * 2 + Defense + Speed;

    public void Display()
    {
        Console.WriteLine(
            $"{Code,-10} {Name,-18} {Class,-10} " +
            $"{Health,-8} {Attack,-10} {Defense,-10} {Speed,-8} {Power}"
        );
    }
}

class Program
{
    const int MAX = 5;

    static List<Warrior> warriors = new();
    static List<Warrior> team = new();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n===== BATTLE TEAM =====");
            Console.WriteLine("1. Add Warrior");
            Console.WriteLine("2. Display Warriors");
            Console.WriteLine("3. Add to Team");
            Console.WriteLine("4. Display Team");
            Console.WriteLine("5. Remove from Team");
            Console.WriteLine("6. Team Stats");
            Console.WriteLine("7. Strongest Warrior");
            Console.WriteLine("8. Sort Team by Speed");
            Console.WriteLine("9. Check Battle");
            Console.WriteLine("10. Count by Class");
            Console.WriteLine("0. Exit");

            int c = ReadInt("Choose: ", 0, 10);

            switch (c)
            {
                case 1: AddWarrior(); break;
                case 2: Display(warriors); break;
                case 3: AddToTeam(); break;
                case 4: Display(team); break;
                case 5: RemoveFromTeam(); break;
                case 6: TeamStats(); break;
                case 7: Strongest(); break;
                case 8: SortTeam(); break;
                case 9: CheckBattle(); break;
                case 10: CountByClass(); break;
                case 0: return;
            }
        }
    }

    // ---------- Core ----------

    static void AddWarrior()
    {
        string code;
        do
        {
            code = ReadString("Code: ");
        } while (warriors.Any(w => w.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));

        warriors.Add(new Warrior
        {
            Code = code,
            Name = ReadString("Name: "),
            Class = ReadClass(),
            Health = ReadInt("Health: ", 0, int.MaxValue),
            Attack = ReadInt("Attack: ", 0, int.MaxValue),
            Defense = ReadInt("Defense: ", 0, int.MaxValue),
            Speed = ReadInt("Speed: ", 0, int.MaxValue)
        });
    }

    static void AddToTeam()
    {
        if (team.Count >= MAX)
        {
            Console.WriteLine("Team full.");
            return;
        }

        string code = ReadString("Code: ");
        var w = warriors.FirstOrDefault(x =>
            x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (w == null || team.Contains(w))
        {
            Console.WriteLine("Invalid or duplicate.");
            return;
        }

        team.Add(w);
    }

    static void RemoveFromTeam()
    {
        string code = ReadString("Code: ");
        team.RemoveAll(w =>
            w.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    static void Display(List<Warrior> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("Empty.");
            return;
        }

        PrintHeader();
        foreach (var w in list)
            w.Display();
    }

    // ---------- Features ----------

    static void TeamStats()
    {
        Console.WriteLine("\n===== TEAM STATS =====");

        Console.WriteLine($"Members: {team.Count}/{MAX}");
        Console.WriteLine($"Health: {team.Sum(w => w.Health)}");
        Console.WriteLine($"Attack: {team.Sum(w => w.Attack)}");
        Console.WriteLine($"Defense: {team.Sum(w => w.Defense)}");
        Console.WriteLine($"Speed: {team.Sum(w => w.Speed)}");
    }

    static void Strongest()
    {
        if (warriors.Count == 0) return;

        int max = warriors.Max(w => w.Power);

        Display(warriors.Where(w => w.Power == max).ToList());
    }

    static void SortTeam()
    {
        team = team
            .OrderByDescending(w => w.Speed)
            .ToList();

        Display(team);
    }

    static void CheckBattle()
    {
        int hp = team.Sum(w => w.Health);
        int atk = team.Sum(w => w.Attack);

        Console.WriteLine("\n===== CHECK =====");
        Console.WriteLine($"Members: {team.Count}/3");
        Console.WriteLine($"Health: {hp}/1000");
        Console.WriteLine($"Attack: {atk}/300");

        bool ok = team.Count >= 3 && hp >= 1000 && atk >= 300;

        Console.WriteLine(ok ? "Ready!" : "Not ready.");
    }

    static void CountByClass()
    {
        Console.WriteLine("\n===== BY CLASS =====");

        var groups = warriors.GroupBy(w => w.Class);

        foreach (var g in groups)
            Console.WriteLine($"{g.Key}: {g.Count()}");
    }

    // ---------- Helpers ----------

    static CharacterClass ReadClass()
    {
        Console.WriteLine("1.Warrior 2.Archer 3.Mage 4.Tank 5.Healer");
        return (CharacterClass)ReadInt("Class: ", 1, 5);
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
            $"{"Code",-10} {"Name",-18} {"Class",-10} " +
            $"{"HP",-8} {"ATK",-10} {"DEF",-10} {"SPD",-8} {"Power"}"
        );
    }
}
