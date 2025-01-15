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

        public Klient(int klientID, string imie, string nazwisko)
        {
            KlientID = klientID;
            Imie = imie;
            Nazwisko = nazwisko;
        }

        public static List<Klient> WczytajKlientow()
        {
           // string sciezka = Config.PobierzSciezke("klienci.json");
            if (File.Exists("klienci.json"))
            {
                string json = File.ReadAllText("klienci.json");
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
            Console.WriteLine($"ID: {KlientID}, Imię: {Imie}, Nazwisko: {Nazwisko}");
        }

        public void DodajZamowienie(Zamowienia zamowienia, Pizza pizza)
        {
            Console.WriteLine($"Dodawanie zamówienia dla klienta: {Imie} {Nazwisko}");
            zamowienia.DodajPizze(pizza);
        }
    }

}