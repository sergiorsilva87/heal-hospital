/* Exams screen — DICOM Send modal (C-MOVE / teleradiology forward). */
(function () {
    var E = window.Exams = window.Exams || {};

    var _modal = document.getElementById('dicomSendModal');
    if (!_modal) return;

    var _bsModal    = bootstrap.Modal.getOrCreateInstance(_modal);
    var _selServer  = document.getElementById('ds-server-select');
    var _secReg     = document.getElementById('ds-section-registered');
    var _secMan     = document.getElementById('ds-section-manual');
    var _regInfo    = document.getElementById('ds-registered-info');
    var _regAe      = document.getElementById('ds-reg-ae');
    var _regIp      = document.getElementById('ds-reg-ip');
    var _regPort    = document.getElementById('ds-reg-port');
    var _pingBtn    = document.getElementById('ds-ping');
    var _pingStatus = document.getElementById('ds-ping-status');
    var _sendBtn    = document.getElementById('ds-send-confirm');
    var _modeRadios = _modal.querySelectorAll('input[name="dsModeRadio"]');

    /* ── Helpers ── */
    var t = function (k) { return (E.t ? E.t(k) : k); };
    var showToast = function (msg) { if (E.showToast) E.showToast(msg); };
    var showModal = function () {
        if (E.showModalEl) {
            E.showModalEl(_modal);
        } else {
            document.body.appendChild(_modal);
            bootstrap.Modal.getOrCreateInstance(_modal).show();
        }
    };

    /* ── Populate server dropdown from bridge data ── */
    if (E.dicomServers && E.dicomServers.length) {
        E.dicomServers.forEach(function (s) {
            var opt = document.createElement('option');
            opt.value          = s.id;
            opt.textContent    = s.name + ' (' + s.aeTitle + ')';
            opt.dataset.ae     = s.aeTitle;
            opt.dataset.ip     = s.ip;
            opt.dataset.port   = s.port;
            _selServer.appendChild(opt);
        });
    }

    /* ── Mode toggle ── */
    function _getMode() {
        for (var i = 0; i < _modeRadios.length; i++) {
            if (_modeRadios[i].checked) return _modeRadios[i].value;
        }
        return 'registered';
    }

    function _applyMode() {
        var isReg = _getMode() === 'registered';
        _secReg.classList.toggle('d-none', !isReg);
        _secMan.classList.toggle('d-none',  isReg);
        _pingStatus.textContent = '';
    }

    _modeRadios.forEach(function (r) { r.addEventListener('change', _applyMode); });

    /* ── Show registered server details on dropdown change ── */
    _selServer.addEventListener('change', function () {
        var opt = _selServer.options[_selServer.selectedIndex];
        if (!opt.value) {
            _regInfo.classList.add('d-none');
            return;
        }
        _regAe.textContent   = opt.dataset.ae;
        _regIp.textContent   = opt.dataset.ip;
        _regPort.textContent = opt.dataset.port;
        _regInfo.classList.remove('d-none');
        _pingStatus.textContent = '';
    });

    /* ── Ping / C-ECHO mock ── */
    _pingBtn.addEventListener('click', function () {
        _pingBtn.disabled = true;
        _pingStatus.innerHTML =
            '<span class="spinner-border spinner-border-sm me-1" role="status"></span>'
            + t('Testando conexão...');
        setTimeout(function () {
            _pingBtn.disabled = false;
            var ok = Math.random() > 0.25;
            _pingStatus.innerHTML = ok
                ? '<i class="bi bi-check-circle-fill text-success me-1"></i><span class="text-success">' + t('Servidor disponível') + '</span>'
                : '<i class="bi bi-x-circle-fill text-danger me-1"></i><span class="text-danger">' + t('Servidor indisponível') + '</span>';
        }, 1200);
    });

    /* ── Send (mock C-MOVE) ── */
    _sendBtn.addEventListener('click', function () {
        _sendBtn.disabled = true;
        _sendBtn.innerHTML =
            '<span class="spinner-border spinner-border-sm me-1" role="status"></span>' + t('Enviando...');
        setTimeout(function () {
            _bsModal.hide();
            _sendBtn.disabled = false;
            _sendBtn.innerHTML = '<i class="bi bi-send me-1"></i>' + t('Reencaminhar');
            _pingStatus.textContent = '';
            showToast(t('Imagens reencaminhadas com sucesso.'));
        }, 1500);
    });

    /* ── Public API ── */
    E.openDicomSend = function (exam) {
        document.getElementById('ds-patient').textContent = exam.patientName || exam.patient || '—';
        document.getElementById('ds-study').textContent   = exam.studyId     || exam.id      || '—';
        /* reset state */
        _modeRadios[0].checked = true;
        _applyMode();
        _selServer.value = '';
        _regInfo.classList.add('d-none');
        document.getElementById('ds-ae').value   = '';
        document.getElementById('ds-port').value = '';
        document.getElementById('ds-ip').value   = '';
        showModal();
    };

    /* ── Legacy table: event delegation via data attribute ── */
    document.addEventListener('click', function (e) {
        var btn = e.target.closest('[data-rx-dicom-send]');
        if (!btn) return;
        E.openDicomSend({ patientName: btn.dataset.patient, studyId: btn.dataset.study });
    });
})();
