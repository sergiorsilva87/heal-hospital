using Heal.Hospital.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Alerts;

[Authorize]
public class IndexModel : PageModel
{
    public MockData.AlertRecord[] Alerts { get; private set; } = [];
    public int UnreadCount { get; private set; }

    public void OnGet()
    {
        Alerts = MockData.Alerts.OrderByDescending(a => a.Timestamp).ToArray();
        UnreadCount = Alerts.Count(a => !a.IsRead);

        ViewData["ActivePage"] = "Alerts";
        ViewData["AlertCount"] = UnreadCount;
    }
}
