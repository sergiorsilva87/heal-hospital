/* Exams screen — shared namespace, helpers and constants.
   window.Exams.data, .locale, .i18n and .t() are provided by the inline
   bootstrap rendered on the page (Razor). This module augments that namespace
   with helpers and lookup tables consumed by the table and modal modules. */
(function () {
    var E = window.Exams = window.Exams || {};

    /* HTML-escape helper for formatter output */
    E.escH = function (s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    };

    /* Confirmation toast */
    E.showToast = function (msg) {
        document.getElementById('rx-toast-body').textContent = msg;
        bootstrap.Toast.getOrCreateInstance(document.getElementById('rx-toast'), { delay: 3500 }).show();
    };

    /* Find a row record by its study id */
    E.recFromStd = function (std) {
        return E.data.find(function (d) { return d.studyId === std; });
    };

    /* Move modal to <body> and show it (avoids stacking-context clipping) */
    E.showModalEl = function (modal) {
        document.body.appendChild(modal);
        bootstrap.Modal.getOrCreateInstance(modal).show();
    };

    /* ── Modality abbreviation & badge colors (non-retired DICOM codes) ── */
    E.modAbbr = {
        'Radiografia': 'DX', 'Tomografia': 'CT', 'Ressonância': 'MR',
        'Ultrassonografia': 'US', 'Mamografia': 'MG', 'PET-CT': 'PT',
        'Fluoroscopia': 'RF', 'Cintilografia': 'NM', 'Angiografia': 'XA',
        'Densitometria': 'BDUS', 'Eletrocardiograma': 'ECG'
    };
    E.modClr = {
        CT:   { bg: '#fff3e0', c: '#bf360c' },
        MR:   { bg: '#f3e5f5', c: '#6a1b9a' },
        US:   { bg: '#e0f7fa', c: '#006064' },
        MG:   { bg: '#fce4ec', c: '#880e4f' },
        PT:   { bg: '#e8f5e9', c: '#1b5e20' },
        DX:   { bg: '#e3f2fd', c: '#0d47a1' },
        CR:   { bg: '#e8eaf6', c: '#1a237e' },
        NM:   { bg: '#f9fbe7', c: '#33691e' },
        RF:   { bg: '#e0f2f1', c: '#004d40' },
        XA:   { bg: '#fbe9e7', c: '#b71c1c' },
        ECG:  { bg: '#fff8e1', c: '#e65100' },
        BDUS: { bg: '#f1f8e9', c: '#558b2f' }
    };

    /* ── Status dot colors — keyed by stable PT label (must match #reception-legend) ── */
    E.statusDotClr = {
        'Liberação':  '#6a1b9a',
        'Disponível': '#0d47a1',
        'Laudando':   '#bf360c',
        'Pendência':  '#b71c1c',
        'Revisão':    '#bf360c',
        'Assinatura': '#bf360c',
        'Aprovado':   '#1b5e20',
        'Cancelado':  '#9e9e9e'
    };

    /* ── Global refresh ── */
    E.refreshAll = function () {
        var btn  = document.getElementById('exams-refresh-btn');
        var icon = btn ? btn.querySelector('i') : null;
        if (icon) icon.classList.add('anim-spin');
        if (E.table) {
            E.table.replaceData(E.data).then(function () {
                if (icon) icon.classList.remove('anim-spin');
                E.showToast(E.t('Lista atualizada.'));
            });
        }
    };

    /* ── Per-row refresh (mock) ── */
    E.refreshRow = function (rec) {
        if (!E.table) return;
        var rows = E.table.getRows();
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].getData().studyId === rec.studyId) {
                var el = rows[i].getElement();
                el.style.opacity    = '0.35';
                el.style.transition = 'opacity 0.15s';
                setTimeout(function (rowEl) {
                    rowEl.style.opacity    = '';
                    rowEl.style.transition = '';
                    E.showToast(E.t('Exame atualizado.'));
                }.bind(null, el), 800);
                break;
            }
        }
    };

    /* ── Bind global refresh button ── */
    var _rfBtn = document.getElementById('exams-refresh-btn');
    if (_rfBtn) _rfBtn.addEventListener('click', function () { E.refreshAll(); });
})();
