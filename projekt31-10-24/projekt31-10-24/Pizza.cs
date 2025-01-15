using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
namespace projekt31_10_24
{
    public class Pizza
    {
       //W tej klasie wyswietla sie listy z pizz, wyswietla pizzy, cene, ich skladnikow

        // Wczytywanie pizz z pliku zamowienia.json
        public static List<Pizza> WczytajPizzeZPliku()
        {
            string sciezkaPliku = "pizzy.json"; // Ścieżka do pliku
            if (File.Exists(sciezkaPliku))
            {
                string json = File.ReadAllText(sciezkaPliku);
                return JsonConvert.DeserializeObject<List<Pizza>>(json) ?? new List<Pizza>();
            }
            else
            {
                Console.WriteLine($"Plik {sciezkaPliku} nie istnieje.");
                return new List<Pizza>();
            }
        }

        // Wyświetlanie listy pizz
        public static void WyswietlDostepnePizze(List<Pizza> pizze)
        {
            Console.WriteLine("Dostępne pizze:");
            for (int i = 0; i < pizze.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {pizze[i].NazwaPizzy} - {pizze[i].CenaPizzy} PLN");
                Console.WriteLine($"   Rozmiar: {pizze[i].RozmiarPizzy}");
                Console.WriteLine($"   Składniki: {string.Join(", ", pizze[i].ListaSkladnikow)}");
            }
        }

     
        }
    }



