/* Exams screen — SLA delay: banner counts, column-1 icon modal and Google Charts
   (gauge, sankey, timeline). Reads window.Exams.data[i].delay produced by the page model. */
(function () {
    var E = window.Exams, t = E.t, escH = E.escH;

    /* ── Google Charts: load gauge/sankey/timeline (loader.js is global in _AuthLayout) ── */
    var _ready = false, _queue = [];
    if (window.google && google.charts) {
        google.charts.load('current', { packages: ['gauge', 'sankey', 'timeline'] });
        google.charts.setOnLoadCallback(function () {
            _ready = true;
            _queue.splice(0).forEach(function (fn) { fn(); });
        });
    }
    function whenReady(fn) { if (_ready) fn(); else _queue.push(fn); }

    /* ── Helpers ─────────────────────────────────────── */
    function fmtDuration(min) {
        min = Math.max(0, Math.round(min));
        if (min < 60) return min + ' ' + t("min");
        var h = Math.floor(min / 60), m = min % 60;
        return m ? (h + 'h ' + m + ' ' + t("min")) : (h + 'h');
    }
    function stateLabel(s) {
        return s === 'overdue' ? t("Atrasado")
             : s === 'near'    ? t("Próximo da expiração")
             : t("No prazo");
    }

    /* ── Chips: count overdue / near and toggle the pill indicators ── */
    E.refreshDelayBanner = function () {
        var over = 0, near = 0;
        E.data.forEach(function (d) {
            if (!d.delay) return;
            if (d.delay.state === 'overdue') over++;
            else if (d.delay.state === 'near') near++;
        });
        var overEl = document.getElementById('rx-delay-chip-overdue');
        var nearEl = document.getElementById('rx-delay-chip-near');
        if (overEl) {
            document.getElementById('rx-delay-overdue-count').textContent = over;
            overEl.classList.toggle('d-none', over === 0);
        }
        if (nearEl) {
            document.getElementById('rx-delay-near-count').textContent = near;
            nearEl.classList.toggle('d-none', near === 0);
        }
    };

    /* Chip click → enable the matching filter switch and apply */
    var chips = document.getElementById('rx-delay-chips');
    if (chips) {
        chips.addEventListener('click', function (e) {
            var chip = e.target.closest('[data-rx-delay-filter]');
            if (!chip) return;
            var which = chip.dataset.rxDelayFilter; // overdue | near
            var cb = document.getElementById(which === 'overdue' ? 'f-overdue' : 'f-near');
            if (cb) {
                cb.checked = true;
                var apply = document.getElementById('f-apply');
                if (apply) apply.click();
            }
        });
    }

    /* ── Chart drawing ───────────────────────────────── */
    function drawGauge(dl) {
        whenReady(function () {
            var el = document.getElementById('dly-gauge');
            if (!el) return;
            var val = Math.min(100, Math.round(dl.consumedPct));
            var data = google.visualization.arrayToDataTable([
                ['Label', 'Value'],
                [t("Prazo"), val]
            ]);
            var opts = {
                width: 220, height: 220, max: 100, min: 0, minorTicks: 5,
                greenFrom: 0, greenTo: 80, yellowFrom: 80, yellowTo: 95,
                redFrom: 95, redTo: 100,
                greenColor: '#2e7d32', yellowColor: '#ef6c00', redColor: '#c62828'
            };
            new google.visualization.Gauge(el).draw(data, opts);
        });
    }

    function steps(dl) {
        // [labelFrom, labelTo, startMs, endMs] for the consecutive stages that exist
        var out = [];
        function add(fromKey, toKey, a, b) {
            if (a == null || b == null) return;
            if (b <= a) return;
            out.push([t(fromKey), t(toKey), a, b]);
        }
        var nowMs = dl.nowMs;
        add("Estudo", "Última imagem", dl.studyMs, dl.lastImageMs);
        add("Última imagem", "Liberação", dl.lastImageMs, dl.liberationMs);
        add("Liberação", "Início do laudo", dl.liberationMs, dl.reportStartMs);
        // last stage: report start → finish (or → now if still open)
        var endMs = dl.reportFinishMs != null ? dl.reportFinishMs : nowMs;
        var endLbl = dl.reportFinishMs != null ? "Finalização" : "Em andamento";
        add("Início do laudo", endLbl, dl.reportStartMs, endMs);
        return out;
    }

    function drawSankey(dl) {
        whenReady(function () {
            var el = document.getElementById('dly-sankey');
            if (!el) return;
            var rows = steps(dl).map(function (s) {
                return [s[0], s[1], Math.max(1, Math.round((s[3] - s[2]) / 60000))]; // weight = minutes
            });
            if (!rows.length) { el.innerHTML = '<p class="text-muted text-center py-4" style="font-size:var(--fs-xs)">' + escH(t("Sem dados suficientes para o fluxo.")) + '</p>'; return; }
            var data = new google.visualization.DataTable();
            data.addColumn('string', t("De"));
            data.addColumn('string', t("Para"));
            data.addColumn('number', t("Minutos"));
            data.addRows(rows);
            new google.visualization.Sankey(el).draw(data, {
                width: '100%', height: 300,
                sankey: { node: { label: { fontSize: 11 } } }
            });
        });
    }

    function drawTimeline(dl) {
        whenReady(function () {
            var el = document.getElementById('dly-timeline-chart');
            if (!el) return;
            var rows = steps(dl);
            if (!rows.length) { el.innerHTML = '<p class="text-muted text-center py-4" style="font-size:var(--fs-xs)">' + escH(t("Sem dados suficientes para a linha do tempo.")) + '</p>'; return; }
            var chart = new google.visualization.Timeline(el);
            var data = new google.visualization.DataTable();
            data.addColumn({ type: 'string', id: 'Etapa' });
            data.addColumn({ type: 'string', id: 'Rótulo' });
            data.addColumn({ type: 'date', id: 'Início' });
            data.addColumn({ type: 'date', id: 'Fim' });
            data.addRows(rows.map(function (s) {
                return [s[0] + ' → ' + s[1], fmtDuration((s[3] - s[2]) / 60000), new Date(s[2]), new Date(s[3])];
            }));
            chart.draw(data, { height: 300, timeline: { showRowLabels: true } });
        });
    }

    /* ── Modal open / fill ───────────────────────────── */
    var _cur = null;
    E.openDelayModal = function (rec) {
        if (!rec || !rec.delay) return;
        _cur = rec.delay;
        var modal = document.getElementById('delayModal');
        modal.querySelector('#dly-study').textContent = rec.studyId + ' · ' + rec.patientName;

        modal.querySelector('#dly-study-at').textContent   = _cur.studyAt;
        modal.querySelector('#dly-lastimg-at').textContent = _cur.lastImageAt;
        modal.querySelector('#dly-lib-at').textContent     = _cur.liberationAt;
        modal.querySelector('#dly-start-at').textContent   = _cur.reportStartAt;
        modal.querySelector('#dly-finish-at').textContent  = _cur.reportFinishAt;

        modal.querySelector('#dly-sla-summary').innerHTML =
            '<div class="d-flex flex-wrap gap-3 align-items-center">'
          + '<span><b>' + escH(t("Alvo")) + ':</b> ' + escH(fmtDuration(_cur.targetMin)) + '</span>'
          + '<span><b>' + escH(t("Consumido")) + ':</b> ' + escH(fmtDuration(_cur.elapsedMin)) + ' (' + escH(String(_cur.consumedPct)) + '%)</span>'
          + '<span class="dly-pill ' + escH(_cur.state) + '">' + escH(stateLabel(_cur.state)) + '</span>'
          + '</div>';

        E.showModalEl(modal);
        drawGauge(_cur);
    };

    /* Redraw the chart of whichever tab becomes visible (needs a sized container) */
    (function () {
        var modal = document.getElementById('delayModal');
        if (!modal) return;
        modal.querySelectorAll('#dly-tabs button[data-bs-toggle="tab"]').forEach(function (btn) {
            btn.addEventListener('shown.bs.tab', function (ev) {
                if (!_cur) return;
                var tgt = ev.target.getAttribute('data-bs-target');
                if (tgt === '#dly-dates') drawGauge(_cur);
                else if (tgt === '#dly-flow') drawSankey(_cur);
                else if (tgt === '#dly-timeline') drawTimeline(_cur);
            });
        });
    })();

    /* Initial banner render */
    E.refreshDelayBanner();
})();
