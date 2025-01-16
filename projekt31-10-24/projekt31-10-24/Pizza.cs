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
            string sciezkaPliku = "pizzy.json";
            if (File.Exists(sciezkaPliku))
            {
                try
                {
                    string json = File.ReadAllText(sciezkaPliku);
                    return JsonConvert.DeserializeObject<List<Pizza>>(json) ?? new List<Pizza>();
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"Błąd odczytu JSON w pliku {sciezkaPliku}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Plik {sciezkaPliku} nie istnieje.");
            }

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



