using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;

int balance = 1000;
int totalmatch = 0;
int totalmatchwon = 0;
int totalmathclost = 0;
string resultname;
Random rand = new Random();

do
{
    Console.WriteLine("What do you want?");
    Console.WriteLine("1. Show remaining balance.");
    Console.WriteLine("2. Start betting.");
    Console.WriteLine("3. Exit");
    int decision;
    while (!int.TryParse(Console.ReadLine(), out decision)) continue;
    if (decision == 2) totalmatch++;
    if (decision == 1)
    {
        Console.WriteLine("Remaining balance: " + balance);
        continue;
    }
    else if (decision == 2 && balance > 0)
    {
        int dicenum1 = rand.Next(1, 9);
        int dicenum2 = rand.Next(1, 9);
        int dicenum3 = rand.Next(1, 9);
        int sum = dicenum1 + dicenum2 + dicenum3;
        int result;
        if (sum > 0 && sum <= 3)
        {
            result = 1;
        }else if (sum > 3 && sum <= 9)
        {
            result = 2;
        }else if (sum > 9 && sum <= 13.5)
        {
            result = 3;
        }else if (sum > 13.5 && sum <= 18)
        {
            result = 4;
        }else if (sum > 18 && sum <= 22.5)
        {
            result = 5;
        }else if (sum > 22.5 && sum <= 27)
        {
            result = 6;
        }
        else
        {
            continue;
        }
        int choice;
        int choice2;
        int choice3;
        int betMoney;
        Console.WriteLine("Choose your bet.");
        Console.WriteLine("1. Bau");
        Console.WriteLine("2. Cua");
        Console.WriteLine("3. Tom");
        Console.WriteLine("4. Ca");
        Console.WriteLine("5. Ga");
        Console.WriteLine("6. Nai");
        while (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("Invalid choice.");
        }

        if (choice > 6 || choice <= 0)
        {
            Console.WriteLine("Invalid choice.");
            continue;
        }
        Console.WriteLine("Choose your second choice.");
        while (!int.TryParse(Console.ReadLine(), out choice2))
        {
            Console.WriteLine("Invalid choice.");
        }

        if (choice2 > 6 || choice2 <= 0 || choice2 == choice)
        {
            Console.WriteLine("Invalid choice.");
            continue;
        }
        Console.WriteLine("Choose your third choice.");
        while (!int.TryParse(Console.ReadLine(), out choice3))
        {
            Console.WriteLine("Invalid choice.");
            continue;
        }

        if (choice3 > 6 || choice3 <= 0 || choice3 == choice || choice3 == choice2)
        {
            Console.WriteLine("Invalid choice.");
            continue;
        }
        Console.WriteLine("Choose your bet money.");
        
        while (!int.TryParse(Console.ReadLine(), out betMoney))
        {
            Console.WriteLine("Please enter a cash amount you want to bet.");
        }

        if (betMoney > balance)
        {
            Console.WriteLine("Insufficient balance");
            continue;
        }

        if (result == 1)
        {
            resultname = "bau";
        }else if (result == 2)
        {
            resultname = "cua";
        }else if (result == 3)
        {
            resultname = "tom";
        }else if (result == 4)
        {
            resultname = "ca";
        }else if (result == 5)
        {
            resultname = "ga";
        }else if (result == 6)
        {
            resultname = "nai";
        }
        else
        {
            continue;
        }
        Console.WriteLine("The result is " + resultname);
        if (choice == result || choice2 == result || choice3 == result)
        {
            Console.WriteLine("You won!");
            balance += betMoney;
            totalmatchwon++;
        }
        else
        {
            Console.WriteLine("You lost!");
            balance -= betMoney;
            totalmathclost++;
        }

        continue;

    }else if (decision == 3)
    {
        Environment.Exit(0);
    }
    Console.WriteLine("Total match: " + totalmatch);
    Console.WriteLine("Total match won: " + totalmatchwon);
    Console.WriteLine("Total match lost: " + totalmathclost);
}while(balance > 0);
