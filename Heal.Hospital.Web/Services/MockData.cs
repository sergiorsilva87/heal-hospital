namespace Heal.Hospital.Web.Services;

/// <summary>Mock data for wireframe — all data is fictional.</summary>
public static class MockData
{
    // ─────────────────────────────────────────────────────────
    // EXAMS
    // ─────────────────────────────────────────────────────────
    public record ExamRecord(
        string Id, string Patient, string Modality, string Physician,
        DateTime Date, string Status, string? ReportUrl);

    public static readonly ExamRecord[] Exams =
    [
        new("EX-001", "Maria Santos",     "Radiografia",     "Dr. Paulo Ferreira",   new DateTime(2026,1,15), "Laudo emitido",  "#"),
        new("EX-002", "João Oliveira",    "Tomografia",      "Dra. Camila Rocha",    new DateTime(2026,1,16), "Em andamento",   null),
        new("EX-003", "Ana Lima",         "Ressonância",     "Dr. Paulo Ferreira",   new DateTime(2026,1,16), "Laudo pendente", null),
        new("EX-004", "Carlos Pereira",   "Ultrassonografia","Dra. Camila Rocha",    new DateTime(2026,1,17), "Agendado",       null),
        new("EX-005", "Fernanda Costa",   "Mamografia",      "Dr. Paulo Ferreira",   new DateTime(2026,1,17), "Concluído",      "#"),
        new("EX-006", "Roberto Souza",    "PET-CT",          "Dra. Camila Rocha",    new DateTime(2026,1,18), "Laudo emitido",  "#"),
        new("EX-007", "Luciana Ferreira", "Radiografia",     "Dr. Paulo Ferreira",   new DateTime(2026,1,18), "Agendado",       null),
        new("EX-008", "Eduardo Martins",  "Tomografia",      "Dra. Camila Rocha",    new DateTime(2026,1,19), "Laudo pendente", null),
        new("EX-009", "Patrícia Alves",   "Ressonância",     "Dr. Paulo Ferreira",   new DateTime(2026,1,19), "Em andamento",   null),
        new("EX-010", "Bruno Gomes",      "Ultrassonografia","Dra. Camila Rocha",    new DateTime(2026,1,20), "Concluído",      "#"),
        new("EX-011", "Camila Torres",    "Radiografia",     "Dr. Paulo Ferreira",   new DateTime(2026,1,20), "Agendado",       null),
        new("EX-012", "Diego Rocha",      "Tomografia",      "Dra. Camila Rocha",    new DateTime(2026,1,21), "Laudo emitido",  "#"),
    ];

    // ─────────────────────────────────────────────────────────
    // ALERTS
    // ─────────────────────────────────────────────────────────
    public record AlertRecord(
        string Id, string Title, string Body,
        DateTime Timestamp, string Severity, bool IsRead);

    public static readonly AlertRecord[] Alerts =
    [
        new("AL-001", "Laudo crítico — Ana Lima",
            "Ressonância EX-003 apresenta achados que requerem atenção imediata.",
            DateTime.UtcNow.AddMinutes(-15), "Urgente", false),

        new("AL-002", "Equipamento RM-2 em manutenção",
            "A ressonância RM-2 está fora de operação até amanhã às 08h.",
            DateTime.UtcNow.AddHours(-2), "Atenção", false),

        new("AL-003", "Novo exame agendado",
            "Exame EX-011 (Camila Torres) agendado para hoje às 14h.",
            DateTime.UtcNow.AddHours(-4), "Informativo", false),

        new("AL-004", "Laudo aprovado — Roberto Souza",
            "Laudo PET-CT EX-006 aprovado e disponível no portal do paciente.",
            DateTime.UtcNow.AddHours(-6), "Informativo", true),

        new("AL-005", "Sistema em manutenção programada",
            "Haverá indisponibilidade parcial no domingo das 02h às 04h.",
            DateTime.UtcNow.AddDays(-1), "Atenção", true),
    ];

    // ─────────────────────────────────────────────────────────
    // ACCESS HISTORY
    // ─────────────────────────────────────────────────────────
    public record AccessEntry(DateTime At, string Ip, string Device, bool Success);

    public static readonly AccessEntry[] AccessHistory =
    [
        new(DateTime.UtcNow.AddMinutes(-5),  "192.168.1.10", "Chrome 124 · Windows 11",   true),
        new(DateTime.UtcNow.AddHours(-2),    "192.168.1.10", "Chrome 124 · Windows 11",   true),
        new(DateTime.UtcNow.AddDays(-1),     "10.0.0.55",    "Safari 17 · macOS Sonoma",  true),
        new(DateTime.UtcNow.AddDays(-2),     "177.93.4.1",   "Firefox 125 · Ubuntu 24",   false),
        new(DateTime.UtcNow.AddDays(-3),     "192.168.1.10", "Chrome 124 · Windows 11",   true),
        new(DateTime.UtcNow.AddDays(-5),     "192.168.1.10", "Edge 123 · Windows 11",     true),
    ];

    // ─────────────────────────────────────────────────────────
    // DASHBOARD STATS (per role)
    // ─────────────────────────────────────────────────────────
    public record StatCard(string Icon, string Value, string Label, string Color);

    public static StatCard[] GetStatsForRole(string role) => role switch
    {
        "Receptionist" =>
        [
            new("bi-calendar-check", "24",    "Exames agendados hoje",   "navy"),
            new("bi-person-check",   "18",    "Check-ins realizados",    "teal"),
            new("bi-hourglass-split","6",     "Aguardando atendimento",  "amber"),
            new("bi-telephone",      "3",     "Ligações em aberto",      "info"),
        ],
        "RadiologyTechnician" =>
        [
            new("bi-camera",         "12",    "Exames realizados hoje",  "navy"),
            new("bi-clock",          "4",     "Na fila de execução",     "amber"),
            new("bi-check-circle",   "8",     "Concluídos",              "green"),
            new("bi-exclamation-triangle","1","Retrabalho necessário",   "red"),
        ],
        "HospitalManager" =>
        [
            new("bi-hospital",       "142",   "Exames no mês",           "navy"),
            new("bi-people",         "7",     "Usuários ativos",         "teal"),
            new("bi-bar-chart",      "94%",   "Taxa de conclusão",       "green"),
            new("bi-bell",           "5",     "Alertas pendentes",       "amber"),
        ],
        "HospitalFinancial" =>
        [
            new("bi-currency-dollar","R$ 48k","Faturamento do mês",      "navy"),
            new("bi-receipt",        "36",    "Faturas geradas",         "teal"),
            new("bi-check2-all",     "29",    "Faturas pagas",           "green"),
            new("bi-exclamation-circle","7",  "Faturas abertas",         "amber"),
        ],
        "RequestingPhysician" =>
        [
            new("bi-file-earmark-text","8",  "Laudos emitidos hoje",    "navy"),
            new("bi-file-earmark-plus","5",  "Aguardando laudo",        "amber"),
            new("bi-people",           "13", "Pacientes atendidos",     "teal"),
            new("bi-patch-check",      "3",  "Laudos assinados",        "green"),
        ],
        "MedicalAuxiliary" =>
        [
            new("bi-pencil-square",   "6",   "Rascunhos em progresso",  "navy"),
            new("bi-send",            "2",   "Aguardando revisão",      "amber"),
            new("bi-archive",         "14",  "Exames na fila",          "teal"),
            new("bi-clock-history",   "3",   "Devolvidos para revisão", "red"),
        ],
        "Transcriptionist" =>
        [
            new("bi-mic",             "9",   "Ditados transcritos hoje","navy"),
            new("bi-hourglass",       "3",   "Na fila de transcrição",  "amber"),
            new("bi-check-all",       "22",  "Concluídos no mês",       "green"),
            new("bi-flag",            "1",   "Marcados para revisão",   "red"),
        ],
        _ =>
        [
            new("bi-clipboard-data",  "0",   "Sem dados",               "navy"),
        ]
    };

