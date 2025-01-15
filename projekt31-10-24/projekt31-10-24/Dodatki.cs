using System;
using System.Collections.Generic;

namespace projekt31_10_24
{
    public class Dodatki
    {
        public Dictionary<string, int> Frytki { get; set; }
        public Dictionary<string, int> Salatka { get; set; }
        public Dictionary<string, int> Napoje { get; set; }

        //też zrobić przez json plik, wyczytamy z dodatki.json-----------
        public Dodatki()
        {
            Frytki = new Dictionary<string, int> { { "Frytki", 1000 } }; // w gramach
            Salatka = new Dictionary<string, int> { { "Sałatka", 5000 } }; // w gramach
            Napoje = new Dictionary<string, int> { { "Sok", 3000 }, { "Cola", 4000 }, { "Woda", 2500 } }; // w mililitrach
        }
        public void WyswietlDodatki()
        {
            //zwracamy string, wyswietlic w program.cs, return string
            Console.WriteLine("Dostępne dodatki:");

            Console.WriteLine("Frytki:");
            foreach (var item in Frytki)
                Console.WriteLine($"- {item.Key}: {item.Value}g");

            Console.WriteLine("\nSałatka:");
            foreach (var item in Salatka)
                Console.WriteLine($"- {item.Key}: {item.Value}g");

            Console.WriteLine("\nNapoje:");
            foreach (var item in Napoje)
                Console.WriteLine($"- {item.Key}: {item.Value}ml");
            Console.WriteLine();
        }

        public bool ZamowDodatek(string nazwa, int ilosc)
        {
            if (Frytki.ContainsKey(nazwa) && Frytki[nazwa] >= ilosc)
            {
                Frytki[nazwa] -= ilosc;
                return true;
            }
            else if (Salatka.ContainsKey(nazwa) && Salatka[nazwa] >= ilosc)
            {
                Salatka[nazwa] -= ilosc;
                return true;
            }
            else if (Napoje.ContainsKey(nazwa) && Napoje[nazwa] >= ilosc)
            {
                Napoje[nazwa] -= ilosc;
                return true;
            }
            else
            {
                Console.WriteLine($"Brak wystarczającej ilości dodatku {nazwa} lub dodatek nie istnieje.");
                return false;
            }
        }

    }

}
