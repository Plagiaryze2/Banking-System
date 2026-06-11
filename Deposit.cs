using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OOP
{
    public partial class Deposit : Form
    {
        private BankUser currentUser;

        public Deposit(BankUser user)
        {
            InitializeComponent();
            currentUser = user;
            lblUsername.Text = $"Depositing for: {currentUser.user}";
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount) && amount > 0)
            {
                string balanceFilePath = "balance.txt";
                decimal currentBalance = 0;
                bool found = false;
                string[] lines = File.ReadAllLines(balanceFilePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(':');
                    if (parts.Length == 2 && parts[0] == currentUser.user && decimal.TryParse(parts[1], out currentBalance))
                    {
                        currentBalance += amount;
                        lines[i] = $"{currentUser.user}:{currentBalance}";
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    File.WriteAllLines(balanceFilePath, lines);

                    string transactionFilePath = "deposit.txt";
                    string transactionDetails = $"{DateTime.Now}: {currentUser.user} - Deposit PKR{amount}";
                    File.AppendAllText(transactionFilePath, transactionDetails + Environment.NewLine);

                    MessageBox.Show($"Successfully deposited PKR{amount}. Your new balance is PKR{currentBalance}.", "Deposit Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error updating balance.", "Deposit Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid positive amount to deposit.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Deposit_Load(object sender, EventArgs e)
        {

        }
    }
}
