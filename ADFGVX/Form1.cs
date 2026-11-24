using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ADFGVX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbVariant.SelectedIndex = 0;    // ADFGX CZ
            cmbMatrixMode.SelectedIndex = 0; // Náhodně vygenerovat
            GenerateNewMatrix();
        }

        private CipherVariant CurrentVariant => cmbVariant.SelectedIndex switch
        {
            1 => CipherVariant.AdfgxEnglish,
            2 => CipherVariant.Adfgvx,
            _ => CipherVariant.AdfgxCzech
        };

        private bool IsManualMatrix => cmbMatrixMode.SelectedIndex == 1;

        #region Správa matice

        private void GenerateNewMatrix()
        {
            string matrix = AdfgvxCipher.GenerateAlphabet(CurrentVariant);
            txtMatrix.Text = matrix;
            UpdateMatrixView();
        }

        private void UpdateMatrixView()
        {
            string current = txtMatrix.Text;
            int size = AdfgvxCipher.GetMatrixSize(CurrentVariant);
            char[] headers = AdfgvxCipher.GetHeaders(CurrentVariant);

            // 1. Znázornění / vyřazování zbývajících znaků
            string remaining = AdfgvxCipher.GetRemainingCharacters(current, CurrentVariant);
            if (remaining.Length == 0 && current.Length >= size * size)
            {
                lblRemaining.Text = "✓ Všechny znaky matice jsou vyplněny.";
                lblRemaining.ForeColor = Color.Green;
            }
            else
            {
                lblRemaining.Text = $"Zbývá ({remaining.Length}): " + string.Join(" ", remaining.ToCharArray());
                lblRemaining.ForeColor = Color.DarkRed;
            }

            // 2. Výpis šifrovací tabulky jako prostý text
            var sb = new StringBuilder();
            sb.Append("    ");
            for (int c = 0; c < size; c++)
                sb.Append(headers[c]).Append(' ');
            sb.AppendLine();

            for (int r = 0; r < size; r++)
            {
                sb.Append(headers[r]).Append(" | ");
                for (int c = 0; c < size; c++)
                {
                    int idx = r * size + c;
                    char ch = idx < current.Length ? current[idx] : '.';
                    sb.Append(ch).Append(' ');
                }
                sb.AppendLine();
            }

            txtTablePreview.Text = sb.ToString();
        }

        #endregion

        #region Obsluha událostí

        private void CmbVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateNewMatrix();
        }

        private void CmbMatrixMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool manual = IsManualMatrix;
            txtMatrix.ReadOnly = !manual;
        }

        private void BtnGenerateMatrix_Click(object sender, EventArgs e)
        {
            GenerateNewMatrix();
        }

        private void TxtMatrix_TextChanged(object sender, EventArgs e)
        {
            UpdateMatrixView();
        }

        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                string key = txtKey.Text;
                string matrix = txtMatrix.Text;
                string input = txtInput.Text;

                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Zadejte text k zašifrování.", "Upozornění", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CipherResult result = AdfgvxCipher.Encrypt(input, key, matrix, CurrentVariant);
                txtOutput.Text = result.OutputText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při šifrování:\n{ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                string key = txtKey.Text;
                string matrix = txtMatrix.Text;
                string input = txtInput.Text;

                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Zadejte text k dešifrování.", "Upozornění", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CipherResult result = AdfgvxCipher.Decrypt(input, key, matrix, CurrentVariant);
                txtOutput.Text = result.OutputText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při dešifrování:\n{ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSwap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOutput.Text)) return;

            txtInput.Text = txtOutput.Text;
            txtOutput.Clear();
        }

        #endregion
    }
}
