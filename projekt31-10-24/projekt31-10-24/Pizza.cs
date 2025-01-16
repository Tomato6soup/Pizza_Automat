using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
namespace projekt31_10_24
{
    public class Pizza
    {
        public string NazwaPizzy { get; set; }
        public double CenaPizzy { get; set; }
        public string RozmiarPizzy { get; set; }
        public Dictionary<string, int> ListaSkladnikow { get; set; }

        public static List<Pizza> WczytajPizzeZPliku()
        {
            string sciezka = "pizzy.json";
            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                return JsonConvert.DeserializeObject<List<Pizza>>(json) ?? new List<Pizza>();
            }

            Console.WriteLine($"Plik {sciezka} nie istnieje.");
            return new List<Pizza>();
        }

        public static void WyswietlDostepnePizze(List<Pizza> pizze)
        {
            Console.WriteLine("Dostępne pizze:");
            foreach (var pizza in pizze)
            {
                Console.WriteLine($"{pizza.NazwaPizzy} - {pizza.CenaPizzy} PLN");
                Console.WriteLine($"   Rozmiar: {pizza.RozmiarPizzy}");
                Console.WriteLine($"   Składniki: {string.Join(", ", pizza.ListaSkladnikow.Keys)}");
            }
        }
    }
}