    // ─────────────────────────────────────────────────────────
    // GOOGLE CHARTS DATA (per role)
    // ─────────────────────────────────────────────────────────
    public static string GetChartDataJson(string role) => role switch
    {
        "HospitalManager" or "Receptionist" =>
            "{\"type\":\"ColumnChart\",\"title\":\"Exames por modalidade (jan/2026)\",\"cols\":[[\"Modalidade\",\"string\"],[\"Quantidade\",\"number\"]],\"rows\":[[\"Radiografia\",42],[\"Tomografia\",31],[\"Ressonancia\",28],[\"Ultrassom\",25],[\"Mamografia\",10],[\"PET-CT\",6]]}",

        "RadiologyTechnician" =>
            "{\"type\":\"PieChart\",\"title\":\"Estudos por status DICOM\",\"cols\":[[\"Status\",\"string\"],[\"Estudos\",\"number\"]],\"rows\":[[\"Stored\",38],[\"Indexed\",31],[\"Receiving\",8],[\"Archived\",25]]}",

        "HospitalFinancial" =>
            "{\"type\":\"ColumnChart\",\"title\":\"Receita bruta vs. liquida (ultimos 6 meses)\",\"cols\":[[\"Mes\",\"string\"],[\"Bruto\",\"number\"],[\"Liquido\",\"number\"]],\"rows\":[[\"Ago\",39000,33800],[\"Set\",41200,35900],[\"Out\",43100,37900],[\"Nov\",41300,36100],[\"Dez\",45200,39800],[\"Jan\",48500,42300]]}",

        "RequestingPhysician" =>
            "{\"type\":\"PieChart\",\"title\":\"Meus exames por status\",\"cols\":[[\"Status\",\"string\"],[\"Quantidade\",\"number\"]],\"rows\":[[\"Concluido\",8],[\"Em andamento\",5],[\"Pendente\",3],[\"Agendado\",2]]}",

        "MedicalAuxiliary" =>
            "{\"type\":\"BarChart\",\"title\":\"Laudos por status\",\"cols\":[[\"Status\",\"string\"],[\"Quantidade\",\"number\"]],\"rows\":[[\"Emitidos\",8],[\"Aguardando assinatura\",5],[\"Rascunho\",3],[\"Devolvidos\",1]]}",

        "Transcriptionist" =>
            "{\"type\":\"ColumnChart\",\"title\":\"Transcricoes por dia (jan/2026)\",\"cols\":[[\"Dia\",\"string\"],[\"Transcricoes\",\"number\"]],\"rows\":[[\"16/jan\",8],[\"17/jan\",9],[\"18/jan\",7],[\"19/jan\",11],[\"20/jan\",9],[\"21/jan\",6]]}",

        _ =>
            "{\"type\":\"ColumnChart\",\"title\":\"Dados\",\"cols\":[[\"Item\",\"string\"],[\"Valor\",\"number\"]],\"rows\":[]}"
    };

    // ─────────────────────────────────────────────────────────
    // SECOND CHART (per role — for roles needing two charts)
    // ─────────────────────────────────────────────────────────
    public static string GetSecondChartDataJson(string role) => role switch
    {
        "RadiologyTechnician" =>
            "{\"type\":\"BarChart\",\"title\":\"Storage por tier (GB)\",\"cols\":[[\"Tier\",\"string\"],[\"GB\",\"number\"]],\"rows\":[[\"Hot\",320],[\"Warm\",850],[\"Cold\",2400]]}",

        "HospitalManager" =>
            "{\"type\":\"LineChart\",\"title\":\"TAT medio - ultimos 7 dias (min)\",\"cols\":[[\"Dia\",\"string\"],[\"TAT\",\"number\"],[\"Meta\",\"number\"]],\"rows\":[[\"23/jan\",48,45],[\"24/jan\",42,45],[\"25/jan\",51,45],[\"26/jan\",39,45],[\"27/jan\",44,45],[\"28/jan\",37,45],[\"29/jan\",41,45]]}",

        "HospitalFinancial" =>
            "{\"type\":\"PieChart\",\"title\":\"Receita por convenio\",\"cols\":[[\"Convenio\",\"string\"],[\"Valor\",\"number\"]],\"rows\":[[\"Unimed\",18500],[\"Bradesco Saude\",12300],[\"SulAmerica\",9400],[\"Particular\",5200],[\"Outros\",3100]]}",

        _ => "{}"
    };

    // ─────────────────────────────────────────────────────────
    // WORKLIST (Receptionist / RadiologyTechnician)
    // ─────────────────────────────────────────────────────────
    public record WorklistItem(
        string ExamId, string Patient, string Modality,
        string Urgency, string DicomStatus, DateTime RequestedAt);

    public static readonly WorklistItem[] Worklist =
    [
        new("EX-011", "Camila Torres",    "Radiografia",      "Rotina",    "Aguardando agendamento", new DateTime(2026,1,20,8,30,0)),
        new("EX-013", "Marcos Vieira",    "Tomografia",       "Urgente",   "Aguardando agendamento", new DateTime(2026,1,20,9,10,0)),
        new("EX-014", "Tereza Nobre",     "Ressonância",      "Critico",   "Aguardando agendamento", new DateTime(2026,1,20,9,45,0)),
        new("EX-012", "Diego Rocha",      "Tomografia",       "Rotina",    "Aguardando agendamento", new DateTime(2026,1,21,10,0,0)),
        new("EX-003", "Ana Lima",         "Ressonância",      "Urgente",   "Aguardando aquisição",   new DateTime(2026,1,16,11,0,0)),
        new("EX-004", "Carlos Pereira",   "Ultrassonografia", "Prioridade","Agendado",               new DateTime(2026,1,17,14,0,0)),
        new("EX-009", "Patrícia Alves",   "Ressonância",      "Urgente",   "Em execução",            new DateTime(2026,1,19,15,0,0)),
    ];

