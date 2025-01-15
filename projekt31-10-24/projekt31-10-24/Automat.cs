using System;
using System.Collections.Generic;

namespace projekt31_10_24
{
    public class Automat
    
    {
        private Dodatki dodatki = new Dodatki(); // Instancja klasy Dodatki

        public bool ZamowDodatek(string nazwa, int ilosc)
        {
            return dodatki.ZamowDodatek(nazwa, ilosc); // Wywołanie metody na instancji
        }
        public int IlePizzyZostalo = 10;
        private Skladniki skladniki = new Skladniki();
        public List<Zamowienia> zamowienia = new List<Zamowienia>();

        public void WybierzPizze(Pizza pizza)
        {
            Console.WriteLine("Wybrana pizza: " + pizza.NazwaPizzy);
            zamowienia.Add(new Zamowienia { Pizza = pizza});
            ZrobPizze(pizza);
        }
        public void ZrobPizze(Pizza pizza)
        {
            Console.WriteLine("Przygotowuję pizzę: " + pizza.NazwaPizzy);
            IlePizzyZostalo--;
            Console.WriteLine("Pizza gotowa!");
            Console.WriteLine();
        }

        public void PokazDostepneSkladniki()
        {
            skladniki.PokazInfo();
        }
        //------------Dodano nowe
        public void PokazHistorieZamowien()
        {
            Console.WriteLine("Historia zamowien: ");
            foreach (var zamowienie in zamowienia)
            {
                Console.WriteLine("Pizza: " + zamowienie.Pizza.NazwaPizzy);
            }

        }
        //-------------------
        
    }


}
