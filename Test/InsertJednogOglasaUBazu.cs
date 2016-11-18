using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class InsertJednogOglasaUBazu
    {
        static void Main()
        {
            Procode.PolovniAutomobili.Data.Provider.DataInstance.Data.Open();
            Procode.PolovniAutomobili.Data.Vehicle.Automobile autoDB = new Procode.PolovniAutomobili.Data.Vehicle.Automobile(Procode.PolovniAutomobili.Data.Provider.DataInstance.Data);
            string adresa = "http://www.polovniautomobili.com/oglas3593818/fiat_punto_12_16v/";
            Procode.PolovniAutomobili.Common.Http.StranaOglasa strOgl = new Procode.PolovniAutomobili.Common.Http.StranaOglasa(adresa);
            try
            {
                if(strOgl.Procitaj())
                    autoDB.Save(strOgl.Automobil);
            }
            catch 
            {
            }
            Procode.PolovniAutomobili.Common.Dnevnik.Isprazni();
            Procode.PolovniAutomobili.Data.Provider.DataInstance.Data.Close();
        }
    }
}
