using System;

class Program
{
    static void Main()
    {
        int intVariable = 10;
        long longVariable = Int64.MaxValue;

        try
        {
            checked {
                intVariable = (int)longVariable;
             }
        }
        catch (OverflowException ex)
        {
            Console.WriteLine("Overflow je otkriven:", ex.Message);
        }

        Console.WriteLine("intVariable: {0}", intVariable);
        Console.WriteLine("longVariable: {0}", longVariable);
    }
}
