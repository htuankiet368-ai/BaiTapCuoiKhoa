using System;

namespace KiemTra.TurnBasedBattleGame;

public class Player
{
    public string Name { get; set; } = "";
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int PotionCount { get; set; }
    public int Gold { get; set; }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    public void Display()
    {
        Console.WriteLine("\n===== PLAYER =====");
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Health: {CurrentHealth}/{MaxHealth}");
        Console.WriteLine($"Attack: {Attack}");
        Console.WriteLine($"Defense: {Defense}");
        Console.WriteLine($"Potions: {PotionCount}");
        Console.WriteLine($"Gold: {Gold}");
    }
}

public class Monster
{
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int GoldReward { get; set; }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public void Display()
    {
        Console.WriteLine("\n===== MONSTER =====");
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Health: {Math.Max(Health, 0)}");
        Console.WriteLine($"Attack: {Attack}");
        Console.WriteLine($"Defense: {Defense}");
        Console.WriteLine($"Gold Reward: {GoldReward}");
    }
}

public class TurnBasedBattleGame
{
    static Random random = new Random();

    static Player player = new Player();

    static int wins;
    static int losses;
    static int escapes;

    public static void Run()
    {
        CreatePlayer();

        while (true)
        {
            player.CurrentHealth = player.MaxHealth;

            Monster monster = CreateRandomMonster();

            Console.WriteLine(
                $"\nA {monster.Name} has appeared!"
            );

            Battle(monster);

            if (!AskYesNo("\nDo you want to play another battle?"))
            {
                DisplayStatistics();
                return;
            }
        }
    }

    static void CreatePlayer()
    {
        Console.WriteLine("\n===== CREATE PLAYER =====");

        player = new Player();

        player.Name = InputString(
            "Enter player name: "
        );

        player.MaxHealth = InputInteger(
            "Enter maximum health: ",
            1,
            int.MaxValue
        );

        player.CurrentHealth = player.MaxHealth;

        player.Attack = InputInteger(
            "Enter attack power: ",
            1,
            int.MaxValue
        );

        player.Defense = InputInteger(
            "Enter defense: ",
            0,
            int.MaxValue
        );

        player.PotionCount = InputInteger(
            "Enter number of healing potions: ",
            0,
            int.MaxValue
        );

        player.Gold = 0;

        wins = 0;
        losses = 0;
        escapes = 0;
    }

    static Monster CreateRandomMonster()
    {
        string[] monsterNames =
        {
            "Slime",
            "Goblin",
            "Black Wolf",
            "Orc",
            "Skeleton"
        };

        Monster monster = new Monster();

        monster.Name =
            monsterNames[random.Next(monsterNames.Length)];

        monster.Health = random.Next(50, 101);
        monster.Attack = random.Next(10, 26);
        monster.Defense = random.Next(0, 11);
        monster.GoldReward = random.Next(20, 51);

        return monster;
    }

    static void Battle(Monster monster)
    {
        while (player.IsAlive() && monster.IsAlive())
        {
            DisplayBattleMenu();

            int choice = InputInteger(
                "Choose an action: ",
                1,
                4
            );

            bool turnUsed = false;

            switch (choice)
            {
                case 1:
                    PlayerAttack(monster);
                    turnUsed = true;
                    break;

                case 2:
                    turnUsed = UsePotion();
                    break;

                case 3:
                    player.Display();
                    monster.Display();
                    break;

                case 4:
                    Console.WriteLine(
                        "You fled from the battle."
                    );

                    escapes++;
                    return;
            }

            if (!monster.IsAlive())
            {
                Console.WriteLine(
                    $"\nYou defeated the {monster.Name}!"
                );

                Console.WriteLine(
                    $"You received {monster.GoldReward} gold."
                );

                player.Gold += monster.GoldReward;
                wins++;

                return;
            }

            if (turnUsed)
            {
                MonsterAttack(monster);
            }

            if (!player.IsAlive())
            {
                Console.WriteLine(
                    "\nThe player has been defeated."
                );

                losses++;
                return;
            }
        }
    }

    static void PlayerAttack(Monster monster)
    {
        int damage =
            player.Attack - monster.Defense;

        if (damage < 1)
        {
            damage = 1;
        }

        int chance = random.Next(1, 101);
        bool criticalHit = chance <= 20;

        if (criticalHit)
        {
            damage *= 2;

            Console.WriteLine(
                $"Critical hit! You dealt {damage} damage."
            );
        }
        else
        {
            Console.WriteLine(
                $"You dealt {damage} damage."
            );
        }

        monster.Health -= damage;

        Console.WriteLine(
            $"{monster.Name}'s Health: " +
            $"{Math.Max(monster.Health, 0)}"
        );
    }

    static void MonsterAttack(Monster monster)
    {
        int damage =
            monster.Attack - player.Defense;

        if (damage < 1)
        {
            damage = 1;
        }

        player.CurrentHealth -= damage;

        Console.WriteLine(
            $"{monster.Name} dealt {damage} damage."
        );

        Console.WriteLine(
            $"Your Health: " +
            $"{Math.Max(player.CurrentHealth, 0)}" +
            $"/{player.MaxHealth}"
        );
    }

    static bool UsePotion()
    {
        if (player.PotionCount == 0)
        {
            Console.WriteLine("You have no potions left.");
            return false;
        }

        if (player.CurrentHealth == player.MaxHealth)
        {
            Console.WriteLine(
                "Your health is already full."
            );

            return false;
        }

        int healthBeforeHealing = player.CurrentHealth;

        player.CurrentHealth += 30;

        if (player.CurrentHealth > player.MaxHealth)
        {
            player.CurrentHealth = player.MaxHealth;
        }

        player.PotionCount--;

        int healed =
            player.CurrentHealth - healthBeforeHealing;

        Console.WriteLine(
            $"You restored {healed} health."
        );

        Console.WriteLine(
            $"Potions remaining: {player.PotionCount}"
        );

        return true;
    }

    static void DisplayBattleMenu()
    {
        Console.WriteLine("\n===== YOUR TURN =====");
        Console.WriteLine("1. Attack");
        Console.WriteLine("2. Heal");
        Console.WriteLine("3. View Status");
        Console.WriteLine("4. Run Away");
    }

    static void DisplayStatistics()
    {
        Console.WriteLine("\n===== STATISTICS =====");
        Console.WriteLine($"Wins: {wins}");
        Console.WriteLine($"Losses: {losses}");
        Console.WriteLine($"Escapes: {escapes}");
        Console.WriteLine($"Total Gold: {player.Gold}");
    }

    static bool AskYesNo(string message)
    {
        while (true)
        {
            Console.Write($"{message} (y/n): ");

            string answer =
                (Console.ReadLine() ?? "").Trim().ToLower();

            if (answer == "y")
            {
                return true;
            }

            if (answer == "n")
            {
                return false;
            }

            Console.WriteLine("Please enter only y or n.");
        }
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

            Console.WriteLine("Input cannot be empty.");
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

            if (int.TryParse(Console.ReadLine(), out value) &&
                value >= minValue &&
                value <= maxValue)
            {
                return value;
            }

            Console.WriteLine("Invalid input.");
        }
    }
}
