using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
namespace projekt31_10_24
{
    public class Klient
    {
        public int KlientID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public double SumaZamowien { get; set; } 

        public Klient(int klientID, string imie, string nazwisko)
        {
            KlientID = klientID;
            Imie = imie;
            Nazwisko = nazwisko;
            SumaZamowien = 0;
        }

        public static List<Klient> WczytajKlientow()
        {
            string sciezka = "klienci.json";
            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                return JsonConvert.DeserializeObject<List<Klient>>(json) ?? new List<Klient>();
            }

            return new List<Klient>();
        }

        public static void ZapiszKlientow(List<Klient> klienci)
        {
            string json = JsonConvert.SerializeObject(klienci, Formatting.Indented);
            File.WriteAllText("klienci.json", json);
        }

        public void WyswietlInformacje()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"ID: {KlientID}, Imię: {Imie}, Nazwisko: {Nazwisko}, Suma zamówień: {SumaZamowien} PLN");
            Console.ResetColor();
        }

        public void DodajZamowienie(Pizza pizza, List<Pizza> historiaZamowien)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Dodawanie zamówienia dla klienta: {Imie} {Nazwisko}");
            historiaZamowien.Add(pizza);
            Console.WriteLine($"Zamówiono pizzę: {pizza.NazwaPizzy} za {pizza.CenaPizzy} PLN");
            Console.ResetColor();
        }
    }

}