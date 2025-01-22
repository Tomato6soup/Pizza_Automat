using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
namespace projekt31_10_24  
{
    public static class Config
    {
        private static Dictionary<string, string> sciezki = new Dictionary<string, string>();
        private static string bazaSciezka = AppDomain.CurrentDomain.BaseDirectory;

        public static void WczytajKonfiguracje()
        {
            string sciezkaConfig = Path.Combine(bazaSciezka, "config.txt");
            if (File.Exists(sciezkaConfig))
            {
                foreach (var linia in File.ReadAllLines(sciezkaConfig))
                {
                    var dane = linia.Split('=');
                    if (dane.Length == 2)
                    {
                        sciezki[dane[0].Trim()] = Path.Combine(bazaSciezka, dane[1].Trim());
                    }
                }
            }
            else
            {
                Console.WriteLine("Plik config.txt nie istnieje.");
            }
        }

        // Pobiera ścieżkę na podstawie klucza
        public static string PobierzSciezke(string klucz)
        {
            if (sciezki.ContainsKey(klucz))
            {
                return sciezki[klucz];
            }
            else
            {
                throw new KeyNotFoundException($"Klucz '{klucz}' nie został znaleziony w konfiguracji.");
            }
        }
    }
}