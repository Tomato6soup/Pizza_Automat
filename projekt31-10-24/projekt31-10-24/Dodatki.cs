using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
namespace projekt31_10_24
{
    public class Dodatki
    {
        public Dictionary<string, int> Frytki { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> Salatka { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> Napoje { get; set; } = new Dictionary<string, int>();

        public Dodatki()
        {
            WczytajDodatkiZJson();
        }

        private void WczytajDodatkiZJson()
        {
            try
            {
                // Pobierz ścieżkę do pliku z config.txt
             //   string sciezkaPliku = Config.PobierzSciezke("dodatki.json");

                // Sprawdź, czy plik istnieje
                if (File.Exists("dodatki.json"))
                {
                    string json = File.ReadAllText("dodatki.json");

                    // Wczytaj dodatki z pliku JSON
                    var dodatki = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

                    // Rozdziel dodatki na kategorie
                    foreach (var dodatek in dodatki)
                    {
                        if (dodatek.Key.ToLower().Contains("frytki"))
                        {
                            Frytki[dodatek.Key] = dodatek.Value;
                        }
                        else if (dodatek.Key.ToLower().Contains("sałatka"))
                        {
                            Salatka[dodatek.Key] = dodatek.Value;
                        }
                        else
                        {
                            Napoje[dodatek.Key] = dodatek.Value;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Plik dodatki.json nie istnieje: {"dodatki.json"}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas wczytywania dodatków: " + ex.Message);
            }
        }

        public string WyswietlDodatki()
        {
            var wynik = new StringBuilder();

            wynik.AppendLine("Dostępne dodatki:");

            wynik.AppendLine("Frytki:");
            foreach (var item in Frytki)
                wynik.AppendLine($"- {item.Key}: {item.Value}g");

            wynik.AppendLine("\nSałatka:");
            foreach (var item in Salatka)
                wynik.AppendLine($"- {item.Key}: {item.Value}g");

            wynik.AppendLine("\nNapoje:");
            foreach (var item in Napoje)
                wynik.AppendLine($"- {item.Key}: {item.Value}ml");

            return wynik.ToString();
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
