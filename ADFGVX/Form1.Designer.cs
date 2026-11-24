namespace ADFGVX
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblVariant = new System.Windows.Forms.Label();
            this.cmbVariant = new System.Windows.Forms.ComboBox();
            this.lblMatrixMode = new System.Windows.Forms.Label();
            this.cmbMatrixMode = new System.Windows.Forms.ComboBox();
            this.btnGenerateMatrix = new System.Windows.Forms.Button();

            this.lblMatrix = new System.Windows.Forms.Label();
            this.txtMatrix = new System.Windows.Forms.TextBox();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.lblTablePreview = new System.Windows.Forms.Label();
            this.txtTablePreview = new System.Windows.Forms.TextBox();

            this.lblKey = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.lblInput = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();

            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnSwap = new System.Windows.Forms.Button();

            this.lblOutput = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();

            this.SuspendLayout();

            // 
            // lblVariant
            // 
            this.lblVariant.AutoSize = true;
            this.lblVariant.Location = new System.Drawing.Point(16, 15);
            this.lblVariant.Name = "lblVariant";
            this.lblVariant.Size = new System.Drawing.Size(38, 15);
            this.lblVariant.TabIndex = 0;
            this.lblVariant.Text = "Verze:";

            // 
            // cmbVariant
            // 
            this.cmbVariant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVariant.FormattingEnabled = true;
            this.cmbVariant.Items.AddRange(new object[] {
                "ADFGX (CZ verze, 5x5, W -> V)",
                "ADFGX (EN verze, 5x5, J -> I)",
                "ADFGVX (6x6, A-Z + 0-9)"
            });
            this.cmbVariant.Location = new System.Drawing.Point(60, 12);
            this.cmbVariant.Name = "cmbVariant";
            this.cmbVariant.Size = new System.Drawing.Size(200, 23);
            this.cmbVariant.TabIndex = 1;
            this.cmbVariant.SelectedIndexChanged += new System.EventHandler(this.CmbVariant_SelectedIndexChanged);

            // 
            // lblMatrixMode
            // 
            this.lblMatrixMode.AutoSize = true;
            this.lblMatrixMode.Location = new System.Drawing.Point(275, 15);
            this.lblMatrixMode.Name = "lblMatrixMode";
            this.lblMatrixMode.Size = new System.Drawing.Size(46, 15);
            this.lblMatrixMode.TabIndex = 2;
            this.lblMatrixMode.Text = "Matice:";

            // 
            // cmbMatrixMode
            // 
            this.cmbMatrixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMatrixMode.FormattingEnabled = true;
            this.cmbMatrixMode.Items.AddRange(new object[] {
                "Náhodně vygenerovat",
                "Zadat ručně"
            });
            this.cmbMatrixMode.Location = new System.Drawing.Point(327, 12);
            this.cmbMatrixMode.Name = "cmbMatrixMode";
            this.cmbMatrixMode.Size = new System.Drawing.Size(155, 23);
            this.cmbMatrixMode.TabIndex = 3;
            this.cmbMatrixMode.SelectedIndexChanged += new System.EventHandler(this.CmbMatrixMode_SelectedIndexChanged);

            // 
            // btnGenerateMatrix
            // 
            this.btnGenerateMatrix.Location = new System.Drawing.Point(495, 11);
            this.btnGenerateMatrix.Name = "btnGenerateMatrix";
            this.btnGenerateMatrix.Size = new System.Drawing.Size(165, 25);
            this.btnGenerateMatrix.TabIndex = 4;
            this.btnGenerateMatrix.Text = "Nová náhodná matice";
            this.btnGenerateMatrix.UseVisualStyleBackColor = true;
            this.btnGenerateMatrix.Click += new System.EventHandler(this.BtnGenerateMatrix_Click);

            // 
            // lblMatrix
            // 
            this.lblMatrix.AutoSize = true;
            this.lblMatrix.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMatrix.Location = new System.Drawing.Point(16, 48);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(189, 15);
            this.lblMatrix.TabIndex = 5;
            this.lblMatrix.Text = "Šifrovací matice (25 nebo 36 znaků):";

            // 
            // txtMatrix
            // 
            this.txtMatrix.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtMatrix.Location = new System.Drawing.Point(16, 68);
            this.txtMatrix.MaxLength = 36;
            this.txtMatrix.Name = "txtMatrix";
            this.txtMatrix.Size = new System.Drawing.Size(430, 23);
            this.txtMatrix.TabIndex = 6;
            this.txtMatrix.TextChanged += new System.EventHandler(this.TxtMatrix_TextChanged);

            // 
            // lblRemaining
            // 
            this.lblRemaining.ForeColor = System.Drawing.Color.DarkRed;
            this.lblRemaining.Location = new System.Drawing.Point(16, 95);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(430, 32);
            this.lblRemaining.TabIndex = 7;
            this.lblRemaining.Text = "Zbývající znaky:";

            // 
            // lblTablePreview
            // 
            this.lblTablePreview.AutoSize = true;
            this.lblTablePreview.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTablePreview.Location = new System.Drawing.Point(460, 48);
            this.lblTablePreview.Name = "lblTablePreview";
            this.lblTablePreview.Size = new System.Drawing.Size(130, 15);
            this.lblTablePreview.TabIndex = 8;
            this.lblTablePreview.Text = "Výpis šifrovací tabulky:";

            // 
            // txtTablePreview
            // 
            this.txtTablePreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.txtTablePreview.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTablePreview.Location = new System.Drawing.Point(460, 68);
            this.txtTablePreview.Multiline = true;
            this.txtTablePreview.Name = "txtTablePreview";
            this.txtTablePreview.ReadOnly = true;
            this.txtTablePreview.Size = new System.Drawing.Size(200, 115);
            this.txtTablePreview.TabIndex = 9;

            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKey.Location = new System.Drawing.Point(16, 135);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(81, 15);
            this.lblKey.TabIndex = 10;
            this.lblKey.Text = "Klíčové slovo:";

            // 
            // txtKey
            // 
            this.txtKey.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtKey.Location = new System.Drawing.Point(16, 155);
            this.txtKey.MaxLength = 50;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(430, 23);
            this.txtKey.TabIndex = 11;
            this.txtKey.Text = "TAJNYKLIC";

            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblInput.Location = new System.Drawing.Point(16, 190);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(82, 15);
            this.lblInput.TabIndex = 12;
            this.lblInput.Text = "Vstupní text:";

            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtInput.Location = new System.Drawing.Point(16, 210);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(644, 75);
            this.txtInput.TabIndex = 13;
            this.txtInput.Text = "Ahoj svete! Tajna zprava cislo 1.";

            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnEncrypt.Location = new System.Drawing.Point(16, 295);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(120, 30);
            this.btnEncrypt.TabIndex = 14;
            this.btnEncrypt.Text = "🔒 Šifrovat";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.BtnEncrypt_Click);

            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnDecrypt.Location = new System.Drawing.Point(145, 295);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(120, 30);
            this.btnDecrypt.TabIndex = 15;
            this.btnDecrypt.Text = "🔓 Dešifrovat";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.BtnDecrypt_Click);

            // 
            // btnSwap
            // 
            this.btnSwap.Location = new System.Drawing.Point(275, 295);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(160, 30);
            this.btnSwap.TabIndex = 16;
            this.btnSwap.Text = "⇅ Vyměnit vstup/výstup";
            this.btnSwap.UseVisualStyleBackColor = true;
            this.btnSwap.Click += new System.EventHandler(this.BtnSwap_Click);

            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblOutput.Location = new System.Drawing.Point(16, 335);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(262, 15);
            this.lblOutput.TabIndex = 17;
            this.lblOutput.Text = "Výstupní text (skupiny po 5 znacích):";

            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtOutput.Location = new System.Drawing.Point(16, 355);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(644, 90);
            this.txtOutput.TabIndex = 18;

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 465);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.btnSwap);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.txtTablePreview);
            this.Controls.Add(this.lblTablePreview);
            this.Controls.Add(this.lblRemaining);
            this.Controls.Add(this.txtMatrix);
            this.Controls.Add(this.lblMatrix);
            this.Controls.Add(this.btnGenerateMatrix);
            this.Controls.Add(this.cmbMatrixMode);
            this.Controls.Add(this.lblMatrixMode);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.lblVariant);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ADFG(V)X Šifra";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblVariant;
        private System.Windows.Forms.ComboBox cmbVariant;
        private System.Windows.Forms.Label lblMatrixMode;
        private System.Windows.Forms.ComboBox cmbMatrixMode;
        private System.Windows.Forms.Button btnGenerateMatrix;

        private System.Windows.Forms.Label lblMatrix;
        private System.Windows.Forms.TextBox txtMatrix;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.Label lblTablePreview;
        private System.Windows.Forms.TextBox txtTablePreview;

        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.TextBox txtInput;

        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnSwap;

        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.TextBox txtOutput;
    }
}
