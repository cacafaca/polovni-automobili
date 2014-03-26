using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Test
{
    class PokretanjeGlavneObrade
    {
        static void Main()
        {
            PolovniAutomobiliDohvatanje.GlavnaObrada obrada = new PolovniAutomobiliDohvatanje.GlavnaObrada();
            obrada.Pokreni();

            System.Console.WriteLine("Lupi enter za kraj obrade.");
            System.Console.ReadLine();
            System.Console.WriteLine("Zaustavljam obradu...");
            obrada.Zaustavi();
        }

    }
}