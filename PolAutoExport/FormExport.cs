using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Procode.PolovniAutomobili.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace PolAutoExport
{
    public partial class FormExport : Form
    {
        public FormExport()
        {
            InitializeComponent();
        }

        private void btIzvozUCVS_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.DefaultExt = "*.CSV";
                saveFileDialog1.Filter = "Coma Separated Value|*.CSV|Sve|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    //DataExport.ExportAutomobiliCSV(saveFileDialog1.FileName);
                    Cursor = Cursors.Default;
                    MessageBox.Show("Gotovo!", "Izvoz");
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Greska: " + ex.Message);
            }
        }

        private void btIzvozUExcel_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                saveFileDialog1.DefaultExt = "*.xlsx";
                saveFileDialog1.Filter = "Excel v2007+|*.xlsx|Sve|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    Procode.PolovniAutomobili.Data.Vehicle.Automobile a = new Procode.PolovniAutomobili.Data.Vehicle.Automobile();
                    ExportToExcel(saveFileDialog1.FileName, a.GetAllAsArray());
                    Cursor = Cursors.Default;
                    DateTime endTime = DateTime.Now;
                    MessageBox.Show(string.Format("Gotovo! Trajanje {0} min.", (endTime-startTime).TotalMinutes), "Izvoz");
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Greska: " + ex.Message);
            }
        }

        public bool ExportToExcel(string fileName, object[,] autos)
        {
            bool success = false;
            if (fileName != null && fileName != string.Empty)
            {
                if (autos != null && autos.Length > 0)
                {
                    Excel.Application exportExcel = null;
                    Excel.Workbook exportWorkbook = null;
                    Excel.Worksheet exportWorksheet = null;

                    try
                    {
                        exportExcel = new Excel.Application();
                        exportExcel.Visible = false;
                        exportWorkbook = exportExcel.Workbooks.Add();
                        exportWorksheet = exportWorkbook.Sheets[1];

                        Excel.Range range = exportWorksheet.get_Range("A1", System.Reflection.Missing.Value).
                            get_Resize(autos.GetLength(0), autos.GetLength(1));
                        range.set_Value(System.Reflection.Missing.Value, autos);

                        exportWorkbook.SaveAs(fileName);
                    }
                    finally
                    {
                        exportWorkbook.Close();

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(exportWorkbook);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(exportWorksheet);
                    }
                }
            }
            return success;
        }
    }
}
