/* Exams screen — download modal (current exam + prior exams). */
(function () {
    var E = window.Exams, t = E.t, escH = E.escH;

    /* Mock OHIF DICOM viewer URL — same for all exams (mocked) */
    var VIEWER_URL = 'https://viewer.ohif.org/viewer?StudyInstanceUIDs=1.2.276.0.7230010.3.1.2.2155604110.4180.1021041295.21';

    /* Download / cold-storage recovery action button */
    function dlIcon(isCold, label) {
        if (isCold) {
            return '<button class="btn btn-sm btn-outline-info py-0 btn-request-recovery" '
                 + 'data-file="' + escH(label) + '" title="' + escH(t("Arquivo em cold storage — solicitar recuperação")) + '">'
                 + '<i class="bi bi-snow"></i> ' + escH(t("Recuperar")) + '</button>';
        }
        return '<a href="#" class="btn btn-sm btn-outline-primary py-0" title="' + escH(t("Baixar")) + '"><i class="bi bi-download"></i></a>';
    }
    E.dlIcon = dlIcon;

    document.getElementById('reception-table-wrap').addEventListener('click', function (e) {
        var btn = e.target.closest('.btn-download-exam');
        if (!btn) return;
        E.openDownloadModal(btn.dataset.study);
    });

    /* Chevron flip when collapse opens/closes */
    (function () {
        var col = document.getElementById('dl-dicom-collapse');
        if (!col) return;
        col.addEventListener('show.bs.collapse', function () {
            var ch = document.getElementById('dl-dicom-chevron');
            if (ch) { ch.classList.remove('bi-chevron-down'); ch.classList.add('bi-chevron-up'); }
        });
        col.addEventListener('hide.bs.collapse', function () {
            var ch = document.getElementById('dl-dicom-chevron');
            if (ch) { ch.classList.remove('bi-chevron-up'); ch.classList.add('bi-chevron-down'); }
        });
    })();

    E.openDownloadModal = function (studyId) {
        var rec = E.recFromStd(studyId);
        if (!rec) return;
        var modal = document.getElementById('downloadModal');
        modal.querySelector('#dl-study').textContent = rec.studyId + ' · ' + rec.patientName;

        /* DICOM viewer links (mocked — same URL for all exams) */
        var frame = modal.querySelector('#dl-dicom-frame');
        var newTab = modal.querySelector('#dl-dicom-newtab');
        if (frame) frame.src = VIEWER_URL;
        if (newTab) newTab.href = VIEWER_URL;

        /* Collapse the DICOM section when re-opening the modal */
        var collapse = modal.querySelector('#dl-dicom-collapse');
        if (collapse) bootstrap.Collapse.getOrCreateInstance(collapse, { toggle: false }).hide();

        /* Reports */
        var rb = modal.querySelector('#dl-reports-body');
        rb.innerHTML = '';
        (rec.reports || []).forEach(function (r) {
            rb.insertAdjacentHTML('beforeend',
                '<tr><td><i class="bi bi-file-earmark-medical me-1 text-danger"></i>' + escH(r.title) + '</td>'
              + '<td class="text-end">' + dlIcon(r.isCold, r.title) + '</td></tr>');
        });
        if (!(rec.reports || []).length)
            rb.innerHTML = '<tr><td colspan="2" class="text-center text-muted py-3">' + escH(t("Nenhum laudo disponível.")) + '</td></tr>';

        /* Attachments */
        var ab = modal.querySelector('#dl-attachments-body');
        ab.innerHTML = '';
        (rec.attachments || []).forEach(function (a) {
            ab.insertAdjacentHTML('beforeend',
                '<tr><td><i class="bi bi-paperclip me-1"></i>' + escH(a.fileName) + '</td>'
              + '<td class="text-nowrap">' + escH(a.uploadedAt) + '</td>'
              + '<td>' + escH(a.type) + '</td>'
              + '<td class="text-nowrap">' + escH(a.size) + '</td>'
              + '<td>' + escH(a.uploadedBy) + '</td>'
              + '<td class="text-end">' + dlIcon(a.isCold, a.fileName) + '</td></tr>');
        });
        if (!(rec.attachments || []).length)
            ab.innerHTML = '<tr><td colspan="6" class="text-center text-muted py-3">' + escH(t("Nenhum arquivo anexado.")) + '</td></tr>';

        /* Prior exams */
        var pb = modal.querySelector('#dl-prior-body');
        pb.innerHTML = '';
        var priors = rec.priorExams || [];
        if (!priors.length) {
            pb.innerHTML = '<p class="text-center text-muted py-3 mb-0">' + escH(t("Nenhum exame anterior encontrado.")) + '</p>';
        } else {
            priors.forEach(function (p) {
                var viewerBtn = '<a href="' + escH(VIEWER_URL) + '" target="_blank" rel="noopener" '
                    + 'class="btn btn-sm btn-outline-primary py-0 d-inline-flex align-items-center gap-1" '
                    + 'title="' + escH(t("Imagens do exame (DICOM)")) + '">'
                    + '<i class="bi bi-images"></i> <span>' + escH(t("Ver imagens")) + '</span></a>';

                var html = '<div class="card mb-2"><div class="card-body py-2 px-3">'
                    + '<div class="d-flex justify-content-between align-items-center flex-wrap gap-1">'
                    + '<div><strong>' + escH(p.title) + '</strong>'
                    + '<span class="text-muted ms-2" style="font-size:var(--fs-xs)">'
                    + escH(p.studyId) + ' · AN ' + escH(p.accessNumber) + ' · ' + escH(p.when) + '</span></div>'
                    + '<div class="d-flex gap-2">' + viewerBtn + dlIcon(p.isCold, p.title) + '</div></div>';
                if ((p.attachments || []).length) {
                    html += '<table class="table table-sm mt-2 mb-0" style="font-size:var(--fs-xs)">'
                         + '<thead class="table-light"><tr><th>' + escH(t("Anexo")) + '</th><th>' + escH(t("Upload")) + '</th><th>' + escH(t("Tipo")) + '</th><th>' + escH(t("Tamanho")) + '</th>'
                         + '<th>' + escH(t("Responsável")) + '</th><th>AN</th><th class="text-end">' + escH(t("Ação")) + '</th></tr></thead><tbody>';
                    p.attachments.forEach(function (a) {
                        html += '<tr><td><i class="bi bi-paperclip me-1"></i>' + escH(a.fileName) + '</td>'
                              + '<td class="text-nowrap">' + escH(a.uploadedAt) + '</td>'
                              + '<td>' + escH(a.type) + '</td><td class="text-nowrap">' + escH(a.size) + '</td>'
                              + '<td>' + escH(a.uploadedBy) + '</td><td>' + escH(p.accessNumber) + '</td>'
                              + '<td class="text-end">' + dlIcon(a.isCold, a.fileName) + '</td></tr>';
                    });
                    html += '</tbody></table>';
                }
                html += '</div></div>';
                pb.insertAdjacentHTML('beforeend', html);
            });
        }

        document.body.appendChild(modal);
        bootstrap.Modal.getOrCreateInstance(modal).show();
    };
})();
