using System;
using System.Collections.Generic;
using System.Linq;

enum EquipmentSlot
{
    Weapon = 1,
    Helmet,
    Armor,
    Shoes
}

class Equipment
{
    public string Code;
    public string Name;
    public EquipmentSlot Slot;
    public int BonusAttack;
    public int BonusDefense;
    public decimal Value;

    public int Score => BonusAttack + BonusDefense;

    public void Display()
    {
        Console.WriteLine(
            $"{Code,-10} {Name,-20} {Slot,-10} " +
            $"{BonusAttack,-10} {BonusDefense,-10} {Value,-10:N0}"
        );
    }
}

class Program
{
    static List<Equipment> inventory = new();
    static List<Equipment> equipped = new();

    static int baseAtk, baseDef;

    static void Main()
    {
        baseAtk = ReadInt("Base Attack: ", 0, int.MaxValue);
        baseDef = ReadInt("Base Defense: ", 0, int.MaxValue);

        while (true)
        {
            Console.WriteLine("\n===== EQUIPMENT =====");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Inventory");
            Console.WriteLine("3. Equip");
            Console.WriteLine("4. Unequip");
            Console.WriteLine("5. Total Stats");
            Console.WriteLine("6. Equipped");
            Console.WriteLine("7. Best Gear");
            Console.WriteLine("8. Total Value");
            Console.WriteLine("0. Exit");

            int c = ReadInt("Choose: ", 0, 8);

            switch (c)
            {
                case 1: Add(); break;
                case 2: Show(inventory); break;
                case 3: Equip(); break;
                case 4: Unequip(); break;
                case 5: TotalStats(); break;
                case 6: Show(equipped); break;
                case 7: BestGear(); break;
                case 8: TotalValue(); break;
                case 0: return;
            }
        }
    }

    // ---------- Core ----------

    static void Add()
    {
        string code;
        do
        {
            code = ReadString("Code: ");
        } while (Exists(code));

        Equipment e = new()
        {
            Code = code,
            Name = ReadString("Name: "),
            Slot = ReadSlot(),
            BonusAttack = ReadInt("Atk+: ", 0, int.MaxValue),
            BonusDefense = ReadInt("Def+: ", 0, int.MaxValue),
            Value = ReadDecimal("Value: ", 0)
        };

        inventory.Add(e);
    }

    static void Equip()
    {
        string code = ReadString("Code: ");

        var item = inventory
            .FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            Console.WriteLine("Not found.");
            return;
        }

        var sameSlot = equipped
            .FirstOrDefault(x => x.Slot == item.Slot);

        if (sameSlot != null)
        {
            inventory.Add(sameSlot);
            equipped.Remove(sameSlot);
        }

        inventory.Remove(item);
        equipped.Add(item);

        Console.WriteLine("Equipped.");
    }

    static void Unequip()
    {
        if (equipped.Count == 0)
        {
            Console.WriteLine("Nothing equipped.");
            return;
        }

        var slot = ReadSlot();

        var item = equipped
            .FirstOrDefault(x => x.Slot == slot);

        if (item == null)
        {
            Console.WriteLine("No item in that slot.");
            return;
        }

        equipped.Remove(item);
        inventory.Add(item);
    }

    static void TotalStats()
    {
        int bonusAtk = equipped.Sum(x => x.BonusAttack);
        int bonusDef = equipped.Sum(x => x.BonusDefense);

        Console.WriteLine($"Attack: {baseAtk} + {bonusAtk} = {baseAtk + bonusAtk}");
        Console.WriteLine($"Defense: {baseDef} + {bonusDef} = {baseDef + bonusDef}");
    }

    static void BestGear()
    {
        var all = inventory.Concat(equipped);

        if (!all.Any())
        {
            Console.WriteLine("No gear.");
            return;
        }

        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            var best = all
                .Where(x => x.Slot == slot)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            Console.WriteLine($"\n{slot}:");

            if (best == null)
                Console.WriteLine("None");
            else
            {
                PrintHeader();
                best.Display();
            }
        }
    }

    static void TotalValue()
    {
        decimal total =
            inventory.Sum(x => x.Value) +
            equipped.Sum(x => x.Value);

        Console.WriteLine($"Total value: {total:N0}");
    }

    // ---------- Helpers ----------

    static void Show(List<Equipment> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("Empty.");
            return;
        }

        PrintHeader();
        list.ForEach(x => x.Display());
    }

    static bool Exists(string code)
    {
        return inventory.Any(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)) ||
               equipped.Any(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    static EquipmentSlot ReadSlot()
    {
        Console.WriteLine("1. Weapon  2. Helmet  3. Armor  4. Shoes");
        return (EquipmentSlot)ReadInt("Slot: ", 1, 4);
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

    static decimal ReadDecimal(string msg, decimal min)
    {
        decimal x;
        while (true)
        {
            Console.Write(msg);
            if (decimal.TryParse(Console.ReadLine(), out x) && x >= min)
                return x;
        }
    }

    static void PrintHeader()
    {
        Console.WriteLine(
            $"{"Code",-10} {"Name",-20} {"Slot",-10} " +
            $"{"Atk+",-10} {"Def+",-10} {"Value",-10}"
        );
    }
}
