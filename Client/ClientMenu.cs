using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOP
{
    public partial class ClientMenu : Form
    {
        private BankUser currentUser;

        public ClientMenu(BankUser clientuser)
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.Image1; 
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.MaximizeBox = false;
            this.MinimizeBox = false;


            currentUser = clientuser; 
            this.Text = $"Client Panel - {currentUser.user}";

            int width = 240;
            int height = 50;
            int btnx = (800 - width) / 2;
            int spacer = 15;
            int starty = 25;

            Button btn1 = new Button
            {
                Text = "Funds Management",
                Font = new Font("Calibri", 18),
                Size = new Size(width, height),
                Location = new Point(btnx, starty),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            btn1.Click += btn1_Click;


            Button btn2 = new Button
            {
                Text = "Transaction History",
                Font = new Font("Calibri", 18),
                Size = new Size(width, height),
                Location = new Point(btnx, starty + (height + spacer) * 1),
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            btn2.Click += (sender, e) =>
            {
                TransactionInfo nextForm = new TransactionInfo(currentUser);
                nextForm.Show();
                this.Hide();
            };

            Button btn3 = new Button
            {
                Text = "Loan Info",
                Font = new Font("Calibri", 18),
                Size = new Size(width, height),
                Location = new Point(btnx, starty + (height + spacer) * 2),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            btn3.Click += (sender, e) =>
            {
                LoanInfo LoanInfo = new LoanInfo(currentUser);
                LoanInfo.Show();
                this.Hide();
            };
            Button btn4 = new Button
            {
                Text = "Interest Calculation",
                Font = new Font("Calibri", 18),
                Size = new Size(width, height),
                Location = new Point(btnx, starty + (height + spacer) * 3),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            btn4.Click += (sender, e) =>
            {
                InterestInfo inter = new InterestInfo(currentUser);
                inter.Show();
                this.Hide();
            };

            Button btn5 = new Button
            {
                Text = "ATM Card Info",
                Font = new Font("Calibri", 18),
                Size = new Size(width, height),
                Location = new Point(btnx, starty + (height + spacer) * 4),
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            btn5.Click += (sender, e) =>
            {
                AtmInfo atmer = new AtmInfo(currentUser);
                atmer.Show();
                this.Hide();
            };

            Button btn6 = new Button
            {
                Text = "Deposit/Withdraw",
                Font = new Font("Calibri", 18),
                Size = new Size(width, height),
                Location = new Point(btnx, starty + (height + spacer) * 5),
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            btn6.Click += (sender, e) =>
            {
                DepositWithdraw depositWithdrawForm = new DepositWithdraw(currentUser);
                depositWithdrawForm.Show();
                this.Hide();
            };
            this.Controls.Add(btn6);

            Button btn7 = new Button
            {
                Text = "Currency Exchange",
                Font = new Font("Calibri", 18),
                Size = new Size(width, height),
                Location = new Point(btnx, starty + (height + spacer) * 6),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            btn7.Click += (sender, e) =>
            {
                CurrencyExchangeForm CurrForm = new CurrencyExchangeForm(currentUser);
                CurrForm.Show();
                this.Hide();
            };

            this.Controls.Add(btn1);
            this.Controls.Add(btn2);
            this.Controls.Add(btn3);
            this.Controls.Add(btn4);
            this.Controls.Add(btn5);
            this.Controls.Add(btn6);
            this.Controls.Add(btn7);

            Button logout = new Button
            {
                Text = "Logout",
                Location = new Point(353, 500),
                AutoSize = true,
                ForeColor = Color.Black,
                BackColor = Color.White,
                Font = new Font("Calibri", 16)
            };
            logout.Click += (sender, e) =>
            {
                this.Close();
                Form1 loginForm = new Form1();
                loginForm.Show();
            };
            this.Controls.Add(logout);
        }
        private void btn1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FundTransfer transferForm = new FundTransfer(currentUser);
            transferForm.Show();
        }
        private void ClientMenu_Load(object sender, EventArgs e)
        {
        }
        
    };
}
