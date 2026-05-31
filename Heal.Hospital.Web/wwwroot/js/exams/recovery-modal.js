/* Exams screen — cold-storage recovery request modal. */
(function () {
    var E = window.Exams, t = E.t;

    document.addEventListener('click', function (e) {
        var btn = e.target.closest('.btn-request-recovery');
        if (!btn) return;
        var modal = document.getElementById('recoveryModal');
        modal.querySelector('#rec-file-name').textContent = btn.dataset.file || '';
        modal._sourceBtn = btn;
        document.body.appendChild(modal);
        bootstrap.Modal.getOrCreateInstance(modal).show();
    });

    document.getElementById('rec-confirm').addEventListener('click', function () {
        var modal = document.getElementById('recoveryModal');
        var btn = modal._sourceBtn;
        if (btn) {
            btn.outerHTML = '<span class="badge bg-info-subtle text-info-emphasis">'
                          + '<i class="bi bi-hourglass-split me-1"></i>' + E.escH(t("Recuperação solicitada")) + '</span>';
        }
        bootstrap.Modal.getOrCreateInstance(modal).hide();
    });
})();
