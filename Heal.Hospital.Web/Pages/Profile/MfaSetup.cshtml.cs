using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Profile;

[Authorize]
public class MfaSetupModel : PageModel
{
    public string FullName { get; private set; } = "";
    public bool MfaEnabled { get; } = true; // mock: always enabled

    public void OnGet()
    {
        FullName = User.FindFirstValue("FullName") ?? "";
        ViewData["ActivePage"] = "Profile.MfaSetup";
        ViewData["AlertCount"] = Heal.Hospital.Web.Services.MockData.Alerts.Count(a => !a.IsRead);
    }
}
