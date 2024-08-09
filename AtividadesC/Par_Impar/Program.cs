using System;

class ParOuImpar
{
    static void Main(string[] args)
    {
        Console.Write("Digite um número inteiro: ");
        int numero = Convert.ToInt32(Console.ReadLine());

        if (numero % 2 == 0)
        {
            Console.WriteLine("O número é par.");
        }
        else
        {
            Console.WriteLine("O número é ímpar.");
        }

        Console.ReadKey();
    }
}