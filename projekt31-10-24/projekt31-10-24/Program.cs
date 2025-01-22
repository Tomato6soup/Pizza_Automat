using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;

namespace projekt31_10_24
{

    public class Program
    {
        public static void Main(string[] args)
        {
            // Wczytanie konfiguracji plików
            Config.WczytajKonfiguracje();
           
            Automat automat = new Automat();
            bool exit = false;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("========== Pizza Automat ==========");
            Console.ResetColor();

            while (!exit)
            {
           
                automat.WyswietlMenu();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("\n===== $ Wybierz opcję: ");
                Console.ResetColor();
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
                        Console.WriteLine("\n (* - _ - *) Hellou World");
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
                        automat.UzupelnijZasoby();
                        break;
                    case "10":
                        automat.WyswietlSumaryczneZuzycie();
                        break; ;
                    case "11":
                        exit = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Nieprawidłowa opcja.");
                        Console.ResetColor();
                        break;
                }
            }
        }

    }
}
