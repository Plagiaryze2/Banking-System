using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOP
{
    public partial class DepositWithdraw : Form
    {
        private BankUser currentUser;

        public DepositWithdraw(BankUser user)
        {
            InitializeComponent();
            currentUser = user;
            lblWelcome.Text = $"Welcome, {currentUser.user}!";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.AutoSize = false;
            lblWelcome.Dock = DockStyle.Top; 
            lblWelcome.Height = 40;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            Deposit depositForm = new Deposit(currentUser);
            depositForm.ShowDialog();
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            Withdraw withdrawForm = new Withdraw(currentUser);
            withdrawForm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClientMenu clientMenu = new ClientMenu(currentUser);
            clientMenu.Show();
            this.Close();
        }

        private void DepositWithdraw_Load(object sender, EventArgs e)
        {

        }
    }
}