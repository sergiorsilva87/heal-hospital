using Heal.Hospital.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Heal.Hospital.Web.Pages.Exams;

[Authorize(Roles = "Receptionist,RadiologyTechnician,HospitalManager,RequestingPhysician,MedicalAuxiliary,Transcriptionist")]
public class IndexModel : PageModel
{
    private const int PageSize = 10;

    public string Role { get; private set; } = "";
    public int UnreadAlerts { get; private set; }

    public MockData.ExamRecord[] FilteredExams { get; private set; } = [];

    public MockData.ReceptionExam[] ReceptionPageItems { get; private set; } = [];
    public string ReceptionExamsJson { get; private set; } = "[]";
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; } = 1;

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? StatusFilter { get; set; }

    [BindProperty(SupportsGet = true, Name = "page")]
    public int PageIndex { get; set; } = 1;

    public bool IsReceptionist => Role == "Receptionist";

    public void OnGet()
    {
        Role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        if (PageIndex < 1) PageIndex = 1;

        if (IsReceptionist)
        {
            var query = MockData.ReceptionExams.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var term = Search.Trim();
                query = query.Where(e =>
                    e.PatientName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (e.SocialName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    e.PatientId.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    e.StudyId.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    e.Cpf.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(StatusFilter) &&
                Enum.TryParse<MockData.ExamReportStatus>(StatusFilter, out var statusEnum))
            {
                query = query.Where(e => e.Status == statusEnum);
            }

            var all = query.OrderByDescending(e => e.StudyDateTime).ToArray();
            TotalCount = all.Length;

            ReceptionExamsJson = System.Text.Json.JsonSerializer.Serialize(
                all.Select(e => new
                {
                    patientId     = e.PatientId,
                    patientName   = e.PatientName,
                    socialNameRaw = e.SocialName ?? "",
                    sexRaw        = e.Sex,
                    modality      = e.Modality,
                    procedure     = e.Procedure,
                    studyDateTime = e.StudyDateTime.ToString("dd/MM/yyyy HH:mm"),
                    statusLabel   = MockData.GetReceptionStatusLabel(e.Status),
                    statusCss     = MockData.GetStatusBadgeCss(e.Status),
                    typeLabel     = MockData.GetExamTypeLabel(e.Type),
                    isEmergency   = e.IsEmergency,
                    hasReport     = e.Status is MockData.ExamReportStatus.Approved
                                             or MockData.ExamReportStatus.ApprovedAfterRevision,
                    birthForInput = e.BirthDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    cpfRaw        = e.Cpf,
                    emailRaw      = e.Email ?? "",
                    accessCodeRaw = e.DownloadAccessCode ?? "",
                    _children     = new[]
                    {
                        new
                        {
                            _isChild            = true,
                            socialName          = e.SocialName ?? "\u2014",
                            sex                 = e.Sex,
                            birthStr            = e.BirthDateTime.ToString("dd/MM/yyyy HH:mm:ss"),
                            ageFormatted        = MockData.FormatDicomAge(e.BirthDateTime, e.StudyDateTime),
                            studyId             = e.StudyId,
                            reportDateTimeStr   = e.ReportDateTime?.ToString("dd/MM/yyyy HH:mm") ?? "\u2014",
                            technicianName      = e.TechnicianName,
                            executingPhysician  = e.ExecutingPhysician ?? "\u2014",
                            requestingPhysician = e.RequestingPhysician,
                            cpfDisplay          = e.Cpf,
                            emailDisplay        = e.Email ?? "\u2014",
                            accessCodeDisplay   = e.DownloadAccessCode ?? "\u2014"
                        }
                    }
                })
            );
        }
        else
        {
            var ordered = MockData.Exams
                .Where(e => string.IsNullOrEmpty(Search) ||
                            e.Patient.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                            e.Id.Contains(Search, StringComparison.OrdinalIgnoreCase))
                .Where(e => string.IsNullOrEmpty(StatusFilter) || e.Status == StatusFilter)
                .OrderByDescending(e => e.Date)
                .ToArray();

            TotalCount = ordered.Length;
            TotalPages = Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
            if (PageIndex > TotalPages) PageIndex = TotalPages;

            FilteredExams = ordered
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToArray();
        }

        UnreadAlerts = MockData.Alerts.Count(a => !a.IsRead);
        ViewData["ActivePage"] = "Exams";
        ViewData["AlertCount"] = UnreadAlerts;
    }
}
