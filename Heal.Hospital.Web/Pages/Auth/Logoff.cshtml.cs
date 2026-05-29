using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Heal.Hospital.Web.Pages.Auth;

public class LogoffModel : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync("HospitalCookies");
        HttpContext.Session.Clear();
        return RedirectToPage("/Login");
    }

    public IActionResult OnGet() => RedirectToPage("/Login");
}
