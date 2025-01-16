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
            string sciezka = "dodatki.json";
            if (File.Exists(sciezka))
            {
                try
                {
                    string json = File.ReadAllText(sciezka);

                    // Wczytaj do tymczasowego słownika z prostą strukturą
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
            }
        }

        public void WyswietlDodatki()
        {
            Console.WriteLine("Dostępne dodatki:");
            foreach (var dodatek in DostepneDodatki)
            {
                Console.WriteLine($"- {dodatek.Key}: {dodatek.Value.Ilosc}g/ml, Cena za jednostkę: {dodatek.Value.CenaZaJednostke} PLN");
            }
        }

        public bool ZamowDodatek(string nazwa, int ilosc, out double cena)
        {
            cena = 0;
            if (DostepneDodatki.ContainsKey(nazwa) && DostepneDodatki[nazwa].Ilosc >= ilosc)
            {
                var (iloscDostepna, cenaZaJednostke) = DostepneDodatki[nazwa];
                DostepneDodatki[nazwa] = (iloscDostepna - ilosc, cenaZaJednostke);
                cena = ilosc * cenaZaJednostke;
                return true;
            }

            return false;
        }
    }


}
