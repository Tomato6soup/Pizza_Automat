using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// scalic dwie klasy w jeden - Automat(opcja zamow pizze, ilosc pizz, czas przygotowania, historia zamowien)
namespace projekt31_10_24
{
    public class Automat
    {
        private readonly List<Klient> klienci = Klient.WczytajKlientow();
        private readonly List<Pizza> historiaZamowien = new List<Pizza>();
        private readonly List<Pizza> pizze = Pizza.WczytajPizzeZPliku();
        private readonly Dodatki dodatki = new Dodatki();
        private readonly Skladniki skladniki = new Skladniki();

        public void WyswietlMenu()
        {
            Console.WriteLine("=== MENU ===");
            Console.WriteLine("1. Dodaj klienta");
            Console.WriteLine("2. Zamów pizzę");
            Console.WriteLine("3. Wyświetl listę klientów");
            Console.WriteLine("4. Wyświetl dostępne dodatki");
            Console.WriteLine("5. Wyświetl dostępne składniki");
            Console.WriteLine("6. Wyświetl historię zamówień");
            Console.WriteLine("7. Wyświetl statystyki zamówień klientów");
            Console.WriteLine("8. Wyjdź");
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

                        foreach (var skladnik in wybranaPizza.ListaSkladnikow)
                        {
                            if (!skladniki.ZuzyjSkladnik(skladnik.Key, skladnik.Value))
                            {
                                Console.WriteLine("Brak składników do przygotowania pizzy.");
                                return;
                            }
                        }

                        klient.DodajZamowienie(wybranaPizza, historiaZamowien);
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
