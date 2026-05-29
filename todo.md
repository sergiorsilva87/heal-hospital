# heal-hospital — Todo (36 Bounded Contexts)

Wireframe coverage tracker + backend feature backlog. Each BC needs: wireframe (list, detail/form, RBAC) and a full backend implementation (CQRS, events, integrations, infrastructure).

Legend: ✅ Done | 🚧 In Progress | ⬜ Not Started

---

## BC-COR — Core Radiology

| BC         | Description                                                       | Wireframe | Backend |
| ---------- | ----------------------------------------------------------------- | --------- | ------- |
| BC-COR-REP | Reporting — Laudo médico, transcrição, assinatura digital         | ⬜        | ⬜      |
| BC-COR-IMG | Imaging — Visualização DICOM, manipulação de imagens              | ⬜        | ⬜      |
| BC-COR-WRK | Worklist — Fila de exames por modalidade/turno                    | ⬜        | ⬜      |
| BC-COR-MOD | Modality Orchestration — Comunicação com equipamentos (DICOM MWL) | ⬜        | ⬜      |
| BC-COR-AIA | AI Assistance — Detecção automática, sugestões de laudo           | ⬜        | ⬜      |
| BC-COR-BIL | Medical Billing — Faturamento de exames, TUSS, glosa              | ⬜        | ⬜      |

### Backend Backlog — BC-COR

#### BC-COR-REP · ReportingContext `WhiteMage.Domain.Reporting`

- **Commands:** `CreateReport` · `OpenReport` · `UpdateReportSections` (auto-save) · `RequestAiDraft` · `AcceptAiDraft` · `SignReport` (ICP-Brasil A3 + TSA carimbo) · `CoSignReport` · `RectifyReport` · `NotifyCriticalFinding` · `CreateReportTemplate` · `PublishReportTemplate`
- **Queries:** `GetReportById` · `GetReportByExamId` · `GetPendingReports` · `GetReportHistory` (retificações) · `GetReportTemplates` · `GetSismamaExportData` · `GetContractualMonthlyReport`
- **Events:** `ReportPublished` → Delivery · Notification · Billing · Analytics · Audit; `ReportRectified` → Delivery · Notification; `CriticalFindingNotified` → Collaboration · Audit; `AiDraftAccepted/Rejected` → Audit
- **Integrations:** ICP-Brasil CFM 2.314/2022 · TSA (NGS2) · FHIR R4 DiagnosticReport · HL7 v2 ORU/R01 · DICOM WADO-RS · NCalc (fórmulas) · SISMAMA/SISCAN (BI-RADS mamografia)
- **Infra:** PostgreSQL (tenant-isolated) · S3/MinIO (PDF assinado) · RabbitMQ · ICP-Brasil CA

#### BC-COR-IMG · ImagingContext `WhiteMage.Domain.Imaging`

- **Commands:** `IngestStudy` (DICOM C-STORE SCP) · `CompleteStudyIngestion` · `ArchiveStudy` (Hot→Warm→Cold) · `RestoreStudy` · `LinkPriorStudies` · `DeleteStudy` (retenção CFM 5 anos) · `UpdateDicomTags` · `RegisterStudySize`
- **Queries:** `QidoStudies` · `WadoRetrieveStudy/Series/Instance` · `GetStudyMetadata` · `GetPriorStudies` · `GetStorageSnapshot` · `SearchStudies`
- **Events:** `StudyReceived` → Worklist · Analytics; `StudyStored` → Reporting; `StudyArchived` → StorageBilling; `StudySizeMeasured` · `StudyDeleted`
- **Integrations:** DICOM C-STORE DIMSE · DICOM STOW-RS / WADO-RS / QIDO-RS · Orthanc EdgeGateway · dcm4chee
- **Infra:** PostgreSQL (tenant-isolated) · S3/MinIO (tiering Hot/Warm/Cold) · RabbitMQ · fo-dicom · DICOM SCP porta 11112

#### BC-COR-WRK · WorklistContext `WhiteMage.Domain.Worklist`

- **Commands:** `CreateExam` · `ScheduleExam` · `MarkExamAcquired` · `MarkExamReadyForReport` · `AssignExamToRadiologist` · `AutoRouteExam` · `StartReport` · `CompleteExam` · `CancelExam` · `RescheduleExam` · `UpdateSlaStatus`
- **Queries:** `GetWorklist` · `GetExamById` · `GetExamByAccessionNumber` · `GetMwlResponse` · `GetSlaReport` · `GetContractualVolumeReport` · `GetRadiologistProductivity` · `GetPendingExamsCount`
- **Events:** `ExamCreated/Scheduled/Acquired/ReadyForReport/Completed/Cancelled` · `ExamAssigned` · `SlaAtRisk` / `SlaBreached` → Notification · Exception
- **Integrations:** HL7 v2 ORM^O01/OML^O21 · DICOM MWL SCP (C-FIND) · DICOM DIMSE
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · Redis (worklist cache) · DICOM MWL SCP · Hangfire (SLA monitoring job)

