using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Profile;

[Authorize]
public class ChangePasswordModel : PageModel
{
    public string FullName { get; private set; } = "";
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    [Microsoft.AspNetCore.Mvc.BindProperty]
    public string CurrentPassword { get; set; } = "";
    [Microsoft.AspNetCore.Mvc.BindProperty]
    public string NewPassword { get; set; } = "";
    [Microsoft.AspNetCore.Mvc.BindProperty]
    public string Confirm { get; set; } = "";

    public void OnGet()
    {
        FullName = User.FindFirstValue("FullName") ?? "";
        ViewData["ActivePage"] = "Profile.ChangePassword";
        ViewData["AlertCount"] = Heal.Hospital.Web.Services.MockData.Alerts.Count(a => !a.IsRead);
    }

    public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
    {
        FullName = User.FindFirstValue("FullName") ?? "";
        ViewData["ActivePage"] = "Profile.ChangePassword";
        ViewData["AlertCount"] = Heal.Hospital.Web.Services.MockData.Alerts.Count(a => !a.IsRead);

        if (NewPassword != Confirm)
        {
            ErrorMessage = "As senhas não coincidem.";
            return Page();
        }
        Success = true;
        return Page();
    }
}
