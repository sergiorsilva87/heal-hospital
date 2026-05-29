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
        ExamReportStatus.AwaitingRelease => "scheduled",
        ExamReportStatus.AvailableForReporting => "scheduled",
        ExamReportStatus.Reporting => "in-progress",
        ExamReportStatus.Pending => "pending",
        ExamReportStatus.AwaitingSignature => "in-progress",
        ExamReportStatus.Approved => "issued",
        ExamReportStatus.RevisionRequested => "pending",
        ExamReportStatus.Reviewing => "in-progress",
        ExamReportStatus.ApprovedAfterRevision => "issued",
        ExamReportStatus.Cancelled => "cancelled",
        _ => "scheduled",
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
}
