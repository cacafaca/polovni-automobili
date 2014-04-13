using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PolAutData;

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
                    PolAutData.Vehicle.Automobile a = new PolAutData.Vehicle.Automobile();
                    a.ExportToExcel(saveFileDialog1.FileName);
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
    }
}
