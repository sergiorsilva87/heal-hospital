using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Profile;

[Authorize]
public class AccountDataModel : PageModel
{
    public string FullName { get; private set; } = "";
    public string Username { get; private set; } = "";
    public string Role     { get; private set; } = "";
    public bool Success    { get; set; }

    [BindProperty] public string Email { get; set; } = "usuario@stmungus.com.br";

    public void OnGet()
    {
        FullName = User.FindFirstValue("FullName") ?? "";
        Username = User.Identity?.Name ?? "";
        Role     = User.FindFirstValue(ClaimTypes.Role) ?? "";
        ViewData["ActivePage"] = "Profile.AccountData";
        ViewData["AlertCount"] = Heal.Hospital.Web.Services.MockData.Alerts.Count(a => !a.IsRead);
    }

    public IActionResult OnPost()
    {
        FullName = User.FindFirstValue("FullName") ?? "";
        Username = User.Identity?.Name ?? "";
        Role     = User.FindFirstValue(ClaimTypes.Role) ?? "";
        ViewData["ActivePage"] = "Profile.AccountData";
        ViewData["AlertCount"] = Heal.Hospital.Web.Services.MockData.Alerts.Count(a => !a.IsRead);
        Success = true;
        return Page();
    }
}
