using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
namespace projekt31_10_24
{
    public class Pizza
    {
        public string NazwaPizzy;
        public double CenaPizzy;
        public char RozmiarPizzy;
        public List<string> ListaSkladnikow;
        public Pizza(string nazwa, double cena, char rozmiar, List<string> skladniki)
        {
            NazwaPizzy = nazwa;
            CenaPizzy = cena;
            RozmiarPizzy = rozmiar;
            ListaSkladnikow = skladniki;
        }
// podajemy pizzy z pliku pizzy.json
        public static Pizza Margaryta()
        {
            return new Pizza("Margaryta", 20, 'M', new List<string> { "Mozzarella", "Pomidory", "Ketchup" });
        }
        public static Pizza Pepperoni()
        {
            return new Pizza("Pepperoni", 25, 'L', new List<string> { "Mozzarella", "Kielbasa", "Ketchup" });
        }
        public int CzasPrzygotowania()
        {
            return 10 + ListaSkladnikow.Count * 2; // Czas podstawowy + czas na składniki
        }
        /*
        public void WyswietlInformacje()
        {
            Console.WriteLine("Pizza: " + NazwaPizzy);
            Console.WriteLine("Cena: " + CenaPizzy + " zł");
            Console.WriteLine("Rozmiar: " + RozmiarPizzy);
            Console.WriteLine("Składniki: " + string.Join(", ", ListaSkladnikow));
            Console.WriteLine("Czas przygotowania: " + CzasPrzygotowania() + " minut\n");
        }*/
        public static void ZapiszStandardowePizze()
        {
            var standardowePizze = new List<Pizza>
            {
                Margaryta(),
                Pepperoni()
            };
            string json = JsonConvert.SerializeObject(standardowePizze, Formatting.Indented);
            File.WriteAllText("pizzy.json", json);
        }

        public static List<Pizza> WczytajStandardowePizze()
        {
            if (File.Exists("pizzy.json"))
            {
                string json = File.ReadAllText("pizzy.json");
                return JsonConvert.DeserializeObject<List<Pizza>>(json) ?? new List<Pizza>();
            }
            return new List<Pizza>();
        }
    }
}

