/* ===== ADMIN SIDEBAR TOGGLE ===== */
(function() {
  var menuToggle = document.getElementById('menuToggle');
  var sidebarClose = document.getElementById('sidebarClose');
  var sidebar = document.getElementById('adminSidebar');
  var overlay = document.getElementById('adminOverlay');

  if (menuToggle && sidebar) {
    menuToggle.addEventListener('click', function() {
      sidebar.classList.add('open');
      if (overlay) overlay.classList.add('show');
    });
  }

  if (sidebarClose && sidebar) {
    sidebarClose.addEventListener('click', function() {
      sidebar.classList.remove('open');
      if (overlay) overlay.classList.remove('show');
    });
  }

  if (overlay) {
    overlay.addEventListener('click', function() {
      sidebar.classList.remove('open');
      overlay.classList.remove('show');
    });
  }

  /* ===== ADMIN THEME TOGGLE ===== */
  var themeToggle = document.getElementById('adminThemeToggle');
  var themeIconLight = document.getElementById('adminThemeIconLight');
  var themeIconDark = document.getElementById('adminThemeIconDark');

  function updateAdminThemeIcon() {
    var isDark = document.documentElement.getAttribute('data-theme') === 'dark';
    if (themeIconLight) themeIconLight.style.display = isDark ? 'none' : 'inline-block';
    if (themeIconDark) themeIconDark.style.display = isDark ? 'inline-block' : 'none';
  }

  if (themeToggle) {
    themeToggle.addEventListener('click', function() {
      var current = document.documentElement.getAttribute('data-theme');
      var next = current === 'dark' ? 'light' : 'dark';
      document.documentElement.setAttribute('data-theme', next);
      localStorage.setItem('dld-theme', next);
      updateAdminThemeIcon();
    });
  }

  updateAdminThemeIcon();
})();
