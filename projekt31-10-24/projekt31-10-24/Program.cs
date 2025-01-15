using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

namespace projekt31_10_24
{

    public class Program
    {
        static void Main(string[] args)
        {
           /// Config.Config1();
            Automat automat = new Automat();
            Dodatki dodatki = new Dodatki();
            Skladniki skladniki = new Skladniki();
            Zamowienia zamowienia = new Zamowienia();
            List<Klient> klienci = Klient.WczytajKlientow();
            // Wczytanie pizz z pliku zamowienia.json
            List<Pizza> pizze = Pizza.WczytajPizzeZPliku();

            if (pizze.Count == 0)
            {
                Console.WriteLine("Brak dostępnych pizz w pliku zamowienia.json.");
                return;
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("=== MENU ===");
                Console.WriteLine("1. Dodaj klienta");
                Console.WriteLine("2. Zamów pizzę");
                Console.WriteLine("3. Wybierz dodatek");
                Console.WriteLine("4. Wyświetl dostępne składniki");
                Console.WriteLine("5. Wyświetl historię zamówień");
                Console.WriteLine("6. Wyjdź");
                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Podaj imię klienta: ");
                        string imie = Console.ReadLine();
                        Console.Write("Podaj nazwisko klienta: ");
                        //jeszcze dodac opcje podaj ID klienta
                        string nazwisko = Console.ReadLine();
                        Klient nowyKlient = new Klient(klienci.Count + 1, imie, nazwisko);
                        klienci.Add(nowyKlient);
                        Klient.ZapiszKlientow(klienci);
                        break;
                    case "2":
                        Console.Write("Podaj ID klienta: ");
                        int klientID = int.Parse(Console.ReadLine());
                        Klient klient = klienci.Find(k => k.KlientID == klientID);
                        if (klient != null)
                        {
                            //Dodać wybór pizzy z zamówienia.json
                            Pizza.WyswietlDostepnePizze(pizze);
                            Pizza wybranaPizza = Pizza.WybierzPizza(pizze);
                            Console.WriteLine($"\nWybrałeś pizzę: {wybranaPizza.NazwaPizzy}");
                            Console.WriteLine($"Cena: {wybranaPizza.CenaPizzy} PLN");
                            Console.WriteLine($"Czas przygotowania: {wybranaPizza.CzasPrzygotowania()} minut");
                            
                            zamowienia.ZapiszZamowieniaDoJson();
                        }
                        else
                        {
                            Console.WriteLine("Nie znaleziono klienta.");
                        }
                        break;
                    case "3":
                        dodatki.WyswietlDodatki();
                        Console.Write("Wybierz dodatek: ");
                        string dodatek = Console.ReadLine();
                        Console.Write("Podaj ilość: ");
                        int ilosc = int.Parse(Console.ReadLine());
                        dodatki.ZamowDodatek(dodatek, ilosc);
                        break;
                    case "4":
                        skladniki.PokazInfo();
                        break;
                    case "5":
                        Console.WriteLine("=== HISTORIA ZAMÓWIEŃ ===");
                        foreach (var klientZam in klienci)
                        {
                            klientZam.WyswietlInformacje();
                            zamowienia.WyswietlZapisaneZamowienia();
                        }
                        break;
                    case "6":
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
