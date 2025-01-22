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
            string sciezka = Config.PobierzSciezke("Skladniki");
            if (File.Exists(sciezka))
            {
                string json = File.ReadAllText(sciezka);
                ListaSkladnikow = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }
            else
            {
                Console.WriteLine($"Plik {sciezka} nie istnieje.");
                ZapiszSkladnikiDoJson();
            }
        }

        public void WyswietlSkladniki()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Dostępne składniki:");
            foreach (var skladnik in ListaSkladnikow)
            {
                Console.WriteLine($"- {skladnik.Key}: {skladnik.Value}g");
                if (skladnik.Value < 100) // Ostrzeżenie przy niskim stanie
                {
                    Console.WriteLine($"UWAGA: Niski stan składnika {skladnik.Key}!");
                }
            }
            Console.ResetColor();
        }

        public void UzupelnijSkladnik(string nazwa, int ilosc)
        {
            if (ListaSkladnikow.ContainsKey(nazwa))
            {
                ListaSkladnikow[nazwa] += ilosc;
                //ZapiszSkladnikiDoJson();
               // Console.WriteLine($"Uzupełniono składnik {nazwa} o {ilosc} jednostek. Nowy stan: {ListaSkladnikow[nazwa]}.");
            }
            else
            {
                ListaSkladnikow[nazwa] = ilosc;
               // Console.WriteLine($"Składnik {nazwa} nie istnieje.");
            }
            ZapiszSkladnikiDoJson();
            Console.WriteLine($"Uzupełniono składnik {nazwa} o {ilosc} jednostek. Nowy stan: {ListaSkladnikow[nazwa]}.");
        }

        private void ZapiszSkladnikiDoJson()
        {
            string sciezka = Config.PobierzSciezke("Skladniki");
            File.WriteAllText(sciezka, JsonConvert.SerializeObject(ListaSkladnikow, Formatting.Indented));
        }

        public bool ZuzyjSkladnik(string nazwa, int ilosc)
        {
            if (ListaSkladnikow.ContainsKey(nazwa) && ListaSkladnikow[nazwa] >= ilosc)
            {
                ListaSkladnikow[nazwa] -= ilosc;
                ZapiszSkladnikiDoJson();
                return true;
            }

            Console.WriteLine($"Brak wystarczającej ilości składnika: {nazwa}");
            return false;
        }
    }
}