#### BC-COR-MOD · ModalityOrchestrationContext `WhiteMage.Domain.ModalityOrchestration`

- **Commands:** `RegisterDicomNode` · `UpdateDicomNode` · `DeactivateDicomNode` · `RecordMppsInProgress` · `RecordMppsCompleted` · `RecordMppsDiscontinued` · `RecordEcho`
- **Queries:** `GetMwlItems` · `GetDicomNodes` · `GetMppsHistory` · `GetNodeConnectivityStatus`
- **Events:** `DicomNodeRegistered` · `ExamAcquisitionStarted/Completed/Discontinued` → Worklist · Audit
- **Integrations:** DICOM C-FIND DIMSE (MWL SCP) · DICOM N-CREATE/N-SET DIMSE (MPPS SCP) · DICOM C-STORE DIMSE · DICOM C-ECHO DIMSE
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · fo-dicom DicomServer · Porta 11112 (DICOM padrão) · 2762 (TLS)

#### BC-COR-AIA · AIAssistanceContext `WhiteMage.Domain.AIAssistance`

- **Commands:** `RequestReportDraft` · `RequestVoiceTranscription` · `RequestPriorSummary` · `RequestCadAnalysis` · `AcceptAiSuggestion` · `RejectAiSuggestion` · `EditAiSuggestion`
- **Queries:** `GetAiSuggestionsByReport` · `GetAiUsageReport` · `GetPendingAiSuggestions`
- **Events:** `AiSuggestionGenerated` → Reporting; `AiSuggestionAccepted/Rejected/Edited` → Audit · StorageBilling
- **Integrations:** Azure OpenAI (GPT-4o) · NVIDIA Clara · Whisper ASR (ditado por voz) · DICOM WADO-RS (análise de imagens)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · Azure OpenAI API · NVIDIA NGC API · Polly (circuit breaker LLM)

#### BC-COR-BIL · MedicalBillingContext `WhiteMage.Domain.MedicalBilling`

- **Commands:** `CreateBillingEntry` · `ApplyGloss` · `ContestGloss` · `ResolveGloss` · `CloseBatch` · `GenerateBpaFile` · `GenerateTissFile` · `MarkBatchSubmitted` · `SettleRadiologistPayment` · `CreatePriceTable`
- **Queries:** `GetBillingEntriesByMonth` · `GetRadiologistStatement` · `GetMonthlyBatchSummary` · `GetGlossReport` · `GetRevenueReport` · `GetProcedurePrice`
- **Events:** `BillingEntryCreated` · `GlossApplied/Resolved` · `MonthlyBatchClosed` · `BpaFileGenerated` · `RadiologistSettled` → Notification
- **Integrations:** TISS 3.x (ANS) · BPA-I (DATASUS/SUS) · TUSS (códigos de procedimento) · Asaas/Juno (pagamentos)
- **Infra:** PostgreSQL (tenant-isolated) · S3/MinIO (arquivos BPA/TISS) · RabbitMQ · Hangfire (batch closing + BPA generation jobs)

---

## BC-GEN — General / Platform Services

| BC         | Description                                         | Wireframe | Backend |
| ---------- | --------------------------------------------------- | --------- | ------- |
| BC-GEN-NOT | Notification — Email, SMS, push, webhook            | ⬜        | ⬜      |
| BC-GEN-INT | Integration — HL7 FHIR, conectores externos         | ⬜        | ⬜      |
| BC-GEN-ANL | Analytics — Relatórios gerenciais, dashboards BI    | ⬜        | ⬜      |
| BC-GEN-API | API Management — Gateway, chaves de API, rate limit | ⬜        | ⬜      |
| BC-GEN-STB | Storage Billing — Custo de armazenamento de imagens | ⬜        | ⬜      |
| BC-GEN-SIT | Site — Portal público, landing page institucional   | ⬜        | ⬜      |

### Backend Backlog — BC-GEN

#### BC-GEN-NOT · NotificationContext

- **Commands:** `SendNotification` · `RetryFailedNotification` · `SuppressNotification` · `RecordDeliveryStatus` · `CreateNotificationTemplate`
- **Queries:** `GetNotificationHistory` · `GetFailedNotifications` · `GetNotificationTemplates` · `GetDeliveryStats`
- **Events consumed:** `ReportPublished` · `CriticalFindingNotified` · `SlaAtRisk` · `SlaBreached`
- **Events published:** `NotificationSent` · `NotificationFailed` → Audit
- **Integrations:** SendGrid SMTP · Twilio SMS · Firebase FCM (push) · Twilio WhatsApp Business API · SignalR (in-app)
- **Infra:** PostgreSQL (shared) · RabbitMQ (consumer) · Redis (template cache) · Hangfire (retry jobs)

