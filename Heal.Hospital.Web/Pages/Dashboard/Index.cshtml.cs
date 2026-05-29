using Heal.Hospital.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Dashboard;

[Authorize]
public class IndexModel : PageModel
{
    private const int DashboardPageSize = 5;

    public string FullName { get; private set; } = "";
    public string Role { get; private set; } = "";
    public MockData.StatCard[] Stats { get; private set; } = [];
    public string ChartDataJson { get; private set; } = "{}";
    public string SecondChartDataJson { get; private set; } = "{}";
    public int UnreadAlerts { get; private set; }

    // Role-specific data (paginated slices)
    public MockData.WorklistItem[] Worklist { get; private set; } = [];
    public MockData.RecentPatient[] RecentPatients { get; private set; } = [];
    public MockData.CriticalFinding[] CriticalFindings { get; private set; } = [];
    public MockData.ReportDraft[] ReportDrafts { get; private set; } = [];
    public MockData.BillingBatch[] BillingBatches { get; private set; } = [];
    public MockData.RadiologistProductivity[] RadiologistStats { get; private set; } = [];
    public MockData.ExamRecord[] RecentReports { get; private set; } = [];

    // Pagination: Receptionist — recent patients
    public int RecentPatientsPage { get; private set; } = 1;
    public int RecentPatientsTotalPages { get; private set; } = 1;

    // Pagination: Receptionist — scheduling queue
    public int WorklistPage { get; private set; } = 1;
    public int WorklistTotalPages { get; private set; } = 1;

    // Pagination: RadiologyTechnician — DICOM worklist
    public int RadWorklistPage { get; private set; } = 1;
    public int RadWorklistTotalPages { get; private set; } = 1;

    // Pagination: HospitalManager — radiologist productivity
    public int RadiologistsPage { get; private set; } = 1;
    public int RadiologistsTotalPages { get; private set; } = 1;

    // Pagination: HospitalFinancial — billing batches
    public int BillingPage { get; private set; } = 1;
    public int BillingTotalPages { get; private set; } = 1;

    // Pagination: RequestingPhysician — recent reports
    public int ReportsPage { get; private set; } = 1;
    public int ReportsTotalPages { get; private set; } = 1;

    // Pagination: MedicalAuxiliary / Transcriptionist — drafts
    public int DraftsPage { get; private set; } = 1;
    public int DraftsTotalPages { get; private set; } = 1;

    [BindProperty(SupportsGet = true, Name = "rp")]   public int RpPage   { get; set; } = 1;
    [BindProperty(SupportsGet = true, Name = "wl")]   public int WlPage   { get; set; } = 1;
    [BindProperty(SupportsGet = true, Name = "rwl")]  public int RwlPage  { get; set; } = 1;
    [BindProperty(SupportsGet = true, Name = "rad")]  public int RadPage  { get; set; } = 1;
    [BindProperty(SupportsGet = true, Name = "bil")]  public int BilPage  { get; set; } = 1;
    [BindProperty(SupportsGet = true, Name = "rph")]  public int RphPage  { get; set; } = 1;
    [BindProperty(SupportsGet = true, Name = "drft")] public int DrftPage { get; set; } = 1;

    public void OnGet()
    {
        FullName = User.FindFirstValue("FullName") ?? User.Identity?.Name ?? "";
        Role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        Stats = MockData.GetStatsForRole(Role);
        ChartDataJson = MockData.GetChartDataJson(Role);
        SecondChartDataJson = MockData.GetSecondChartDataJson(Role);
        UnreadAlerts = MockData.Alerts.Count(a => !a.IsRead);

        switch (Role)
        {
            case "Receptionist":
                Paginate(MockData.RecentPatients, DashboardPageSize, RpPage,
                    out var rp, out int rpPages, out int rpPage);
                RecentPatients = rp; RecentPatientsTotalPages = rpPages; RecentPatientsPage = rpPage;

                Paginate(MockData.Worklist.Where(w => w.DicomStatus.StartsWith("Aguardando agendamento")).ToArray(),
                    DashboardPageSize, WlPage, out var wl, out int wlPages, out int wlPage);
                Worklist = wl; WorklistTotalPages = wlPages; WorklistPage = wlPage;
                break;

            case "RadiologyTechnician":
                Paginate(MockData.Worklist.Where(w => w.DicomStatus != "Aguardando agendamento").ToArray(),
                    DashboardPageSize, RwlPage, out var rwl, out int rwlPages, out int rwlPage);
                Worklist = rwl; RadWorklistTotalPages = rwlPages; RadWorklistPage = rwlPage;
                break;

            case "HospitalManager":
                Paginate(MockData.RadiologistStats, DashboardPageSize, RadPage,
                    out var rad, out int radPages, out int radPage);
                RadiologistStats = rad; RadiologistsTotalPages = radPages; RadiologistsPage = radPage;
                break;

            case "HospitalFinancial":
                Paginate(MockData.BillingBatches, DashboardPageSize, BilPage,
                    out var bil, out int bilPages, out int bilPage);
                BillingBatches = bil; BillingTotalPages = bilPages; BillingPage = bilPage;
                break;

            case "RequestingPhysician":
                CriticalFindings = MockData.CriticalFindings;
                var rphAll = MockData.Exams.Where(e => e.Physician == "Dr. Paulo Ferreira").ToArray();
                Paginate(rphAll, DashboardPageSize, RphPage,
                    out var rph, out int rphPages, out int rphPage);
                RecentReports = rph; ReportsTotalPages = rphPages; ReportsPage = rphPage;
                break;

            case "MedicalAuxiliary":
                Paginate(MockData.ReportDrafts.Where(d => d.AuthoredBy.StartsWith("Dra.")).ToArray(),
                    DashboardPageSize, DrftPage, out var aux, out int auxPages, out int auxPage);
                ReportDrafts = aux; DraftsTotalPages = auxPages; DraftsPage = auxPage;
                break;

            case "Transcriptionist":
                Paginate(MockData.ReportDrafts.Where(d => d.AuthoredBy == "João Batista").ToArray(),
                    DashboardPageSize, DrftPage, out var trn, out int trnPages, out int trnPage);
                ReportDrafts = trn; DraftsTotalPages = trnPages; DraftsPage = trnPage;
                break;
        }

        ViewData["ActivePage"] = "Dashboard";
        ViewData["AlertCount"] = UnreadAlerts;
    }

    private static void Paginate<T>(T[] source, int pageSize, int requestedPage,
        out T[] items, out int totalPages, out int currentPage)
    {
        totalPages  = Math.Max(1, (int)Math.Ceiling(source.Length / (double)pageSize));
        currentPage = Math.Clamp(requestedPage, 1, totalPages);
        items       = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToArray();
    }
}
