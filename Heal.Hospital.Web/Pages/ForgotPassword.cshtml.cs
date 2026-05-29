using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Heal.Hospital.Web.Pages;

public class ForgotPasswordModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Informe o usuário ou e-mail.")]
    public string UsernameOrEmail { get; set; } = "";

    public bool Sent { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        // Mock: always succeed (don't reveal whether user exists)
        Sent = true;
        return Page();
    }
}