#### BC-GEN-INT · IntegrationContext

- **Commands:** `ProcessInboundHl7Message` · `ProcessInboundFhirResource` · `SendOutboundHl7Message` · `SendOutboundFhirResource` · `SendWebhook` · `RetryFailedMessage` · `RegisterIntegrationEndpoint`
- **Queries:** `GetIntegrationLogs` · `GetFailedMessages` · `GetIntegrationEndpoints`
- **Events:** `Hl7MessageProcessed` · `IntegrationFailed` · `WebhookSent`
- **Endpoints:** `POST /fhir/*` (FHIR R4 REST) · MLLP server porta 2575 (HL7 v2)
- **Integrations:** HL7 v2 ORM^O01/OML^O21 · HL7 v2 ADT · HL7 v2 ORU^R01 · FHIR R4 REST · DICOM STOW-RS
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · Worker.Hl7 BackgroundService · NHapi 3.x · Firely SDK (FHIR validation)

#### BC-GEN-ANL · AnalyticsContext

- **Commands:** (somente read model — consome eventos de todos os BCs)
- **Queries:** `GetExamVolumeReport` · `GetTatDistribution` · `GetSlaCompliance` · `GetRadiologistProductivity` · `GetQualityIndicators` · `GetBillingOverview` · `GetPlatformOverview` · `GetAiUsageStats`
- **Events consumed:** Fanout de todos os BCs (projeções read model)
- **Integrations:** Framework Donabedian (indicadores de qualidade)
- **Infra:** PostgreSQL (shared, particionado por data/mês) · RabbitMQ (consumer) · Read model projections (tabelas desnormalizadas)

#### BC-GEN-API · ApiManagementContext

- **Commands:** `CreateApiKey` · `RotateApiKey` · `RevokeApiKey` · `UpdateRateLimit` · `RecordApiUsage`
- **Queries:** `GetApiKeys` · `GetApiUsageStats` · `ValidateApiKey` · `GetRateLimitStatus`
- **Events:** `ApiKeyCreated/Revoked/Rotated` · `RateLimitExceeded` → Notification
- **Endpoints:** `POST /api/v1/public/*` (rate-limited)
- **Infra:** PostgreSQL (shared) · Redis (sliding window rate limiting + API key cache) · Scalar (OpenAPI docs)

#### BC-GEN-STB · StorageBillingContext

- **Commands:** `RecordStorageSnapshot` · `RecordExamReceived` · `RecordReportPublished` · `RecordUserActive` · `RecordNotificationSent` · `RecordAiUsage` · `RecordApiOverage` · `CloseMeter` · `GenerateInvoice` · `IssueInvoice` · `MarkInvoicePaid`
- **Queries:** `GetCurrentUsage` · `GetQuotaStatus` · `GetInvoiceHistory` · `GetContractualMonthlyReport` · `GetPlatformCostMargin` · `GetQuotaAlertStatus`
- **Events:** `MeterClosed` · `InvoiceGenerated/Issued` · `QuotaAlert` / `QuotaExceeded` → Notification · Tenant; `ContractualReportGenerated`
- **Integrations:** Modelos RaaS e FixedContract · Quota enforcement cross-BC
- **Infra:** PostgreSQL (shared) · RabbitMQ (consumer) · Redis (quota counters, TTL mensal) · S3/MinIO (PDFs de invoice) · Hangfire (meter closing + invoice generation)

#### BC-GEN-SIT · SiteContext

- **Commands:** `UpdateSiteConfig` · `PublishSite` · `CreateArticle` · `PublishArticle` · `SubmitLead` · `HandleLead`
- **Queries:** `GetPublicSite` · `GetArticles` · `GetArticleBySlug` · `GetLeads` · `GetSitemap`
- **Events:** `SitePublished` · `LeadSubmitted` → Notification (CRM)
- **Integrations:** reCAPTCHA v3 (formulário de contato/lead)
- **Infra:** PostgreSQL (shared) · Redis (site config cache, TTL 5 min) · S3/MinIO (logo + branding assets)

---

## BC-PEP — Patient Encounter & Progress

| BC         | Description                                             | Wireframe | Backend |
| ---------- | ------------------------------------------------------- | --------- | ------- |
| BC-PEP-ENC | Encounter — Atendimento clínico, admissão, alta         | ⬜        | ⬜      |
| BC-PEP-CDO | Clinical Documentation — Anamnese, evolução, prontuário | ⬜        | ⬜      |
| BC-PEP-PRE | Prescription — Receituário, controle de medicamentos    | ⬜        | ⬜      |
| BC-PEP-CON | Condition — CID-10, diagnósticos, alergias              | ⬜        | ⬜      |
| BC-PEP-VIT | Vital Signs — PA, SpO2, temperatura, peso/altura        | ⬜        | ⬜      |
| BC-PEP-ORD | Orders — Pedidos de exames, solicitações clínicas       | ⬜        | ⬜      |
| BC-PEP-CTL | Clinical Timeline — Linha do tempo do paciente          | ⬜        | ⬜      |

