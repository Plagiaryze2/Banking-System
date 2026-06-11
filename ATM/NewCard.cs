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
    public partial class NewCard : Form
    {
        private BankUser currentuser;
        public NewCard(BankUser user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            currentuser = user;
            SetupForm();
            this.FormClosing += Form1_FormClosing;
        }
        private void SetupForm()
        {
            this.Text = "Request ATM Card";
            this.Size = new System.Drawing.Size(300, 200);

            Label pinlab = new Label { Text = "Set 4-digit PIN:", Location = new System.Drawing.Point(30, 30) };
            TextBox pinb = new TextBox { Location = new System.Drawing.Point(130, 30), Width = 100 };
            Button request = new Button { Text = "Request", Location = new System.Drawing.Point(90, 80), Width = 100 };

            this.Controls.Add(pinlab);
            this.Controls.Add(pinb);
            this.Controls.Add(request);

            request.Click += (s, e) =>
            {
                string pin = pinb.Text.Trim();
                if (pin.Length == 4 && int.TryParse(pin, out _))
                {
                    File.AppendAllText("atm.txt", $"{currentuser.user}:{pin}:0{Environment.NewLine}");
                    MessageBox.Show("ATM card requested successfully!", "Success");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Enter a valid 4-digit PIN.");
                }
            };
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AtmInfo form1 = new AtmInfo(currentuser);

            form1.Show();
        }
    }
}
