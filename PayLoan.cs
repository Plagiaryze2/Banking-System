using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP
{
    public partial class PayLoan : Form
    {
        private BankUser currentUser;

        public PayLoan(BankUser obj1)
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.Image8;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            currentUser = obj1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoanInfo prevForm = new LoanInfo(currentUser);
            prevForm.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string enteredAmount = textBox1.Text;

            if (!double.TryParse(enteredAmount, out double amount))
            {
                MessageBox.Show("Please enter a valid numeric amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentUser.loanmoney == 0)
            {
                MessageBox.Show("You Do Not Have a Loan to Pay", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (amount > currentUser.loanmoney)
            {
                MessageBox.Show($"Your Loan Due is: {currentUser.loanmoney}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string loanFile = "loan.txt";
            string[] loanLines = File.Exists(loanFile) ? File.ReadAllLines(loanFile) : new string[0];
            bool userFoundInLoan = false;

            for (int i = 0; i < loanLines.Length; i++)
            {
                string[] parts = loanLines[i].Split(':');
                if (parts[0] == currentUser.user)
                {
                    double existingLoan = double.Parse(parts[1]);

                    if (existingLoan == 0)
                    {
                        MessageBox.Show("You have no outstanding loan to pay.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    double newLoanDue = existingLoan - amount;
                    loanLines[i] = $"{currentUser.user}:{newLoanDue}";
                    userFoundInLoan = true;
                    break;
                }
            }

            if (!userFoundInLoan)
            {
                double newLoanDue = (double)currentUser.loanmoney - amount;
                using (StreamWriter sw = File.AppendText(loanFile))
                {
                    sw.WriteLine($"{currentUser.user}:{newLoanDue}");
                }
            }
            else
            {
                File.WriteAllLines(loanFile, loanLines);
            }

            string balanceFile = "balance.txt";
            string[] balanceLines = File.Exists(balanceFile) ? File.ReadAllLines(balanceFile) : new string[0];
            bool userFoundInBalance = false;

            for (int i = 0; i < balanceLines.Length; i++)
            {
                string[] parts = balanceLines[i].Split(':');
                if (parts[0] == currentUser.user)
                {
                    double existingBalance = double.Parse(parts[1]);
                    double newBalance = existingBalance - amount;
                    balanceLines[i] = $"{currentUser.user}:{newBalance}";
                    userFoundInBalance = true;
                    break;
                }
            }

            if (!userFoundInBalance)
            {
                using (StreamWriter sw = File.AppendText(balanceFile))
                {
                    sw.WriteLine($"{currentUser.user}:{amount}");
                }
            }
            else
            {
                File.WriteAllLines(balanceFile, balanceLines);
            }

            currentUser.loanmoney -= amount;
            if (currentUser.loanmoney < 0)
                currentUser.loanmoney = 0;

            MessageBox.Show($"Payment Successful! Remaining Loan: {currentUser.loanmoney}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
