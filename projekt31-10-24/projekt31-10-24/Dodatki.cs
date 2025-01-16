using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
namespace projekt31_10_24
{
    public class Dodatki
    {
        public Dictionary<string, int> DostepneDodatki { get; private set; } = new Dictionary<string, int>();

        public Dodatki()
        {
            WczytajDodatkiZJson();
        }

        private void WczytajDodatkiZJson()
        {
            string sciezka = "dodatki.json";
            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                DostepneDodatki = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }
            else
            {
                Console.WriteLine($"Plik {sciezka} nie istnieje.");
            }
        }

        public void WyswietlDodatki()
        {
            Console.WriteLine("Dostępne dodatki:");
            foreach (var dodatek in DostepneDodatki)
            {
                Console.WriteLine($"- {dodatek.Key}: {dodatek.Value}g/ml");
            }
        }

        public bool ZamowDodatek(string nazwa, int ilosc)
        {
            if (DostepneDodatki.ContainsKey(nazwa) && DostepneDodatki[nazwa] >= ilosc)
            {
                DostepneDodatki[nazwa] -= ilosc;
                return true;
            }

            Console.WriteLine($"Brak wystarczającej ilości dodatku: {nazwa}");
            return false;
        }
    }


}
