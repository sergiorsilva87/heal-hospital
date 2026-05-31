/* Exams screen — critical finding modals (unresolved + resolved history). */
(function () {
    var E = window.Exams, t = E.t, escH = E.escH;

    E.openCriticalModal = function (rec) {
        var c = rec.criticalDetail || {};
        var head = rec.studyId + ' · ' + rec.patientName;
        if (c.state === 'notified') {
            var modal = document.getElementById('criticalResolvedModal');
            modal.querySelector('#crv-study').textContent = head;
            modal.querySelector('#crv-timeline').innerHTML =
                '<div class="card mb-2 border-danger-subtle"><div class="card-body py-2 px-3">'
              + '<div class="fw-semibold text-danger mb-1"><i class="bi bi-exclamation-triangle-fill me-1"></i>' + escH(t("Achado crítico registrado")) + '</div>'
              + '<div style="font-size:var(--fs-sm)"><i class="bi bi-calendar-event me-1"></i>' + escH(c.detectedAt || '') + '</div>'
              + '<div style="font-size:var(--fs-sm)"><i class="bi bi-person-badge me-1"></i>' + escH(c.physician || '') + ' · ' + escH(c.crm || '') + '/' + escH(c.uf || '') + '</div>'
              + '<div class="alert alert-danger mt-2 mb-0" style="font-size:var(--fs-sm)">' + escH(c.message || '') + '</div>'
              + '</div></div>'
              + '<div class="text-center text-muted my-1"><i class="bi bi-arrow-down"></i></div>'
              + '<div class="card mb-0 border-success-subtle"><div class="card-body py-2 px-3">'
              + '<div class="fw-semibold text-success mb-1"><i class="bi bi-check2-circle me-1"></i>' + escH(t("Achado resolvido")) + '</div>'
              + '<div style="font-size:var(--fs-sm)"><i class="bi bi-calendar-check me-1"></i>' + escH(c.resolvedAt || '') + '</div>'
              + '<div style="font-size:var(--fs-sm)"><i class="bi bi-person-check me-1"></i>' + escH(c.resolvedBy || '') + ' · ' + escH(c.resolvedRole || '') + '</div>'
              + '<div class="alert alert-success mt-2 mb-0" style="font-size:var(--fs-sm)">' + escH(c.contactNote || '') + '</div>'
              + '</div></div>';
            E.showModalEl(modal);
            return;
        }
        var m = document.getElementById('criticalUnresolvedModal');
        m.querySelector('#cu-study').textContent     = head;
        m.querySelector('#cu-date').textContent      = c.detectedAt || '';
        m.querySelector('#cu-physician').textContent = c.physician || '';
        m.querySelector('#cu-crm').textContent       = (c.crm || '') + ' / ' + (c.uf || '');
        m.querySelector('#cu-message').textContent   = c.message || '';
        m.querySelector('#cu-patient').textContent   = rec.patientName || '';
        m.querySelector('#cu-phone').textContent     = rec.phoneRaw || '';
        m.querySelector('#cu-email').textContent     = rec.emailRaw || t("(sem e-mail cadastrado)");
        var note = m.querySelector('#cu-note');
        note.value = '';
        note.classList.remove('is-invalid');
        E.showModalEl(m);
    };

    (function () {
        var m = document.getElementById('criticalUnresolvedModal');
        function validateAnd(cb) {
            var note = m.querySelector('#cu-note');
            if (!note.value.trim()) { note.classList.add('is-invalid'); note.focus(); return; }
            note.classList.remove('is-invalid');
            bootstrap.Modal.getOrCreateInstance(m).hide();
            cb();
        }
        m.querySelector('#cu-note').addEventListener('input', function () { this.classList.remove('is-invalid'); });
        m.querySelector('#cu-register').addEventListener('click', function () {
            validateAnd(function () { E.showToast(t("Contato registrado com sucesso.")); });
        });
        m.querySelector('#cu-resolve').addEventListener('click', function () {
            validateAnd(function () { E.showToast(t("Achado crítico marcado como resolvido.")); });
        });
    })();
})();
