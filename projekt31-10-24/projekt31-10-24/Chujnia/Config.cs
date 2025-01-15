using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
namespace projekt31_10_24  
{
    public static class Config
    {
        private static  Dictionary<string, string> sciezki = new Dictionary<string, string>();

        public static void Config1()
        {
            if (File.Exists("config.txt"))
            {
                foreach (var linia in File.ReadAllLines("config.txt"))
                {
                    var dane = linia.Split('=');
                    if (dane.Length == 2)
                    {
                        sciezki[dane[0].Trim()] = dane[1].Trim();
                    }
                }
            }
        }

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