### Backend Backlog — BC-PEP

#### BC-PEP-ENC · EncounterContext `WhiteMage.Domain.Encounter`

- **Commands:** `ScheduleEncounter` · `StartEncounter` · `UpdateChiefComplaint` · `AddIcd10Code` · `CloseEncounter` · `CancelEncounter` · `LinkVideoSession`
- **Queries:** `GetEncounterById` · `GetPatientEncounters` · `GetTodayEncounters` · `GetActiveEncounter`
- **Events:** `EncounterStarted` → Orders · VitalSigns; `EncounterCompleted` → Billing · Timeline; `EncounterCancelled`
- **Integrations:** FHIR R4 Encounter resource
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ

#### BC-PEP-CDO · ClinicalDocumentationContext `WhiteMage.Domain.ClinicalDocumentation`

- **Commands:** `CreateClinicalDocument` · `UpdateDocumentSections` · `AddDiagnosis` · `SignDocument` (ICP-Brasil) · `AmendDocument`
- **Queries:** `GetDocumentsByEncounter` · `GetDocumentsByPatient` · `GetDiagnosisHistory` · `GetDocumentTemplates`
- **Events:** `ClinicalDocumentCreated/Signed` · `DiagnosisAdded` → Timeline · Audit
- **Integrations:** ICP-Brasil assinatura digital · FHIR R4 Composition resource
- **Infra:** PostgreSQL (tenant-isolated) · S3/MinIO (PDF assinado) · RabbitMQ · ICP-Brasil CA

#### BC-PEP-PRE · PrescriptionContext `WhiteMage.Domain.Prescription`

- **Commands:** `CreatePrescription` · `UpdatePrescription` · `SignPrescription` (ICP-Brasil) · `CancelPrescription` · `RenewPrescription` · `MarkDispensed`
- **Queries:** `GetPrescriptionsByEncounter` · `GetActivePrescriptions` · `GetPrescriptionHistory` · `GetMedicationInteractions`
- **Events:** `PrescriptionSigned` → Notification (paciente); `PrescriptionCancelled`
- **Integrations:** ANVISA BNAFAR/CMED (banco de medicamentos) · ICP-Brasil · Portaria SVS/MS 344/1998 (psicotrópicos/controlados)
- **Infra:** PostgreSQL (tenant-isolated) · S3/MinIO (PDF receituário) · RabbitMQ · ICP-Brasil CA · Banco interno de medicamentos (BNAFAR/CMED sync)

#### BC-PEP-CON · ConditionContext `WhiteMage.Domain.Condition`

- **Commands:** `AddCondition` · `UpdateConditionStatus` · `AddAllergy` · `RemoveAllergy`
- **Queries:** `GetActiveConditions` · `GetConditionHistory` · `GetAllergies` · `GetFamilyHistory` · `GetComorbiditySummary`
- **Events:** `ConditionAdded/StatusUpdated` · `AllergyAdded` → Prescription (interações) · Timeline
- **Integrations:** FHIR R4 Condition + AllergyIntolerance resources · CID-10 (tabela codificada)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ

#### BC-PEP-VIT · VitalSignsContext `WhiteMage.Domain.VitalSigns`

- **Commands:** `RecordVitalSigns` · `UpdateVitalObservation`
- **Queries:** `GetLatestVitalSigns` · `GetVitalSignsByEncounter` · `GetVitalSignsSeries` · `GetAbnormalVitalSigns`
- **Events:** `VitalSignsRecorded` → Timeline; `AbnormalVitalDetected` → Notification · Exception
- **Integrations:** FHIR R4 Observation (LOINC codes) · DICOM ORU^R01 de monitores de beira-leito (Fase 2)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ

#### BC-PEP-ORD · OrdersContext `WhiteMage.Domain.Orders`

- **Commands:** `CreateOrder` · `SignOrder` (ICP-Brasil) · `MarkOrderSent` · `MarkOrderCollected` · `AttachResult` · `CancelOrder`
- **Queries:** `GetOrdersByEncounter` · `GetPendingOrdersByPatient` · `GetOrderHistory`
- **Events:** `ImagingOrderCreated` → Worklist (C-FIND MWL); `OrderSigned` → Integration (HL7 OML^O21); `OrderResulted` · `OrderCancelled`
- **Integrations:** HL7 v2 OML^O21 · FHIR R4 ServiceRequest · ICP-Brasil assinatura
- **Infra:** PostgreSQL (tenant-isolated) · S3/MinIO (PDF pedido) · RabbitMQ · ICP-Brasil CA

