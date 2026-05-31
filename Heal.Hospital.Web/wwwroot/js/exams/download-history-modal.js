/* Exams screen — patient download history modal. */
(function () {
    var E = window.Exams, t = E.t, escH = E.escH;

    E.openDownloadHistoryModal = function (rec) {
        var m = document.getElementById('downloadHistoryModal');
        m.querySelector('#dh-study').textContent = rec.studyId + ' · ' + rec.patientName;
        var body = m.querySelector('#dh-body');
        body.innerHTML = '';
        (rec.downloadEvents || []).forEach(function (d) {
            body.insertAdjacentHTML('beforeend',
                '<tr><td class="text-nowrap"><i class="bi bi-clock-history me-1"></i>' + escH(d.at) + '</td>'
              + '<td>' + escH(d.device) + '</td>'
              + '<td class="text-nowrap"><code>' + escH(d.ip) + '</code></td></tr>');
        });
        if (!(rec.downloadEvents || []).length)
            body.innerHTML = '<tr><td colspan="3" class="text-center text-muted py-3">' + escH(t("O paciente ainda não baixou os laudos.")) + '</td></tr>';
        E.showModalEl(m);
    };
})();
