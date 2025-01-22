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
        private Dictionary<string, int> zuzycieSkladnikow = new Dictionary<string, int>();
        private Dictionary<string, int> zuzycieDodatkow = new Dictionary<string, int>();
        public List<Klient> klienci = Klient.WczytajKlientow();
        public List<Pizza> historiaZamowien = new List<Pizza>();
        public List<Pizza> pizze = Pizza.WczytajPizzeZPliku();
        public Dodatki dodatki = new Dodatki();
        public Skladniki skladniki = new Skladniki();
        public Automat()
        {
            WczytajHistorieZamowien(); // Wczytanie zapisanej historii zamówień
            WczytajZuzycieSkladnikow(); // Wczytanie sumarycznego zużycia składników
            WczytajZuzycieDodatkow(); // Wczytanie sumarycznego zużycia dodatków
        }
        
        public void WyswietlMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1. Dodaj klienta");
            Console.WriteLine("2. Zamów pizzę");
            Console.WriteLine("3. Zamów special dodatek (-_-)");
            Console.WriteLine("4. Wyświetl listę klientów");
            Console.WriteLine("5. Wyświetl dostępne dodatki");
            Console.WriteLine("6. Wyświetl dostępne składniki");
            Console.WriteLine("7. Wyświetl historię zamówień");
            Console.WriteLine("8. Wyświetl statystyki zamówień klientów");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("9. Uzupełnij zasoby (admin)");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("10. Wyświetl zużycie składników i dodatków ");
            Console.WriteLine("11. Wyjdź");
            Console.ResetColor();

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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
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
                            if (zuzycieSkladnikow.ContainsKey(skladnik.Key))
                            {
                                zuzycieSkladnikow[skladnik.Key] += skladnik.Value;
                            }
                            else
                            {
                                zuzycieSkladnikow[skladnik.Key] = skladnik.Value;
                            }

                            ZapiszZuzycieSkladnikow();
                        }
                        historiaZamowien.Add(wybranaPizza);
                        ZapiszHistorieZamowien();

                        Console.WriteLine($"Pizza {wybranaPizza.NazwaPizzy} została zamówiona.");

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
            Console.ResetColor();
        }

        public double ZamowDodatekDoPizzy()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            dodatki.WyswietlDodatki();
            Console.Write("Wybierz dodatek: ");
            string nazwaDodatku = Console.ReadLine();
            Console.Write("Podaj ilość (w gramach/ml): ");
            if (int.TryParse(Console.ReadLine(), out int ilosc))
            {
                if (dodatki.ZamowDodatek(nazwaDodatku, ilosc, out double cena))
                {
                    if (zuzycieDodatkow.ContainsKey(nazwaDodatku))
                    {
                        zuzycieDodatkow[nazwaDodatku] += ilosc;
                    }
                    else
                    {
                        zuzycieDodatkow[nazwaDodatku] = ilosc;
                    }
                    ZapiszZuzycieDodatkow();
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
            Console.ResetColor();
            return 0;
        }
        private void WczytajZuzycieDodatkow()
        {
            string sciezka = Config.PobierzSciezke("ZuzycieDodatkow");
            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                zuzycieDodatkow = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }
            else
            {
                Console.WriteLine($"Plik {sciezka} nie istnieje. Tworzenie nowego pliku...");
                ZapiszZuzycieDodatkow(); 
            }
        }
        private void ZapiszZuzycieSkladnikow()
        {
            string sciezka = Config.PobierzSciezke("ZuzycieSkladnikow");
            File.WriteAllText(sciezka, JsonConvert.SerializeObject(zuzycieSkladnikow, Formatting.Indented));
        }
        private void ZapiszZuzycieDodatkow()
        {
            string sciezka = Config.PobierzSciezke("ZuzycieDodatkow");
            File.WriteAllText(sciezka, JsonConvert.SerializeObject(zuzycieDodatkow, Formatting.Indented));
        }
        private void WczytajZuzycieSkladnikow()
        {
            string sciezka = Config.PobierzSciezke("ZuzycieSkladnikow");
            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                zuzycieSkladnikow = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }
            else
            {
                Console.WriteLine($"Plik {sciezka} nie istnieje. Tworzenie nowego pliku...");
                ZapiszZuzycieSkladnikow();
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
            Console.WriteLine($"Liczba zamówień w historii: {historiaZamowien.Count}");

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

            Console.WriteLine("=== Statystyki zamówień ===");
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
        public void UzupelnijZasoby()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("1. Uzupełnij składniki");
            Console.WriteLine("2. Uzupełnij dodatki");
            Console.Write("Wybierz opcję: ");
            string choice = Console.ReadLine();

            switch (choice)
            {

                case "1":
                    Console.Write("Podaj nazwę składnika: ");
                    string skladnik = Console.ReadLine();
                    Console.Write("Podaj ilość do uzupełnienia: ");
                    if (int.TryParse(Console.ReadLine(), out int iloscSkladnika))
                    {
                        skladniki.UzupelnijSkladnik(skladnik, iloscSkladnika);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowa ilość.");
                    }
                    break;

                case "2":
                    Console.Write("Podaj nazwę dodatku: ");
                    string dodatek = Console.ReadLine();
                    Console.Write("Podaj ilość do uzupełnienia: ");
                    if (int.TryParse(Console.ReadLine(), out int iloscDodatku))
                    {
                        dodatki.UzupelnijDodatek(dodatek, iloscDodatku);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowa ilość.");
                    }
                    break;

                default:
                    Console.WriteLine("Nieprawidłowy wybór.");
                    break;
            }
            Console.ResetColor();
        }
        private void WczytajHistorieZamowien()
        {
            string sciezka = Config.PobierzSciezke("Zamowienia");

            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                var wczytaneZamowienia = JsonConvert.DeserializeObject<List<Pizza>>(json);

                if (wczytaneZamowienia != null)
                {
                    // Reset listy przed wczytaniem
                    historiaZamowien.Clear();
                    historiaZamowien = wczytaneZamowienia
                        .GroupBy(pizza => new
                        {
                            pizza.NazwaPizzy,
                            pizza.CenaPizzy,
                            pizza.RozmiarPizzy,
                            Skladniki = string.Join(",", pizza.ListaSkladnikow.Select(s => $"{s.Key}:{s.Value}"))
                        })
                        .Select(grupa => grupa.First())
                        .ToList();
                }
            }
            else
            {
                Console.WriteLine($"Plik {sciezka} nie istnieje. Tworzenie nowego pliku...");
                ZapiszHistorieZamowien();
            }
        }
        private void ZapiszHistorieZamowien()
        {
            string sciezka = Config.PobierzSciezke("Zamowienia");

            List<Pizza> zapisaneZamowienia = new List<Pizza>();
            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                zapisaneZamowienia = JsonConvert.DeserializeObject<List<Pizza>>(json) ?? new List<Pizza>();
            }
            var wszystkieZamowienia = zapisaneZamowienia
                .Concat(historiaZamowien)
                .GroupBy(pizza => new
                {
                    pizza.NazwaPizzy,
                    pizza.CenaPizzy,
                    pizza.RozmiarPizzy,
                    Skladniki = string.Join(",", pizza.ListaSkladnikow.Select(s => $"{s.Key}:{s.Value}"))
                })
                .Select(grupa => grupa.First())
                .ToList();

            File.WriteAllText(sciezka, JsonConvert.SerializeObject(wszystkieZamowienia, Formatting.Indented));
        }

        public void WyswietlSumaryczneZuzycie()
        {
            Console.WriteLine("=== Sumaryczne zużycie składników ===");
            foreach (var skladnik in zuzycieSkladnikow)
            {
                Console.WriteLine($"- {skladnik.Key}: {skladnik.Value}g");
            }

            Console.WriteLine("\n=== Sumaryczne zużycie dodatków ===");
            foreach (var dodatek in zuzycieDodatkow)
            {
                Console.WriteLine($"- {dodatek.Key}: {dodatek.Value}g/ml");
            }
        }
    }


}
