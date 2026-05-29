using Heal.Hospital.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Heal.Hospital.Web.Pages;

public class LoginModel : PageModel
{
    private readonly MockAuthService _auth;

    public LoginModel(MockAuthService auth) => _auth = auth;

    [BindProperty]
    [Required]
    public string Username { get; set; } = "";

    [BindProperty]
    [Required]
    public string Password { get; set; } = "";

    [BindProperty]
    public bool RememberMe { get; set; }

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Dashboard/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = _auth.Validate(Username, Password);
        if (user is null)
        {
            ErrorMessage = "Credenciais inválidas.";
            return Page();
        }

        // Store username in session for MFA flow
        HttpContext.Session.SetString("mfa_pending_user", user.Username);

        return RedirectToPage("/Mfa");
    }
}
