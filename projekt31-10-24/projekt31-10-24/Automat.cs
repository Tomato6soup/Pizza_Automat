using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace projekt31_10_24
{
    public class Automat
    {
        public List<Klient> klienci = Klient.WczytajKlientow();
        public List<Pizza> historiaZamowien = new List<Pizza>();
        public List<Pizza> pizze = Pizza.WczytajPizzeZPliku();
        public Dodatki dodatki = new Dodatki();
        public Skladniki skladniki = new Skladniki();

        public void WyswietlMenu()
        {
            Console.WriteLine("=== MENU ===");
            Console.WriteLine("1. Dodaj klienta");
            Console.WriteLine("2. Zamów pizzę");
            Console.WriteLine("3. Zamów dodatek (-_-)");
            Console.WriteLine("4. Wyświetl listę klientów");
            Console.WriteLine("5. Wyświetl dostępne dodatki");
            Console.WriteLine("6. Wyświetl dostępne składniki");
            Console.WriteLine("7. Wyświetl historię zamówień");
            Console.WriteLine("8. Wyświetl statystyki zamówień klientów");
            Console.WriteLine("9. Wyjdź");
        }

        public void DodajKlienta()
        {
            Console.Write("Podaj imię klienta: ");
            string imie = Console.ReadLine();
            Console.Write("Podaj nazwisko klienta: ");
            string nazwisko = Console.ReadLine();
            int id = klienci.Count + 1;
            Klient nowyKlient = new Klient(id, imie, nazwisko);
            klienci.Add(nowyKlient);
            Klient.ZapiszKlientow(klienci);
            Console.WriteLine("Dodano nowego klienta.");
        }

        public void ZamowPizze()
        {
            Console.Write("Podaj ID klienta: ");
            if (int.TryParse(Console.ReadLine(), out int klientID))
            {
                Klient klient = klienci.Find(k => k.KlientID == klientID);
                if (klient != null)
                {
                    Pizza.WyswietlDostepnePizze(pizze);
                    Console.Write("Wybierz numer pizzy: ");
                    if (int.TryParse(Console.ReadLine(), out int numer) && numer > 0 && numer <= pizze.Count)
                    {
                        var wybranaPizza = pizze[numer - 1];
                        Console.WriteLine($"Zamówiłeś pizzę: {wybranaPizza.NazwaPizzy}");
                        double sumaZamowienia = wybranaPizza.CenaPizzy;

                        foreach (var skladnik in wybranaPizza.ListaSkladnikow)
                        {
                            if (!skladniki.ZuzyjSkladnik(skladnik.Key, skladnik.Value))
                            {
                                Console.WriteLine("Brak składników do przygotowania pizzy.");
                                return;
                            }
                        }

                        Console.WriteLine("Czy chcesz dodać dodatek do pizzy? (tak/nie): ");
                        string decyzja = Console.ReadLine()?.ToLower();
                        if (decyzja == "tak")
                        {
                            sumaZamowienia += ZamowDodatekDoPizzy();
                        }

                        klient.SumaZamowien += sumaZamowienia;
                        Klient.ZapiszKlientow(klienci);

                        klient.DodajZamowienie(wybranaPizza, historiaZamowien);
                        Console.WriteLine($"Całkowita kwota zamówienia: {sumaZamowienia} PLN");
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy wybór.");
                    }
                }
                else
                {
                    Console.WriteLine("Nie znaleziono klienta o podanym ID.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy format ID.");
            }
        }
        public void SpecialDodatek()
        {
            int totalProgress = 50;
            int progress = 0;

            Console.WriteLine("Progress: ");
            while (progress <= totalProgress)
            {
                string bar = new string('#', progress);
                string emptySpace = new string(' ', totalProgress - progress);
                Console.SetCursorPosition(0, 1); // Move the cursor to the second line
                Console.Write($"[{bar}{emptySpace}] {progress}%");
                Thread.Sleep(100);  // Pause for a moment
                progress++;
            }

            Console.WriteLine("\nTask Complete!");
        }
        public double ZamowDodatekDoPizzy()
        {
            dodatki.WyswietlDodatki();
            Console.Write("Wybierz dodatek: ");
            string nazwaDodatku = Console.ReadLine();
            Console.Write("Podaj ilość (w gramach/ml): ");
            if (int.TryParse(Console.ReadLine(), out int ilosc))
            {
                if (dodatki.ZamowDodatek(nazwaDodatku, ilosc, out double cena))
                {
                    Console.WriteLine($"Dodano {nazwaDodatku} ({ilosc}g/ml) do zamówienia za {cena} PLN.");
                    return cena;
                }
                else
                {
                    Console.WriteLine("Nie udało się zamówić dodatku.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowa ilość.");
            }

            return 0;
        }

        public void WyswietlKlientow()
        {
            Console.WriteLine("Lista klientów:");
            foreach (var klient in klienci)
            {
                klient.WyswietlInformacje();
            }
        }
        public void WyswietlSkladniki()
        {
            skladniki.WyswietlSkladniki();
        }

        public void WyswietlDodatki()
        {
            dodatki.WyswietlDodatki();
        }
        public void WyswietlHistorieZamowien()
        {
            Console.WriteLine("Historia zamówień:");
            foreach (var pizza in historiaZamowien)
            {
                Console.WriteLine($"- {pizza.NazwaPizzy} za {pizza.CenaPizzy} PLN");
            }
        }
        public void WyswietlStatystykiZamowien()
        {
            if (!historiaZamowien.Any())
            {
                Console.WriteLine("Brak zamówień do wyświetlenia.");
                return;
            }

            Console.WriteLine("=== Statystyki zamówień klientów ===");
            var grupyZamowien = historiaZamowien
                .GroupBy(pizza => pizza.NazwaPizzy)
                .Select(grupa => new
                {
                    RodzajPizzy = grupa.Key,
                    Ilosc = grupa.Count()
                })
                .OrderByDescending(stat => stat.Ilosc);

            foreach (var statystyka in grupyZamowien)
            {
                Console.WriteLine($"- {statystyka.RodzajPizzy}: {statystyka.Ilosc} szt.");
            }
        }
    }


}
