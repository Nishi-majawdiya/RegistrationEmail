# 📧 ASP.NET Core MVC Registration Email System

## 🚀 Overview
This project is a user registration system built using ASP.NET Core MVC.  
It allows users to register through a form and receive a dynamic email with their details and an activation link.

---

## ✨ Features

- User Registration Form (MVC Architecture)
- Strong Password Validation
- Email Sending using SMTP (Gmail)
- HTML Email Template Support
- Dynamic Data Injection (First Name, Last Name, Email, Link)
- Two Registration Modes:
  - Normal Email
  - Template-based Email

---

## 🛠️ Tech Stack

- ASP.NET Core MVC
- C#
- Razor Views
- Bootstrap (UI)
- SMTP (Gmail App Password)

---

## 🔐 Password Requirements

The temporary password must contain:

- At least 1 uppercase letter  
- At least 1 lowercase letter  
- At least 1 numeric digit  
- At least 1 special character  
- Minimum length: 10 characters  

---

## 📸 Screenshots

### 📝 Registration Form
![Register](screenshots/register.png)

### 📧 Email Received
![Email](screenshots/email.png)

---

## ⚙️ How to Run

1. Clone the repository
2. Open project in Visual Studio / VS Code
3. Configure SMTP in `appsettings.json`
4. Run the project
5. Open in browser:


---

## 📬 SMTP Configuration

Update in `appsettings.json`:

```json
"Smtp": {
  "Host": "smtp.gmail.com",
  "Port": "587",
  "Username": "your-email@gmail.com",
  "Password": "your-app-password",
  "From": "your-email@gmail.com"
}

⚠️ Use App Password, not your Gmail password