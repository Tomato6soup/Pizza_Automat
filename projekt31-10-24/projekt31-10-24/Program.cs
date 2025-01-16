using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

namespace projekt31_10_24
{

    public class Program
    {
        public static void Main(string[] args)
        {
            Automat automat = new Automat();
            bool exit = false;
           
            while (!exit)
            {
           
                automat.WyswietlMenu();
                Console.Write("\n===== $ Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        automat.DodajKlienta();
                        break;
                    case "2":
                        automat.ZamowPizze();
                        break;
                    case "3":
                        automat.SpecialDodatek();
                        Console.Write("\n===== ");
                        break;
                    case "4":
                        automat.WyswietlKlientow();
                        break;
                    case "5":
                        automat.WyswietlDodatki();
                        break;
                    case "6":
                        automat.WyswietlSkladniki();
                        break;
                    case "7":
                        automat.WyswietlHistorieZamowien();
                        break;
                    case "8":
                        automat.WyswietlStatystykiZamowien();
                        break;
                    case "9":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }
        }

    }
}
