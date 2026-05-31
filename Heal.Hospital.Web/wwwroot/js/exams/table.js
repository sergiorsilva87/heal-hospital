/* Exams screen — Tabulator table init, loading overlay and icon dispatch. */
(function () {
    var E = window.Exams, t = E.t, escH = E.escH;

    /* ── Loading overlay (external div, immune to Tabulator lifecycle) ── */
    var _overlay = document.getElementById('rx-loading-overlay');
    var _spinnerSmHtml =
        '<div class="spinner-border spinner-border-sm" role="status" style="color:var(--clr-navy-600)">'
        + '<span class="visually-hidden">' + t("Processando...") + '</span></div>'
        + '<span>' + t("Processando…") + '</span>';

    var _initialLoading = true;
    var _opTimer = null;

    function _showOpOverlay() {
        if (_initialLoading) return;
        if (_opTimer) { clearTimeout(_opTimer); _opTimer = null; }
        _overlay.innerHTML = _spinnerSmHtml;
        _overlay.classList.add('rx-op');
        _overlay.classList.remove('hidden');
    }
    function _hideOpOverlay() {
        if (_initialLoading) return;
        if (_opTimer) clearTimeout(_opTimer);
        _opTimer = setTimeout(function () {
            _overlay.classList.add('hidden');
            _opTimer = null;
        }, 120);
    }

    var table = new Tabulator("#reception-table", {
        data: [],
        layout: "fitColumns",
        resizableColumns: false,
        rowFormatter: function (row) {
            var d  = row.getData();
            var el = row.getElement();
            if (d.isEmergency) el.classList.add('row-emergency');
            if (el.querySelector('.rx-detail')) return;

            var detail = document.createElement('div');
            detail.className = 'rx-detail';

            var r1 = '<div class="rx-detail-row">';
            if (d.socialNameRaw) r1 += '<span><b>' + t("Nome social:") + '</b> ' + escH(d.socialNameRaw) + '</span>';
            r1 += '<span><b>' + t("Nasc.:") + '</b> '      + escH(d.birthStr)          + '</span>';
            r1 += '<span><b>' + t("Idade:") + '</b> '       + escH(d.ageFormatted)      + '</span>';
            r1 += '<span><b>' + t("ID Estudo:") + '</b> '   + escH(d.studyId)           + '</span>';
            r1 += '<span><b>' + t("Data Estudo:") + '</b> '   + escH(d.studyDateTime)     + '</span>';
            r1 += '<span><b>' + t("Liberação:") + '</b> '        + escH(d.liberationDateStr) + '</span>';
            r1 += '<span><b>' + t("Data Laudo:") + '</b> '    + escH(d.reportDateTimeStr) + '</span>';
            r1 += '</div>';

            var r2 = '<div class="rx-detail-row">';
            r2 += '<span><b>' + t("Unidade:") + '</b> '     + escH(d.unit)               + '</span>';
            r2 += '<span><b>' + t("Técnico:") + '</b> '   + escH(d.technicianName)     + '</span>';
            r2 += '<span><b>' + t("Executor:") + '</b> '    + escH(d.executingPhysician)  + '</span>';
            r2 += '<span><b>' + t("Solicitante:") + '</b> ' + escH(d.requestingPhysician) + '</span>';
            r2 += '</div>';

            detail.innerHTML = r1 + r2;
            el.appendChild(detail);
        },
        pagination: true,
        paginationMode: "local",
        paginationSize: 10,
        paginationSizeSelector: [10, 15, 25, 50],
        locale: E.locale,
        langs: {
            "pt-br": {
                pagination: {
                    page_size: "Itens por página",
                    first: "Primeira", first_title: "Primeira página",
                    last: "Última",    last_title: "Última página",
                    prev: "Anterior",  prev_title: "Página anterior",
                    next: "Próxima",   next_title: "Próxima página",
                    all: "Todos",
                    counter: { showing: "Exibindo", of: "de", rows: "registros", pages: "páginas" }
                }
            },
            "en": {
                pagination: {
                    page_size: "Page size:",
                    first: "First", first_title: "First Page",
                    last: "Last",   last_title: "Last Page",
                    prev: "Prev",   prev_title: "Previous Page",
                    next: "Next",   next_title: "Next Page",
                    all: "All",
                    counter: { showing: "Showing", of: "of", rows: "rows", pages: "pages" }
                }
            },
            "es": {
                pagination: {
                    page_size: "Por pág.:",
                    first: "Primera",   first_title: "Primera página",
                    last: "Última",     last_title: "Última página",
                    prev: "Anterior",   prev_title: "Página anterior",
                    next: "Siguiente",  next_title: "Página siguiente",
                    all: "Todos",
                    counter: { showing: "Mostrando", of: "de", rows: "registros", pages: "páginas" }
                }
            }
        },
        columns: [
            /* ── Col 1: ID Paciente ─────────────────────────────── */
            {
                title: t("ID Paciente"), field: "patientId", widthGrow: 1, minWidth: 220,
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    var std = escH(d.studyId);
                    var f1 = d.criticalFinding === 'unnotified'
                        ? '<i class="bi bi-exclamation-triangle-fill rx-flag-critical-unnotified rx-icon-btn" data-rx-act="critical" data-std="' + std + '" title="' + escH(t("Achado crítico não resolvido")) + '"></i>'
                        : d.criticalFinding === 'notified'
                        ? '<i class="bi bi-patch-check-fill rx-flag-critical-notified rx-icon-btn" data-rx-act="critical" data-std="' + std + '" title="' + escH(t("Achado crítico resolvido")) + '"></i>'
                        : '<i class="rx-flag-placeholder"></i>';
                    var f2 = d.hasPendency
                        ? '<i class="bi bi-flag-fill rx-flag-pendency rx-icon-btn" data-rx-act="pendency" data-std="' + std + '" title="' + escH(t("Exame com pendências")) + '"></i>'
                        : '<i class="rx-flag-placeholder"></i>';
                    var f3 = d.attachmentCount > 0
                        ? '<span class="rx-attach-item rx-icon-btn" data-rx-act="documents" data-std="' + std + '"><i class="bi bi-paperclip rx-flag-attach" title="' + d.attachmentCount + ' ' + escH(t("doc(s) anexado(s)")) + '"></i><span class="rx-badge-count">' + d.attachmentCount + '</span></span>'
                        : '<i class="bi bi-paperclip rx-flag-attach-empty rx-icon-btn" data-rx-act="documents" data-std="' + std + '" title="' + escH(t("Sem documentos")) + '"></i>';
                    var f4 = d.isArchived
                        ? '<i class="bi bi-archive-fill rx-flag-archived rx-icon-btn" data-rx-act="archived" data-std="' + std + '" title="' + escH(t("Exame arquivado (cold storage)")) + '"></i>'
                        : '<i class="rx-flag-placeholder"></i>';
                    var f5 = d.downloadCodeGenerated
                        ? '<i class="bi bi-key-fill rx-flag-code-generated rx-icon-btn" data-rx-act="code" data-std="' + std + '" title="' + escH(t("Código de download do laudo gerado")) + '"></i>'
                        : '<i class="rx-flag-placeholder"></i>';
                    var f6 = d.reportDownloaded
                        ? '<i class="bi bi-cloud-arrow-down-fill rx-flag-downloaded rx-icon-btn" data-rx-act="download" data-std="' + std + '" title="' + escH(t("Laudo já baixado pelo paciente")) + '"></i>'
                        : '<i class="rx-flag-placeholder"></i>';
                    return '<span class="rx-flags-slot">' + f1 + f2 + f3 + f4 + f5 + f6 + '</span>'
                         + '<i class="bi bi-chevron-right rx-toggle-icon" title="' + escH(t("Detalhes")) + '"></i>'
                         + '<code style="color:var(--clr-navy-700)">' + escH(d.patientId) + '</code>';
                },
                cellClick: function (e, cell) {
                    if (e.target.closest('[data-rx-act]')) return;
                    var rowEl  = cell.getRow().getElement();
                    var detail = rowEl.querySelector('.rx-detail');
                    if (!detail) return;
                    var isOpen = detail.classList.toggle('open');
                    rowEl.classList.toggle('row-open', isOpen);
                }
            },
            /* ── Col 2: Paciente (pai) | child: handled by rowFormatter */
            {
                title: t("Paciente"), field: "patientName", widthGrow: 2, minWidth: 140,
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    if (d._isChild) return '';
                    return escH(d.patientName);
                }
            },
            /* ── Col 3: Gênero (pai) | vazio (filho) ────────────── */
            {
                title: t("Sexo"), field: "sexRaw", width: 90, hozAlign: "center", headerHozAlign: "center",
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    if (d._isChild) return '';
                    return escH(d.sexRaw);
                }
            },
            /* ── Col 4: AN = Nº Acesso (pai) | vazio (filho) ───────── */
            {
                title: "AN", field: "accessCodeRaw", width: 82, headerTooltip: t("Número de Acesso"),
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    if (d._isChild) return '';
                    return '<code style="color:var(--clr-navy-700)">' + escH(d.accessCodeRaw) + '</code>';
                }
            },
            /* ── Col 5: Mod. (pai) | child: handled by rowFormatter */
            {
                title: t("Mod."), field: "modality", width: 120,
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    if (d._isChild) return '';
                    var abbr = E.modAbbr[d.modality] || d.modality;
                    var clr  = E.modClr[abbr] || { bg: '#f0f0f0', c: '#444' };
                    return '<span style="display:inline-block;padding:1px 9px;border-radius:20px;font-weight:600;white-space:nowrap;background:' + clr.bg + ';color:' + clr.c + '">' + escH(abbr) + '</span>'
                         + '<span class="mod-img-count" title="' + d.imageCount + ' ' + escH(t("imagem(ns)")) + '">' + d.imageCount + '<i class="bi bi-images" style="font-size:.55rem;margin-left:1px"></i></span>';
                }
            },
            /* ── Col 6: Procedimento (pai) | child: handled by rowFormatter */
            {
                title: t("Procedimento"), field: "procedure", widthGrow: 1, minWidth: 120,
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    if (d._isChild) return '';
                    return escH(d.procedure);
                }
            },
            /* ── Col 7: Situação (pai) | child: handled by rowFormatter */
            {
                title: t("Situação"), field: "statusLabel", width: 140, hozAlign: "center", headerHozAlign: "center",
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    if (d._isChild) return '';
                    var clr = E.statusDotClr[d.statusLabel] || '#666';
                    return '<span style="display:inline-block;width:14px;height:14px;border-radius:50%;background:' + clr + ';vertical-align:middle;cursor:default" title="' + escH(d.statusLabel) + '"></span>';
                }
            },
            /* ── Col 8: Tipo (pai) | child: handled by rowFormatter */
            {
                title: t("Tipo"), field: "typeLabel", widthGrow: 1, minWidth: 60,
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    if (d._isChild) return '';
                    return '<span class="badge" style="background:var(--clr-navy-100);color:var(--clr-navy-900)">' + escH(d.typeLabel) + '</span>';
                }
            },
            /* ── Col: Ações ───────────────────────────────── */
            {
                title: t("Ações"), field: "hasReport", width: 140, hozAlign: "right",
                headerSort: false,
                formatter: function (cell) {
                    var d = cell.getRow().getData();
                    if (d._isChild) return '';
                    var html = '<div class="d-flex gap-1 justify-content-end">';
                    html += '<button class="btn btn-sm btn-outline-primary btn-edit-patient py-0"'
                          + ' data-id="'     + escH(d.patientId)     + '"'
                          + ' data-name="'   + escH(d.patientName)   + '"'
                          + ' data-social="' + escH(d.socialNameRaw) + '"'
                          + ' data-mother="' + escH(d.motherNameRaw) + '"'
                          + ' data-phone="'  + escH(d.phoneRaw)      + '"'
                          + ' data-sex="'    + escH(d.sexRaw)        + '"'
                          + ' data-birth="'  + escH(d.birthForInput) + '"'
                          + ' data-study="'  + escH(d.studyForInput) + '"'
                          + ' data-cpf="'    + escH(d.cpfRaw)        + '"'
                          + ' data-email="'  + escH(d.emailRaw)      + '"'
                          + ' data-code="'   + escH(d.accessCodeRaw) + '"'
                          + ' title="' + escH(t("Editar dados do paciente")) + '">'
                          + '<i class="bi bi-pencil-square"></i></button>';
                    html += '<button class="btn btn-sm btn-outline-primary btn-download-exam py-0"'
                          + ' data-study="' + escH(d.studyId) + '"'
                          + ' title="' + escH(t("Baixar laudos e anexos")) + '"><i class="bi bi-download"></i></button>';
                    if (d.hasReport) {
                        html += '<a href="#" class="btn btn-sm btn-outline-primary py-0" title="' + escH(t("Imprimir laudo")) + '" target="_blank"><i class="bi bi-printer"></i></a>';
                    }
                    html += '</div>';
                    return html;
                }
            }
        ]
    });
    E.table = table;

    /* ── Initial load: overlay visible until data renders ── */
    setTimeout(function () {
        table.setData(E.data).then(function () {
            _initialLoading = false;
            _overlay.classList.add('hidden');
        });
    }, 3000 + Math.floor(Math.random() * 2001));

    /* ── Brief loader on sort, filter and page change ────── */
    table.on('dataSorting',   _showOpOverlay);
    table.on('dataSorted',    _hideOpOverlay);
    table.on('dataFiltering', _showOpOverlay);
    table.on('dataFiltered',  _hideOpOverlay);
    table.on('pageLoading',   _showOpOverlay);
    table.on('pageLoaded',    _hideOpOverlay);

    /* ── Column-1 indicator icon dispatch ────────────── */
    document.getElementById('reception-table-wrap').addEventListener('click', function (e) {
        var el = e.target.closest('[data-rx-act]');
        if (!el) return;
        e.stopPropagation();
        var rec = E.recFromStd(el.dataset.std);
        if (!rec) return;
        var act = el.dataset.rxAct;
        if (act === 'critical')       E.openCriticalModal(rec);
        else if (act === 'pendency')  E.openPendencyModal(rec);
        else if (act === 'documents') E.openDownloadModal(rec.studyId);
        else if (act === 'archived')  E.openArchivedModal(rec);
        else if (act === 'code')      E.openCodeModal(rec);
        else if (act === 'download')  E.openDownloadHistoryModal(rec);
    });
})();
