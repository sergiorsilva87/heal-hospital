/* Exams screen — download code lookup modal. */
(function () {
    var E = window.Exams, t = E.t;

    E.openCodeModal = function (rec) {
        var m = document.getElementById('downloadCodeModal');
        m.querySelector('#dc-study').textContent     = rec.studyId + ' · ' + rec.patientName;
        m.querySelector('#dc-code').textContent      = rec.accessCodeRaw || '';
        m.querySelector('#dc-generated').textContent = rec.codeGeneratedAt || '\u2014';
        E.showModalEl(m);
    };

    document.getElementById('dc-copy').addEventListener('click', function () {
        var code = document.getElementById('dc-code').textContent;
        if (navigator.clipboard) navigator.clipboard.writeText(code);
        E.showToast(t("Código copiado."));
    });
})();
