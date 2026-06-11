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
    using System;
    using System.IO;
    using System.Windows.Forms;

    public partial class DeposMenu : Form
    {
        private BankUser currentUser;

        public DeposMenu(BankUser user)
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.Image10;
          this.BackgroundImageLayout = ImageLayout.Stretch;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            currentUser = user;
            this.Load += FormLoader;
        }

        private void FormLoader(object sender, EventArgs e)
        {
            LoadWithdrawl();
        }

        private void LoadWithdrawl()
        {
            ListView listView = new ListView();
            listView.Dock = DockStyle.Fill;
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.Font = new Font("Calibri", 18, FontStyle.Bold);

            listView.Columns.Add("Date & Time", 250);
            listView.Columns.Add("Action", 220);
            listView.Columns.Add("Amount", 200);
            listView.Columns.Add("Details", 450);

            string fileName = "deposit.txt";
            if (!File.Exists(fileName))
            {
                MessageBox.Show("Transaction file not found.");
                return;
            }

            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                if (line.Contains($"{currentUser.user} -"))
                {
                    string[] parts = line.Split(new[] { ": " }, 2, StringSplitOptions.None);
                    if (parts.Length < 2) continue;

                    string dateTime = parts[0];
                    string transactionDetails = parts[1];

                    string[] userAndRest = transactionDetails.Split(new[] { " - " }, 2, StringSplitOptions.None);
                    if (userAndRest.Length < 2) continue;

                    string user = userAndRest[0];
                    string actionAndAmount = userAndRest[1];

                    int amountIndex = actionAndAmount.IndexOfAny("0123456789".ToCharArray());
                    if (amountIndex == -1) continue;

                    string action = actionAndAmount.Substring(0, amountIndex).Trim();
                    string remaining = actionAndAmount.Substring(amountIndex).Trim();

                    string amount = remaining;
                    string details = "";

                    int detailsIndex = remaining.IndexOf('(');
                    if (detailsIndex != -1)
                    {
                        amount = remaining.Substring(0, detailsIndex).Trim();
                        details = remaining.Substring(detailsIndex).Trim();
                    }

                    ListViewItem item = new ListViewItem(dateTime);
                    item.SubItems.Add(action);
                    item.SubItems.Add(amount);
                    item.SubItems.Add(details);
                    listView.Items.Add(item);
                }
            }

            this.Controls.Add(listView);
        }
    }
}
