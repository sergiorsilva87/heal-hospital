/* Exams screen — archived items (cold storage) modal. */
(function () {
    var E = window.Exams, t = E.t, escH = E.escH;

    E.openArchivedModal = function (rec) {
        var m = document.getElementById('archivedModal');
        m.querySelector('#ar-study').textContent = rec.studyId + ' · ' + rec.patientName;
        var body = m.querySelector('#ar-body');
        body.innerHTML = '';
        (rec.archivedItems || []).forEach(function (a) {
            body.insertAdjacentHTML('beforeend',
                '<tr><td><i class="bi bi-file-earmark me-1"></i>' + escH(a.name) + '</td>'
              + '<td>' + escH(a.kind) + '</td>'
              + '<td class="text-nowrap">' + escH(a.size) + '</td>'
              + '<td class="text-nowrap">' + escH(a.archivedAt) + '</td></tr>');
        });
        if (!(rec.archivedItems || []).length)
            body.innerHTML = '<tr><td colspan="4" class="text-center text-muted py-3">' + escH(t("Nenhum item arquivado.")) + '</td></tr>';
        E.showModalEl(m);
    };

    document.getElementById('ar-restore').addEventListener('click', function () {
        bootstrap.Modal.getOrCreateInstance(document.getElementById('archivedModal')).hide();
        E.showToast(t("Restauração solicitada. Você será avisado quando os arquivos estiverem disponíveis (pode levar algumas horas)."));
    });
})();
