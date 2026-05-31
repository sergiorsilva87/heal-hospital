/* Exams screen — edit patient modal. */
(function () {
    var E = window.Exams, t = E.t, escH = E.escH, data = E.data;

    document.getElementById('reception-table-wrap').addEventListener('click', function (e) {
        var btn = e.target.closest('.btn-edit-patient');
        if (!btn) return;
        var modal = document.getElementById('editPatientModal');
        modal.querySelector('#ep-patient-id').value  = btn.dataset.id     || '';
        modal.querySelector('#ep-name').value        = btn.dataset.name   || '';
        modal.querySelector('#ep-social').value      = btn.dataset.social || '';
        modal.querySelector('#ep-mother').value      = btn.dataset.mother || '';
        modal.querySelector('#ep-phone').value       = btn.dataset.phone  || '';
        modal.querySelector('#ep-sex').value         = btn.dataset.sex    || 'M';
        modal.querySelector('#ep-birth').value       = btn.dataset.birth  || '';
        modal.querySelector('#ep-study').value       = (btn.dataset.study || '').slice(0, 16);
        modal.querySelector('#ep-cpf').value         = btn.dataset.cpf    || '';
        modal.querySelector('#ep-email').value       = btn.dataset.email  || '';
        modal.querySelector('#ep-access-code').value = btn.dataset.code   || '';

        /* Render change history for this patient */
        var rec = data.find(function (d) { return d.patientId === btn.dataset.id; });
        var body = modal.querySelector('#ep-history-body');
        body.innerHTML = '';
        var logs = (rec && rec.changeLogs) || [];
        if (logs.length === 0) {
            body.innerHTML = '<tr><td colspan="5" class="text-center text-muted py-3">'
                           + escH(t("Nenhuma alteração registrada.")) + '</td></tr>';
        } else {
            logs.forEach(function (l) {
                body.insertAdjacentHTML('beforeend',
                    '<tr>'
                  + '<td><div class="d-flex align-items-center gap-2">'
                  + '<span class="hosp-avatar" style="width:26px;height:26px;font-size:.6rem">' + escH(l.initials) + '</span>'
                  + '<span>' + escH(l.userName) + '</span></div></td>'
                  + '<td>' + escH(l.userRole) + '</td>'
                  + '<td>' + escH(l.unit) + '</td>'
                  + '<td>' + escH(l.summary) + '</td>'
                  + '<td class="text-nowrap">' + escH(l.at) + '</td>'
                  + '</tr>');
            });
        }
        /* Always reset to first tab on open */
        bootstrap.Tab.getOrCreateInstance(modal.querySelector('#ep-tab-data-btn')).show();

        document.body.appendChild(modal);
        bootstrap.Modal.getOrCreateInstance(modal).show();
    });

    document.getElementById('ep-gen-code').addEventListener('click', function () {
        document.getElementById('ep-access-code').value =
            Math.floor(100000 + Math.random() * 900000).toString();
    });
})();
