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

namespace OOP
{
    public partial class TransactionInfo : Form
    {
        private BankUser currentUser;
        public TransactionInfo(BankUser obj1)
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.Image5;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            currentUser = obj1;
            StyleUI ();
        }
        private void StyleUI()
        {
            
            label1.Text = "Transaction History";
            label1.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            label1.Location = new Point(250, 50);
            label1.ForeColor = Color.DeepSkyBlue;
            label1.BackColor = Color.Transparent;
            label1.TextAlign = ContentAlignment.MiddleCenter;

            
            StyleButton(button1, "Withdrawal", Color.FromArgb(20, 33, 61), Color.White, Color.MidnightBlue);
            StyleButton(button2, "Deposit", Color.FromArgb(31, 64, 104), Color.White, Color.MidnightBlue);
            StyleButton(button3, "Return", Color.FromArgb(40, 49, 59), Color.LightGray, Color.Gray);
        }

        private void StyleButton(Button btn, string text, Color bgColor, Color textColor, Color borderColor)
        {
            btn.Text = text;
            btn.BackColor = bgColor;
            btn.ForeColor = textColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = borderColor;
            btn.FlatAppearance.BorderSize = 2;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Withdrawl nextForm1 = new Withdrawl(currentUser);
            nextForm1.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DeposMenu newForm2 = new DeposMenu(currentUser);
            newForm2.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ClientMenu prevForm = new ClientMenu(currentUser);
            prevForm.Show();
            this.Close();
        }
    }
}
