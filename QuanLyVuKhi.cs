using System;
using System.Collections.Generic;

namespace KiemTra.WeaponStoreManagement;

public enum WeaponType
{
    Sword = 1,
    Bow = 2,
    Staff = 3,
    Dagger = 4,
    Axe = 5
}

public class Weapon
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public WeaponType Type { get; set; }
    public int Damage { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public void Display()
    {
        Console.WriteLine(
            $"{Code,-8} {Name,-20} {Type,-10} " +
            $"{Damage,-12} {Price,-12:N0} {StockQuantity}"
        );
    }
}

public class InventoryWeapon
{
    public Weapon Weapon { get; set; } = new Weapon();
    public int Quantity { get; set; }
}

public class Player
{
    public string Name { get; set; } = "";
    public decimal Money { get; set; }

    public List<InventoryWeapon> Inventory { get; set; }
        = new List<InventoryWeapon>();
}

public class WeaponStoreManager
{
    static List<Weapon> store = new List<Weapon>();
    static Player player = new Player();

    public static void Run()
    {
        InitializeStore();
        CreatePlayer();

        while (true)
        {
            DisplayMenu();

            int choice = ReadInteger(
                "Choose an option: ",
                0,
                6
            );

            switch (choice)
            {
                case 1:
                    DisplayStore();
                    break;

                case 2:
                    SearchWeaponByName();
                    break;

                case 3:
                    BuyWeapon();
                    break;

                case 4:
                    DisplayInventory();
                    break;

                case 5:
                    SellWeapon();
                    break;

                case 6:
                    FindStrongestAffordableWeapon();
                    break;

                case 0:
                    return;
            }
        }
    }

    static void InitializeStore()
    {
        // Clear previous data before restarting
        store.Clear();

        store.Add(new Weapon
        {
            Code = "W01",
            Name = "Iron Sword",
            Type = WeaponType.Sword,
            Damage = 30,
            Price = 300,
            StockQuantity = 5
        });

        store.Add(new Weapon
        {
            Code = "W02",
            Name = "Wind Bow",
            Type = WeaponType.Bow,
            Damage = 45,
            Price = 500,
            StockQuantity = 4
        });

        store.Add(new Weapon
        {
            Code = "W03",
            Name = "Fire Staff",
            Type = WeaponType.Staff,
            Damage = 70,
            Price = 800,
            StockQuantity = 2
        });

        store.Add(new Weapon
        {
            Code = "W04",
            Name = "Dagger",
            Type = WeaponType.Dagger,
            Damage = 25,
            Price = 200,
            StockQuantity = 6
        });

        store.Add(new Weapon
        {
            Code = "W05",
            Name = "Battle Axe",
            Type = WeaponType.Axe,
            Damage = 90,
            Price = 1200,
            StockQuantity = 1
        });
    }

    static void CreatePlayer()
    {
        Console.WriteLine("\n===== CREATE PLAYER =====");

        player = new Player();

        player.Name = ReadString(
            "Enter player name: "
        );

        player.Money = ReadDecimal(
            "Enter starting money: ",
            0
        );
    }

    static void DisplayMenu()
    {
        Console.WriteLine(
            $"\n===== WEAPON STORE ====="
        );

        Console.WriteLine(
            $"Player: {player.Name}"
        );

        Console.WriteLine(
            $"Money: {player.Money:N0}"
        );

        Console.WriteLine("1. Display Store");
        Console.WriteLine("2. Search Weapon by Name");
        Console.WriteLine("3. Buy Weapon");
        Console.WriteLine("4. Display Inventory");
        Console.WriteLine("5. Sell Weapon");
        Console.WriteLine("6. Strongest Affordable Weapon");
        Console.WriteLine("0. Exit");
    }

    static void DisplayStore()
    {
        Console.WriteLine("\n===== WEAPON LIST =====");

        PrintHeader();

        for (int i = 0; i < store.Count; i++)
        {
            store[i].Display();
        }
    }

    static void SearchWeaponByName()
    {
        string keyword = ReadString(
            "Enter weapon name or part of the name: "
        );

        bool found = false;

        PrintHeader();

        for (int i = 0; i < store.Count; i++)
        {
            if (store[i].Name.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase))
            {
                store[i].Display();
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Weapon not found.");
        }
    }

