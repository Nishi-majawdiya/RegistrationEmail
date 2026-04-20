using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly EmailService _emailService;

    public AccountController(EmailService emailService)
    {
        _emailService = emailService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, string submitType)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (!IsValidPassword(model.TemporaryPassword))
        {
            ModelState.AddModelError("", "Password does not meet requirements.");
            return View(model);
        }

        try
        {
            string loginLink = "https://localhost:5001/account/login";

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

            ViewBag.Message = "Email sent successfully!";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error sending email: " + ex.Message);
        }

        return View(model);
    }

    private bool IsValidPassword(string password)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(password,
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{10,}$");
    }
}