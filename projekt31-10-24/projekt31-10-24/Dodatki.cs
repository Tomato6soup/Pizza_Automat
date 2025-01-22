using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
namespace projekt31_10_24
{
    public class Dodatki
    {
        public Dictionary<string, (int Ilosc, double CenaZaJednostke)> DostepneDodatki { get; private set; } = new Dictionary<string, (int, double)>();

        public Dodatki()
        {
            WczytajDodatkiZJson();
        }

        private void WczytajDodatkiZJson()
        {
            string sciezka = Config.PobierzSciezke("Dodatki");
            if (File.Exists(sciezka))
            {
                try
                {
                    string json = File.ReadAllText(sciezka);
                    var tymczasoweDodatki = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);
                    if (tymczasoweDodatki != null)
                    {
                        foreach (var dodatek in tymczasoweDodatki)
                        {
                            if (dodatek.Value.ContainsKey("Ilosc") && dodatek.Value.ContainsKey("CenaZaJednostke"))
                            {
                                int ilosc = Convert.ToInt32(dodatek.Value["Ilosc"]);
                                double cenaZaJednostke = dodatek.Value["CenaZaJednostke"];
                                DostepneDodatki[dodatek.Key] = (ilosc, cenaZaJednostke);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas wczytywania dodatków: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Plik {sciezka} nie istnieje.");
                ZapiszDodatkiDoJson();
            }
        }

        public void WyswietlDodatki()
        {
            Console.WriteLine("Dostępne dodatki:");
            foreach (var dodatek in DostepneDodatki)
            {
                Console.WriteLine($"- {dodatek.Key}: {dodatek.Value.Ilosc}g/ml, Cena za jednostkę: {dodatek.Value.CenaZaJednostke} PLN");
                if (dodatek.Value.Ilosc < 100) // Ostrzeżenie przy niskim stanie
                {
                    Console.WriteLine($"UWAGA: Niski stan dodatku {dodatek.Key}!");
                }
            }
        }

        public void UzupelnijDodatek(string nazwa, int ilosc)
        {
            if (DostepneDodatki.ContainsKey(nazwa))
            {
                var (iloscDostepna, cenaZaJednostke) = DostepneDodatki[nazwa];
                DostepneDodatki[nazwa] = (iloscDostepna + ilosc, cenaZaJednostke);
               // ZapiszDodatkiDoJson();
               // Console.WriteLine($"Uzupełniono dodatek {nazwa} o {ilosc} jednostek. Nowy stan: {DostepneDodatki[nazwa].Ilosc}.");
            }
            else
            {
                DostepneDodatki[nazwa] = (ilosc, 0.6); // Jeśli nowy dodatek, ceny za jednostkę
               // Console.WriteLine($"Dodatek {nazwa} nie istnieje.");
            }
            ZapiszDodatkiDoJson();
            Console.WriteLine($"Uzupełniono dodatek {nazwa} o {ilosc} jednostek. Nowy stan: {DostepneDodatki[nazwa].Ilosc}.");
        }

        private void ZapiszDodatkiDoJson()
        {
            string sciezka = Config.PobierzSciezke("Dodatki");
            var dodatkiDoZapisu = new Dictionary<string, Dictionary<string, double>>();
            foreach (var dodatek in DostepneDodatki)
            {
                dodatkiDoZapisu[dodatek.Key] = new Dictionary<string, double>
            {
                { "Ilosc", dodatek.Value.Ilosc },
                { "CenaZaJednostke", dodatek.Value.CenaZaJednostke }
            };
            }
            File.WriteAllText(sciezka, JsonConvert.SerializeObject(dodatkiDoZapisu, Formatting.Indented));
        }


        public bool ZamowDodatek(string nazwa, int ilosc, out double cena)
        {
            cena = 0;
            if (DostepneDodatki.ContainsKey(nazwa) && DostepneDodatki[nazwa].Ilosc >= ilosc)
            {
                var (iloscDostepna, cenaZaJednostke) = DostepneDodatki[nazwa];
                DostepneDodatki[nazwa] = (iloscDostepna - ilosc, cenaZaJednostke);
                cena = ilosc * cenaZaJednostke;
                ZapiszDodatkiDoJson();
                return true;
            }
            Console.WriteLine($"Brak wystarczającej ilości dodatku: {nazwa}");
            return false;
        }
    }
}