    // ─────────────────────────────────────────────────────────
    // RECENT PATIENTS (Receptionist)
    // ─────────────────────────────────────────────────────────
    public record RecentPatient(
        string Name, string Modality, string UrgencyLevel, DateTime RegisteredAt);

    public static readonly RecentPatient[] RecentPatients =
    [
        new("Camila Torres",    "Radiografia",      "Rotina",    new DateTime(2026,1,20,8,30,0)),
        new("Marcos Vieira",    "Tomografia",       "Urgente",   new DateTime(2026,1,20,9,10,0)),
        new("Tereza Nobre",     "Ressonância",      "Critico",   new DateTime(2026,1,20,9,45,0)),
        new("Diego Rocha",      "Tomografia",       "Rotina",    new DateTime(2026,1,21,10,0,0)),
        new("Patrícia Alves",   "Ressonância",      "Urgente",   new DateTime(2026,1,19,15,0,0)),
        new("Eduardo Martins",  "Tomografia",       "Prioridade",new DateTime(2026,1,19,8,0,0)),
        new("Luciana Ferreira", "Radiografia",      "Rotina",    new DateTime(2026,1,18,11,30,0)),
        new("Roberto Souza",    "PET-CT",           "Urgente",   new DateTime(2026,1,18,14,0,0)),
        new("Ana Lima",         "Ressonância",      "Urgente",   new DateTime(2026,1,16,9,0,0)),
        new("Carlos Pereira",   "Ultrassonografia", "Prioridade",new DateTime(2026,1,17,16,0,0)),
    ];

    // ─────────────────────────────────────────────────────────
    // CRITICAL FINDINGS (RequestingPhysician)
    // ─────────────────────────────────────────────────────────
    public record CriticalFinding(
        string ExamId, string Patient, string Modality,
        string FindingText, DateTime DetectedAt, bool IsAcknowledged);

    public static readonly CriticalFinding[] CriticalFindings =
    [
        new("EX-003", "Ana Lima",       "Ressonância",
            "Massa pulmonar com densidade aumentada — suspeita de neoplasia.",
            DateTime.UtcNow.AddHours(-2), false),
        new("EX-009", "Patrícia Alves", "Ressonância",
            "Hemorragia subaracnóide identificada na RM de crânio.",
            DateTime.UtcNow.AddHours(-5), false),
        new("EX-001", "Maria Santos",   "Radiografia",
            "Fratura de fêmur proximal confirmada.",
            DateTime.UtcNow.AddDays(-1), true),
    ];

    // ─────────────────────────────────────────────────────────
    // REPORT DRAFTS (MedicalAuxiliary / Transcriptionist)
    // ─────────────────────────────────────────────────────────
    public record ReportDraft(
        string ExamId, string Patient, string Modality,
        string ReportStatus, string AuthoredBy, DateTime LastUpdated);

    public static readonly ReportDraft[] ReportDrafts =
    [
        new("EX-002", "João Oliveira",   "Tomografia",  "InProgress", "Dra. Camila Rocha", DateTime.UtcNow.AddHours(-1)),
        new("EX-008", "Eduardo Martins", "Tomografia",  "Draft",      "Dra. Camila Rocha", DateTime.UtcNow.AddHours(-3)),
        new("EX-009", "Patrícia Alves",  "Ressonância", "Draft",      "Dra. Camila Rocha", DateTime.UtcNow.AddHours(-5)),
        new("EX-003", "Ana Lima",        "Ressonância", "InProgress", "João Batista",       DateTime.UtcNow.AddHours(-2)),
        new("EX-007", "Luciana Ferreira","Radiografia", "Draft",      "João Batista",       DateTime.UtcNow.AddHours(-4)),
    ];

    // ─────────────────────────────────────────────────────────
    // BILLING BATCHES (HospitalFinancial)
    // ─────────────────────────────────────────────────────────
    public record BillingBatch(
        string BatchId, string Period, int ExamCount,
        decimal GrossValue, decimal NetValue, string BatchStatus);

    public static readonly BillingBatch[] BillingBatches =
    [
        new("LOTE-2026-01", "Janeiro/2026",   142, 48_500m, 42_300m, "Open"),
        new("LOTE-2025-12", "Dezembro/2025",  138, 45_200m, 39_800m, "Submitted"),
        new("LOTE-2025-11", "Novembro/2025",  125, 41_300m, 36_100m, "Paid"),
        new("LOTE-2025-10", "Outubro/2025",   130, 43_100m, 37_900m, "Paid"),
        new("LOTE-2025-09", "Setembro/2025",  118, 38_900m, 33_700m, "Paid"),
    ];

    // ─────────────────────────────────────────────────────────
    // RADIOLOGIST PRODUCTIVITY (HospitalManager)
    // ─────────────────────────────────────────────────────────
    public record RadiologistProductivity(
        string Name, int ReportsIssued, int AvgTatMinutes, string Availability);

    public static readonly RadiologistProductivity[] RadiologistStats =
    [
        new("Dr. Paulo Ferreira",  58, 42, "Ativo"),
        new("Dra. Camila Rocha",   54, 38, "Ativo"),
        new("Dr. Ricardo Nunes",   41, 55, "Ativo"),
        new("Dra. Sofia Mendes",   29, 61, "Férias"),
    ];

    // ─────────────────────────────────────────────────────────
    // RECEPTION — Rich exam list (Receptionist screen)
    // ─────────────────────────────────────────────────────────

    public enum ExamReportStatus
    {
        AwaitingRelease = 1,
        AvailableForReporting = 2,
        Reporting = 3,
        Pending = 4,
        AwaitingSignature = 5,
        Approved = 6,
        RevisionRequested = 7,
        Reviewing = 8,
        ApprovedAfterRevision = 9,
        Cancelled = 10,
    }

    public enum ExamType
    {
        Emergency = 1,   // PS (Pronto Socorro)
        Elective = 2,    // Agenda / Eletivo
        Inpatient = 3,   // Internação
    }

    public record ReceptionExam(
        string StudyId,                 // friendly study id (not Study UID)
        string PatientId,
        string PatientName,
        string? SocialName,
        string Sex,                     // M / F / O
        DateTime BirthDateTime,         // includes time for newborns
        string Cpf,
        string? Email,
        string Modality,
        string Procedure,
        DateTime StudyDateTime,
        DateTime? ReportDateTime,
        string TechnicianName,          // released the exam
        string? ExecutingPhysician,
        string RequestingPhysician,
        ExamReportStatus Status,
        ExamType Type,
        bool IsEmergency,
        string Unit,
        DateTime? LiberationDateTime,
        string? CriticalFinding,        // null | "unnotified" | "notified"
        bool HasPendency,
        int ImageCount,                 // DICOM image count
        int AttachmentCount,            // attached documents count
        string? DownloadAccessCode,     // 6-digit code for patient portal
        bool IsArchived                 // exam in cold storage; recovery required
    );

