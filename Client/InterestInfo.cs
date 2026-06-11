using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace OOP
{
    public partial class InterestInfo : Form
    {
        private BankUser currentUser;
        private Label princilab, ratelab, timlab, interlab, balLab;
        private TextBox timeTextBox;
        private RadioButton simrad, comrad;
        private Button calculateButton, applyButton;
        private const double fixer = 5.0;
        private double latestInter = 0;
        private double newBal = 0;
        private string interlogs = "last_interest.txt";

        public InterestInfo(BankUser user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            currentUser = user;
            SetupForm();
            CreateControls();
            LoadInterestDate();
            
            this.FormClosing += Form1_FormClosing;
        }

        private void SetupForm()
        {
            this.BackgroundImage = Properties.Resources.Image2; 
            this.BackgroundImageLayout = ImageLayout.Stretch; 

            this.Text = "Interest Calculator";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void CreateControls()
        {
            double liveBalance = GetLiveBalance(currentUser.user);

            princilab = CreateLabel($"Principal: {liveBalance:C}", 30);
            princilab.BackColor = Color.Transparent;
            ratelab = CreateLabel($"Interest Rate: {fixer}% (Fixed)", 70);
            ratelab.BackColor = Color.Transparent;

            timlab = CreateLabel("Time (in years):", 110);
            timlab.BackColor = Color.Transparent;
            timeTextBox = new TextBox { Location = new Point(180, 110), Size = new Size(100, 25) };
           
            this.Controls.Add(timeTextBox);

            simrad = new RadioButton { Text = "Simple Interest", Location = new Point(50, 150), Checked = true };
            comrad = new RadioButton { Text = "Compound Interest", Location = new Point(200, 150) };
            
            
            this.Controls.Add(simrad);
            this.Controls.Add(comrad);

            calculateButton = new Button
            {
                Text = "Calculate",
                Location = new Point(180, 190),
                Size = new Size(100, 30),
                BackColor = Color.AliceBlue
            };
            calculateButton.Click += CalculateButton_Click;
            this.Controls.Add(calculateButton);

            interlab = CreateLabel("Interest: ", 240);
            interlab.BackColor = Color.Transparent;
            balLab = CreateLabel("New Balance: ", 280);
            interlab.BackColor = Color.Transparent;

            applyButton = new Button
            {
                Text = "Apply to Balance",
                Location = new Point(150, 320),
                Size = new Size(180, 35),
                BackColor = Color.LightBlue
            };
            applyButton.Click += ApplyButton_Click;
            this.Controls.Add(applyButton);
        }

        private Label CreateLabel(string text, int top)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(30, top),
                AutoSize = true,
                Font = new Font("Calibri", 11)
            };
            this.Controls.Add(label);
            return label;
        }
         


        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(timeTextBox.Text, out double timeYears) || timeYears <= 0)
            {
                MessageBox.Show("Please enter a valid time (in years).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double principal = GetLiveBalance(currentUser.user);

            princilab.Text = $"Principal: {principal:C}";

            if (simrad.Checked)
            {
                latestInter = (principal * fixer * timeYears) / 100;
                newBal = principal + latestInter;
            }
            else
            {
                double compoundAmount = principal * Math.Pow(1 + fixer / 100, timeYears);
                latestInter = compoundAmount - principal;
                newBal = compoundAmount;
            }

            interlab.Text = $"Interest: {latestInter:C}";
            balLab.Text = $"New Balance: {newBal:C}";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientMenu form2 = new ClientMenu(currentUser);
            form2.Show();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (IsInterestRecentlyApplied(currentUser.user))
            {
                MessageBox.Show("Interest can only be applied once every 30 days.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (latestInter == 0)
            {
                MessageBox.Show("Please calculate the interest before applying.", "Action Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] lines = File.ReadAllLines("balance.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(currentUser.user + ":"))
                {
                    lines[i] = $"{currentUser.user}:{newBal}";
                    break;
                }
            }
            File.WriteAllLines("balance.txt", lines);

            File.AppendAllText(interlogs, $"{currentUser.user}:{DateTime.Now:yyyy-MM-dd}{Environment.NewLine}");

            currentUser.money = newBal;

            MessageBox.Show("Interest successfully applied to your balance!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            applyButton.Enabled = false;
        }

        private void LoadInterestDate()
        {
            if (!File.Exists(interlogs))
                return;

            string[] logLines = File.ReadAllLines(interlogs);
            foreach (var line in logLines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2 && parts[0] == currentUser.user)
                {
                    if (DateTime.TryParse(parts[1], out DateTime lastDate))
                    {
                        if ((DateTime.Now - lastDate).TotalDays < 30)
                        {
                            applyButton.Enabled = false;
                        }
                    }
                }
            }
        }

        private bool IsInterestRecentlyApplied(string username)
        {
            if (!File.Exists(interlogs))
                return false;

            string[] lines = File.ReadAllLines(interlogs);
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2 && parts[0] == username && DateTime.TryParse(parts[1], out DateTime date))
                {
                    return (DateTime.Now - date).TotalDays < 30;
                }
            }
            return false;
        }

        private double GetLiveBalance(string username)
        {
            if (!File.Exists("balance.txt"))
                return 0;

            foreach (var line in File.ReadAllLines("balance.txt"))
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
