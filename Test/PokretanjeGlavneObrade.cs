using System;
using System.Collections.Generic;
using System.Text;
using Procode.PolovniAutomobili.Common;

namespace Test
{
    class PokretanjeGlavneObrade
    {
        static void Main()
        {
            Procode.PolovniAutomobili.Dohvatanje.GlavnaObrada obrada = new Procode.PolovniAutomobili.Dohvatanje.GlavnaObrada();
            obrada.Pokreni();

            System.Console.WriteLine("Lupi enter za kraj obrade.");
            System.Console.ReadLine();
            System.Console.WriteLine("Zaustavljam obradu...");
            obrada.Zaustavi();
        }

    }
}