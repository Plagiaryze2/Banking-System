using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OOP;
namespace OOP
{
    public partial class Form1 : Form
    {
        Label label1, welcome;
        TextBox usr;
        TextBox pss;
        RadioButton client;
        RadioButton Admin;
        Button loginb;
        Button shopassb;
        System.Windows.Forms.Timer wt;
        BankUser authenticatedUser;

     

        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.Text = "FAST NU Banking System";

            label1 = new Label
            {
                Text = "FAST NU Bank",
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Calibri", 36),
                Location = new Point(228, 25),
                AutoSize = true
            };
            this.Controls.Add(label1);

            client = new RadioButton
            {
                Text = "Client",
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Calibri", 16),
                Location = new Point(248, 400),
                AutoSize = true
            };
            this.Controls.Add(client);

            Admin = new RadioButton
            {
                Text = "Admin",
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Calibri", 16),
                Location = new Point(418, 400),
                AutoSize = true
            };
            this.Controls.Add(Admin);

            Label pass = new Label
            {
                Text = "Password: ",
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Calibri", 16),
                Location = new Point(135, 350),
                AutoSize = true
            };
            this.Controls.Add(pass);

            Label usern = new Label
            {
                Text = "Username: ",
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Calibri", 16),
                Location = new Point(135, 300),
                AutoSize = true
            };
            this.Controls.Add(usern);

            usr = new TextBox
            {
                Font = new Font("Calibri", 16),
                Location = new Point(250, 298),
                Width = 250
            };
            this.Controls.Add(usr);

            pss = new TextBox
            {
                Font = new Font("Calibri", 16),
                Location = new Point(250, 348),
                Width = 250,
                PasswordChar = '*'
            };
            this.Controls.Add(pss);

            shopassb = new Button
            {
                Text = "Show",
                Font = new Font("Calibri", 12),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Location = new Point(510, 350),
                AutoSize = true
            };
            shopassb.Click += new EventHandler(showpassb);
            this.Controls.Add(shopassb);

            loginb = new Button
            {
                Text = "Login",
                ForeColor = Color.Black,
                BackColor = Color.White,
                Font = new Font("Calibri", 16),
                Location = new Point(340, 480),
                AutoSize = true
            };
            loginb.Click += new EventHandler(loginb_Click);
            this.Controls.Add(loginb);
             

              
            Point signupLinkPosition = new Point(280, 450);
            LinkLabel signupLink = new LinkLabel
            {
                Text = "Don't have an account? Sign Up",
                LinkColor = Color.White,
                ActiveLinkColor = Color.LightBlue,
                Font = new Font("Calibri", 12),
                BackColor = Color.Transparent,
                Location = signupLinkPosition,
                AutoSize = true
            };
            signupLink.Click += SignupLink_Click;  
            this.Controls.Add(signupLink);



            wt = new System.Windows.Forms.Timer
            {
                Interval = 3000
            };
            wt.Tick += new EventHandler(WelcomeTimer_Tick);
        }
        private void showpassb(object sender, EventArgs e)
        {
            if (pss.PasswordChar == '*')
            {
                pss.PasswordChar = '\0';
                shopassb.Text = "Hide";
            }
            else
            {
                pss.PasswordChar = '*';
                shopassb.Text = "Show";
            }
        }
        private void loginb_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(usr.Text) && !string.IsNullOrWhiteSpace(pss.Text))
            {
                if (client.Checked || Admin.Checked)
                {
                    authenticatedUser = AuthenticateUser(usr.Text, pss.Text);
                    if (authenticatedUser != null)
                    {
                        foreach (Control control in this.Controls)
                            control.Visible = false;

                        welcome = new Label
                        {
                            BackColor = Color.Transparent,
                            ForeColor = Color.White,
                            Font = new Font("Calibri", 24),
                            Location = new Point(268, 240),
                            AutoSize = true,
                            Text = $"Welcome {authenticatedUser.role}!"
                        };
                        this.Controls.Add(welcome);
                        wt.Start();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username, password, or role.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select either Client or Admin.", "Role Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please fill both username and password fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void SignupLink_Click(object sender, EventArgs e)
        {
            Signuppage signup = new Signuppage();
            signup.Show();
            this.Hide(); 
        }
        private string GetUserStatus(string username)
        {
            if (!File.Exists("status.txt"))
                return "Active";

            foreach (var line in File.ReadAllLines("status.txt"))
            {
                var parts = line.Split(':');
                if (parts.Length == 2 && parts[0] == username)
                {
                    return parts[1];
                }
            }

            return "Active"; 
        }
        private void WelcomeTimer_Tick(object sender, EventArgs e)
        {
            if (welcome != null)
            {
                welcome.Visible = false;
            }

            wt.Stop();

            if (Admin.Checked)
            {
                AdminForm adminForm = new AdminForm(authenticatedUser);
                adminForm.Show();
                this.Hide();
            }
            else if (client.Checked)
            {
                ClientMenu clientMenu = new ClientMenu(authenticatedUser);
                clientMenu.Show();
                this.Hide();
            }
        }
        private BankUser AuthenticateUser(string username, string password)
        {
            string[] lines = File.ReadAllLines("users.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 3)
                {
                    string strname = parts[0];
                    string strpass = parts[1];
                    string strrole = parts[2];

                    string selectedRole = client.Checked ? "Client" : "Admin";

                    if (strname == username && strpass == password && strrole == selectedRole)
                    {
                        string status = GetUserStatus(username);
                        if (status == "Frozen")
                        {
                            MessageBox.Show("Your account is frozen and cannot be logged into.", "Account Frozen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return null;  
                        }

                        double? money = Getmoney(username);

                        BankUser user = new BankUser(strname, strpass, strrole)
                        {
                            money = money,
                            status = status
                        };

                        if (selectedRole == "Client")
                        {
                            if (File.Exists("atm.txt"))
                            {
                                foreach (var atmLine in File.ReadAllLines("atm.txt"))
                                {
                                    var data = atmLine.Split(':');
                                    if (data.Length == 3 && data[0] == username)
                                    {
                                        user.cardno = data[1];
                                        user.atmbal = double.TryParse(data[2], out double ab) ? ab : (double?)null;
                                        break;
                                    }
                                }
                            }

                            if (File.Exists("loan.txt"))
                            {
                                foreach (var loanLine in File.ReadAllLines("loan.txt"))
                                {
                                    var data = loanLine.Split(':');
                                    if (data.Length == 2 && data[0] == username)
                                    {
                                        user.loanmoney = double.TryParse(data[1], out double la) ? la : (double?)null;
                                        break;
                                    }
                                }
                            }
                        }

                        return user;
                    }
                }
            }

            return null;
        }

        private double? Getmoney(string username)
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
    }
}
