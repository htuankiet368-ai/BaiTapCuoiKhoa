using System;

Random random = new Random();

string[] animals =
{
    "Bau",
    "Cua",
    "Tom",
    "Ca",
    "Ga",
    "Nai"
};

int balance = 1000;

int totalGames = 0;
int totalWins = 0;
int totalLosses = 0;
int biggestWin = 0;

while (balance > 0)
{
    Console.WriteLine("\n========== BAU CUA ==========");
    Console.WriteLine("1. Show Balance");
    Console.WriteLine("2. Play");
    Console.WriteLine("3. Statistics");
    Console.WriteLine("4. Exit");
    Console.Write("Choice: ");

    if (!int.TryParse(Console.ReadLine(), out int menu))
    {
        Console.WriteLine("Invalid input.");
        continue;
    }

    if (menu == 1)
    {
        Console.WriteLine($"Balance: {balance}");
        continue;
    }

    if (menu == 3)
    {
        Console.WriteLine("\n===== Statistics =====");
        Console.WriteLine($"Games Played : {totalGames}");
        Console.WriteLine($"Games Won    : {totalWins}");
        Console.WriteLine($"Games Lost   : {totalLosses}");
        Console.WriteLine($"Biggest Win  : {biggestWin}");
        Console.WriteLine($"Balance      : {balance}");
        continue;
    }

    if (menu == 4)
    {
        break;
    }

    if (menu != 2)
    {
        Console.WriteLine("Invalid menu.");
        continue;
    }

    Console.WriteLine("\nChoose an animal:");

    for (int i = 0; i < animals.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {animals[i]}");
    }

    Console.Write("Your choice: ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Invalid input.");
        continue;
    }

    if (choice < 1 || choice > 6)
    {
        Console.WriteLine("Invalid animal.");
        continue;
    }

    Console.Write("Bet amount: ");

    if (!int.TryParse(Console.ReadLine(), out int bet))
    {
        Console.WriteLine("Invalid amount.");
        continue;
    }

    if (bet <= 0)
    {
        Console.WriteLine("Bet must be greater than 0.");
        continue;
    }

    if (bet > balance)
    {
        Console.WriteLine("Not enough balance.");
        continue;
    }

    totalGames++;

    int die1 = random.Next(1, 7);
    int die2 = random.Next(1, 7);
    int die3 = random.Next(1, 7);

    Console.WriteLine("\nDice:");
    Console.WriteLine($"1. {animals[die1 - 1]}");
    Console.WriteLine($"2. {animals[die2 - 1]}");
    Console.WriteLine($"3. {animals[die3 - 1]}");

    int matches = 0;

    if (die1 == choice) matches++;
    if (die2 == choice) matches++;
    if (die3 == choice) matches++;

    if (matches > 0)
    {
        int winnings = bet * matches;

        balance += winnings;

        if (winnings > biggestWin)
            biggestWin = winnings;

        totalWins++;

        Console.WriteLine($"\nYou matched {matches} dice!");
        Console.WriteLine($"You won {winnings}.");
    }
    else
    {
        balance -= bet;
        totalLosses++;

        Console.WriteLine("\nNo matches.");
        Console.WriteLine($"You lost {bet}.");
    }

    Console.WriteLine($"Current balance: {balance}");
}

Console.WriteLine("\n========== GAME OVER ==========");
Console.WriteLine($"Final Balance : {balance}");
Console.WriteLine($"Games Played  : {totalGames}");
Console.WriteLine($"Games Won     : {totalWins}");
Console.WriteLine($"Games Lost    : {totalLosses}");
Console.WriteLine($"Biggest Win   : {biggestWin}");
Console.WriteLine("Thank you for playing!");
