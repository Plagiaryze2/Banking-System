using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OOP
{
    public partial class AdminForm : Form
    {
        private BankUser currad;
        private ContextMenuStrip cmenu;
        public AdminForm(BankUser adminUser)
        {
            currad = adminUser;
            InitializeComponent();

            cmenu = new ContextMenuStrip();

            ToolStripMenuItem freezer = new ToolStripMenuItem("Freeze", null, freezer_Click);
            ToolStripMenuItem deleter = new ToolStripMenuItem("Delete", null, deleter_Click);

            foreach (ToolStripMenuItem item in cmenu.Items)
            {
                item.Font = new Font("Calibri", 16);  
                item.ForeColor = Color.Black; 
                item.BackColor = Color.White; 
                item.Padding = new Padding(10); 
                item.AutoSize = true;
            }

            cmenu.BackColor = Color.White;
            cmenu.ForeColor = Color.Black; 
            cmenu.Font = new Font("Calibri", 16); 

            cmenu.Items.Add(freezer);
            cmenu.Items.Add(deleter);

            this.Text = $"Admin Panel - {currad.user}";
            this.Size = new Size(800, 600);

            Button logout = new Button
            {
                Text = "Logout",
                Location = new Point(350, 500),
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

            DataGridView grid = new DataGridView
            {
                Location = new Point(100, 50),
                Size = new Size(600, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false
            };

            grid.DataSource = LoadUserData();
            grid.AutoGenerateColumns = true;

            grid.ColumnHeadersVisible = true;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 18, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersHeight = 40;

            grid.DefaultCellStyle.Font = new Font("Calibri", 14);
            grid.DefaultCellStyle.SelectionBackColor = Color.GreenYellow;
            grid.DefaultCellStyle.SelectionForeColor = Color.IndianRed;
            grid.RowTemplate.Height = 30;

            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;

            grid.AllowUserToResizeColumns = false;
            grid.AllowUserToResizeRows = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.Controls.Add(grid);

            grid.MouseDown += rightclick;
        }
        private DataTable LoadUserData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Username");
            table.Columns.Add("Password");
            table.Columns.Add("Status");
            table.Columns.Add("Bank Balance");
            table.Columns.Add("ATM Card No");
            table.Columns.Add("ATM Balance");
            table.Columns.Add("Loan Amount");

            Dictionary<string, Tuple<string, string>> atminfo = new Dictionary<string, Tuple<string, string>>();
            if (File.Exists("atm.txt"))
            {
                foreach (var line in File.ReadAllLines("atm.txt"))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 3)
                    {
                        string user = parts[0];
                        string cardNo = parts[1];
                        string balance = parts[2];
                        atminfo[user] = Tuple.Create(cardNo, balance);
                    }
                }
            }

            Dictionary<string, string> loanData = new Dictionary<string, string>();
            if (File.Exists("loan.txt"))
            {
                foreach (var line in File.ReadAllLines("loan.txt"))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string user = parts[0];
                        string amount = parts[1];
                        loanData[user] = amount;
                    }
                }
            }

            Dictionary<string, string> statinf = new Dictionary<string, string>();
            if (File.Exists("status.txt"))
            {
                foreach (var line in File.ReadAllLines("status.txt"))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string user = parts[0];
                        string status = parts[1];
                        statinf[user] = status;
                    }
                }
            }

            if (File.Exists("users.txt"))
            {
                foreach (var line in File.ReadAllLines("users.txt"))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 3)
                    {
                        string username = parts[0];
                        string password = parts[1];
                        string role = parts[2];

                        double? bal = getbal(username);

                        string atmbal = "-";
                        if (atminfo.ContainsKey(username) && double.TryParse(atminfo[username].Item2, out double atmbalance))
                        {
                            atmbal = atmbalance.ToString("C");
                        }

                        string loanbal = "-";
                        if (loanData.ContainsKey(username) && double.TryParse(loanData[username], out double loanval))
                        {
                            loanbal = loanval.ToString("C");
                        }

                        string status = statinf.ContainsKey(username) ? statinf[username] : "-";

                        if (role == "Client")
                        {
                            string cardNo = "-";
                            if (atminfo.ContainsKey(username))
                            {
                                cardNo = atminfo[username].Item1;
                            }

                            table.Rows.Add(username, password, status, bal?.ToString("C"), cardNo, atmbal, loanbal);
                        }
                    }
                }
            }

            return table;
        }
        private double? getbal(string username)
        {
            if (File.Exists("balance.txt"))
            {
                foreach (var line in File.ReadAllLines("balance.txt"))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && parts[0] == username)
                    {
                        return double.TryParse(parts[1], out double balance) ? balance : (double?)null;
                    }
                }
            }

            return null;
        }
        private void rightclick(object sender, MouseEventArgs e)
        {
            var grid = sender as DataGridView;
            var tester = grid.HitTest(e.X, e.Y);

            if (tester.Type == DataGridViewHitTestType.Cell && e.Button == MouseButtons.Right)
            {
                if (tester.ColumnIndex == grid.Columns["Username"].Index)  
                {
                    grid.ClearSelection();

                    grid.Rows[tester.RowIndex].Cells[tester.ColumnIndex].Selected = true;

                    string username = grid.Rows[tester.RowIndex].Cells[tester.ColumnIndex].Value.ToString();
                    string status = getstatus(username);

                    cmenu.Items.Clear();

                    if (status == "Frozen")
                    {
                        ToolStripMenuItem activtor = new ToolStripMenuItem("Activate", null, activtor_Click);
                        cmenu.Items.Add(activtor);
                    }
                    else
                    {
                        ToolStripMenuItem freezer = new ToolStripMenuItem("Freeze", null, freezer_Click);
                        cmenu.Items.Add(freezer);
                    }

                    ToolStripMenuItem deleter = new ToolStripMenuItem("Delete", null, deleter_Click);
                    cmenu.Items.Add(deleter);

                    cmenu.Show(grid, e.Location);
                }
            }
        }
        private string getstatus(string username)
        {
            if (File.Exists("status.txt"))
            {
                foreach (var line in File.ReadAllLines("status.txt"))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && parts[0] == username)
                    {
                        return parts[1];  
                    }
                }
            }
            return "Active"; 
        }
        private void freezer_Click(object sender, EventArgs e)
        {
            var grid = (DataGridView)this.Controls[1];
            if (grid.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please right-click a username to freeze.");
                return;  
            }
            var cellselect = grid.SelectedCells[0];
            string username = cellselect.Value.ToString();
            statupdate(username, "Frozen");

            grid.DataSource = LoadUserData();
        }
        private void activtor_Click(object sender, EventArgs e)
        {
            var grid = (DataGridView)this.Controls[1];  
            if (grid.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please right-click a username to activate.");
                return;  
            }

            var cellselect = grid.SelectedCells[0]; 
            string username = cellselect.Value.ToString();

            statupdate(username, "Active");

            grid.DataSource = LoadUserData();
        }
        private void deleter_Click(object sender, EventArgs e)
        {
            var grid = (DataGridView)this.Controls[1];  
            if (grid.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please right-click a username to delete.");
                return;  
            }

            var cellselect = grid.SelectedCells[0]; 
            string username = cellselect.Value.ToString();

            deluser(username);

            grid.DataSource = LoadUserData();
        }
        private void statupdate(string username, string newStatus)
        {
            foreach (DataRow row in ((DataTable)((DataGridView)this.Controls[1]).DataSource).Rows)
            {
                if (row["Username"].ToString() == username)
                {
                    row["Status"] = newStatus;
                }
            }

            List<string> lines = new List<string>(File.ReadAllLines("status.txt"));
            bool userFound = false;
            for (int i = 0; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split(':');
                if (parts.Length == 2 && parts[0] == username)
                {
                    lines[i] = $"{username}:{newStatus}";
                    userFound = true;
                    break;
                }
            }

            if (!userFound)
            {
                lines.Add($"{username}:{newStatus}");
            }

            File.WriteAllLines("status.txt", lines);
        }
        private void deluser(string username)
        {
            List<string> userstr = new List<string>(File.ReadAllLines("users.txt"));
            userstr.RemoveAll(line => line.Split(':')[0] == username);
            File.WriteAllLines("users.txt", userstr);

            List<string> balancestr = new List<string>(File.ReadAllLines("balance.txt"));
            balancestr.RemoveAll(line => line.Split(':')[0] == username);
            File.WriteAllLines("balance.txt", balancestr);

            List<string> atmstr = new List<string>(File.ReadAllLines("atm.txt"));
            atmstr.RemoveAll(line => line.Split(':')[0] == username);
            File.WriteAllLines("atm.txt", atmstr);

            List<string> loanstr = new List<string>(File.ReadAllLines("loan.txt"));
            loanstr.RemoveAll(line => line.Split(':')[0] == username);
            File.WriteAllLines("loan.txt", loanstr);

            List<string> statustr = new List<string>(File.ReadAllLines("status.txt"));
            statustr.RemoveAll(line => line.Split(':')[0] == username);
            File.WriteAllLines("status.txt", statustr);
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            this.SuspendLayout();
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Name = "AdminForm";
            this.ResumeLayout(false);
        }
    }
}
