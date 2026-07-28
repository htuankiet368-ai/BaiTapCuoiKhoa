using System;
using System.Collections.Generic;

namespace KiemTra.BauCuaGame;

public enum Animal
{
    Gourd = 1,
    Crab,
    Shrimp,
    Fish,
    Chicken,
    Deer
}

public class Account
{
    public string Name { get; set; } = "";
    public int Balance { get; set; } = 1000;
}

public class Bet
{
    public Animal Animal { get; set; }
    public int Amount { get; set; }
}

public class BauCuaGame
{
    static Random random = new Random();

    static Account account = new Account();

    static int totalRounds;
    static int wins;
    static int losses;
    static int biggestWin;

    public static void Run()
    {
        CreateAccount();

        while (true)
        {
            if (account.Balance <= 0)
            {
                Console.WriteLine("\nYou ran out of money!");
                DisplayStats();
                return;
            }

            DisplayMenu();

            int choice = InputInt("Choose: ", 0, 3);

            switch (choice)
            {
                case 1:
                    PlayRound();
                    break;

                case 2:
                    Console.WriteLine($"Balance: {account.Balance}");
                    break;

                case 3:
                    DisplayStats();
                    break;

                case 0:
                    DisplayStats();
                    return;
            }
        }
    }

    static void CreateAccount()
    {
        Console.WriteLine("\n===== CREATE ACCOUNT =====");

        account = new Account();
        account.Name = InputString("Enter name: ");
        account.Balance = 1000;

        totalRounds = 0;
        wins = 0;
        losses = 0;
        biggestWin = 0;
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\n===== BAU CUA GAME =====");
        Console.WriteLine($"Player: {account.Name}");
        Console.WriteLine($"Balance: {account.Balance}");

        Console.WriteLine("1. Play");
        Console.WriteLine("2. View Balance");
        Console.WriteLine("3. Statistics");
        Console.WriteLine("0. Exit");
    }

    static void PlayRound()
    {
        Console.WriteLine("\n===== PLACE BET =====");

        ShowAnimals();

        int count = InputInt("How many animals to bet: ", 1, 6);

        List<Bet> bets = new List<Bet>();

        int remaining = account.Balance;

        for (int i = 1; i <= count; i++)
        {
            if (remaining == 0) break;

            Animal animal;

            while (true)
            {
                int pick = InputInt($"Choose animal {i}: ", 1, 6);
                animal = (Animal)pick;

                if (!IsAlreadyBet(bets, animal))
                    break;

                Console.WriteLine("Already chosen.");
            }

            int amount = InputInt(
                $"Bet for {animal} (remaining {remaining}): ",
                1,
                remaining
            );

            bets.Add(new Bet { Animal = animal, Amount = amount });

            remaining -= amount;
        }

        int totalBet = 0;
        foreach (var b in bets) totalBet += b.Amount;

        account.Balance -= totalBet;

        Animal d1 = (Animal)random.Next(1, 7);
        Animal d2 = (Animal)random.Next(1, 7);
        Animal d3 = (Animal)random.Next(1, 7);

        Console.WriteLine($"\nResult: {d1} - {d2} - {d3}");

        int totalReturn = 0;

        foreach (var b in bets)
        {
            int countAppear = 0;

            if (b.Animal == d1) countAppear++;
            if (b.Animal == d2) countAppear++;
            if (b.Animal == d3) countAppear++;

            if (countAppear > 0)
            {
                int reward = b.Amount * (countAppear + 1);
                totalReturn += reward;

                Console.WriteLine($"{b.Animal} x{countAppear} → +{reward}");
            }
            else
            {
                Console.WriteLine($"{b.Animal} lost (-{b.Amount})");
            }
        }

        account.Balance += totalReturn;

        int profit = totalReturn - totalBet;

        totalRounds++;

        if (profit > 0)
        {
            wins++;
            Console.WriteLine($"You WIN (+{profit})");

            if (profit > biggestWin)
                biggestWin = profit;
        }
        else
        {
            losses++;
            Console.WriteLine($"You LOSE ({profit})");
        }

        Console.WriteLine($"Balance: {account.Balance}");
    }

    static bool IsAlreadyBet(List<Bet> bets, Animal animal)
    {
        foreach (var b in bets)
        {
            if (b.Animal == animal)
                return true;
        }
        return false;
    }

    static void ShowAnimals()
    {
        Console.WriteLine("1. Gourd");
        Console.WriteLine("2. Crab");
        Console.WriteLine("3. Shrimp");
        Console.WriteLine("4. Fish");
        Console.WriteLine("5. Chicken");
        Console.WriteLine("6. Deer");
    }

    static void DisplayStats()
    {
        Console.WriteLine("\n===== STATISTICS =====");
        Console.WriteLine($"Player: {account.Name}");
        Console.WriteLine($"Rounds: {totalRounds}");
        Console.WriteLine($"Wins: {wins}");
        Console.WriteLine($"Losses: {losses}");
        Console.WriteLine($"Biggest Win: {biggestWin}");
        Console.WriteLine($"Final Balance: {account.Balance}");
    }

    static string InputString(string msg)
    {
        while (true)
        {
            Console.Write(msg);
            string input = (Console.ReadLine() ?? "").Trim();

            if (input != "") return input;

            Console.WriteLine("Cannot be empty.");
        }
    }

    static int InputInt(string msg, int min, int max)
    {
        while (true)
        {
            Console.Write(msg);

            if (int.TryParse(Console.ReadLine(), out int val)
                && val >= min && val <= max)
                return val;

            Console.WriteLine("Invalid input.");
        }
    }
}