    static void BuyWeapon()
    {
        string code = ReadString(
            "Enter weapon code to buy: "
        );

        int index = FindWeaponInStore(code);

        if (index == -1)
        {
            Console.WriteLine(
                "Weapon code does not exist."
            );

            return;
        }

        Weapon weapon = store[index];

        if (weapon.StockQuantity <= 0)
        {
            Console.WriteLine("Weapon is out of stock.");
            return;
        }

        if (player.Money < weapon.Price)
        {
            Console.WriteLine(
                "Player does not have enough money."
            );

            return;
        }

        // Deduct money
        player.Money -= weapon.Price;

        // Reduce store stock
        weapon.StockQuantity--;

        int inventoryIndex =
            FindWeaponInInventory(code);

        // Increase quantity if already owned
        if (inventoryIndex != -1)
        {
            player.Inventory[inventoryIndex]
                .Quantity++;
        }
        else
        {
            InventoryWeapon newWeapon =
                new InventoryWeapon();

            newWeapon.Weapon = weapon;
            newWeapon.Quantity = 1;

            player.Inventory.Add(newWeapon);
        }

        Console.WriteLine(
            $"Successfully purchased {weapon.Name}."
        );

        Console.WriteLine(
            $"Remaining money: {player.Money:N0}"
        );
    }

    static void DisplayInventory()
    {
        if (player.Inventory.Count == 0)
        {
            Console.WriteLine("Inventory is empty.");
            return;
        }

        Console.WriteLine("\n===== INVENTORY =====");

        Console.WriteLine(
            $"{"Code",-8} {"Name",-20} " +
            $"{"Quantity",-12} {"Sell Price"}"
        );

        for (int i = 0;
             i < player.Inventory.Count;
             i++)
        {
            InventoryWeapon item =
                player.Inventory[i];

            decimal sellPrice =
                item.Weapon.Price * 60 / 100;

            Console.WriteLine(
                $"{item.Weapon.Code,-8} " +
                $"{item.Weapon.Name,-20} " +
                $"{item.Quantity,-12} " +
                $"{sellPrice:N0}"
            );
        }
    }

    static void SellWeapon()
    {
        string code = ReadString(
            "Enter weapon code to sell: "
        );

        int inventoryIndex =
            FindWeaponInInventory(code);

        if (inventoryIndex == -1)
        {
            Console.WriteLine(
                "Player does not own this weapon."
            );

            return;
        }

        InventoryWeapon item =
            player.Inventory[inventoryIndex];

        decimal sellPrice =
            item.Weapon.Price * 60 / 100;

        // Add money to player
        player.Money += sellPrice;

        // Reduce inventory quantity
        item.Quantity--;

        // Increase store stock
        item.Weapon.StockQuantity++;

        // Remove if quantity reaches zero
        if (item.Quantity == 0)
        {
            player.Inventory.RemoveAt(
                inventoryIndex
            );
        }

        Console.WriteLine(
            $"Sold {item.Weapon.Name}."
        );

        Console.WriteLine(
            $"Received: {sellPrice:N0}"
        );

        Console.WriteLine(
            $"Current money: {player.Money:N0}"
        );
    }

    static void FindStrongestAffordableWeapon()
    {
        Weapon? strongestWeapon = null;

        for (int i = 0; i < store.Count; i++)
        {
            Weapon weapon = store[i];

            bool inStock = weapon.StockQuantity > 0;

            bool affordable =
                weapon.Price <= player.Money;

            if (inStock && affordable)
            {
                if (strongestWeapon == null ||
                    weapon.Damage >
                    strongestWeapon.Damage)
                {
                    strongestWeapon = weapon;
                }
            }
        }

        if (strongestWeapon == null)
        {
            Console.WriteLine(
                "There are no weapons in stock that you can afford."
            );

            return;
        }

        Console.WriteLine(
            "The strongest weapon you can afford:"
        );

        PrintHeader();
        strongestWeapon.Display();
    }

    static int FindWeaponInStore(
        string code)
    {
        for (int i = 0; i < store.Count; i++)
        {
            if (store[i].Code.Equals(
                    code,
                    StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    static int FindWeaponInInventory(
        string code)
    {
        for (int i = 0;
             i < player.Inventory.Count;
             i++)
        {
            if (player.Inventory[i]
                .Weapon.Code.Equals(
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
                    out value) &&
                value >= minValue &&
                value <= maxValue)
            {
                return value;
            }

            Console.WriteLine(
                "Invalid input."
            );
        }
    }

    static decimal ReadDecimal(
        string message,
        decimal minValue)
    {
        while (true)
        {
            Console.Write(message);

            decimal value;

            if (decimal.TryParse(
                    Console.ReadLine(),
                    out value) &&
                value >= minValue)
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
            $"{"Code",-8} {"Name",-20} " +
            $"{"Type",-10} {"Damage",-12} " +
            $"{"Price",-12} {"Stock"}"
        );
    }
}
