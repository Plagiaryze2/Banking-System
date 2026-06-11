using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;// for DllImport


namespace OOP
{
    public partial class LoanInfo : Form
    {
        private BankUser currentUser;

        public LoanInfo(BankUser obj1)
        {
            InitializeComponent();
            currentUser = obj1;
            this.BackgroundImage = Properties.Resources.Image3;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            StyleButton(button1, Color.OrangeRed);   
            StyleButton(button2, Color.CornflowerBlue); 
            StyleButton(button3, Color.MediumTurquoise);
        }
        private void StyleButton(Button btn, Color baseColor)
        {
            btn.BackColor = Color.FromArgb(100, baseColor.R, baseColor.G, baseColor.B); 
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.White;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
         

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file2 = "loan.txt";
            double loanAmount = 0;

            string[] loanLines = File.ReadAllLines(file2);
            foreach (string line in loanLines)
            {
                string[] parts = line.Split(':');
                if (parts[0].Trim() == currentUser.user.Trim())
                {
                    if (double.TryParse(parts[1], out double parsedLoan))
                    {
                        loanAmount = parsedLoan;
                    }
                    break;
                }
            }

            currentUser.loanmoney = loanAmount;
            currentUser.money = GetLatestBalance(currentUser.user);  

            if (loanAmount > 0)
            {
                PayLoan newForm1 = new PayLoan(currentUser);
                newForm1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("No Loan Pending", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string file2 = "loan.txt";
            bool userFound = false;
            double loanAmount = 0;

            if (File.Exists(file2))
            {
                string[] loanLines = File.ReadAllLines(file2);
                foreach (string line in loanLines)
                {
                    string[] parts = line.Split(':');
                    if (parts[0].Trim() == currentUser.user.Trim())
                    {
                        userFound = true;
                        if (double.TryParse(parts[1], out double parsedLoan))
                        {
                            loanAmount = parsedLoan;
                        }
                        break;
                    }
                }
            }

            if (userFound && loanAmount > 0)
            {
                MessageBox.Show("Cannot Take Loan, When Loan Pending", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                TakeLoan newForm2 = new TakeLoan(currentUser);
                newForm2.Show();
                this.Hide();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            ClientMenu prevForm = new ClientMenu(currentUser);
            prevForm.Show();
        }
        private double GetLatestBalance(string username)
        {
            string filePath = "balance.txt";
            if (!File.Exists(filePath)) return 0;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(':');
                if (parts.Length == 2 && parts[0].Trim() == username.Trim())
                {
                    if (double.TryParse(parts[1], out double balance))
                        return balance;
                }
            }

            return 0;
        }

    }
}
