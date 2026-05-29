using Heal.Hospital.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Heal.Hospital.Web.Pages;

public class MfaModel : PageModel
{
    private readonly MockAuthService _auth;

    public MfaModel(MockAuthService auth) => _auth = auth;

    [BindProperty]
    [Required(ErrorMessage = "Informe o código.")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "O código deve ter 6 dígitos.")]
    public string Code { get; set; } = "";

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        var pendingUser = HttpContext.Session.GetString("mfa_pending_user");
        if (string.IsNullOrEmpty(pendingUser))
            return RedirectToPage("/Login");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var pendingUser = HttpContext.Session.GetString("mfa_pending_user");
        if (string.IsNullOrEmpty(pendingUser))
            return RedirectToPage("/Login");

        if (!ModelState.IsValid)
            return Page();

        if (Code != MockAuthService.MfaCode)
        {
            ErrorMessage = "Código inválido ou expirado.";
            return Page();
        }

        var user = _auth.Validate(pendingUser, null!);
        // Validate by username only (password was checked on Login step)
        // Re-lookup user by username
        user = MockAuthService.FindByUsername(pendingUser);
        if (user is null)
            return RedirectToPage("/Login");

        HttpContext.Session.Remove("mfa_pending_user");

        var principal = MockAuthService.BuildPrincipal(user);
        await HttpContext.SignInAsync("HospitalCookies", principal,
            new AuthenticationProperties { IsPersistent = true });

        // Log access timestamp
        HttpContext.Session.SetString("last_login", DateTime.UtcNow.ToString("o"));

        return RedirectToPage("/Dashboard/Index");
    }
}
