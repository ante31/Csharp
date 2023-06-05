using System;

namespace project3
{
    public enum AccountType
    {
        Savings,
        CheckingAccount,
        GiroAccount
    }

    public struct BankAccount
    {
        public int accountNumber;
        public decimal balance;
        public AccountType accountType;
    }

    class Program
    {
        static BankAccount[] accounts = new BankAccount[5];
        static int numAccounts = 0;

        static void Main()
        {
            bool done = false;
            while (!done)
            {
                Console.WriteLine("Odaberite opciju:");
                Console.WriteLine("1. Upiši novi račun");
                Console.WriteLine("2. Ispiši sve račune");
                Console.WriteLine("0. Izlaz");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddAccount();
                        break;
                    case "2":
                        ListAccounts();
                        break;
                    case "0":
                        done = true;
                        break;
                    default:
                        Console.WriteLine("Nepoznata opcija.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void AddAccount()
        {
            if (numAccounts >= accounts.Length)
            {
                Console.WriteLine("Nema više mjesta za nove račune.");
                return;
            }


            BankAccount account = new BankAccount();

            Console.Write("Broj računa: ");
            account.accountNumber = int.Parse(Console.ReadLine());

            Console.Write("Iznos na računu: ");
            account.balance = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Vrsta računa:");
            Console.WriteLine("1. Štednja");
            Console.WriteLine("2. TekućiRačun");
            Console.WriteLine("3. ŽiroRačun");

            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    account.accountType = AccountType.Savings;
                    break;
                case 2:
                    account.accountType = AccountType.CheckingAccount;
                    break;
                case 3:
                    account.accountType = AccountType.GiroAccount;
                    break;
                default:
                    Console.WriteLine("Nepoznata vrsta računa.");
                    return;
            }

            accounts[numAccounts] = account;
            numAccounts++;

            Console.WriteLine("Račun uspješno dodan.");
        }

        static void ListAccounts()
        {
            if (numAccounts == 0)
            {
                Console.WriteLine("Nema računa za ispis.");
                return;
            }

            foreach (BankAccount account in accounts)
            {
                if (account.accountNumber != 0)
                {
                    Console.WriteLine("Broj računa: " + account.accountNumber);
                    Console.WriteLine("Iznos na računu: " + account.balance);
                    Console.WriteLine("Vrsta računa: " + account.accountType);
                    Console.WriteLine();
                }
            }
        }
    }
}