
using System;

namespace vjezba1
{
    class Program
    {
        static void Main()
        {
            try
            {
                string val, val1;
                Console.Write("Enter first integer: ");
                val = Console.ReadLine();
                Console.Write("Enter second integer: ");
                val1 = Console.ReadLine();

                int num1 = Convert.ToInt32(val);
                int num2 = Convert.ToInt32(val1);

                if (num2 == 0)
                {
                    throw new Exception("Cannot divide by zero.");
                }

                var result = (float)num1 / num2;
                Console.WriteLine(result);
                Console.WriteLine((int)result);
                Console.WriteLine("Currency: " + result.ToString("C"));
                Console.WriteLine("Integer: " + result);
                Console.WriteLine("Scientific: " + result.ToString("E"));
                Console.WriteLine("Fixed-point: " + result.ToString("F2"));
                Console.WriteLine("General: " + result.ToString("G"));
                Console.WriteLine("Number: " + result.ToString("N"));
                Console.WriteLine("Hexadecimal: " + ((int)result).ToString("X"));

            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: {0}", ex.Message);
            }
        }
    }
}
