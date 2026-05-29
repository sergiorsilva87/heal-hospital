using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Heal.Hospital.Web.Pages;

public class ResetPasswordModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Token { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Informe a nova senha.")]
    [MinLength(8, ErrorMessage = "Mínimo de 8 caracteres.")]
    public string NewPassword { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Confirme a nova senha.")]
    public string Confirm { get; set; } = "";

    public bool Success { get; set; }
    public bool InvalidToken { get; set; }
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        // Mock: treat any non-empty token as valid
        if (string.IsNullOrWhiteSpace(Token))
            InvalidToken = true;
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        if (NewPassword != Confirm)
        {
            ErrorMessage = "As senhas não coincidem.";
            return Page();
        }

        // Mock: always succeed
        Success = true;
        return Page();
    }
}
