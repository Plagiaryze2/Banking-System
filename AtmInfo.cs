using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OOP
{
    public partial class AtmInfo : Form
    {
        private BankUser currentuser;
        private Label balLab;
        private TextBox amounted;
        private Button depoButt, withButt, reqCard;
        private string atmFile = "atm.txt";
        private double currentBalance = 0;
        private string atmPin;

        public AtmInfo(BankUser user)
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.Image6;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.MaximizeBox= false;
            this.MinimizeBox = false;
            currentuser = user;
            this.FormClosing += Form1_FormClosing;
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "ATM Information";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Beige;

            if (UserHasAtm(currentuser.user))
            {
                balLab = new Label
                {
                    Text = $"ATM Balance: {currentBalance:C}",
                    Location = new Point(30, 30),
                    AutoSize = true
                    

                };
                balLab.ForeColor = Color.White;
                balLab.BackColor = Color.Transparent;
                balLab.ForeColor = Color.White;

                this.Controls.Add(balLab);

                amounted = new TextBox
                {
                    Location = new Point(30, 75),
                    Size = new Size(100, 25)
                };
                this.Controls.Add(amounted);

                depoButt = new Button
                {
                    Text = "Deposit",
                    Location = new Point(150, 70),
                    Size = new Size(80, 30),
                   
                };
                depoButt.BackColor = Color.DeepSkyBlue;
                depoButt.ForeColor = Color.White;
                depoButt.FlatStyle = FlatStyle.Flat;
                depoButt.FlatAppearance.BorderSize = 0;

                depoButt.Click += depoButt_Click;
                this.Controls.Add(depoButt);

                withButt = new Button
                {
                    Text = "Withdraw",
                    Location = new Point(240, 70),
                    Size = new Size(80, 30)
                };
                withButt.BackColor = Color.MediumSeaGreen;
                withButt.ForeColor = Color.White;
                withButt.FlatStyle = FlatStyle.Flat;
                withButt.FlatAppearance.BorderSize = 0;

                withButt.Click += withButt_Click;
                this.Controls.Add(withButt);
            }
            else
            {
                reqCard = new Button
                {
                    Text = "Request ATM Card",
                    Location = new Point(100, 100),
                    Size = new Size(180, 40),
                    BackColor = Color.LightBlue
                };
                reqCard.Click += reqCard_Click;
                this.Controls.Add(reqCard);
            }
        }

        private void depoButt_Click(object sender, EventArgs e)
        {
            if (double.TryParse(amounted.Text, out double amount) && amount > 0)
            {
                currentBalance += amount;
                UpdateAtmFile(currentuser.user, atmPin, currentBalance);
                balLab.Text = $"ATM Balance: {currentBalance:C}";
                MessageBox.Show("Deposit successful!", "Success");
            }
            else
            {
                MessageBox.Show("Enter a valid deposit amount.");
            }
        }

        private void withButt_Click(object sender, EventArgs e)
        {
            if (double.TryParse(amounted.Text, out double amount) && amount > 0)
            {
                if (amount > currentBalance)
                {
                    MessageBox.Show("Insufficient ATM balance.");
                    return;
                }

                currentBalance -= amount;
                UpdateAtmFile(currentuser.user, atmPin, currentBalance);
                balLab.Text = $"ATM Balance: {currentBalance:C}";
                MessageBox.Show("Withdrawal successful!", "Success");
            }
            else
            {
                MessageBox.Show("Enter a valid withdrawal amount.");
            }
        }

        private void reqCard_Click(object sender, EventArgs e)
        {
            NewCard card = new NewCard(currentuser);  
            card.Show();
            this.Hide(); 
        }

        private bool UserHasAtm(string username)
        {
            if (!File.Exists(atmFile)) return false;

            string[] lines = File.ReadAllLines(atmFile);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 3 && parts[0] == username)
                {
                    atmPin = parts[1];
                    currentBalance = double.TryParse(parts[2], out double bal) ? bal : 0;
                    return true;
                }
            }
            return false;
        }

        private void UpdateAtmFile(string username, string pin, double balance)
        {
            var lines = File.ReadAllLines(atmFile).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(username + ":"))
                {
                    lines[i] = $"{username}:{pin}:{balance}";
                    break;
                }
            }
            File.WriteAllLines(atmFile, lines);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientMenu form1 = new ClientMenu(currentuser);
            form1.Show();
        }
    }
}