    private static readonly DateTime _now = new(2026, 1, 29, 10, 30, 0);

    public static readonly ReceptionExam[] ReceptionExams =
    [
        new("STD-100001", "PAC-00012", "Maria Santos",       null,                       "F", new DateTime(1958,3,12,7,15,0),  "412.876.901-22", "maria.santos@email.com",  "Radiografia",     "RX Tórax PA + Perfil",        new DateTime(2026,1,29,9,10,0),   new DateTime(2026,1,29,9,55,0),  "Carlos Mendes",  "Dr. Paulo Ferreira",    "Dr. Roberto Alves",       ExamReportStatus.Approved,              ExamType.Emergency, true,  "Unidade Sé",               new DateTime(2026,1,29,9,25,0),  "notified",   false,   4,  1, "847291", false),
        new("STD-100002", "PAC-00045", "João Oliveira",      null,                       "M", new DateTime(1972,11,5,22,40,0), "298.114.503-88", "joao.olv@email.com",      "Tomografia",      "TC Crânio sem contraste",     new DateTime(2026,1,29,8,30,0),   null,                            "Carlos Mendes",  "Dra. Camila Rocha",     "Dra. Helena Prado",       ExamReportStatus.Reporting,             ExamType.Emergency, true,  "Unidade Sé",               new DateTime(2026,1,29,8,45,0),  "unnotified", false, 312,  0, null, false),
        new("STD-100003", "PAC-00078", "Ana Lima",           "Ana Beatriz",              "F", new DateTime(1989,6,21,14,5,0),  "503.227.119-04", "ana.lima@email.com",      "Ressonância",     "RM Crânio com contraste",     new DateTime(2026,1,29,7,45,0),   new DateTime(2026,1,29,9,20,0),  "Carlos Mendes",  "Dr. Paulo Ferreira",    "Dr. Marcelo Tavares",     ExamReportStatus.AwaitingSignature,     ExamType.Elective,  false, "Unidade Liberdade",       new DateTime(2026,1,29,8,0,0),   null,         true,  186,  2, null, false),
        new("STD-100004", "PAC-00091", "Carlos Pereira",     null,                       "M", new DateTime(1965,9,30,11,20,0), "118.609.224-71", null,                       "Ultrassonografia","USG Abdome Total",            new DateTime(2026,1,29,7,0,0),    null,                            "Carlos Mendes",  null,                    "Dra. Helena Prado",       ExamReportStatus.AvailableForReporting, ExamType.Elective,  false, "Unidade Tatuapé",          new DateTime(2026,1,29,7,20,0),  null,         false,  18,  0, null, false),
        new("STD-100005", "PAC-00103", "Fernanda Costa",     "Nanda Costa",              "F", new DateTime(1980,2,14,3,0,0),   "740.118.302-55", "fernanda.c@email.com",    "Mamografia",      "Mamografia bilateral",        new DateTime(2026,1,29,6,30,0),   new DateTime(2026,1,29,8,15,0),  "Patrícia Aoki",  "Dra. Camila Rocha",     "Dra. Lúcia Rangel",       ExamReportStatus.Approved,              ExamType.Elective,  false, "Unidade Pinheiros",       new DateTime(2026,1,29,6,50,0),  null,         false,   6,  1, "190348", false),
        new("STD-100006", "PAC-00114", "Roberto Souza",      null,                       "M", new DateTime(1955,7,8,17,30,0),  "228.514.770-09", "rsouza@email.com",        "PET-CT",          "PET-CT oncológico",           new DateTime(2026,1,28,16,0,0),   new DateTime(2026,1,29,7,40,0),  "Patrícia Aoki",  "Dra. Camila Rocha",     "Dr. Eduardo Nicolau",     ExamReportStatus.Approved,              ExamType.Inpatient, false, "Unidade Santana",         new DateTime(2026,1,28,16,30,0), null,         false, 524,  1, "552108", false),
        new("STD-100007", "PAC-00127", "Luciana Ferreira",   null,                       "F", new DateTime(1993,12,1,9,10,0),  "609.330.481-66", "lu.ferreira@email.com",   "Radiografia",     "RX Coluna Lombar AP+P",       new DateTime(2026,1,28,15,20,0),  null,                            "Carlos Mendes",  null,                    "Dr. Roberto Alves",       ExamReportStatus.AwaitingRelease,       ExamType.Elective,  false, "Unidade Mooca",           null,                            null,         false,   3,  0, null, false),
        new("STD-100008", "PAC-00138", "Eduardo Martins",    null,                       "M", new DateTime(1970,4,18,12,55,0), "337.815.220-43", "eduardo.m@email.com",     "Tomografia",      "TC Tórax HR",                 new DateTime(2026,1,28,14,10,0),  null,                            "Patrícia Aoki",  "Dra. Camila Rocha",     "Dra. Helena Prado",       ExamReportStatus.Pending,               ExamType.Inpatient, false, "Unidade Santo André",      new DateTime(2026,1,28,14,35,0), "unnotified", true,  278,  0, null, false),
        new("STD-100009", "PAC-00145", "Patrícia Alves",     null,                       "F", new DateTime(1986,8,25,5,40,0),  "812.460.137-78", null,                       "Ressonância",     "RM Coluna Lombar",            new DateTime(2026,1,28,11,30,0),  new DateTime(2026,1,28,18,20,0), "Carlos Mendes",  "Dr. Paulo Ferreira",    "Dr. Marcelo Tavares",     ExamReportStatus.Approved,              ExamType.Elective,  false, "Unidade Liberdade",       new DateTime(2026,1,28,11,55,0), null,         false, 220,  1, "603927", false),
        new("STD-100010", "PAC-00156", "Bruno Gomes",        null,                       "M", new DateTime(1998,1,3,19,15,0),  "445.901.668-12", "bruno.g@email.com",       "Ultrassonografia","USG Tireoide com Doppler",    new DateTime(2026,1,28,10,0,0),   new DateTime(2026,1,28,13,45,0), "Patrícia Aoki",  "Dra. Camila Rocha",     "Dra. Lúcia Rangel",       ExamReportStatus.Approved,              ExamType.Elective,  false, "Unidade Tatuapé",          new DateTime(2026,1,28,10,20,0), null,         false,  24,  0, "778214", false),
        new("STD-100011", "PAC-00163", "Camila Torres",      null,                       "F", new DateTime(2026,1,28,23,52,0), "—",              null,                       "Radiografia",     "RX Tórax neonato AP",         new DateTime(2026,1,29,2,10,0),   null,                            "Carlos Mendes",  null,                    "Dra. Helena Prado",       ExamReportStatus.AvailableForReporting, ExamType.Emergency, true,  "Unidade Sé",               new DateTime(2026,1,29,2,40,0),  "unnotified", false,   2,  0, null, false),
        new("STD-100012", "PAC-00170", "Diego Rocha",        null,                       "M", new DateTime(1962,10,12,8,0,0),  "115.804.227-31", "diego.r@email.com",       "Tomografia",      "TC Abdome com contraste",     new DateTime(2026,1,28,9,30,0),   new DateTime(2026,1,28,16,0,0),  "Carlos Mendes",  "Dr. Paulo Ferreira",    "Dr. Eduardo Nicolau",     ExamReportStatus.Approved,              ExamType.Elective,  false, "Unidade Pinheiros",       new DateTime(2026,1,28,9,55,0),  null,         false, 445,  1, "224680", false),
        new("STD-100013", "PAC-00181", "Tereza Nobre",       null,                       "F", new DateTime(1947,5,9,16,25,0),  "660.108.452-90", "tereza.n@email.com",      "Ressonância",     "RM Crânio",                   new DateTime(2026,1,27,17,40,0),  new DateTime(2026,1,28,10,30,0), "Patrícia Aoki",  "Dra. Camila Rocha",     "Dr. Marcelo Tavares",     ExamReportStatus.RevisionRequested,     ExamType.Emergency, true,  "Unidade Lapa",            new DateTime(2026,1,27,18,5,0),  "unnotified", false, 195,  2, "390712", false),
        new("STD-100014", "PAC-00198", "Marcos Vieira",      null,                       "M", new DateTime(1979,11,22,6,50,0), "228.901.665-04", "m.vieira@email.com",      "Tomografia",      "TC Crânio sem contraste",     new DateTime(2026,1,27,15,0,0),   new DateTime(2026,1,28,9,0,0),   "Carlos Mendes",  "Dr. Paulo Ferreira",    "Dra. Helena Prado",       ExamReportStatus.Reviewing,             ExamType.Emergency, true,  "Unidade Lapa",            new DateTime(2026,1,27,15,20,0), null,         false, 298,  1, null, false),
        new("STD-100015", "PAC-00204", "Helena Castro",      "Lena Castro",              "F", new DateTime(1991,3,18,10,5,0),  "734.116.880-25", "lena@email.com",          "Mamografia",      "Mamografia bilateral",        new DateTime(2026,1,27,13,15,0),  new DateTime(2026,1,27,17,30,0), "Patrícia Aoki",  "Dra. Camila Rocha",     "Dra. Lúcia Rangel",       ExamReportStatus.ApprovedAfterRevision, ExamType.Elective,  false, "Unidade Mooca",           new DateTime(2026,1,27,13,40,0), null,         false,   8,  0, "118453", true),
        new("STD-100016", "PAC-00215", "Felipe Andrade",     null,                       "M", new DateTime(1985,7,7,0,30,0),   "503.227.114-18", null,                       "Ultrassonografia","USG Obstétrica",              new DateTime(2026,1,27,11,0,0),   null,                            "Carlos Mendes",  null,                    "Dr. Roberto Alves",       ExamReportStatus.Cancelled,             ExamType.Elective,  false, "Unidade Santana",         null,                            null,         false,  14,  0, null, false),
        new("STD-100017", "PAC-00226", "Júlia Bittencourt",  null,                       "F", new DateTime(1968,12,29,4,15,0), "118.553.770-44", "ju.bit@email.com",        "Ressonância",     "RM Joelho direito",           new DateTime(2026,1,26,16,40,0),  new DateTime(2026,1,27,9,10,0),  "Carlos Mendes",  "Dr. Paulo Ferreira",    "Dr. Marcelo Tavares",     ExamReportStatus.Approved,              ExamType.Elective,  false, "Unidade Tatuapé",          new DateTime(2026,1,26,17,5,0),  null,         false, 164,  1, "445119", true),
        new("STD-100018", "PAC-00237", "André Vasconcelos",  null,                       "M", new DateTime(1959,8,3,22,0,0),   "660.812.227-33", "andre.v@email.com",       "PET-CT",          "PET-CT oncológico",           new DateTime(2026,1,26,9,30,0),   new DateTime(2026,1,27,8,0,0),   "Patrícia Aoki",  "Dra. Camila Rocha",     "Dr. Eduardo Nicolau",     ExamReportStatus.Approved,              ExamType.Inpatient, false, "Unidade Santana",         new DateTime(2026,1,26,9,55,0),  null,         false, 611,  2, "601238", false),
        new("STD-100019", "PAC-00244", "Sandra Pinheiro",    null,                       "F", new DateTime(1975,4,11,13,40,0), "445.219.088-71", null,                       "Tomografia",      "TC Tórax HR",                 new DateTime(2026,1,26,8,15,0),   null,                            "Carlos Mendes",  null,                    "Dra. Helena Prado",       ExamReportStatus.AwaitingRelease,       ExamType.Elective,  false, "Unidade Pinheiros",       null,                            null,         true,  356,  0, null, false),
        new("STD-100020", "PAC-00255", "Vinicius Prado",     null,                       "M", new DateTime(1990,2,28,7,30,0),  "734.660.119-08", "v.prado@email.com",       "Radiografia",     "RX Cotovelo direito",         new DateTime(2026,1,26,7,0,0),    new DateTime(2026,1,26,10,40,0), "Patrícia Aoki",  "Dr. Paulo Ferreira",    "Dr. Roberto Alves",       ExamReportStatus.Approved,              ExamType.Emergency, true,  "Unidade Santo André",      new DateTime(2026,1,26,7,20,0),  "notified",   false,   5,  0, "229017", false),
        new("STD-100021", "PAC-00263", "Beatriz Moraes",     null,                       "F", new DateTime(2024,9,15,11,22,0), "—",              null,                       "Ultrassonografia","USG Quadril neonatal",        new DateTime(2026,1,25,14,30,0),  new DateTime(2026,1,25,18,0,0),  "Carlos Mendes",  "Dra. Camila Rocha",     "Dra. Lúcia Rangel",       ExamReportStatus.Approved,              ExamType.Elective,  false, "Unidade Liberdade",       new DateTime(2026,1,25,15,5,0),  null,         false,  22,  0, "778902", true),
        new("STD-100022", "PAC-00274", "Renato Macedo",      null,                       "M", new DateTime(1953,6,6,18,45,0),  "118.227.553-90", "r.macedo@email.com",      "Ressonância",     "RM Coluna Cervical",          new DateTime(2026,1,25,11,0,0),   null,                            "Patrícia Aoki",  "Dra. Camila Rocha",     "Dr. Marcelo Tavares",     ExamReportStatus.Pending,               ExamType.Inpatient, false, "Unidade Santo André",      new DateTime(2026,1,25,11,30,0), null,         true,  142,  0, null, false),
        new("STD-100023", "PAC-00285", "Larissa Duarte",     "Lari Duarte",              "F", new DateTime(1996,10,17,21,10,0),"660.445.901-22", "lari.d@email.com",        "Mamografia",      "Mamografia bilateral",        new DateTime(2026,1,25,9,30,0),   new DateTime(2026,1,25,15,40,0), "Carlos Mendes",  "Dr. Paulo Ferreira",    "Dra. Lúcia Rangel",       ExamReportStatus.Approved,              ExamType.Elective,  false, "Unidade Mooca",           new DateTime(2026,1,25,10,0,0),  null,         false,   6,  1, "334871", false),
        new("STD-100024", "PAC-00296", "Otávio Brandão",     null,                       "M", new DateTime(1981,1,4,15,5,0),   "503.660.227-77", null,                       "Tomografia",      "TC Crânio sem contraste",     new DateTime(2026,1,24,17,20,0),  new DateTime(2026,1,25,8,30,0),  "Patrícia Aoki",  "Dra. Camila Rocha",     "Dra. Helena Prado",       ExamReportStatus.ApprovedAfterRevision, ExamType.Elective,  false, "Unidade Tatuapé",          new DateTime(2026,1,24,17,45,0), null,         false, 387,  0, "990116", true),
        new("STD-100025", "PAC-00307", "Mariana Lobo",       null,                       "F", new DateTime(1969,11,11,2,55,0), "118.901.227-13", "m.lobo@email.com",        "Ultrassonografia","USG Tireoide",                new DateTime(2026,1,24,10,40,0),  null,                            "Carlos Mendes",  null,                    "Dra. Lúcia Rangel",       ExamReportStatus.AvailableForReporting, ExamType.Elective,  false, "Unidade Lapa",            new DateTime(2026,1,24,11,5,0),  null,         false,  16,  0, null, false),
        new("STD-100026", "PAC-00318", "Henrique Soares",    null,                       "M", new DateTime(1944,7,29,8,10,0),  "445.660.118-66", "h.soares@email.com",      "PET-CT",          "PET-CT oncológico",           new DateTime(2026,1,24,8,0,0),    new DateTime(2026,1,25,11,15,0), "Patrícia Aoki",  "Dra. Camila Rocha",     "Dr. Eduardo Nicolau",     ExamReportStatus.Approved,              ExamType.Inpatient, false, "Unidade Santana",         new DateTime(2026,1,24,8,25,0),  null,         false, 492,  1, "612507", false),
    ];

