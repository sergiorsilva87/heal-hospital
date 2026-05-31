/* Exams screen — pendency modal. */
(function () {
    var E = window.Exams, t = E.t;

    E.openPendencyModal = function (rec) {
        var p = rec.pendencyDetail || {};
        var m = document.getElementById('pendencyModal');
        m.querySelector('#pd-study').textContent     = rec.studyId + ' · ' + rec.patientName;
        m.querySelector('#pd-physician').textContent = p.physician || '';
        m.querySelector('#pd-crm').textContent       = (p.crm || '') + ' / ' + (p.uf || '');
        m.querySelector('#pd-date').textContent      = p.openedAt || '';
        m.querySelector('#pd-text').textContent      = p.text || '';
        var reply = m.querySelector('#pd-reply');
        reply.value = '';
        reply.classList.remove('is-invalid');
        m.querySelector('#pd-file').value = '';
        E.showModalEl(m);
    };

    (function () {
        var m = document.getElementById('pendencyModal');
        m.querySelector('#pd-reply').addEventListener('input', function () { this.classList.remove('is-invalid'); });
        m.querySelector('#pd-send').addEventListener('click', function () {
            var reply = m.querySelector('#pd-reply');
            if (!reply.value.trim()) { reply.classList.add('is-invalid'); reply.focus(); return; }
            bootstrap.Modal.getOrCreateInstance(m).hide();
            E.showToast(t("Resposta enviada para a pendência."));
        });
    })();
})();
