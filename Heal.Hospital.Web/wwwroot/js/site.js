/* ============================================================
   St. Mungus — Hospital System  |  site.js
   ============================================================ */
(function () {
    'use strict';

    // --- Font scale ----------------------------------------
    const FONT_KEY = 'hosp-font-scale';
    const STEP = 0.1;
    const MIN = 0.9;
    const MAX = 1.4;

    function normalize(v) { return Math.min(MAX, Math.max(MIN, Number(v.toFixed(2)))); }
    function getScale() {
        const s = parseFloat(localStorage.getItem(FONT_KEY) || '1');
        return Number.isNaN(s) ? 1 : normalize(s);
    }
    function applyScale(s) {
        const n = normalize(s);
        document.documentElement.style.setProperty('--hosp-font-scale', n.toString());
        localStorage.setItem(FONT_KEY, n.toString());
    }

    applyScale(getScale());

    const incBtn = document.getElementById('font-increase');
    const decBtn = document.getElementById('font-decrease');
    if (incBtn) incBtn.addEventListener('click', () => applyScale(getScale() + STEP));
    if (decBtn) decBtn.addEventListener('click', () => applyScale(getScale() - STEP));

    // --- Sidebar active link --------------------------------
    (function markActiveSidebarLink() {
        const path = window.location.pathname.toLowerCase();
        document.querySelectorAll('.hosp-sidebar-link').forEach(function (link) {
            const href = (link.getAttribute('href') || '').toLowerCase();
            if (href && href !== '/' && path.startsWith(href)) {
                link.classList.add('active');
                link.setAttribute('aria-current', 'page');
            }
        });
    })();

    // --- Mobile sidebar toggle (if needed) ------------------
    const sidebarToggle = document.getElementById('sidebar-toggle');
    const sidebar = document.querySelector('.hosp-sidebar');
    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', function () {
            sidebar.classList.toggle('sidebar-open');
        });
        document.addEventListener('click', function (e) {
            if (!sidebar.contains(e.target) && !sidebarToggle.contains(e.target)) {
                sidebar.classList.remove('sidebar-open');
            }
        });
    }
})();
