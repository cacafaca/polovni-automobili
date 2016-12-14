using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procode.PolovniAutomobili.DohvatanjeTestFromConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionStringMsSql = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nemanja\Source\Repos\polovni-automobili\PolAutData\Databases\PolovniAutomobili.mdf;Integrated Security=True";

            Dohvatanje.GlavnaObrada mainProcess = new Dohvatanje.GlavnaObrada(new Data.DbContext(
                Data.Provider.ProviderType.MsSql, connectionStringMsSql));

            mainProcess.Pokreni();

        }
    }
}
