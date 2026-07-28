using System;
using System.Collections.Generic;
using System.Linq;

enum WeaponType
{
    Sword = 1, Bow, Staff, Dagger, Axe
}

class Weapon
{
    public string Code;
    public string Name;
    public WeaponType Type;
    public int Damage;
    public decimal Price;
    public int Stock;

    public void Display()
    {
        Console.WriteLine(
            $"{Code,-8} {Name,-20} {Type,-10} " +
            $"{Damage,-10} {Price,-10:N0} {Stock}"
        );
    }
}

class InventoryItem
{
    public Weapon Weapon;
    public int Quantity;
}

class Player
{
    public string Name;
    public decimal Money;
    public List<InventoryItem> Inventory = new();
}

class Program
{
    static List<Weapon> store = new();
    static Player player = new();
    const decimal SELL_RATE = 0.6m;

    static void Main()
    {
        InitStore();
        CreatePlayer();

        while (true)
        {
            Menu();

            int c = ReadInt("Choose: ", 0, 6);

            switch (c)
            {
                case 1: ShowStore(); break;
                case 2: SearchWeapon(); break;
                case 3: Buy(); break;
                case 4: ShowInventory(); break;
                case 5: Sell(); break;
                case 6: StrongestAffordable(); break;
                case 0: return;
            }
        }
    }

    // ---------- Setup ----------

    static void InitStore()
    {
        store = new List<Weapon>
        {
            new() { Code="W01", Name="Iron Sword", Type=WeaponType.Sword, Damage=30, Price=300, Stock=5 },
            new() { Code="W02", Name="Wind Bow", Type=WeaponType.Bow, Damage=45, Price=500, Stock=4 },
            new() { Code="W03", Name="Fire Staff", Type=WeaponType.Staff, Damage=70, Price=800, Stock=2 },
            new() { Code="W04", Name="Dagger", Type=WeaponType.Dagger, Damage=25, Price=200, Stock=6 },
            new() { Code="W05", Name="Battle Axe", Type=WeaponType.Axe, Damage=90, Price=1200, Stock=1 }
        };
    }

    static void CreatePlayer()
    {
        Console.WriteLine("\n=== CREATE PLAYER ===");
        player.Name = ReadString("Name: ");
        player.Money = ReadDecimal("Money: ", 0);
    }

    // ---------- Menu ----------

    static void Menu()
    {
        Console.WriteLine("\n=== STORE ===");
        Console.WriteLine($"Player: {player.Name} | Money: {player.Money:N0}");
        Console.WriteLine("1. Store");
        Console.WriteLine("2. Search");
        Console.WriteLine("3. Buy");
        Console.WriteLine("4. Inventory");
        Console.WriteLine("5. Sell");
        Console.WriteLine("6. Strongest Affordable");
        Console.WriteLine("0. Exit");
    }

    // ---------- Features ----------

    static void ShowStore()
    {
        PrintHeader();
        store.ForEach(w => w.Display());
    }

    static void SearchWeapon()
    {
        string key = ReadString("Search: ");

        var result = store
            .Where(w => w.Name.Contains(key, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (result.Count == 0)
        {
            Console.WriteLine("Not found.");
            return;
        }

        PrintHeader();
        result.ForEach(w => w.Display());
    }

    static void Buy()
    {
        string code = ReadString("Code: ");

        var w = store.Find(x =>
            x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (w == null)
        {
            Console.WriteLine("Not found.");
            return;
        }

        if (w.Stock <= 0)
        {
            Console.WriteLine("Out of stock.");
            return;
        }

        if (player.Money < w.Price)
        {
            Console.WriteLine("Not enough money.");
            return;
        }

        player.Money -= w.Price;
        w.Stock--;

        var item = player.Inventory
            .Find(i => i.Weapon.Code == w.Code);

        if (item != null)
            item.Quantity++;
        else
            player.Inventory.Add(new InventoryItem { Weapon = w, Quantity = 1 });

        Console.WriteLine($"Bought {w.Name}");
    }

    static void ShowInventory()
    {
        if (player.Inventory.Count == 0)
        {
            Console.WriteLine("Empty.");
            return;
        }

        Console.WriteLine("\nCode     Name                 Qty   Sell");
        foreach (var i in player.Inventory)
        {
            decimal sell = i.Weapon.Price * SELL_RATE;

            Console.WriteLine(
                $"{i.Weapon.Code,-8} {i.Weapon.Name,-20} {i.Quantity,-5} {sell:N0}"
            );
        }
    }

    static void Sell()
    {
        string code = ReadString("Code: ");

        var item = player.Inventory
            .Find(i => i.Weapon.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            Console.WriteLine("You don't own this.");
            return;
        }

        decimal sell = item.Weapon.Price * SELL_RATE;

        player.Money += sell;
        item.Quantity--;
        item.Weapon.Stock++;

        if (item.Quantity == 0)
            player.Inventory.Remove(item);

        Console.WriteLine($"Sold {item.Weapon.Name} (+{sell:N0})");
    }

    static void StrongestAffordable()
    {
        var w = store
            .Where(x => x.Stock > 0 && x.Price <= player.Money)
            .OrderByDescending(x => x.Damage)
            .FirstOrDefault();

        if (w == null)
        {
            Console.WriteLine("No weapon available.");
            return;
        }

        PrintHeader();
        w.Display();
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
            $"{"Code",-8} {"Name",-20} {"Type",-10} {"Dmg",-10} {"Price",-10} {"Stock"}"
        );
    }
}
