<div align="center">

# 🏦 FAST NU Banking System

### A Feature-Rich Desktop Banking & ATM Management Application

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.NET Framework](https://img.shields.io/badge/.NET_Framework_4.8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D4?style=for-the-badge&logo=windows&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)

*An OOP course project built at FAST National University, Lahore — Group 3, SE-2B*

---

</div>

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Screenshots](#screenshots)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Project Architecture](#project-architecture)
- [Data Storage](#data-storage)
- [Usage Guide](#usage-guide)
- [Contributing](#contributing)
- [License](#license)

---

## 🔍 Overview

**FAST NU Banking System** is a comprehensive desktop banking application built using C# Windows Forms. It simulates a real-world banking environment with role-based access for **Clients** and **Administrators**, covering everything from basic deposits and withdrawals to fund transfers, currency exchange, loan management, ATM card operations, and interest calculations.

This project demonstrates core **Object-Oriented Programming** concepts including encapsulation, inheritance, and modular form-based architecture.

---

## ✨ Features

### 🔐 Authentication & Security
- **Login System** — Role-based authentication (Client / Admin)
- **User Registration** — Secure signup with password strength validation (min 8 characters, uppercase, lowercase, digit, special character)
- **Account Freeze/Activate** — Admin can freeze or activate any client account
- **Show/Hide Password** — Toggle password visibility on login & signup forms

### 👤 Client Features
| Feature | Description |
|---------|-------------|
| 💰 **Deposit Funds** | Deposit money into your bank account with full transaction logging |
| 💸 **Withdraw Funds** | Withdraw money with balance validation & loan checks |
| 🔄 **Fund Transfer** | Peer-to-peer money transfer between registered clients |
| 💱 **Currency Exchange** | Convert PKR to USD, EUR, or GBP at real-time simulated rates |
| 🏧 **ATM Card Management** | Request a new ATM card, set 4-digit PIN, ATM deposits & withdrawals |
| 📊 **Interest Calculator** | Simple & Compound interest calculation with 30-day cooldown |
| 🏦 **Take Loan** | Request loans with 20% balance eligibility requirement |
| 💳 **Pay Loan** | Partial or full loan repayment |
| 📜 **Transaction History** | Separate deposit & withdrawal history logs |

### 🛡️ Admin Features
| Feature | Description |
|---------|-------------|
| 👥 **User Management** | View all registered clients in a data grid |
| ❄️ **Freeze Accounts** | Suspend client accounts to prevent access |
| ✅ **Activate Accounts** | Reactivate frozen client accounts |
| 🗑️ **Delete Users** | Permanently remove client accounts and all associated data |

---

## 📸 Screenshots

> *Screenshots coming soon — Run the application to explore the beautiful UI with custom background themes!*

<!--
Add screenshots here:
![Login Screen](screenshots/login.png)
![Client Dashboard](screenshots/client-menu.png)
![Admin Panel](screenshots/admin-panel.png)
-->

---

## 🛠️ Tech Stack

| Component | Technology |
|-----------|------------|
| **Language** | C# |
| **Framework** | .NET Framework 4.8 |
| **UI** | Windows Forms (WinForms) |
| **IDE** | Visual Studio 2019/2022 |
| **Data Storage** | File-based (Text Files) |
| **Platform** | Windows (AnyCPU) |

---

## 🚀 Getting Started

### Prerequisites

- **Windows OS** (7 / 10 / 11)
- **Visual Studio 2019 or later** with *.NET Desktop Development* workload
- **.NET Framework 4.8** runtime

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/FAST-NU-Banking-System.git
   ```

2. **Open the solution**
   ```
   Open OOP.sln in Visual Studio
   ```

3. **Build & Run**
   ```
   Press F5 or click Start to build and run the application
   ```

4. **Create an account**
   - Click the **Sign Up** link on the login page
   - Register with a username and a strong password
   - Log in as a **Client** to explore all banking features

### Default Admin Access

> To access the admin panel, manually add an admin entry in `users.txt`:
> ```
> admin:YourPassword1!:Admin
> ```

---

## 🏗️ Project Architecture

```
📁 FAST-NU-Banking-System/
│
├── 📄 Program.cs                    # Application entry point
├── 📄 BankUser.cs                   # Core data model class
├── 📄 OOP.sln                       # Visual Studio solution file
├── 📄 OOP.csproj                    # Project configuration
├── 📄 App.config                    # Application configuration
│
├── 🔐 Authentication
│   ├── 📄 Form1.cs                  # Login screen (main entry form)
│   └── 📄 Signuppage.cs             # User registration form
│
├── 🛡️ Admin
│   └── 📄 AdminPanel.cs             # Admin dashboard & user management
│
├── 👤 Client
│   ├── 📄 ClientMenu.cs             # Client main navigation menu
│   ├── 📄 FundTransfer.cs           # Peer-to-peer fund transfers
│   ├── 📄 CurrencyExchangeForm.cs   # PKR ↔ USD/EUR/GBP exchange
│   ├── 📄 DepositWithdraw.cs        # Deposit/Withdraw hub
│   ├── 📄 Deposit.cs                # Deposit funds
│   ├── 📄 Withdraw.cs               # Withdraw funds
│   ├── 📄 InterestInfo.cs           # Interest calculator
│   └── 📄 TransactionInfo.cs        # Transaction history hub
│
├── 🏦 Loans
│   ├── 📄 LoanInfo.cs               # Loan management hub
│   ├── 📄 TakeLoan.cs               # Request a loan
│   └── 📄 PayLoan.cs                # Repay loan
│
├── 🏧 ATM
│   ├── 📄 AtmInfo.cs                # ATM card info & operations
│   └── 📄 NewCard.cs                # Request new ATM card
│
├── 📜 History
│   ├── 📄 Withdrawl.cs              # Withdrawal history viewer
│   └── 📄 DeposMenu.cs              # Deposit history viewer
│
├── 📁 Resources/                    # Background images & assets
└── 📁 Properties/                   # Assembly info & settings
```

---

## 💾 Data Storage

The application uses **file-based persistence** with colon-delimited text files:

| File | Format | Purpose |
|------|--------|---------|
| `users.txt` | `username:password:role` | User credentials & roles |
| `balance.txt` | `username:amount` | Account balances |
| `status.txt` | `username:status` | Account status (Active/Frozen) |
| `atm.txt` | `username:cardno:balance` | ATM card data |
| `loan.txt` | `username:amount` | Outstanding loan amounts |
| `transactions.txt` | `datetime: user - action amount` | Withdrawal & exchange logs |
| `deposit.txt` | `datetime: user - Deposit amount` | Deposit logs |
| `last_interest.txt` | `username:date` | Interest cooldown tracking |

> ⚠️ **Note:** Data files are generated at runtime and excluded from version control via `.gitignore`.

---

## 📖 Usage Guide

### As a Client
1. **Sign up** → Create an account with a strong password
2. **Login** → Select "Client" role and enter credentials
3. **Navigate** → Use the client menu to access all banking features
4. **Deposit** → Add funds to your account
5. **Transfer** → Send money to other registered users
6. **Exchange** → Convert PKR to foreign currencies
7. **ATM** → Request an ATM card and manage ATM balance
8. **Loans** → Take or repay loans based on eligibility

### As an Admin
1. **Login** → Select "Admin" role
2. **Manage Users** → View all clients in the admin grid
3. **Right-click** → Freeze, activate, or delete user accounts

---

## 🤝 Contributing

Contributions are welcome! If you'd like to improve this project:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is developed as part of the **OOP Course** at [FAST National University, Lahore](https://lhr.nu.edu.pk/). It is open-source and available for educational purposes.

---

<div align="center">

**Built with ❤️ by Group 3 — SE-2B | FAST NUCES Lahore**

*Object-Oriented Programming Course Project — 2025*

</div>
