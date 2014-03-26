using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace PolAutData
{
    public static class DataExport
    {
        private static DataSet DajAutomobil()
        {
            Data data = new Data();
            DataSet ds = data.Otvori("select  * from automobil", null);
            data.ZatvoriKonekciju(); // da li sme da se zatvori
            return ds;
        }

        public static void ExportAutomobiliCSV(string path)
        {
            DataSet ds = DajAutomobil();
            using (StreamWriter file = new StreamWriter(path))
            {
                //zaglavlje
                string naziv = string.Empty;
                foreach (Object col in ds.Tables[0].Columns)
                {
                    naziv += '"' + col.ToString()+"\";";
                }
                file.WriteLine(naziv);

                //podaci
                foreach (DataRow red in ds.Tables[0].Rows)
                {
                    string redText = string.Empty;
                    foreach (Object kolona in red.ItemArray)
                    {
                        //redText += kolona.ToString().Replace(System.Environment.NewLine, "") + ';';
                        redText += '"' + kolona.ToString().Replace('\r', ' ').Replace('\n', ' ') + "\";";
                    }
                    file.WriteLine(redText);
                }
                file.Close();
            }
        }

        public static void ExportAutomobiliExcel(string path)
        {
            DataSet ds = DajAutomobil();

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();;
            if (excel == null)
            {
                throw new Exception("Ne mogu da otvorim excel.");
            }
            excel.Visible = false;
            excel.DisplayAlerts = false;

            Workbook dok = excel.Workbooks.Add(1);
            try
            {
                Worksheet ws = (Worksheet)dok.Sheets[1];

                //zaglavlje
                Range poc = ws.Cells[1, 1];
                Range kraj = ws.Cells[1, ds.Tables[0].Columns.Count];
                Range zaglavlje = ws.get_Range(poc, kraj);
                zaglavlje.Interior.Color = System.Drawing.Color.Gray;
                zaglavlje.Font.Bold = true;
                for (int brKol = 0; brKol < ds.Tables[0].Columns.Count; brKol++)
                {
                    DataColumn kol = ds.Tables[0].Columns[brKol];
                    //Range range = ws.get_Range(brKol+1);
                    //range.
                    ws.Cells[1, brKol + 1] = kol.ToString();
                }

                // priprema u niz
                string[,] niz2d = new string[ds.Tables[0].Rows.Count, ds.Tables[0].Columns.Count];
                for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow red = ds.Tables[0].Rows[i];
                    for (int j = 0; j< red.Table.Columns.Count; j++)
                    {
                        niz2d[i, j] = red[j].ToString();
                    }
                }

                //podaci
                poc = ws.Cells[2, 1];
                kraj = ws.Cells[1 + ds.Tables[0].Rows.Count, ds.Tables[0].Columns.Count];
                Range podaci = ws.get_Range(poc, kraj);
                podaci.WrapText = false;
                podaci.set_Value(XlRangeValueDataType.xlRangeValueDefault, niz2d);
                
                /*for (int brojReda = 0; brojReda < ds.Tables[0].Rows.Count; brojReda++)
                {
                    DataRow red = ds.Tables[0].Rows[brojReda];
                    for (int brojKolone = 0; brojKolone < red.Table.Columns.Count; brojKolone++)
                    {
                        ws.Cells[brojReda + 2, brojKolone + 1] = red[brojKolone].ToString();
                    }
                } */               
                dok.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false,
                    false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
            }
            finally
            {
                dok.Close();
                excel.Quit();
                excel = null;
            }
            //GC.Collect();
        }
    }
}