    /// <summary>
    /// Receptionist-friendly status label.
    /// AwaitingSignature is hidden from receptionist — shown as "Laudando".
    /// </summary>
    public static string GetReceptionStatusLabel(ExamReportStatus status) => status switch
    {
        ExamReportStatus.AwaitingRelease => "Liberação",
        ExamReportStatus.AvailableForReporting => "Disponível",
        ExamReportStatus.Reporting => "Laudando",
        ExamReportStatus.Pending => "Pendência",
        ExamReportStatus.AwaitingSignature => "Assinatura",
        ExamReportStatus.Approved => "Aprovado",
        ExamReportStatus.RevisionRequested => "Revisão",
        ExamReportStatus.Reviewing => "Laudando",
        ExamReportStatus.ApprovedAfterRevision => "Aprovado",
        ExamReportStatus.Cancelled => "Cancelado",
        _ => status.ToString(),
    };

    /// <summary>Maps status to the <c>.badge-status</c> CSS modifier used in hospital.css.</summary>
    public static string GetStatusBadgeCss(ExamReportStatus status) => status switch
    {
        ExamReportStatus.AwaitingRelease => "release",
        ExamReportStatus.AvailableForReporting => "available",
        ExamReportStatus.Reporting => "reporting",
        ExamReportStatus.Pending => "pendency",
        ExamReportStatus.AwaitingSignature => "signature",
        ExamReportStatus.Approved => "approved",
        ExamReportStatus.RevisionRequested => "revision",
        ExamReportStatus.Reviewing => "reporting",
        ExamReportStatus.ApprovedAfterRevision => "approved",
        ExamReportStatus.Cancelled => "cancelled",
        _ => "available",
    };

