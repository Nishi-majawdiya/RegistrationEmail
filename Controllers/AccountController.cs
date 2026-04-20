using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class AccountController : Controller
{
    private readonly EmailService _emailService;

    public AccountController(EmailService emailService)
    {
        _emailService = emailService;
    }

    // GET: Register Page
    public IActionResult Register()
    {
        return View();
    }

    // POST: Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, string submitType)
    {
        if (!ModelState.IsValid)
            return View(model);

        // 🔐 Password Validation with detailed errors
        var passwordErrors = GetPasswordErrors(model.TemporaryPassword);

        if (passwordErrors.Any())
        {
            ModelState.AddModelError("TemporaryPassword",
                "Missing: " + string.Join(", ", passwordErrors));
            return View(model);
        }

        try
        {
            string loginLink = "https://localhost:7125/Account/Login";

            // ✅ Normal Email
            if (submitType == "normal")
            {
                string body = $@"
                    <h3>User Registration</h3>
                    <p><b>First Name:</b> {model.FirstName}</p>
                    <p><b>Last Name:</b> {model.LastName}</p>
                    <p><b>Email:</b> {model.Email}</p>
                    <p><b>Password:</b> {model.TemporaryPassword}</p>
                    <p><a href='{loginLink}'>Login Here</a></p>
                ";

                await _emailService.SendEmailAsync(model.Email, "Registration", body);
            }
            // ✅ Template Email
            else if (submitType == "template")
            {
                var templatePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/templates/RegistrationTemplate.html");

                string html = await System.IO.File.ReadAllTextAsync(templatePath);

                html = html.Replace("{{FirstName}}", model.FirstName)
                           .Replace("{{LastName}}", model.LastName)
                           .Replace("{{Email}}", model.Email)
                           .Replace("{{Password}}", model.TemporaryPassword)
                           .Replace("{{LoginLink}}", loginLink);

                await _emailService.SendEmailAsync(model.Email, "Registration", html);
            }

            ViewBag.Message = "✅ Email sent successfully!";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "❌ Error sending email: " + ex.Message);
        }

        return View(model);
    }

    // 🔐 Password Validation Method
    private List<string> GetPasswordErrors(string password)
    {
        var errors = new List<string>();

        if (!Regex.IsMatch(password, @"[A-Z]"))
            errors.Add("at least one uppercase letter");

        if (!Regex.IsMatch(password, @"[a-z]"))
            errors.Add("at least one lowercase letter");

        if (!Regex.IsMatch(password, @"\d"))
            errors.Add("at least one number");

        if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
            errors.Add("at least one special character");

        if (password.Length < 10)
            errors.Add("minimum 10 characters");

        return errors;
    }

    public IActionResult Login()
    {
        return View();
    }
}