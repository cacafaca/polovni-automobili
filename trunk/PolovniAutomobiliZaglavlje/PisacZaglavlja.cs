using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PolovniAutomobiliZaglavlje
{
    class PisacZaglavlja
    {
        Thread Pisac;
        bool DaLiDaRadim;
        public PisacZaglavlja()
        {
            DaLiDaRadim = false;
            Pisac = new Thread((ThreadStart)Radi);
        }
        public void Start()
        {
            DaLiDaRadim = true;
            Pisac.Start();
        }
        public void Stop()
        {
            DaLiDaRadim = false;
        }
        private void Radi()
        {
            while (DaLiDaRadim)
            {
                JednaObrada();
            }
        }
        private void JednaObrada()
        {
            string adresaZaglavlja = DajAdresu();

            Common.Http.StranaZaglavlja zaglavlje = new Common.Http.StranaZaglavlja(adresaZaglavlja);
            zaglavlje.Procitaj();

            //obradi stranu

            //upisi u bazu
        }
        private string DajAdresu()
        {
            return null;
        }
        private void ObradiStranu(string strana)
        {
        }
        private void UpisiUBazu()
        {
        }
    }
}
