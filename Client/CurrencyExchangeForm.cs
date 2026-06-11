using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OOP
{
    public partial class CurrencyExchangeForm : Form
    {
        private BankUser currentUser;

        private string balanceFilePath = "balance.txt";
        private string transactionLogPath = "transactions.txt";

        private readonly decimal pkrToUsdRate = 0.0036m;
        private readonly decimal pkrToEurRate = 0.0031m;
        private readonly decimal pkrToGbpRate = 0.0026m;

        private readonly decimal usdToPkrRate = 278.50m;
        private readonly decimal eurToPkrRate = 322.58m;
        private readonly decimal gbpToPkrRate = 384.62m;

        public CurrencyExchangeForm(BankUser obj1)
        {
            InitializeComponent();
            currentUser = obj1;
            LoadAccountBalance();
            SetupComboBoxes();
            this.BackgroundImage = Properties.Resources.Image4;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void SetupComboBoxes()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("PKR");
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = false;

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new string[] { "USD", "EUR", "GBP" });
            comboBox2.SelectedIndex = 0;
        }


        private void LoadAccountBalance()
        {
            label7.Text = $"Account: {currentUser.user}";
            label7.Font = new Font("Arial", 18);
            label8.Font = new Font("Arial", 18);
            label7.BackColor = Color.Transparent;


            if (File.Exists(balanceFilePath))
            {
                var lines = File.ReadAllLines(balanceFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && parts[0] == currentUser.user)
                    {
                        if (decimal.TryParse(parts[1], out decimal balance))
                        {
                            label8.Text = $"Current Balance: {balance} PKR";
                            label8.BackColor = Color.Transparent;
                            return;
                        }
                    }
                }
            }

            label8.Text = "Current Balance: N/A";
        }

        private decimal GetCurrentBalance()
        {
            if (File.Exists(balanceFilePath))
            {
                var lines = File.ReadAllLines(balanceFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && parts[0] == currentUser.user)
                    {
                        if (decimal.TryParse(parts[1], out decimal balance))
                        {
                            return balance;
                        }
                    }
                }
            }
            return 0;
        }

        private void UpdateBalance(decimal newBalance)
        {
            var lines = File.Exists(balanceFilePath) ? File.ReadAllLines(balanceFilePath) : new string[0];
            bool userFound = false;

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(':');
                if (parts.Length == 2 && parts[0] == currentUser.user)
                {
                    lines[i] = $"{currentUser.user}:{newBalance}";
                    userFound = true;
                    break;
                }
            }

            if (!userFound)
            {
                Array.Resize(ref lines, lines.Length + 1);
                lines[lines.Length - 1] = $"{currentUser.user}:{newBalance}";
            }

            File.WriteAllLines(balanceFilePath, lines);
        }

        private void LogTransaction(string transactionType, decimal amount, string details)
        {
            string logEntry = $"{DateTime.Now}: {currentUser.user} - {transactionType} {amount} PKR ({details})";
            File.AppendAllText(transactionLogPath, logEntry + Environment.NewLine);
        }

        private decimal GetExchangeRate(string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency) return 1m;

            if (fromCurrency == "PKR" && toCurrency == "USD") return pkrToUsdRate;
            if (fromCurrency == "PKR" && toCurrency == "EUR") return pkrToEurRate;
            if (fromCurrency == "PKR" && toCurrency == "GBP") return pkrToGbpRate;

            if (fromCurrency == "USD" && toCurrency == "PKR") return usdToPkrRate;
            if (fromCurrency == "EUR" && toCurrency == "PKR") return eurToPkrRate;
            if (fromCurrency == "GBP" && toCurrency == "PKR") return gbpToPkrRate;

            if (fromCurrency == "USD" && toCurrency == "EUR") return pkrToEurRate / pkrToUsdRate;
            if (fromCurrency == "USD" && toCurrency == "GBP") return pkrToGbpRate / pkrToUsdRate;
            if (fromCurrency == "EUR" && toCurrency == "USD") return pkrToUsdRate / pkrToEurRate;
            if (fromCurrency == "EUR" && toCurrency == "GBP") return pkrToGbpRate / pkrToEurRate;
            if (fromCurrency == "GBP" && toCurrency == "USD") return pkrToUsdRate / pkrToGbpRate;
            if (fromCurrency == "GBP" && toCurrency == "EUR") return pkrToEurRate / pkrToGbpRate;

            return 1m;
        }

        private decimal RoundCurrency(decimal amount, string currency)
        {
            if (currency != "PKR")
            {
                return Math.Round(amount, 2);
            }
            return Math.Round(amount, 0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void button2_click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(textBox1.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid positive amount to exchange.",
                                "Invalid Amount",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            string fromCurrency = comboBox1.SelectedItem.ToString();
            string toCurrency = comboBox2.SelectedItem.ToString();

            if (fromCurrency != "PKR")
            {
                MessageBox.Show("You can only convert from PKR to another currency.",
                                "Unsupported Conversion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (toCurrency == "PKR")
            {
                MessageBox.Show("Please select a different currency to convert to.",
                                "Invalid Conversion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            decimal currentBalance = GetCurrentBalance();

            if (amount > currentBalance)
            {
                MessageBox.Show("Insufficient PKR funds for this exchange.",
                                "Insufficient Funds",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            decimal exchangeRate = GetExchangeRate(fromCurrency, toCurrency);
            decimal convertedAmount = amount * exchangeRate;
            convertedAmount = RoundCurrency(convertedAmount, toCurrency);

            decimal newBalance = currentBalance - amount;

            UpdateBalance(newBalance);
            LogTransaction("Currency Exchange", amount, $"{amount} PKR to {convertedAmount} {toCurrency}");

            MessageBox.Show($"Successfully exchanged {amount} PKR to {convertedAmount} {toCurrency}\n" +
                            $"Exchange Rate: 1 PKR = {exchangeRate} {toCurrency}\n" +
                            $"New Balance: {newBalance} PKR",
                            "Exchange Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            LoadAccountBalance();
            textBox1.Clear();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ClientMenu prevForm = new ClientMenu(currentUser);
            prevForm.Show();
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
