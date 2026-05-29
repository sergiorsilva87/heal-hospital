using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Profile;

[Authorize]
public class PersonalDataModel : PageModel
{
    public string FullName      { get; private set; } = "";
    public string Role          { get; private set; } = "";
    public string? ProfRegLabel { get; private set; }   // "CRM", "CRTR", or null
    public bool Success         { get; set; }

    [BindProperty] public string DisplayName { get; set; } = "";
    [BindProperty] public string Phone       { get; set; } = "(11) 9 0000-0000";
    [BindProperty] public string Department  { get; set; } = "Radiologia";
    [BindProperty] public string ProfRegNumber { get; set; } = "";

    private void Populate()
    {
        FullName    = User.FindFirstValue("FullName") ?? "";
        Role        = User.FindFirstValue(ClaimTypes.Role) ?? "";
        DisplayName = FullName;
        ProfRegLabel = Role switch
        {
            "RequestingPhysician" => "CRM",
            "MedicalAuxiliary"    => "CRM",
            "RadiologyTechnician" => "CRTR",
            _                     => null
        };
        ViewData["ActivePage"] = "Profile.PersonalData";
        ViewData["AlertCount"] = Heal.Hospital.Web.Services.MockData.Alerts.Count(a => !a.IsRead);
    }

    public void OnGet() => Populate();

    public IActionResult OnPost()
    {
        Populate();
        Success = true;
        return Page();
    }
}
