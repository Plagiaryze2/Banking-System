using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OOP
{
    public partial class Signuppage : Form
    {
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$";

        private Label nameLabel, passwordLabel, confirmPasswordLabel, titleLabel;
        private TextBox nameTextBox, passwordTextBox, confirmPasswordTextBox;
        private Button signupButton, showPasswordButton, showConfirmPasswordButton;

        public Signuppage()
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.Image7;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            SetupForm();
            CreateControls();
            this.FormClosing += Form1_FormClosing;
        }

        private void SetupForm()
        {
            this.Text = "User Registration";
            this.BackColor = Color.MediumPurple;
            this.ClientSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void CreateControls()
        {
            titleLabel = new Label
            {
                Text = "SIGNUP PAGE",
                Font = new Font("Calibri", 24, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Location = new Point(200, 80),
                
            };
            this.Controls.Add(titleLabel);

            nameLabel = new Label
            {
                Text = "Username:",
                Font = new Font("Calibri", 12),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Location = new Point(150, 150)
            };
            this.Controls.Add(nameLabel);

            nameTextBox = new TextBox
            {
                Location = new Point(250, 150),
                Size = new Size(200, 75),
                Font = new Font("Calibri", 11)
            };
            nameTextBox.KeyPress += NameTextBox_KeyPress;
            this.Controls.Add(nameTextBox);

            passwordLabel = new Label
            {
                Text = "Password:",
                Font = new Font("Calibri", 12),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Location = new Point(150, 200)
            };
            this.Controls.Add(passwordLabel);

            passwordTextBox = new TextBox
            {
                Location = new Point(250, 200),
                Size = new Size(200, 25),
                Font = new Font("Calibri", 11),
                PasswordChar = '*'
            };
            this.Controls.Add(passwordTextBox);

            showPasswordButton = new Button
            {
                Text = "Show",
                Location = new Point(460, 200),
                Size = new Size(60, 25),
                Font = new Font("Calibri", 8),
                BackColor = Color.LightBlue
            };
            showPasswordButton.Click += ShowPasswordButton_Click;
            this.Controls.Add(showPasswordButton);

            confirmPasswordLabel = new Label
            {
                Text = "Confirm Password:",
                Font = new Font("Calibri", 12),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(100, 250)
            };
            this.Controls.Add(confirmPasswordLabel);

            confirmPasswordTextBox = new TextBox
            {
                Location = new Point(250, 250),
                Size = new Size(200, 25),
                Font = new Font("Calibri", 11),
                PasswordChar = '*'
            };
            this.Controls.Add(confirmPasswordTextBox);

            showConfirmPasswordButton = new Button
            {
                Text = "Show",
                Location = new Point(460, 250),
                Size = new Size(60, 25),
                Font = new Font("Calibri", 8),
                BackColor = Color.LightBlue
            };
            showConfirmPasswordButton.Click += ShowConfirmPasswordButton_Click;
            this.Controls.Add(showConfirmPasswordButton);

            signupButton = new Button
            {
                Text = "Sign Up",
                Location = new Point(250, 300),
                Size = new Size(100, 35),
                Font = new Font("Calibri", 12),
               BackColor = Color.MediumOrchid,
                ForeColor = Color.White
            };
            
            signupButton.Click += SignupButton_Click;
            this.Controls.Add(signupButton);
        }

        private void SignupButton_Click(object sender, EventArgs e)
        {
            string username = nameTextBox.Text.Trim();
            string password = passwordTextBox.Text;
            string confirmPassword = confirmPasswordTextBox.Text;
            string usersFilePath = "users.txt";

            if (string.IsNullOrEmpty(username))
            {
                ShowError("Please enter a username.", nameTextBox);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("Please enter a password.", passwordTextBox);
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Passwords do not match.", confirmPasswordTextBox);
                return;
            }

            if (!Regex.IsMatch(password, PasswordPattern))
            {
                MessageBox.Show("Password must contain:\n- At least 8 characters\n- One uppercase letter\n" +
                    "- One lowercase letter\n- One number\n- One special character",
                    "Password Requirements", MessageBoxButtons.OK, MessageBoxIcon.Information);
                passwordTextBox.Focus();
                return;
            }

            if (UserExists(username, usersFilePath))
            {
                ShowError("Username already exists. Please choose a different name.", nameTextBox);
                return;
            }

            try
            {
                RegisterUser(username, password, usersFilePath);
                MessageBox.Show("Registration successful! You can now login.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Form1 mainForm = new Form1(); 
                mainForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UserExists(string username, string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split(':');
                if (parts.Length > 0 && parts[0].Equals(username, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private void RegisterUser(string username, string password, string filePath)
        {
            string userRecord = $"{username}:{password}:Client{Environment.NewLine}";
            string statusRecord = $"{username}:Active{Environment.NewLine}";
            string balanceRecord = $"{username}:0{Environment.NewLine}";

            File.AppendAllText("users.txt", userRecord);     
            File.AppendAllText("status.txt", statusRecord); 
            File.AppendAllText("balance.txt", balanceRecord); 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 form1 = new Form1();

            form1.Show();

        }
        private void ShowError(string message, Control controlToFocus)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            controlToFocus.Focus();
            if (controlToFocus is TextBox textBox)
                textBox.SelectAll();
        }

        private void ShowPasswordButton_Click(object sender, EventArgs e)
        {
            TogglePasswordVisibility(passwordTextBox, showPasswordButton);
        }

        private void ShowConfirmPasswordButton_Click(object sender, EventArgs e)
        {
            TogglePasswordVisibility(confirmPasswordTextBox, showConfirmPasswordButton);
        }

        private void TogglePasswordVisibility(TextBox textBox, Button button)
        {
            textBox.PasswordChar = textBox.PasswordChar == '*' ? '\0' : '*';
            button.Text = textBox.PasswordChar == '*' ? "Show" : "Hide";
        }

        private void NameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }
    }
}