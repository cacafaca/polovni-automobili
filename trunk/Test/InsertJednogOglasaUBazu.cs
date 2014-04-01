using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class InsertJednogOglasaUBazu
    {
        static void Main()
        {
            //PolAutData.AutomobilDB autoDB = new PolAutData.AutomobilDB();
            string adresa = "http://www.polovniautomobili.com/oglas3071964/opel_kadett_suza_12/";
            Common.Http.StranaOglasa strOgl = new Common.Http.StranaOglasa(adresa);
            try
            {
                //if(strOgl.Procitaj())
                    //autoDB.Snimi2(strOgl.Automobil);
            }
            catch 
            {
            }
            Common.Dnevnik.Isprazni();
        }
    }
}
