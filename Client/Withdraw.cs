using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OOP
{
    public partial class Withdraw : Form
    {
        private BankUser currentUser;

        public Withdraw(BankUser user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            currentUser = user;
            lblUsername.Text = $"Withdrawing for: {currentUser.user}";
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount) && amount > 0)
            {
                string loanFilePath = "loan.txt";
                if (File.Exists(loanFilePath))
                {
                    string[] loanLines = File.ReadAllLines(loanFilePath);
                    foreach (string loanLine in loanLines)
                    {
                        string[] loanParts = loanLine.Split(':');
                        if (loanParts.Length == 2 && loanParts[0] == currentUser.user)
                        {
                            MessageBox.Show("You have an outstanding loan. Please pay it off before withdrawing.", "Loan Outstanding", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                string balanceFilePath = "balance.txt";
                decimal currentBalance = 0;
                bool found = false;
                string[] lines = File.ReadAllLines(balanceFilePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(':');
                    if (parts.Length == 2 && parts[0] == currentUser.user && decimal.TryParse(parts[1], out currentBalance))
                    {
                        if (currentBalance >= amount)
                        {
                            currentBalance -= amount;
                            lines[i] = $"{currentUser.user}:{currentBalance}";
                            found = true;
                            break;
                        }
                        else
                        {
                            MessageBox.Show("Insufficient balance.", "Withdraw Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                if (found)
                {
                    File.WriteAllLines(balanceFilePath, lines);
                    string transactionFilePath = "transactions.txt";
                    string transactionDetails = $"{DateTime.Now}: {currentUser.user} - Withdrawal PKR{amount}";
                    File.AppendAllText(transactionFilePath, transactionDetails + Environment.NewLine);

                    MessageBox.Show($"Successfully withdrew PKR{amount}. Your new balance is PKR{currentBalance}.", "Withdraw Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error updating balance.", "Withdraw Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid positive amount to withdraw.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Withdraw_Load(object sender, EventArgs e)
        {

        }
    }
}