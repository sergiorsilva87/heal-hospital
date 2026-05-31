using Heal.Hospital.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Profile;

[Authorize]
public class IndexModel : PageModel
{
    // Display properties
    public string FullName      { get; private set; } = "";
    public string Username      { get; private set; } = "";
    public string Role          { get; private set; } = "";
    public string? ProfRegLabel { get; private set; }

    // Success flags
    public bool AccountDataSuccess { get; set; }
    public bool PersonalDataSuccess { get; set; }
    public bool PhotoSuccess { get; set; }
    public bool PasswordSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    // MFA
    public bool MfaEnabled { get; } = true;

    // Access history pagination
    public int HistoryPage { get; set; } = 1;
    public int HistoryPageSize { get; } = 10;
    public int HistoryTotal { get; set; }
    public int HistoryTotalPages { get; set; }
    public MockData.AccessEntry[] PagedHistory { get; set; } = [];
    public string? FilterDateFrom { get; set; }
    public string? FilterDateTo { get; set; }
    public string? FilterStatus { get; set; }

    // AccountData form
    [BindProperty] public string Email { get; set; } = "usuario@stmungus.com.br";

    // PersonalData form
    [BindProperty] public string DisplayName   { get; set; } = "";
    [BindProperty] public string Phone         { get; set; } = "(11) 9 0000-0000";
    [BindProperty] public string Department    { get; set; } = "Radiologia";
    [BindProperty] public string ProfRegNumber { get; set; } = "";

    // Password form
    [BindProperty] public string CurrentPassword { get; set; } = "";
    [BindProperty] public string NewPassword     { get; set; } = "";
    [BindProperty] public string Confirm         { get; set; } = "";

    private void Populate()
    {
        FullName    = User.FindFirstValue("FullName") ?? "";
        Username    = User.Identity?.Name ?? "";
        Role        = User.FindFirstValue(ClaimTypes.Role) ?? "";
        DisplayName = FullName;
        ProfRegLabel = Role switch
        {
            "RequestingPhysician" => "CRM",
            "MedicalAuxiliary"    => "CRM",
            "RadiologyTechnician" => "CRTR",
            _                     => null
        };
        ViewData["ActivePage"] = "Profile.Index";
        ViewData["AlertCount"] = MockData.Alerts.Count(a => !a.IsRead);
    }

    private void LoadHistory(int page, string? dateFrom, string? dateTo, string? status)
    {
        FilterDateFrom = dateFrom;
        FilterDateTo   = dateTo;
        FilterStatus   = status;

        var query = MockData.AccessHistory.AsEnumerable();

        if (DateTime.TryParse(dateFrom, out var from))
            query = query.Where(e => e.At >= from.ToUniversalTime());
        if (DateTime.TryParse(dateTo, out var to))
            query = query.Where(e => e.At <= to.AddDays(1).ToUniversalTime());
        if (!string.IsNullOrEmpty(status))
        {
            var success = status == "success";
            query = query.Where(e => e.Success == success);
        }

        var all = query.OrderByDescending(e => e.At).ToArray();
        HistoryTotal      = all.Length;
        HistoryTotalPages = (int)Math.Ceiling((double)HistoryTotal / HistoryPageSize);
        HistoryPage       = Math.Max(1, Math.Min(page, Math.Max(1, HistoryTotalPages)));
        PagedHistory      = all.Skip((HistoryPage - 1) * HistoryPageSize).Take(HistoryPageSize).ToArray();
    }

    public void OnGet(int historyPage = 1, string? dateFrom = null, string? dateTo = null, string? status = null)
    {
        Populate();
        LoadHistory(historyPage, dateFrom, dateTo, status);

        bool historyActive = historyPage > 1 || !string.IsNullOrEmpty(dateFrom) || !string.IsNullOrEmpty(dateTo) || !string.IsNullOrEmpty(status);
        ViewData["ActiveTab"] = historyActive ? "history" : "profile";
    }

    public IActionResult OnPostAccountdata()
    {
        Populate();
        LoadHistory(1, null, null, null);
        ViewData["ActiveTab"] = "profile";
        AccountDataSuccess = true;
        return Page();
    }

    public IActionResult OnPostPersonaldata()
    {
        Populate();
        LoadHistory(1, null, null, null);
        ViewData["ActiveTab"] = "profile";
        PersonalDataSuccess = true;
        return Page();
    }

    public IActionResult OnPostPhoto()
    {
        Populate();
        LoadHistory(1, null, null, null);
        ViewData["ActiveTab"] = "profile";
        PhotoSuccess = true;
        return Page();
    }

    public IActionResult OnPostPassword()
    {
        Populate();
        LoadHistory(1, null, null, null);
        ViewData["ActiveTab"] = "profile";
        if (NewPassword != Confirm)
        {
            ErrorMessage = "As senhas não coincidem.";
            return Page();
        }
        PasswordSuccess = true;
        return Page();
    }
}