#### BC-PEP-CTL · ClinicalTimelineContext `WhiteMage.Domain.ClinicalTimeline`

- **Commands:** (somente read model — sem estado próprio)
- **Queries:** `GetPatientTimeline` · `GetTimelineSummary` · `GetRecentClinicalContext`
- **Events consumed:** Fanout de todos os BCs PEP + Reporting (projeções desnormalizadas)
- **Infra:** PostgreSQL (tenant-isolated, particionado por data) · RabbitMQ (consumer) · Read model projections

---

## BC-PLT — Platform / Identity

| BC         | Description                                         | Wireframe                | Backend |
| ---------- | --------------------------------------------------- | ------------------------ | ------- |
| BC-PLT-IDN | Identity — Usuários, perfis, autenticação, MFA      | 🚧 (wireframe auth done) | ⬜      |
| BC-PLT-TNT | Tenant — Multi-unidade, configurações por filial    | ⬜                       | ⬜      |
| BC-PLT-PAT | Patient — Cadastro de pacientes, dados demográficos | ⬜                       | ⬜      |
| BC-PLT-AUD | Audit — Trilha de auditoria, logs de acesso         | 🚧 (AccessHistory done)  | ⬜      |
| BC-PLT-PRF | Preference — Preferências do usuário, tema, idioma  | 🚧 (Profile pages done)  | ⬜      |

### Backend Backlog — BC-PLT

#### BC-PLT-IDN · IdentityContext `WhiteMage.Domain.Identity`

- **Commands:** `RegisterUser` · `ConfirmEmail` · `Login` · `RefreshToken` · `Logout` · `SwitchTenant` · `Enable2FA` · `Disable2FA` · `ChangePassword` · `ForgotPassword` · `ResetPassword` · `UpdateUserProfile` · `AddUserToTenant` · `RemoveUserFromTenant` · `SuspendUser` · `ReactivateUser` · `DeleteUser`
- **Queries:** `GetUserById` · `GetUserByEmail` · `GetUserTenantMemberships` · `GetUsersOfTenant` · `GetActiveRadiologists` · `ValidateApiKey`
- **Endpoints REST:** `POST /auth/register` · `/auth/confirm-email` · `/auth/login` · `/auth/refresh-token` · `/auth/logout` · `/auth/switch-tenant` · `/auth/2fa/enable|disable` · `/auth/password/change|forgot|reset`
- **Events:** `UserRegistered` · `UserLoggedIn/Out` · `UserAddedToTenant` · `UserSuspended` · `PasswordReset` → todos → Audit
- **Integrations:** JWT Bearer · TOTP (Google Authenticator) · SMS 2FA (Twilio) · CRM (credenciamento radiologista)
- **Infra:** PostgreSQL (shared) · Redis (session + JWT cache) · SendGrid SMTP (confirmação de email)

#### BC-PLT-TNT · TenantContext `WhiteMage.Domain.Tenant`

- **Commands:** `CreateTenant` · `ProvisionTenant` · `ActivateTenant` · `SuspendTenant` · `ReactivateTenant` · `DecommissionTenant` · `UpdateTenantBranding` · `UpdateTenantConfig` · `CreateContract` · `ActivateContract` · `AmendContract` · `TerminateContract`
- **Queries:** `GetTenantById` · `GetTenantBySlug` · `GetActiveContract` · `ListTenants` · `GetTenantInfraConfig` · `IsModuleEnabled`
- **Events:** `TenantCreated/Provisioned/Activated/Suspended/Decommissioned` · `ContractActivated/Amended/Terminated` · `TenantBrandingUpdated` → Audit
- **Integrations:** Terraform (provisionamento infra) · Modelos RaaS e FixedContract · AWS/MinIO (bucket por tenant)
- **Infra:** PostgreSQL (shared) · Redis (tenant contract cache, TTL 5 min) · Terraform · AWS/MinIO (bucket provisioning)

#### BC-PLT-PAT · PatientContext `WhiteMage.Domain.Patient`

- **Commands:** `RegisterPatient` · `UpdatePatientDemographics` · `RecordPatientConsent` · `RevokePatientConsent` · `GeneratePortalAccessToken` · `MergePatients` · `RegisterPatientDeceased` · `RequestAnonymization` · `CompleteAnonymization`
- **Queries:** `FindPatientByCpf` · `FindPatientByNameAndDob` · `GetPatientById` · `GetPatientByMedicalRecord` · `GetPatientByDicomId` · `GetPatientConsents` · `ValidatePortalToken` · `SearchPatients`
- **Events:** `PatientRegistered` → Worklist; `PatientDemographicsUpdated` · `PatientsMerged` · `PatientConsentRevoked` · `PatientAnonymizationRequested` → Audit · Imaging
- **Integrations:** HL7 v2 ADT · FHIR R4 Patient resource · DICOM Patient ID · LGPD (anonimização, consentimento)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · AES-256 (CPF + email em repouso)

