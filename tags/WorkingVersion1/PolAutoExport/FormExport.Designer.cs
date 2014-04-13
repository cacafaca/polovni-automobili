namespace PolAutoExport
{
    partial class FormExport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btIzvozUCVS = new System.Windows.Forms.Button();
            this.btIzvozUExcel = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // btIzvozUCVS
            // 
            this.btIzvozUCVS.Location = new System.Drawing.Point(12, 12);
            this.btIzvozUCVS.Name = "btIzvozUCVS";
            this.btIzvozUCVS.Size = new System.Drawing.Size(268, 23);
            this.btIzvozUCVS.TabIndex = 0;
            this.btIzvozUCVS.Text = "Izvoz u CVS fajl";
            this.btIzvozUCVS.UseVisualStyleBackColor = true;
            this.btIzvozUCVS.Click += new System.EventHandler(this.btIzvozUCVS_Click);
            // 
            // btIzvozUExcel
            // 
            this.btIzvozUExcel.Location = new System.Drawing.Point(12, 41);
            this.btIzvozUExcel.Name = "btIzvozUExcel";
            this.btIzvozUExcel.Size = new System.Drawing.Size(268, 23);
            this.btIzvozUExcel.TabIndex = 1;
            this.btIzvozUExcel.Text = "Izvoz u Excel";
            this.btIzvozUExcel.UseVisualStyleBackColor = true;
            this.btIzvozUExcel.Click += new System.EventHandler(this.btIzvozUExcel_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "*.CSV";
            this.saveFileDialog1.Filter = "Coma Separated Value|*.CSV|Sve|*.*";
            // 
            // FormExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 78);
            this.Controls.Add(this.btIzvozUExcel);
            this.Controls.Add(this.btIzvozUCVS);
            this.Name = "FormExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PolAut Export";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btIzvozUCVS;
        private System.Windows.Forms.Button btIzvozUExcel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