    public static string GetExamTypeLabel(ExamType type) => type switch
    {
        ExamType.Emergency => "PS",
        ExamType.Elective => "Eletivo",
        ExamType.Inpatient => "Int.",
        _ => type.ToString(),
    };

    /// <summary>
    /// Formats age as DICOM AS (Age String) value representation: <c>nnn[Y|M|W|D]</c>.
    /// Example: 045Y, 011M, 002W, 005D.
    /// </summary>
    public static string FormatDicomAge(DateTime birth, DateTime reference)
    {
        var totalDays = (reference.Date - birth.Date).TotalDays;
        if (totalDays < 0) totalDays = 0;

        if (totalDays < 7)
            return $"{(int)totalDays:D3}D";

        var years = reference.Year - birth.Year;
        if (birth.Date > reference.Date.AddYears(-years)) years--;
        if (years >= 1)
            return $"{years:D3}Y";

        var months = (reference.Year - birth.Year) * 12 + (reference.Month - birth.Month);
        if (reference.Day < birth.Day) months--;
        if (months >= 1)
            return $"{months:D3}M";

        return $"{(int)(totalDays / 7):D3}W";
    }

    /// <summary>Reference "now" used by FormatDicomAge in views (kept fixed for deterministic wireframe display).</summary>
    public static DateTime ReceptionNow => _now;

    // ─────────────────────────────────────────────────────────
    // RECEPTION — Editable extra fields (deterministic mock)
    // ─────────────────────────────────────────────────────────

    private static readonly string[] _motherFirstNames =
        ["Maria", "Ana", "Joana", "Teresa", "Lúcia", "Rosa", "Cláudia", "Sônia", "Vera", "Cristina"];
    private static readonly string[] _motherLastNames =
        ["Santos", "Oliveira", "Lima", "Pereira", "Costa", "Souza", "Ferreira", "Alves", "Gomes", "Rocha"];

    /// <summary>Deterministic mock phone number derived from the patient id.</summary>
    public static string GetMockPhone(string patientId)
    {
        var seed = Math.Abs(patientId.GetHashCode());
        var ddd = 11 + seed % 89;
        var part1 = 90000 + seed / 7 % 10000;
        var part2 = 1000 + seed / 13 % 9000;
        return $"({ddd}) 9{part1:D4}-{part2:D4}";
    }

    /// <summary>True when a patient-portal download code has been generated for the exam.</summary>
    public static bool WasDownloadCodeGenerated(ReceptionExam e) =>
        !string.IsNullOrEmpty(e.DownloadAccessCode);

    /// <summary>Deterministic mock: whether the patient already downloaded the report(s).</summary>
    public static bool WasReportDownloaded(ReceptionExam e)
    {
        if (!WasDownloadCodeGenerated(e))
            return false;
        var seed = Math.Abs((e.PatientId + e.StudyId).GetHashCode());
        return seed % 5 != 0; // ~80% of generated codes were already used by the patient
    }

    /// <summary>Deterministic mock mother's name derived from the patient id.</summary>
    public static string GetMockMotherName(string patientId)
    {
        var seed = Math.Abs(patientId.GetHashCode());
        var first = _motherFirstNames[seed % _motherFirstNames.Length];
        var last = _motherLastNames[seed / 3 % _motherLastNames.Length];
        return $"{first} {last}";
    }

