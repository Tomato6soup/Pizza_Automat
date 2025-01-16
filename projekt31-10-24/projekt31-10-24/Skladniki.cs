using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt31_10_24
{
    public class Skladniki
    {
        public Dictionary<string, int> ListaSkladnikow { get; private set; } = new Dictionary<string, int>();

        public Skladniki()
        {
            WczytajSkladnikiZJson();
        }

        private void WczytajSkladnikiZJson()
        {
            string sciezka = "skladniki.json";
            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                ListaSkladnikow = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }
            else
            {
                Console.WriteLine($"Plik {sciezka} nie istnieje.");
            }
        }

        public void WyswietlSkladniki()
        {
            Console.WriteLine("Dostępne składniki:");
            foreach (var skladnik in ListaSkladnikow)
            {
                Console.WriteLine($"- {skladnik.Key}: {skladnik.Value}g");
            }
        }

        public bool ZuzyjSkladnik(string nazwa, int ilosc)
        {
            if (ListaSkladnikow.ContainsKey(nazwa) && ListaSkladnikow[nazwa] >= ilosc)
            {
                ListaSkladnikow[nazwa] -= ilosc;
                return true;
            }

            Console.WriteLine($"Brak wystarczającej ilości składnika: {nazwa}");
            return false;
        }
    }
}