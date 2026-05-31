/* ============================================================
   St. Mungus — Hospital System  |  site.js
   ============================================================ */
(function () {
    'use strict';

    // --- Font scale (fixed at 1.0; user toggle removed) -----
    document.documentElement.style.setProperty('--hosp-font-scale', '1');

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
