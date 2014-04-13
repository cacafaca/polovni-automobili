using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class InsertJednogOglasaUBazu
    {
        static void Main()
        {
            PolAutData.Provider.DataInstance.Data.Open();
            PolAutData.Vehicle.Automobile autoDB = new PolAutData.Vehicle.Automobile(PolAutData.Provider.DataInstance.Data);
            string adresa = "http://www.polovniautomobili.com/oglas3593818/fiat_punto_12_16v/";
            Common.Http.StranaOglasa strOgl = new Common.Http.StranaOglasa(adresa);
            try
            {
                if(strOgl.Procitaj())
                    autoDB.Save(strOgl.Automobil);
            }
            catch 
            {
            }
            Common.Dnevnik.Isprazni();
            PolAutData.Provider.DataInstance.Data.Close();
        }
    }
}
