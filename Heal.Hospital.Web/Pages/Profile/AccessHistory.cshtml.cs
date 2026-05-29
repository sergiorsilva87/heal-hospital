using Heal.Hospital.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Profile;

[Authorize]
public class AccessHistoryModel : PageModel
{
    public string FullName { get; private set; } = "";
    public MockData.AccessEntry[] History { get; private set; } = [];

    public void OnGet()
    {
        FullName = User.FindFirstValue("FullName") ?? "";
        History  = MockData.AccessHistory;
        ViewData["ActivePage"] = "Profile.AccessHistory";
        ViewData["AlertCount"] = MockData.Alerts.Count(a => !a.IsRead);
    }
}
