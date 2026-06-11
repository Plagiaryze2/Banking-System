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
    public partial class TakeLoan : Form
    {
        private BankUser currentUser;

        public TakeLoan(BankUser obj1)
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.Image9;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            currentUser = obj1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string loanText = textBox1.Text;
            double loanAmount;

            if (!double.TryParse(loanText, out loanAmount))
            {
                MessageBox.Show("Please enter a valid loan amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentUser.money >= 0.20 * loanAmount)
            {
                currentUser.money += loanAmount;
                currentUser.loanmoney = loanAmount;

                string file1 = "balance.txt";

                string[] lines = File.ReadAllLines(file1);

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(':');
                    if (parts[0].Equals(currentUser.user))
                    {
                        lines[i] = $"{parts[0]}:{currentUser.money}";
                        break;
                    }
                }
                File.WriteAllLines(file1, lines);


                string file2 = "loan.txt";
                string[] loanLines = File.Exists(file2) ? File.ReadAllLines(file2) : new string[0];
                bool userFound = false;

                for (int i = 0; i < loanLines.Length; i++)
                {
                    string[] parts = loanLines[i].Split(':');
                    if (parts[0].Equals(currentUser.user))
                    {
                        double existingLoan = double.Parse(parts[1]);

                        if (existingLoan == 0)
                        {
                            loanLines[i] = $"{currentUser.user}:{loanAmount}";
                            File.WriteAllLines(file2, loanLines);
                            MessageBox.Show("Loan granted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("You already have Loan Pending", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        userFound = true;
                        break;
                    }
                }

                if (!userFound)
                {
                    using (StreamWriter sw = File.AppendText(file2))
                    {
                        sw.WriteLine($"{currentUser.user}:{loanAmount}");
                    }

                    MessageBox.Show("Loan granted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Loan exceeds 20% of balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            LoanInfo prevForm = new LoanInfo(currentUser);
            prevForm.Show();
            this.Close();
        }
    }
}