#### BC-PLT-AUD · AuditContext `WhiteMage.Domain.Audit`

- **Commands:** `RecordAuditEntry` (append-only — invocado por todos os BCs)
- **Queries:** `SearchAuditEntries` · `GetAuditEntryById` · `GetReportAuditTrail` · `GetPatientAccessLog` · `GetUserActivity` · `GetSecurityEvents` · `ExportAuditReport` · `GetComplianceChecklist`
- **Events consumed:** Fanout de todos os BCs (trilha imutável)
- **Integrations:** SIEM export (opcional) · Splunk/ELK (opcional) · CFM 2.314/2022 (obrigatoriedade trilha laudo)
- **Infra:** PostgreSQL (shared, append-only, particionado por mês) · RabbitMQ (consumer) · Triggers (imutabilidade) · Arquivamento cold storage (> 2 anos)

#### BC-PLT-PRF · PreferenceContext `WhiteMage.Domain.Preference`

- **Commands:** `SetPreference` · `DeletePreference` · `ResetScopedPreferences`
- **Queries:** `GetPreference` · `GetResolvedPreferencesByCategory` · `GetRawPreferences` · `GetViewerPreferences`
- **Events:** `PreferenceUpdated/Deleted` → Audit (opcional)
- **Integrations:** Resolução hierárquica User > Tenant > System
- **Infra:** PostgreSQL (shared) · Redis (preference cache, TTL 15 min)

---

## BC-SUP — Support / Operations

| BC         | Description                                                 | Wireframe | Backend |
| ---------- | ----------------------------------------------------------- | --------- | ------- |
| BC-SUP-VWR | Viewer — Visualizador web de imagens DICOM (zero-footprint) | ⬜        | ⬜      |
| BC-SUP-DEL | Delivery — Entrega de laudos, compartilhamento seguro       | ⬜        | ⬜      |
| BC-SUP-SCH | Scheduling — Agenda de exames, horários, disponibilidade    | ⬜        | ⬜      |
| BC-SUP-COL | Clinical Collaboration — Teleconsulta, segunda opinião      | ⬜        | ⬜      |
| BC-SUP-UNT | Units and Rooms — Salas, equipamentos, leitos               | ⬜        | ⬜      |
| BC-SUP-EXC | Exception — Tratamento de exceções, fila de pendências      | ⬜        | ⬜      |

### Backend Backlog — BC-SUP

#### BC-SUP-VWR · ViewerContext `WhiteMage.Domain.Viewer`

- **Commands:** `CreateHangingProtocol` · `PublishHangingProtocol` · `GenerateWadoToken` (TTL 15 min) · `CreateStudyShareLink` · `RevokeStudyShareLink` · `RecordShareLinkAccess`
- **Queries:** `GetBestHangingProtocol` · `GetHangingProtocols` · `GetViewerConfig` · `GetStudyShareLink`
- **Endpoints REST:** `GET /viewer/{studyUid}?token={wadoToken}` · `GET /share/{shareToken}`
- **Events:** `HangingProtocolPublished` · `WadoTokenGenerated` · `StudyShareLinkCreated/Accessed/Revoked` → Audit
- **Integrations:** OHIF Viewer v3 · Weasis · DWV · DICOM WADO-RS (recuperação de instâncias)
- **Infra:** PostgreSQL (tenant-isolated) · Redis (WADO token cache, TTL 15 min) · S3/MinIO (hanging protocols)

#### BC-SUP-DEL · DeliveryContext `WhiteMage.Domain.Delivery`

- **Commands:** `CreateDeliveryRecord` · `SendDeliveryNotification` · `RecordDeliveryViewed` · `RecordDeliveryDownloaded` · `GenerateDicomZip` · `RevokeDeliveryToken` · `UpdateDeliveryForRectification`
- **Queries:** `ValidateDeliveryToken` · `GetDeliveryRecordByReport` · `GetPatientDeliveries` · `GetDeliveryMetrics`
- **Endpoints REST:** `GET /delivery/{token}?pin={pin}` · `GET /delivery/download/{reportId}` · `GET /delivery/cd/{reportId}`
- **Events:** `DeliveryRecordCreated` · `DeliveryAcknowledged/Downloaded` · `DicomZipReady` → Notification
- **Integrations:** PIN via SMS (Twilio) · Magic link via email · LGPD (token de acesso paciente)
- **Infra:** PostgreSQL (tenant-isolated) · S3/MinIO (PDF + DICOM ZIP) · RabbitMQ · Hangfire (ZIP assíncrono)

#### BC-SUP-SCH · SchedulingContext `WhiteMage.Domain.Scheduling`

