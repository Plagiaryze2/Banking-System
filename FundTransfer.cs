using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OOP
{
    
    public partial class FundTransfer : Form
    {
        private Label ReceiverLabel, AmountLabel, CurrentBalanceLabel;
        private TextBox ReceiverBox, AmountBox;
        private Button TransferButton, BackButton;

        private string filePath;
        private Dictionary<string, decimal> balances;
        private BankUser currentUser;

        public FundTransfer(BankUser loggedInUser)
        {
            InitializeComponent();
            currentUser = loggedInUser;
            filePath = Path.Combine(Application.StartupPath, "balance.txt");

            InitializeUI();
            LoadBalances();
            LoadUserRoles();
            UpdateBalanceDisplay();
        }

        private void InitializeUI()
        {
            this.Text = $"Fund Transfer - {currentUser}";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.ClientSize = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            CurrentBalanceLabel = new Label
            {
                Text = "Current Balance: Loading...",
                Location = new Point(50, 30),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            this.Controls.Add(CurrentBalanceLabel);

            ReceiverLabel = new Label
            {
                Text = "Receiver Name:",
                Location = new Point(50, 100),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black
            };
            this.Controls.Add(ReceiverLabel);

            ReceiverBox = new TextBox
            {
                Location = new Point(200, 100),
                Size = new Size(250, 30),
                Font = new Font("Arial", 12)
            };
            ReceiverBox.KeyPress += ReceiverBox_KeyPress;
            this.Controls.Add(ReceiverBox);

            AmountLabel = new Label
            {
                Text = "Amount:",
                Location = new Point(50, 150),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black
            };
            this.Controls.Add(AmountLabel);

            AmountBox = new TextBox
            {
                Location = new Point(200, 150),
                Size = new Size(250, 30),
                Font = new Font("Arial", 12)
            };
            AmountBox.KeyPress += AmountBox_KeyPress;
            this.Controls.Add(AmountBox);

            TransferButton = new Button
            {
                Text = "Transfer",
                Location = new Point(100, 250),
                Size = new Size(120, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Green
            };
            TransferButton.Click += Transfer_Click;
            this.Controls.Add(TransferButton);

            BackButton = new Button
            {
                Text = "Back",
                Location = new Point(280, 250),
                Size = new Size(120, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Red
            };
            BackButton.Click += Back_Click;
            this.Controls.Add(BackButton);
        }

        private void LoadBalances()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    balances = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
                    {
                        { currentUser.user, 0 }
                    };
                    SaveBalances();
                    return;
                }

                var lines = File.ReadAllLines(filePath);
                balances = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && decimal.TryParse(parts[1], out decimal balance))
                    {
                        balances[parts[0].Trim()] = balance;
                    }
                }

                if (!balances.ContainsKey(currentUser.user))
                {
                    balances[currentUser.user] = 0;
                    SaveBalances();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading balances: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveBalances()
        {
            try
            {
                var lines = balances.Select(kvp => $"{kvp.Key}:{kvp.Value}").ToArray();
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving balances: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBalanceDisplay()
        {
            if (balances != null && balances.ContainsKey(currentUser.user))
            {
                CurrentBalanceLabel.Text = $"Current Balance: {balances[currentUser.user]:C}";
            }
        }

        private void ReceiverBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void AmountBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private Dictionary<string, string> userRoles;

        private void LoadUserRoles()
        {
            try
            {
                string usersFilePath = Path.Combine(Application.StartupPath, "users.txt");

                if (!File.Exists(usersFilePath))
                {
                    userRoles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    return;
                }

                var lines = File.ReadAllLines(usersFilePath);
                userRoles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 3)  
                    {
                        userRoles[parts[0].Trim()] = parts[2].Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsReceiverClient(string receiver)
        {
            return userRoles.ContainsKey(receiver) && userRoles[receiver] == "Client";
        }
        private void Transfer_Click(object sender, EventArgs e)
        {
            string receiver = ReceiverBox.Text.Trim();
            string amountText = AmountBox.Text.Trim();

            if (!IsReceiverClient(receiver))
            {
                MessageBox.Show($"Receiver '{receiver}' is either not found or not a Client", "Transfer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReceiverBox.SelectAll();
                ReceiverBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(receiver))
            {
                MessageBox.Show("Please enter receiver name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReceiverBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(amountText))
            {
                MessageBox.Show("Please enter amount", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AmountBox.Focus();
                return;
            }

            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid positive amount", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AmountBox.SelectAll();
                AmountBox.Focus();
                return;
            }

            if (!balances.ContainsKey(receiver))
            {
                MessageBox.Show($"Receiver '{receiver}' not found in system", "Transfer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReceiverBox.SelectAll();
                ReceiverBox.Focus();
                return;
            }
            if (balances[currentUser.user] < amount)
            {
                MessageBox.Show($"Insufficient balance. Your current balance is {balances[currentUser.user]:C}",
                    "Transfer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AmountBox.SelectAll();
                AmountBox.Focus();
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Transfer {amount:C} to {receiver}?\n\nYour new balance will be {balances[currentUser.user] - amount:C}",
                "Confirm Transfer",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No)
            {
                return;
            }

            try
            {
                balances[currentUser.user] -= amount;
                balances[receiver] += amount;
                SaveBalances();

                MessageBox.Show($"Transfer successful!\n\n" +
                                $"Amount: {amount:C}\n" +
                                $"To: {receiver}\n" +
                                $"New Balance: {balances[currentUser.user]:C}",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                UpdateBalanceDisplay();
                ReceiverBox.Clear();
                AmountBox.Clear();
                ReceiverBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transfer failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            this.Hide();
            ClientMenu cm = new ClientMenu(currentUser);
            cm.Show();
        }
    }
}