using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class FbParalelnaTransakcija
    {
        public static void Main()
        {
            Procode.PolovniAutomobili.Data.Provider.Data d1;
            d1 = Procode.PolovniAutomobili.Data.Provider.Data.GetNewDataInstance();
            //d2 = PolAutData.Provider.Data.GetNewDataInstance();
            d1.Open();
            //d2.Open();
            d1.BeginTran();
            d1.BeginTran();
            System.Data.DataSet ds1;
            d1.GetDataSet("select 1 from RDB$DATABASE", out ds1);
            //d2.GetDataSet("select 2 from RDB$DATABASE", out ds2);
            d1.CommitTran();
            d1.CommitTran();
            d1.Close();
            //d2.Close();
        }
    }
}