- **Commands:** `CreateSchedule` · `AddScheduleSlot` · `ApproveFreelancerSlot` · `BlockSchedule` · `UpdateQueueSize` · `UpdateModalityCredential`
- **Queries:** `GetAvailableRadiologists` · `GetRadiologistSchedule` · `GetAvailabilityCalendar` · `GetCredentialedRadiologists` · `GetWorkloadSnapshot`
- **Events:** `SlotApproved` → Notification (radiologista freelancer); `ScheduleBlocked` · `CredentialUpdated`
- **Integrations:** Row-Level Security multi-tenant (visibilidade freelancer) · CRM (credenciamento)
- **Infra:** PostgreSQL (shared com RLS) · RabbitMQ

#### BC-SUP-COL · ClinicalCollaborationContext `WhiteMage.Domain.ClinicalCollaboration`

- **Commands:** `OpenSecondOpinionThread` · `SendClinicalMessage` · `ResolveSecondOpinion` · `CloseThread` · `AcknowledgeCriticalFinding` · `AddParticipant`
- **Queries:** `GetActiveThreads` · `GetThreadById` · `GetCriticalFindingsPendingAck` · `GetSecondOpinionSlaReport`
- **Endpoints:** `WebSocket /hubs/clinical` (SignalR real-time)
- **Events:** `SecondOpinionRequested/Completed` → Reporting; `ClinicalMessageSent` · `CriticalFindingAcknowledged` · `ThreadClosed` → Audit
- **Integrations:** SignalR (mensageria em tempo real) · CFM Parecer 14/2017 (teleinterconsulta)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · SignalR Hub

#### BC-SUP-UNT · UnitsAndRoomsContext `WhiteMage.Domain.UnitsAndRooms`

- **Commands:** `CreateHealthUnit` · `UpdateHealthUnit` · `DeactivateUnit` · `AddExamRoom` · `UpdateExamRoom` · `DeactivateRoom` · `LinkDicomNodeToRoom`
- **Queries:** `GetHealthUnits` · `GetUnitRooms` · `GetRoomsByModality` · `GetRoomAvailability`
- **Events:** `HealthUnitCreated` · `ExamRoomAdded/Deactivated` → Scheduling · ModalityOrchestration
- **Integrations:** CNES (Cadastro Nacional de Estabelecimentos de Saúde) · ANVISA DATAMED (registro de equipamentos)
- **Infra:** PostgreSQL (tenant-isolated)

#### BC-SUP-EXC · ExceptionContext `WhiteMage.Domain.Exception`

- **Commands:** `CreateException` · `AssignException` · `AddExceptionComment` · `ResolveException` · `DismissException` · `EscalateException`
- **Queries:** `GetOpenExceptions` · `GetExceptionsByExam` · `GetExceptionSlaReport` · `GetExceptionDashboard`
- **Events consumed:** `SlaBreached` (Worklist) · `AbnormalVitalDetected` (VitalSigns)
- **Events published:** `ExceptionCreated` · `ExceptionEscalated` → Notification; `ExceptionResolved` → Audit
- **Integrations:** Programa de gestão de qualidade · Notificação CFM/ANVISA (não-conformidades)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · Hangfire (SLA escalation job)

---

## BC-TUS — Tele-Ultrasound

| BC         | Description                                            | Wireframe | Backend |
| ---------- | ------------------------------------------------------ | --------- | ------- |
| BC-TUS-REM | Remote Session — Sessão de ultrassom remoto            | ⬜        | ⬜      |
| BC-TUS-LIV | Live Video — Streaming de vídeo em tempo real          | ⬜        | ⬜      |
| BC-TUS-GUI | Remote Guidance — Orientação remota ao operador        | ⬜        | ⬜      |
| BC-TUS-EQP | Ultrasound Equipment — Gestão de sondas e equipamentos | ⬜        | ⬜      |

### Backend Backlog — BC-TUS

#### BC-TUS-REM · RemoteSessionContext `WhiteMage.Domain.RemoteSession`

- **Commands:** `ScheduleRemoteSession` · `RecordConsent` · `StartRemoteSession` · `CompleteRemoteSession` · `CancelRemoteSession`
- **Queries:** `GetRemoteSessionById` · `GetScheduledSessions` · `GetSessionsByPatient` · `GetActiveSession`
- **Events:** `RemoteSessionScheduled/Started/Completed/Cancelled` → Notification · Billing · Audit
- **Integrations:** ABNT ISO 13131 (telemedicina) · CFM 2.228/2019 · Consentimento LGPD
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ

#### BC-TUS-LIV · LiveVideoContext `WhiteMage.Domain.LiveVideo`