    // ─────────────────────────────────────────────────────────
    // PATIENT CHANGE HISTORY (Tab "Histórico de alteração")
    // ─────────────────────────────────────────────────────────
    public record PatientChangeLog(
        string UserName, string UserRole, string Unit, DateTime At, string Summary);

    /// <summary>Deterministic mock change history for a given patient.</summary>
    public static PatientChangeLog[] GetChangeLogs(string patientId)
    {
        var seed = Math.Abs(patientId.GetHashCode());
        var count = 2 + seed % 4; // 2..5 entries
        var users = new (string Name, string Role, string Unit)[]
        {
            ("Carlos Mendes",   "Recepcionista",          "Unidade Sé"),
            ("Patrícia Aoki",   "Técnica de Radiologia",  "Unidade Pinheiros"),
            ("Dra. Camila Rocha","Médica Radiologista",   "Unidade Liberdade"),
            ("Renata Lopes",    "Supervisora",            "Unidade Tatuapé"),
            ("Bruno Tavares",   "Recepcionista",          "Unidade Mooca"),
        };
        var summaries = new[]
        {
            "Atualizou o nome social do paciente.",
            "Corrigiu o CPF do paciente.",
            "Alterou o e-mail de contato.",
            "Ajustou a data e hora do estudo.",
            "Atualizou o telefone de contato.",
            "Corrigiu o nome da mãe.",
            "Gerou novo código de download do laudo.",
        };
        var list = new List<PatientChangeLog>();
        for (var i = 0; i < count; i++)
        {
            var u = users[(seed + i) % users.Length];
            var s = summaries[(seed / 2 + i) % summaries.Length];
            var at = _now.AddDays(-(i + 1)).AddHours(-(seed % 9)).AddMinutes(-(seed % 47));
            list.Add(new PatientChangeLog(u.Name, u.Role, u.Unit, at, s));
        }
        return list.ToArray();
    }

    // ─────────────────────────────────────────────────────────
    // DOWNLOAD — Reports, attachments and prior exams
    // ─────────────────────────────────────────────────────────
    public record ExamReportFile(string Title, bool IsCold);

    public record ExamAttachment(
        string FileName, DateTime UploadedAt, long SizeBytes, string UploadedBy, bool IsCold);

    public record PriorExamEntry(
        string StudyId, string AccessNumber, DateTime When, string Title, bool IsCold,
        ExamAttachment[] Attachments);

