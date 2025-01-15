using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
// scalic dwie klasy w jeden - Automat(opcja zamow pizze, ilosc pizz, czas przygotowania, historia zamowien)
namespace projekt31_10_24
{
    public class Automat
    
    {
        private Dodatki dodatki = new Dodatki(); // Instancja klasy Dodatki

        public bool ZamowDodatek(string nazwa, int ilosc)
        {
            return dodatki.ZamowDodatek(nazwa, ilosc); // Wywołanie metody na instancji
        }
        public int IlePizzyZostalo = 10;
        private Skladniki skladniki = new Skladniki();
        public List<Zamowienia> zamowienia = new List<Zamowienia>();

        public void WybierzPizze(Pizza pizza)
        {
            Console.WriteLine("Wybrana pizza: " + pizza.NazwaPizzy);
            zamowienia.Add(new Zamowienia { Pizza = pizza});
            ZrobPizze(pizza);
        }
        public void ZrobPizze(Pizza pizza)
        {
            Console.WriteLine("Przygotowuję pizzę: " + pizza.NazwaPizzy);
            IlePizzyZostalo--;
            Console.WriteLine("Pizza gotowa!");
            Console.WriteLine();
        }

        public void PokazDostepneSkladniki()
        {
            skladniki.PokazInfo();
        }
        //------------Dodano nowe
        public void PokazHistorieZamowien()
        {
            Console.WriteLine("Historia zamowien: ");
            foreach (var zamowienie in zamowienia)
            {
                Console.WriteLine("Pizza: " + zamowienie.Pizza.NazwaPizzy);
            }

        }
        //-------------------
        
    }
    public class Zamowienia

    {
        public Pizza Pizza { get; set; }
        public List<Pizza> pizzas = new List<Pizza>();
        public string plikSciezka = "pizzy.json";

        public void DodajPizze(Pizza pizza)
        {
            pizzas.Add(pizza);
            ZapiszDoPliku(pizza);
            Console.WriteLine("Dodano pizzę: " + pizza.NazwaPizzy);
            //--
            Console.WriteLine("Data zamowienia: " + DateTime.Now);
            //--
        }
        //LINQ pytania potem bedzie
        private void ZapiszDoPliku(Pizza pizza)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(plikSciezka, true))
                {
                    writer.WriteLine($"Pizza: {pizza.NazwaPizzy}, Cena: {pizza.CenaPizzy} zł, Rozmiar: {pizza.RozmiarPizzy}, Data zamówienia: {DateTime.Now}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd przy zapisywaniu do pliku: " + e.Message);
            }
        }
        // Metoda odczytująca wszystkie zamówienia z pliku
        public void WyswietlZapisaneZamowienia()
        {
            try
            {
                if (File.Exists(plikSciezka))
                {
                    Console.WriteLine("Zapisane zamówienia:");
                    string[] lines = File.ReadAllLines(plikSciezka);
                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.WriteLine("Brak zapisanych zamówień.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd przy odczycie z pliku: " + e.Message);
            }
        }
        /*
        // Metoda wyświetlająca liczbę pizz każdego rodzaju
        public void WyswietlLiczbePizz()
        {
            var liczbaPizz = pizzas
                .GroupBy(p => p.NazwaPizzy)
                .Select(g => new { Nazwa = g.Key, Ilosc = g.Count() });

            Console.WriteLine("Liczba pizz każdego rodzaju:");
            foreach (var p in liczbaPizz)
            {
                Console.WriteLine($"- {p.Nazwa}: {p.Ilosc}");
            }
        }

        // Metoda wyświetlająca średni czas przygotowania każdej pizzy
        public void WyswietlSredniCzasPrzygotowania()
        {
            var czasPrzygotowania = pizzas
                .GroupBy(p => p.NazwaPizzy)
                .Select(g => new { Nazwa = g.Key, SredniCzas = g.Average(p => p.CzasPrzygotowania()) });

            Console.WriteLine("Średni czas przygotowania każdego rodzaju pizzy:");
            foreach (var p in czasPrzygotowania)
            {
                Console.WriteLine($"- {p.Nazwa}: {p.SredniCzas} minut");
            }
        }
        public void ZapiszZamowieniaDoJson()
        {
            string json = JsonConvert.SerializeObject(pizzas, Formatting.Indented);
            File.WriteAllText("zamowienia.json", json);
        }
        */
        public void WczytajZamowieniaZJson()
        {
            if (File.Exists("zamowienia.json"))
            {
                string json = File.ReadAllText("zamowienia.json");
                pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json) ?? new List<Pizza>();
            }
        }


    }
