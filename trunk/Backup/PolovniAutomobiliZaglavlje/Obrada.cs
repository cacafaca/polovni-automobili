using System;
using System.Collections.Generic;
using Common;
using System.Text;

namespace PolovniAutomobiliZaglavlje
{
    class Obrada
    {
        PisacZaglavlja[] pisacZaglavlja;
        byte BrojPisaca;
        byte BrojPisacaPodrazumevano = 2;
        public Obrada()
        {
            try
            {
                BrojPisaca = (byte)Properties.Settings.Default.BrojPisaca;
            }
            catch (Exception ex)
            {
                BrojPisaca = BrojPisacaPodrazumevano;
                EventLogger.WriteEventError("U config fajlu nije dobar broj pisaca.", ex);
            }
            pisacZaglavlja = new PisacZaglavlja[BrojPisaca];
        }
        public void Start()
        {
            for (int i = 0; i < BrojPisaca; i++)
            {
                pisacZaglavlja[i].Start();
            }
        }
        public void Stop()
        {
            for (int i = 0; i < BrojPisaca; i++)
            {
                pisacZaglavlja[i].Start();
            }
        }
        public void Pause()
        {
        }
    }
}