- **Commands:** `CreateVideoSession` · `JoinVideoSession` · `LeaveVideoSession` · `EndVideoSession`
- **Queries:** `GetVideoSessionToken` · `GetVideoSessionStatus`
- **Endpoints:** WebSocket (WebRTC signaling via SignalR)
- **Events:** `VideoSessionCreated/Ended` → RemoteSession · Audit
- **Integrations:** WebRTC (P2P ou media server) · STUN/TURN (NAT traversal) · CFM 2.228/2019 (requisitos qualidade vídeo)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · SignalR Hub (WebRTC sinalização) · coturn ou Twilio NTS (STUN/TURN) · S3/MinIO (gravação opcional)

#### BC-TUS-GUI · RemoteGuidanceContext `WhiteMage.Domain.RemoteGuidance`

- **Commands:** `StartGuidanceSession` · `AddAnnotation` · `CaptureDicomFrame` · `SendProbeCommand` · `AckProbeCommand` · `EndGuidanceSession`
- **Queries:** `GetGuidanceSession` · `GetAnnotationsBySession`
- **Endpoints:** WebRTC Data Channel (anotações de baixa latência)
- **Events:** `GuidanceSessionStarted` · `DicomFrameCaptured` → Imaging (C-STORE); `GuidanceSessionEnded` → Audit
- **Integrations:** WebRTC Data Channel (baixa latência) · DICOM C-STORE (captura de frames)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · WebRTC Data Channel

#### BC-TUS-EQP · UltrasoundEquipmentContext `WhiteMage.Domain.UltrasoundEquipment`

- **Commands:** `RegisterDevice` · `UpdateDeviceFirmware` · `SetDeviceConnectivity` · `AssignDeviceToRoom` · `RecordMaintenance` · `DeactivateDevice`
- **Queries:** `GetDeviceById` · `GetDevicesByUnit` · `GetOnlineDevices` · `GetDevicesDueMaintenance`
- **Events:** `DeviceRegistered` · `DeviceConnectivityChanged` → Notification; `MaintenanceRecorded` · `DeviceDeactivated`
- **Integrations:** ANVISA DATAMED (registro de equipamentos) · DICOM C-ECHO (heartbeat de conectividade) · DICOM C-STORE (captura)
- **Infra:** PostgreSQL (tenant-isolated) · RabbitMQ · Hangfire (heartbeat job, intervalo 2 min)

---

## Cross-Cutting / Shared

| Module              | Description                                   | Wireframe | Backend |
| ------------------- | --------------------------------------------- | --------- | ------- |
| COMMON-CrossCutting | Logging, caching, error handling, resiliência | —         | ⬜      |
| SHARED-SharedKernel | Tipos base, value objects, domain events      | —         | ⬜      |

### Backend Backlog — Cross-Cutting

#### COMMON · CrossCutting

- **Capabilities:** Multi-tenancy resolution middleware · JWT Bearer authentication · OpenTelemetry (tracing + metrics) · Redis caching (multi-tenant) · RabbitMQ event publishing (Outbox pattern) · Hangfire background jobs · Health checks (`GET /health`) · Metrics endpoint (`GET /metrics`)
- **Infra:** PostgreSQL (Outbox table — persistência antes de publicar) · Redis · RabbitMQ · Jaeger (distributed tracing) · Prometheus (metrics) · Loki/Seq (centralized logging)

#### SHARED · SharedKernel `WhiteMage.SharedKernel` (NuGet package)

- **Abstrações CQRS:** `ICommand` · `IQuery` · `ICommandHandler<T>` · `IQueryHandler<T,R>` · `IEventHandler<T>`
- **Domain primitives:** `AggregateRoot` · `Entity` · `ValueObject` · `DomainEvent` · `OutboxMessage`
- **Patterns:** Result pattern (sem exceções no fluxo de negócio) · Specification pattern · Guard clauses
- **Infra:** NuGet package — sem banco de dados ou dependências de infraestrutura

---

## Infrastructure / Wireframe

| Task                                                                               | Status |
| ---------------------------------------------------------------------------------- | ------ |
| Program.cs (auth, session, i18n)                                                   | ✅     |
| MockAuthService (7 users, MFA)                                                     | ✅     |
| MockData (exams, alerts, stats, charts)                                            | ✅     |
| SharedResource (PT-BR / EN / ES)                                                   | ✅     |
| hospital.css (navy design system)                                                  | ✅     |
| \_PublicLayout.cshtml                                                              | ✅     |
| \_AuthLayout.cshtml (sidebar + topbar)                                             | ✅     |
| Login + MFA + ForgotPassword + ResetPassword                                       | ✅     |
| Dashboard/Index (Google Charts, stat cards)                                        | ✅     |
| Exams/Index (search + table)                                                       | ✅     |
| Alerts/Index                                                                       | ✅     |
| Profile: AccountData, PersonalData, Photo, ChangePassword, MfaSetup, AccessHistory | ✅     |
| copilot-instructions.md                                                            | ✅     |
| todo.md                                                                            | ✅     |
