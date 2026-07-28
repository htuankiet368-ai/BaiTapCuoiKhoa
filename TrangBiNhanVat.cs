using System;
using System.Collections.Generic;

namespace KiemTra.CharacterEquipmentSystem;

public enum EquipmentSlot
{
    Weapon = 1,
    Helmet = 2,
    Armor = 3,
    Shoes = 4
}

public class Equipment
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public EquipmentSlot Slot { get; set; }
    public int BonusAttack { get; set; }
    public int BonusDefense { get; set; }
    public decimal Value { get; set; }

    public int CalculateEquipmentScore()
    {
        return BonusAttack + BonusDefense;
    }

    public void Display()
    {
        Console.WriteLine(
            $"{Code,-10} {Name,-20} {Slot,-10} " +
            $"{BonusAttack,-12} " +
            $"{BonusDefense,-12} " +
            $"{Value,-12:N0}"
        );
    }
}

public class CharacterEquipmentSystem
{
    static List<Equipment> inventory =
        new List<Equipment>();

    static List<Equipment> equippedItems =
        new List<Equipment>();

    static int baseAttack;
    static int baseDefense;

    public static void Run()
    {
        EnterCharacterStats();

        while (true)
        {
            DisplayMenu();

            int choice = InputInteger(
                "Choose an option: ",
                0,
                8
            );

            switch (choice)
            {
                case 1:
                    AddEquipment();
                    break;

                case 2:
                    DisplayInventory();
                    break;

                case 3:
                    EquipItem();
                    break;

                case 4:
                    UnequipItem();
                    break;

                case 5:
                    DisplayTotalStats();
                    break;

                case 6:
                    DisplayEquippedItems();
                    break;

                case 7:
                    FindBestEquipment();
                    break;

                case 8:
                    CalculateTotalAssetValue();
                    break;

                case 0:
                    return;
            }
        }
    }

    static void EnterCharacterStats()
    {
        Console.WriteLine(
            "\n===== ENTER CHARACTER STATS ====="
        );

        baseAttack = InputInteger(
            "Enter base attack: ",
            0,
            int.MaxValue
        );

        baseDefense = InputInteger(
            "Enter base defense: ",
            0,
            int.MaxValue
        );
    }

    static void DisplayMenu()
    {
        Console.WriteLine(
            "\n===== EQUIPMENT SYSTEM ====="
        );

        Console.WriteLine("1. Add equipment to inventory");
        Console.WriteLine("2. Display inventory");
        Console.WriteLine("3. Equip item");
        Console.WriteLine("4. Unequip item");
        Console.WriteLine("5. Display total character stats");
        Console.WriteLine("6. Display equipped items");
        Console.WriteLine("7. Find best equipment by slot");
        Console.WriteLine("8. Calculate total asset value");
        Console.WriteLine("0. Back");
    }

    static void AddEquipment()
    {
        string code = InputString(
            "Enter equipment code: "
        );

        if (CodeExists(code))
        {
            Console.WriteLine(
                "Equipment code already exists."
            );

            return;
        }

        Equipment equipment = new Equipment();

        equipment.Code = code;

        equipment.Name = InputString(
            "Enter equipment name: "
        );

        equipment.Slot = InputEquipmentSlot();

        equipment.BonusAttack =
            InputInteger(
                "Enter bonus attack: ",
                0,
                int.MaxValue
            );

        equipment.BonusDefense =
            InputInteger(
                "Enter bonus defense: ",
                0,
                int.MaxValue
            );

        equipment.Value = InputDecimal(
            "Enter equipment value: ",
            0
        );

        inventory.Add(equipment);

        Console.WriteLine(
            "Equipment added to inventory."
        );
    }

