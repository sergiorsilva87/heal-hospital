/* Exams screen — filter panel, active-filter badge and warning banner. */
(function () {
    var E = window.Exams, t = E.t, table = E.table, data = E.data;

    /* ── Filter panel ─────────────────────────────── */
    var _procsByMod = {};
    data.forEach(function (d) {
        if (!_procsByMod[d.modality]) _procsByMod[d.modality] = [];
        if (_procsByMod[d.modality].indexOf(d.procedure) === -1)
            _procsByMod[d.modality].push(d.procedure);
    });
    Object.keys(_procsByMod).forEach(function (k) { _procsByMod[k].sort(); });

    /* Populate executor dropdown */
    var execs = data.filter(function (d) { return d.executingPhysician !== '—'; })
                    .map(function (d) { return d.executingPhysician; })
                    .filter(function (v, i, a) { return a.indexOf(v) === i; }).sort();
    var selE = document.getElementById('f-executor');
    execs.forEach(function (v) { var o = document.createElement('option'); o.value = v; o.textContent = v; selE.appendChild(o); });

    /* Populate unit dropdown */
    var units = data.map(function (d) { return d.unit; })
                    .filter(function (v, i, a) { return a.indexOf(v) === i; }).sort();
    var selU = document.getElementById('f-unit');
    units.forEach(function (v) { var o = document.createElement('option'); o.value = v; o.textContent = v; selU.appendChild(o); });

    /* Populate modality dropdown */
    var mods = data.map(function (d) { return d.modality; })
                   .filter(function (v, i, a) { return a.indexOf(v) === i; }).sort();
    var selMod = document.getElementById('f-modality');
    mods.forEach(function (v) { var o = document.createElement('option'); o.value = v; o.textContent = v; selMod.appendChild(o); });

    function buildProcs(mod) {
        var sel = document.getElementById('f-procedure');
        sel.innerHTML = '<option value="">' + E.escH(t("Todos")) + '</option>';
        if (!mod) { sel.disabled = true; return; }
        sel.disabled = false;
        (_procsByMod[mod] || []).forEach(function (v) {
            var o = document.createElement('option'); o.value = v; o.textContent = v; sel.appendChild(o);
        });
    }
    buildProcs('');

    document.getElementById('f-modality').addEventListener('change', function () {
        buildProcs(this.value);
        document.getElementById('f-procedure').value = '';
    });

    /* Active filter badge */
    function updateFilterBadge() {
        var count = 0;
        ['f-patient-id','f-patient-name','f-access-code'].forEach(function (id) {
            if (document.getElementById(id).value.trim()) count++;
        });
        ['f-executor','f-unit','f-modality','f-procedure','f-study-from','f-study-to','f-date-from','f-date-to','f-type','f-status'].forEach(function (id) {
            if (document.getElementById(id).value) count++;
        });
        ['f-emergency','f-pendency','f-critical-unnotified','f-critical-notified','f-archived'].forEach(function (id) {
            if (document.getElementById(id).checked) count++;
        });
        var badge = document.getElementById('rx-filter-badge');
        if (count > 0) {
            badge.textContent = count + (count === 1 ? ' ' + t("aplicado") : ' ' + t("aplicados"));
            badge.classList.remove('d-none');
        } else {
            badge.classList.add('d-none');
        }
    }

    function applyFilters() {
        var pid    = document.getElementById('f-patient-id').value.trim().toLowerCase();
        var pname  = document.getElementById('f-patient-name').value.trim().toLowerCase();
        var access = document.getElementById('f-access-code').value.trim().toLowerCase();
        var exec   = document.getElementById('f-executor').value;
        var unit   = document.getElementById('f-unit').value;
        var mod    = document.getElementById('f-modality').value;
        var proc   = document.getElementById('f-procedure').value;
        var dfrom  = document.getElementById('f-date-from').value;
        var dto    = document.getElementById('f-date-to').value;
        var sfrom  = document.getElementById('f-study-from').value;
        var sto    = document.getElementById('f-study-to').value;
        var type   = document.getElementById('f-type').value;
        var status = document.getElementById('f-status').value;
        var emer   = document.getElementById('f-emergency').checked;
        var pend   = document.getElementById('f-pendency').checked;
        var cun    = document.getElementById('f-critical-unnotified').checked;
        var cno    = document.getElementById('f-critical-notified').checked;
        var arch   = document.getElementById('f-archived').checked;
        table.setFilter(function (d) {
            if (pid    && d.patientId.toLowerCase().indexOf(pid) === -1)    return false;
            if (pname  && d.patientName.toLowerCase().indexOf(pname) === -1) return false;
            if (access && d.accessCodeRaw.toLowerCase().indexOf(access) === -1) return false;
            if (exec   && d.executingPhysician !== exec)                    return false;
            if (unit   && d.unit !== unit)                                   return false;
            if (mod    && d.modality !== mod)                                return false;
            if (proc   && d.procedure !== proc)                              return false;
            if (dfrom  && (d.liberationDateOnly < dfrom || !d.liberationDateOnly)) return false;
            if (dto    && (d.liberationDateOnly > dto   || !d.liberationDateOnly)) return false;
            if (sfrom  && d.studyDateOnly < sfrom) return false;
            if (sto    && d.studyDateOnly > sto)   return false;
            if (type   && d.typeLabel !== type)                              return false;
            if (status && d.statusLabel !== status)                          return false;
            if (emer   && !d.isEmergency)                                    return false;
            if (pend   && !d.hasPendency)                                    return false;
            if (cun && !cno && d.criticalFinding !== 'unnotified')           return false;
            if (cno && !cun && d.criticalFinding !== 'notified')             return false;
            if (cun && cno && !d.criticalFinding)                            return false;
            if (arch  && !d.isArchived)                                      return false;
            return true;
        });
        updateFilterBadge();
    }

    document.getElementById('f-apply').addEventListener('click', applyFilters);
    document.getElementById('f-clear').addEventListener('click', function () {
        ['f-patient-id','f-patient-name','f-access-code'].forEach(function (id) {
            document.getElementById(id).value = '';
        });
        ['f-executor','f-unit','f-modality','f-type','f-status','f-study-from','f-study-to','f-date-from','f-date-to'].forEach(function (id) {
            document.getElementById(id).value = '';
        });
        buildProcs('');
        ['f-emergency','f-pendency','f-critical-unnotified','f-critical-notified','f-archived'].forEach(function (id) {
            document.getElementById(id).checked = false;
        });
        table.clearFilter();
        updateFilterBadge();
    });
    document.querySelectorAll('#rx-filter-panel input[type="text"], #rx-filter-panel input[type="date"]').forEach(function (inp) {
        inp.addEventListener('keydown', function (e) { if (e.key === 'Enter') applyFilters(); });
    });

    /* ── Filter warning banner ─────────────────────── */
    (function () {
        var header = document.getElementById('rx-filter-header');
        var warn = document.getElementById('rx-filter-warning');
        if (!header || !warn) return;
        function refresh() {
            var badge = document.getElementById('rx-filter-badge');
            var hasFilters = badge && !badge.classList.contains('d-none');
            var collapsed = header.getAttribute('aria-expanded') !== 'true';
            warn.classList.toggle('d-none', !(hasFilters && collapsed));
        }
        document.getElementById('f-apply').addEventListener('click', function () { setTimeout(refresh, 0); });
        document.getElementById('f-clear').addEventListener('click', function () { setTimeout(refresh, 0); });
        document.getElementById('rx-filter-body').addEventListener('shown.bs.collapse', refresh);
        document.getElementById('rx-filter-body').addEventListener('hidden.bs.collapse', refresh);
        refresh();
    })();
})();
