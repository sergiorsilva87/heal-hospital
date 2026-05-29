using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Profile;

[Authorize]
public class PhotoModel : PageModel
{
    public string FullName { get; private set; } = "";
    public bool Success    { get; set; }

    public void OnGet()
    {
        FullName = User.FindFirstValue("FullName") ?? "";
        ViewData["ActivePage"] = "Profile.Photo";
        ViewData["AlertCount"] = Heal.Hospital.Web.Services.MockData.Alerts.Count(a => !a.IsRead);
    }

    public IActionResult OnPost()
    {
        FullName = User.FindFirstValue("FullName") ?? "";
        ViewData["ActivePage"] = "Profile.Photo";
        ViewData["AlertCount"] = Heal.Hospital.Web.Services.MockData.Alerts.Count(a => !a.IsRead);
        Success = true;
        return Page();
    }
}