    /// <summary>Formats a byte count as B / KB / MB (pt-BR friendly).</summary>
    public static string FormatFileSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:0.#} KB".Replace('.', ',');
        return $"{bytes / (1024.0 * 1024.0):0.#} MB".Replace('.', ',');
    }

    private static string ExtOf(string fileName)
    {
        var ext = System.IO.Path.GetExtension(fileName).TrimStart('.').ToUpperInvariant();
        return string.IsNullOrEmpty(ext) ? "Arquivo" : ext;
    }

    public static string AttachmentTypeLabel(string fileName) => ExtOf(fileName) switch
    {
        "PDF" => "PDF",
        "JPG" or "JPEG" or "PNG" => "Imagem",
        "DOC" or "DOCX" => "Documento",
        "DCM" => "DICOM",
        "ZIP" => "Compactado",
        _ => ExtOf(fileName),
    };

    /// <summary>Mock list of report files generated by an exam (an exam may produce 1..N reports).</summary>
    public static ExamReportFile[] GetExamReports(ReceptionExam exam)
    {
        var seed = Math.Abs(exam.StudyId.GetHashCode());
        var n = 1 + seed % 2; // 1..2 reports
        var list = new List<ExamReportFile>
        {
            new($"Laudo — {exam.Procedure}", exam.IsArchived),
        };
        if (n > 1)
            list.Add(new ExamReportFile($"Laudo complementar — {exam.Modality}", exam.IsArchived));
        return list.ToArray();
    }

    /// <summary>Mock list of files attached to an exam by the radiology technician.</summary>
    public static ExamAttachment[] GetAttachments(ReceptionExam exam)
    {
        if (exam.AttachmentCount <= 0) return [];
        var seed = Math.Abs(exam.StudyId.GetHashCode());
        var names = new[] { "termo_consentimento.pdf", "guia_convenio.jpg", "pedido_medico.pdf", "exame_anterior.dcm", "relatorio.docx" };
        var people = new[] { "Carlos Mendes", "Patrícia Aoki", "Bruno Tavares" };
        var list = new List<ExamAttachment>();
        for (var i = 0; i < exam.AttachmentCount; i++)
        {
            var name = names[(seed + i) % names.Length];
            var size = 24_000L + (seed * (i + 3) % 4_500_000);
            var who = people[(seed + i) % people.Length];
            var at = exam.StudyDateTime.AddMinutes(15 * (i + 1));
            list.Add(new ExamAttachment(name, at, size, who, exam.IsArchived));
        }
        return list.ToArray();
    }

    /// <summary>Mock list of prior exams of the same patient.</summary>
    public static PriorExamEntry[] GetPriorExams(ReceptionExam exam)
    {
        var seed = Math.Abs(exam.PatientId.GetHashCode());
        var n = 2 + seed % 2; // 2..3 prior exams (sempre preenchido)
        var titles = new[] { "RX Tórax PA", "TC Crânio", "USG Abdome", "RM Coluna", "Mamografia bilateral" };
        var people = new[] { "Carlos Mendes", "Patrícia Aoki" };
        var list = new List<PriorExamEntry>();
        for (var i = 0; i < n; i++)
        {
            var when = exam.StudyDateTime.AddMonths(-(i + 1) * 3).AddDays(-(seed % 20));
            var title = titles[(seed + i) % titles.Length];
            var cold = (seed + i) % 2 == 0;
            var studyId = $"STD-0{90000 + (seed + i) % 9000}";
            var an = $"{100000 + (seed * (i + 2)) % 900000}";
            var atts = new List<ExamAttachment>();
            var attCount = 1 + (seed + i) % 2; // 1..2 anexos por exame anterior
            var names = new[] { "laudo_anterior.pdf", "guia_convenio.jpg", "termo_consentimento.pdf", "comparativo.dcm" };
            for (var a = 0; a < attCount; a++)
            {
                var size = 18_000L + (seed * (a + 2) % 3_000_000);
                atts.Add(new ExamAttachment(names[(seed + a + i) % names.Length], when.AddMinutes(10 * (a + 1)),
                    size, people[(seed + a) % people.Length], cold));
            }
            list.Add(new PriorExamEntry(studyId, an, when, title, cold, atts.ToArray()));
        }
        return list.ToArray();
    }

    // ─────────────────────────────────────────────────────────
    // RECEPTION — Critical findings / pendency / archive / downloads detail
    // (deterministic mock used by the clickable column-1 icon modals)
    // ─────────────────────────────────────────────────────────
    private static readonly (string Name, string Crm, string Uf)[] _radiologists =
    [
        ("Dr. Paulo Ferreira", "CRM 112847", "SP"),
        ("Dra. Camila Rocha",  "CRM 98233",  "SP"),
        ("Dr. Ricardo Nunes",  "CRM 145902", "SP"),
        ("Dra. Sofia Mendes",  "CRM 76110",  "RJ"),
    ];

    private static readonly string[] _criticalMessages =
    [
        "Achado compatível com hemorragia subaracnóidea aguda. Necessária avaliação neurocirúrgica imediata.",
        "Massa pulmonar espiculada no lobo superior direito, suspeita de neoplasia. Encaminhar com urgência.",
        "Sinais de tromboembolismo pulmonar central bilateral. Contatar a equipe assistente com prioridade.",
        "Fratura-luxação cervical instável. Imobilização e avaliação ortopédica urgentes.",
        "Aneurisma de aorta abdominal com sinais de rotura iminente. Acionar cirurgia vascular.",
        "Apendicite aguda com sinais de perfuração. Avaliação cirúrgica imediata recomendada.",
    ];

    public record CriticalDetail(
        string State, DateTime DetectedAt, string Physician, string Crm, string Uf, string Message,
        string? ResolvedBy, string? ResolvedRole, DateTime? ResolvedAt, string? ContactNote);

    /// <summary>Deterministic mock detail for the critical-finding icon modals.</summary>
    public static CriticalDetail? GetCriticalDetail(ReceptionExam e)
    {
        if (string.IsNullOrEmpty(e.CriticalFinding)) return null;
        var seed = Math.Abs((e.StudyId + "crit").GetHashCode());
        var rad = _radiologists[seed % _radiologists.Length];
        var msg = _criticalMessages[seed % _criticalMessages.Length];
        var detectedAt = (e.ReportDateTime ?? e.StudyDateTime).AddMinutes(-(15 + seed % 90));

        if (e.CriticalFinding == "notified")
        {
            var resolvers = new (string Name, string Role)[]
            {
                ("Carlos Mendes", "Recepção"),
                ("Patrícia Aoki", "Técnico(a) de Radiologia"),
            };
            var r = resolvers[seed % resolvers.Length];
            var notes = new[]
            {
                "Paciente contatado por telefone e orientado a retornar imediatamente ao pronto-socorro.",
                "Contato realizado com a médica assistente, que assumiu a conduta do caso.",
                "Responsável pelo paciente informado; transporte para o hospital acionado.",
            };
            var note = notes[seed % notes.Length];
            var resolvedAt = detectedAt.AddMinutes(20 + seed % 70);
            return new CriticalDetail("notified", detectedAt, rad.Name, rad.Crm, rad.Uf, msg,
                r.Name, r.Role, resolvedAt, note);
        }

        return new CriticalDetail("unnotified", detectedAt, rad.Name, rad.Crm, rad.Uf, msg,
            null, null, null, null);
    }

    private static readonly string[] _pendencyTexts =
    [
        "Faltou a guia de autorização do convênio. Favor anexar o documento para liberação do laudo.",
        "Exame anterior comparativo não localizado no sistema. Solicito anexar para análise evolutiva.",
        "Dados clínicos insuficientes na requisição. Necessário informar a indicação clínica detalhada.",
        "Imagens com artefato de movimento. Avaliar a necessidade de reaquisição de algumas séries.",
        "Termo de consentimento do contraste não consta no prontuário. Anexar antes da assinatura.",
    ];

    public record PendencyDetail(
        string Physician, string Crm, string Uf, DateTime OpenedAt, string Text);

    /// <summary>Deterministic mock detail for the pendency icon modal.</summary>
    public static PendencyDetail? GetPendencyDetail(ReceptionExam e)
    {
        if (!e.HasPendency) return null;
        var seed = Math.Abs((e.StudyId + "pend").GetHashCode());
        var rad = _radiologists[seed % _radiologists.Length];
        var text = _pendencyTexts[seed % _pendencyTexts.Length];
        var openedAt = (e.LiberationDateTime ?? e.StudyDateTime).AddMinutes(30 + seed % 180);
        return new PendencyDetail(rad.Name, rad.Crm, rad.Uf, openedAt, text);
    }

    public record ArchivedItem(string Name, string Kind, string Size, DateTime ArchivedAt);

    /// <summary>Deterministic mock list of archived (cold-storage) items: reports, DICOM images and documents.</summary>
    public static ArchivedItem[] GetArchivedItems(ReceptionExam e)
    {
        if (!e.IsArchived) return [];
        var seed = Math.Abs((e.StudyId + "arch").GetHashCode());
        var archivedAt = e.StudyDateTime.AddDays(180 + seed % 540);
        var list = new List<ArchivedItem>
        {
            new($"Laudo assinado — {e.Procedure}.pdf", "Laudo", FormatFileSize(180_000 + seed % 90_000), archivedAt),
            new($"Série DICOM — {e.ImageCount} imagens", "Imagem DICOM", FormatFileSize((long)e.ImageCount * 524_288L), archivedAt),
        };
        if (e.AttachmentCount > 0)
            list.Add(new("Documentos anexados.zip", "Documento", FormatFileSize(240_000 + seed % 300_000), archivedAt));
        return list.ToArray();
    }

    public record DownloadEvent(DateTime At, string Device, string IpAddress);

    /// <summary>Deterministic mock list of patient report-download events.</summary>
    public static DownloadEvent[] GetDownloadEvents(ReceptionExam e)
    {
        if (!WasReportDownloaded(e)) return [];
        var seed = Math.Abs((e.StudyId + "dl").GetHashCode());
        var count = 1 + seed % 3; // 1..3 downloads
        var devices = new[]
        {
            "Android · App HealPaciente",
            "iPhone · Safari",
            "Windows · Chrome",
            "macOS · Safari",
        };
        var baseAt = (e.ReportDateTime ?? e.StudyDateTime).AddHours(2 + seed % 30);
        var list = new List<DownloadEvent>();
        for (var i = 0; i < count; i++)
        {
            var at = baseAt.AddHours(i * (6 + seed % 12)).AddMinutes(seed % 50);
            var dev = devices[(seed + i) % devices.Length];
            var ip = $"189.{seed % 200}.{seed / 3 % 200}.{seed / 7 % 200}";
            list.Add(new DownloadEvent(at, dev, ip));
        }
        return list.ToArray();
    }

    /// <summary>Deterministic mock timestamp for when the patient-portal download code was generated.</summary>
    public static DateTime? GetCodeGeneratedAt(ReceptionExam e)
    {
        if (!WasDownloadCodeGenerated(e)) return null;
        var seed = Math.Abs((e.StudyId + "code").GetHashCode());
        return (e.ReportDateTime ?? e.StudyDateTime).AddMinutes(10 + seed % 120);
    }
}