    static void DisplayInventory()
    {
        if (inventory.Count == 0)
        {
            Console.WriteLine(
                "Inventory is empty."
            );

            return;
        }

        Console.WriteLine(
            "\n===== INVENTORY ====="
        );

        PrintHeader();

        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].Display();
        }
    }

    static void EquipItem()
    {
        string code = InputString(
            "Enter equipment code from inventory: "
        );

        int inventoryIndex =
            FindIndexByCode(inventory, code);

        if (inventoryIndex == -1)
        {
            Console.WriteLine(
                "Equipment not found in inventory."
            );

            return;
        }

        Equipment newEquipment =
            inventory[inventoryIndex];

        int equippedIndex =
            FindBySlot(newEquipment.Slot);

        if (equippedIndex != -1)
        {
            Equipment oldEquipment =
                equippedItems[equippedIndex];

            inventory.Add(oldEquipment);

            equippedItems.RemoveAt(
                equippedIndex
            );

            Console.WriteLine(
                $"{oldEquipment.Name} was returned to the inventory."
            );
        }

        inventory.RemoveAt(inventoryIndex);

        equippedItems.Add(newEquipment);

        Console.WriteLine(
            $"{newEquipment.Name} has been equipped."
        );
    }

    static void UnequipItem()
    {
        if (equippedItems.Count == 0)
        {
            Console.WriteLine(
                "No equipment is currently equipped."
            );

            return;
        }

        EquipmentSlot slot =
            InputEquipmentSlot();

        int equippedIndex =
            FindBySlot(slot);

        if (equippedIndex == -1)
        {
            Console.WriteLine(
                "No equipment is equipped in this slot."
            );

            return;
        }

        Equipment equipment =
            equippedItems[equippedIndex];

        equippedItems.RemoveAt(
            equippedIndex
        );

        inventory.Add(equipment);

        Console.WriteLine(
            $"{equipment.Name} was moved back to the inventory."
        );
    }

    static void DisplayTotalStats()
    {
        int totalBonusAttack = 0;
        int totalBonusDefense = 0;

        for (int i = 0;
             i < equippedItems.Count;
             i++)
        {
            totalBonusAttack +=
                equippedItems[i].BonusAttack;

            totalBonusDefense +=
                equippedItems[i].BonusDefense;
        }

        int totalAttack =
            baseAttack +
            totalBonusAttack;

        int totalDefense =
            baseDefense +
            totalBonusDefense;

        Console.WriteLine(
            "\n===== TOTAL CHARACTER STATS ====="
        );

        Console.WriteLine(
            $"Attack: {baseAttack} + " +
            $"{totalBonusAttack} = {totalAttack}"
        );

        Console.WriteLine(
            $"Defense: {baseDefense} + " +
            $"{totalBonusDefense} = {totalDefense}"
        );
    }

    static void DisplayEquippedItems()
    {
        if (equippedItems.Count == 0)
        {
            Console.WriteLine(
                "No equipment is currently equipped."
            );

            return;
        }

        Console.WriteLine(
            "\n===== EQUIPPED ITEMS ====="
        );

        PrintHeader();

        for (int i = 0;
             i < equippedItems.Count;
             i++)
        {
            equippedItems[i].Display();
        }
    }

    static void FindBestEquipment()
    {
        List<Equipment> allEquipment =
            new List<Equipment>();

        for (int i = 0; i < inventory.Count; i++)
        {
            allEquipment.Add(inventory[i]);
        }

        for (int i = 0;
             i < equippedItems.Count;
             i++)
        {
            allEquipment.Add(
                equippedItems[i]
            );
        }

        if (allEquipment.Count == 0)
        {
            Console.WriteLine(
                "No equipment available."
            );

            return;
        }

        Console.WriteLine(
            "\n===== BEST EQUIPMENT ====="
        );

        for (int slot = 1;
             slot <= 4;
             slot++)
        {
            Equipment? bestEquipment = null;

            for (int i = 0;
                 i < allEquipment.Count;
                 i++)
            {
                Equipment equipment =
                    allEquipment[i];

                if ((int)equipment.Slot == slot)
                {
                    if (bestEquipment == null ||
                        equipment.CalculateEquipmentScore()
                        >
                        bestEquipment
                            .CalculateEquipmentScore())
                    {
                        bestEquipment =
                            equipment;
                    }
                }
            }

            Console.WriteLine(
                $"\nSlot {(EquipmentSlot)slot}:"
            );

            if (bestEquipment == null)
            {
                Console.WriteLine(
                    "No equipment available."
                );
            }
            else
            {
                PrintHeader();
                bestEquipment.Display();
            }
        }
    }

    static void CalculateTotalAssetValue()
    {
        decimal totalValue = 0;

        for (int i = 0; i < inventory.Count; i++)
        {
            totalValue += inventory[i].Value;
        }

        for (int i = 0;
             i < equippedItems.Count;
             i++)
        {
            totalValue +=
                equippedItems[i].Value;
        }

        Console.WriteLine(
            $"Total asset value: {totalValue:N0}"
        );
    }

    static int FindIndexByCode(
        List<Equipment> list,
        string code)
    {
        for (int i = 0;
             i < list.Count;
             i++)
        {
            if (list[i].Code.Equals(
                    code,
                    StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    static int FindBySlot(
        EquipmentSlot slot)
    {
        for (int i = 0;
             i < equippedItems.Count;
             i++)
        {
            if (equippedItems[i].Slot == slot)
            {
                return i;
            }
        }

        return -1;
    }

    static bool CodeExists(string code)
    {
        if (FindIndexByCode(inventory, code) != -1)
        {
            return true;
        }

        if (FindIndexByCode(
                equippedItems,
                code) != -1)
        {
            return true;
        }

        return false;
    }

    static EquipmentSlot InputEquipmentSlot()
    {
        Console.WriteLine("1. Weapon");
        Console.WriteLine("2. Helmet");
        Console.WriteLine("3. Armor");
        Console.WriteLine("4. Shoes");

        int choice = InputInteger(
            "Choose equipment slot: ",
            1,
            4
        );

        return (EquipmentSlot)choice;
    }

    static string InputString(string message)
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

    static int InputInteger(
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

    static decimal InputDecimal(
        string message,
        decimal minValue)
    {
        while (true)
        {
            Console.Write(message);

            decimal value;

            if (decimal.TryParse(
                    Console.ReadLine(),
                    out value)
                &&
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
            $"{"Code",-10} {"Name",-20} " +
            $"{"Slot",-10} {"Atk +",-12} " +
            $"{"Def +",-12} {"Value",-12}"
        );
    }
}
