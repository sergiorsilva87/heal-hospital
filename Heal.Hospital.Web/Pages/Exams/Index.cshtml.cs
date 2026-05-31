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
                    accessCodeRaw       = e.DownloadAccessCode ?? "",
                    unit                = e.Unit,
                    birthStr            = e.BirthDateTime.ToString("dd/MM/yyyy HH:mm:ss"),
                    ageFormatted        = MockData.FormatDicomAge(e.BirthDateTime, e.StudyDateTime),
                    studyId             = e.StudyId,
                    reportDateTimeStr   = e.ReportDateTime?.ToString("dd/MM/yyyy HH:mm") ?? "\u2014",
                    liberationDateStr   = e.LiberationDateTime?.ToString("dd/MM/yyyy HH:mm") ?? "\u2014",
                    criticalFinding     = e.CriticalFinding,
                    hasPendency         = e.HasPendency,
                    technicianName      = e.TechnicianName,
                    executingPhysician  = e.ExecutingPhysician ?? "\u2014",
                    requestingPhysician = e.RequestingPhysician,
                    studyDateOnly       = e.StudyDateTime.ToString("yyyy-MM-dd"),
                    liberationDateOnly  = e.LiberationDateTime?.ToString("yyyy-MM-dd") ?? "",
                    imageCount          = e.ImageCount,
                    attachmentCount     = e.AttachmentCount,
                    isArchived          = e.IsArchived,
                    downloadCodeGenerated = MockData.WasDownloadCodeGenerated(e),
                    reportDownloaded      = MockData.WasReportDownloaded(e),
                    phoneRaw            = MockData.GetMockPhone(e.PatientId),
                    motherNameRaw       = MockData.GetMockMotherName(e.PatientId),
                    studyForInput       = e.StudyDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    changeLogs = MockData.GetChangeLogs(e.PatientId).Select(c => new
                    {
                        userName = c.UserName,
                        userRole = c.UserRole,
                        unit     = c.Unit,
                        at       = c.At.ToString("dd/MM/yyyy HH:mm"),
                        initials = string.Join("", c.UserName
                                        .Replace("Dr. ", "").Replace("Dra. ", "")
                                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                        .Take(2).Select(p => p[0])),
                        summary  = c.Summary
                    }),
                    reports = MockData.GetExamReports(e).Select(r => new
                    {
                        title  = r.Title,
                        isCold = r.IsCold
                    }),
                    attachments = MockData.GetAttachments(e).Select(a => new
                    {
                        fileName   = a.FileName,
                        uploadedAt = a.UploadedAt.ToString("dd/MM/yyyy HH:mm"),
                        type       = MockData.AttachmentTypeLabel(a.FileName),
                        size       = MockData.FormatFileSize(a.SizeBytes),
                        uploadedBy = a.UploadedBy,
                        isCold     = a.IsCold
                    }),
                    priorExams = MockData.GetPriorExams(e).Select(p => new
                    {
                        studyId       = p.StudyId,
                        accessNumber  = p.AccessNumber,
                        when          = p.When.ToString("dd/MM/yyyy HH:mm"),
                        title         = p.Title,
                        isCold        = p.IsCold,
                        attachments   = p.Attachments.Select(a => new
                        {
                            fileName   = a.FileName,
                            uploadedAt = a.UploadedAt.ToString("dd/MM/yyyy HH:mm"),
                            type       = MockData.AttachmentTypeLabel(a.FileName),
                            size       = MockData.FormatFileSize(a.SizeBytes),
                            uploadedBy = a.UploadedBy,
                            isCold     = a.IsCold
                        })
                    }),
                    criticalDetail = MockData.GetCriticalDetail(e) is { } cd ? new
                    {
                        state        = cd.State,
                        detectedAt   = cd.DetectedAt.ToString("dd/MM/yyyy HH:mm"),
                        physician    = cd.Physician,
                        crm          = cd.Crm,
                        uf           = cd.Uf,
                        message      = cd.Message,
                        resolvedBy   = cd.ResolvedBy,
                        resolvedRole = cd.ResolvedRole,
                        resolvedAt   = cd.ResolvedAt?.ToString("dd/MM/yyyy HH:mm"),
                        contactNote  = cd.ContactNote
                    } : null,
                    pendencyDetail = MockData.GetPendencyDetail(e) is { } pd ? new
                    {
                        physician = pd.Physician,
                        crm       = pd.Crm,
                        uf        = pd.Uf,
                        openedAt  = pd.OpenedAt.ToString("dd/MM/yyyy HH:mm"),
                        text      = pd.Text
                    } : null,
                    archivedItems = MockData.GetArchivedItems(e).Select(a => new
                    {
                        name       = a.Name,
                        kind       = a.Kind,
                        size       = a.Size,
                        archivedAt = a.ArchivedAt.ToString("dd/MM/yyyy")
                    }),
                    codeGeneratedAt = MockData.GetCodeGeneratedAt(e)?.ToString("dd/MM/yyyy HH:mm"),
                    downloadEvents = MockData.GetDownloadEvents(e).Select(d => new
                    {
                        at     = d.At.ToString("dd/MM/yyyy HH:mm"),
                        device = d.Device,
                        ip     = d.IpAddress
                    })
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
