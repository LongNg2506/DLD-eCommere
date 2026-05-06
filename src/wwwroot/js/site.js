// =============================================
// THEME TOGGLE
// =============================================
(function initTheme() {
    const savedTheme = localStorage.getItem('chrono-theme') || 'light';
    document.documentElement.setAttribute('data-theme', savedTheme);
    updateThemeIcon(savedTheme);
})();

function updateThemeIcon(theme) {
    const lightIcon = document.getElementById('themeIconLight');
    const darkIcon = document.getElementById('themeIconDark');
    if (lightIcon && darkIcon) {
        lightIcon.style.display = theme === 'light' ? 'block' : 'none';
        darkIcon.style.display = theme === 'dark' ? 'block' : 'none';
    }
}

document.getElementById('themeToggle')?.addEventListener('click', function () {
    const current = document.documentElement.getAttribute('data-theme');
    const next = current === 'dark' ? 'light' : 'dark';
    document.documentElement.setAttribute('data-theme', next);
    localStorage.setItem('chrono-theme', next);
    updateThemeIcon(next);
});

// =============================================
// CART DRAWER (Server-side)
// =============================================

function getCartUrls() {
    const el = document.getElementById('cartUrls');
    if (!el) return { addUrl: '', removeUrl: '', updateUrl: '', countUrl: '', partialUrl: '' };
    return {
        addUrl: el.dataset.addUrl || '',
        removeUrl: el.dataset.removeUrl || '',
        updateUrl: el.dataset.updateUrl || '',
        countUrl: el.dataset.countUrl || '',
        partialUrl: el.dataset.partialUrl || ''
    };
}

function getAntiForgeryToken() {
    return document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';
}

function openCart() {
    document.getElementById('cartDrawer')?.classList.add('open');
    document.getElementById('cartOverlay')?.classList.add('open');
    document.body.style.overflow = 'hidden';
    loadCartDrawer();
}

function closeCart() {
    document.getElementById('cartDrawer')?.classList.remove('open');
    document.getElementById('cartOverlay')?.classList.remove('open');
    document.body.style.overflow = '';
}

async function loadCartDrawer() {
    const urls = getCartUrls();
    if (!urls.partialUrl) return;
    try {
        const res = await fetch(urls.partialUrl, {
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });
        if (!res.ok) return;

        const cartBody = document.getElementById('cartBody');
        const text = await res.text();
        if (cartBody) {
            cartBody.innerHTML = text;
        }
        updateCartBadgeFromHtml();
    } catch (e) {
        console.error('Failed to load cart:', e);
    }
}

function updateCartBadgeFromHtml() {
    const items = document.querySelectorAll('#cartBody .cart-item');
    const badge = document.getElementById('cartBadge');
    const itemCountEl = document.getElementById('cartItemCount');
    const totalEl = document.getElementById('cartTotal');
    const emptyEl = document.getElementById('cartEmpty');
    const footerEl = document.getElementById('cartFooter');

    let totalQty = 0;
    items.forEach(item => {
        const qtyEl = item.querySelector('.qty-value');
        if (qtyEl) totalQty += parseInt(qtyEl.textContent) || 0;
    });

    if (badge) badge.textContent = totalQty;
    if (itemCountEl) itemCountEl.textContent = totalQty;

    const hiddenTotal = document.getElementById('cartHiddenTotal');
    if (hiddenTotal) {
        const totalAmount = parseFloat(hiddenTotal.value) || 0;
        if (totalEl) totalEl.textContent = formatVND(totalAmount);
    }

    if (items.length === 0) {
        if (emptyEl) emptyEl.style.display = 'block';
        if (footerEl) footerEl.style.display = 'none';
    } else {
        if (emptyEl) emptyEl.style.display = 'none';
        if (footerEl) footerEl.style.display = 'block';
    }
}

// addToCart: supports both old 5-param signature and new 2-param
function addToCart(productId, name, salePrice, originalPrice, discount) {
    const urls = getCartUrls();
    if (!urls.addUrl) {
        showToast('Cart service unavailable', 'error');
        return;
    }

    const token = getAntiForgeryToken();
    fetch(urls.addUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': token,
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: `productId=${productId}&quantity=1`
    })
    .then(res => res.json())
    .then(data => {
        if (data.success) {
            showToast(`${name} added to cart!`, 'success');
            updateCartBadge();
        } else {
            showToast(data.message || 'Failed to add to cart', 'error');
        }
    })
    .catch(() => showToast('Failed to add to cart', 'error'));
}

async function removeFromCart(productId) {
    const urls = getCartUrls();
    if (!urls.removeUrl) return;
    const token = getAntiForgeryToken();
    try {
        const res = await fetch(urls.removeUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'RequestVerificationToken': token,
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: `productId=${productId}`
        });
        const data = await res.json();
        if (data.success) {
            loadCartDrawer();
            updateCartBadge();
        }
    } catch (e) { showToast('Failed to remove item', 'error'); }
}

async function changeQty(productId, delta) {
    const qtyEl = document.querySelector(`[data-product-id="${productId}"] .qty-value`);
    if (!qtyEl) {
        loadCartDrawer();
        return;
    }
    const currentQty = parseInt(qtyEl.textContent) || 0;
    const newQty = currentQty + delta;
    if (newQty <= 0) {
        removeFromCart(productId);
        return;
    }

    const urls = getCartUrls();
    if (!urls.updateUrl) return;
    const token = getAntiForgeryToken();
    try {
        const res = await fetch(urls.updateUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'RequestVerificationToken': token,
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: `productId=${productId}&quantity=${newQty}`
        });
        const data = await res.json();
        if (data.success) {
            loadCartDrawer();
        } else {
            showToast(data.message || 'Failed to update quantity', 'error');
        }
    } catch (e) { showToast('Failed to update quantity', 'error'); }
}

async function updateCartBadge() {
    const urls = getCartUrls();
    if (!urls.countUrl) return;
    try {
        const res = await fetch(urls.countUrl, {
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });
        if (res.ok) {
            const data = await res.json();
            const badge = document.getElementById('cartBadge');
            if (badge) badge.textContent = data.count || 0;
        }
    } catch (e) {}
}

function formatVND(amount) {
    return Number(amount).toLocaleString('en-US') + ' VND';
}

function escapeHtml(str) {
    const div = document.createElement('div');
    div.textContent = str;
    return div.innerHTML;
}

function goToCheckout() {
    window.location.href = '/Checkout';
}

document.getElementById('cartBtn')?.addEventListener('click', openCart);
document.getElementById('cartClose')?.addEventListener('click', closeCart);
document.getElementById('cartOverlay')?.addEventListener('click', closeCart);
document.addEventListener('DOMContentLoaded', updateCartBadge);

// =============================================
// TOAST NOTIFICATIONS
// =============================================
function showToast(message, type = 'success') {
    const container = document.getElementById('toastContainer');
    if (!container) return;

    const icon = type === 'success'
        ? '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round"><polyline points="20 6 9 17 4 12"/></svg>'
        : '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round"><circle cx="12" cy="12" r="10"/><line x1="15" y1="9" x2="9" y2="15"/><line x1="9" y1="9" x2="15" y2="15"/></svg>';

    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.innerHTML = `<span class="toast-icon">${icon}</span><span class="toast-message">${escapeHtml(message)}</span>`;
    container.appendChild(toast);

    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(100px)';
        toast.style.transition = 'all 0.3s ease';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// =============================================
// FLASH DEALS TIMER
// =============================================
function initFlashTimer(hoursLeft = 10) {
    let totalSeconds = hoursLeft * 3600;

    function updateTimer() {
        const h = Math.floor(totalSeconds / 3600);
        const m = Math.floor((totalSeconds % 3600) / 60);
        const s = totalSeconds % 60;

        const el = document.getElementById('flashTimer');
        if (el) {
            el.innerHTML = `
                <div class="timer-block"><span class="timer-number">${String(h).padStart(2, '0')}</span><span class="timer-label">Hours</span></div>
                <span class="timer-sep">:</span>
                <div class="timer-block"><span class="timer-number">${String(m).padStart(2, '0')}</span><span class="timer-label">Min</span></div>
                <span class="timer-sep">:</span>
                <div class="timer-block"><span class="timer-number">${String(s).padStart(2, '0')}</span><span class="timer-label">Sec</span></div>
            `;
        }

        if (totalSeconds > 0) {
            totalSeconds--;
        }
    }

    updateTimer();
    setInterval(updateTimer, 1000);
}

// =============================================
// SCROLL ANIMATIONS
// =============================================
function initScrollAnimations() {
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.1 });

    document.querySelectorAll('.fade-in-up').forEach(el => observer.observe(el));
}

// =============================================
// WISHLIST TOGGLE
// =============================================
document.addEventListener('click', function (e) {
    const btn = e.target.closest('.wishlist');
    if (btn) {
        btn.classList.toggle('active');
        const isActive = btn.classList.contains('active');
        showToast(isActive ? 'Added to wishlist!' : 'Removed from wishlist', 'success');
    }
});

// =============================================
// SEARCH PANEL TOGGLE
// =============================================
const searchToggle = document.getElementById('searchToggle');
const searchPanel = document.getElementById('headerSearchPanel');
const searchInput = searchPanel?.querySelector('input');

searchToggle?.addEventListener('click', function () {
    const isOpen = searchPanel?.classList.contains('open');
    if (searchPanel) {
        searchPanel.classList.toggle('open');
    }
    if (!isOpen && searchInput) {
        setTimeout(() => searchInput.focus(), 50);
    }
});

document.addEventListener('click', function (e) {
    if (searchPanel?.classList.contains('open') &&
        !searchPanel.contains(e.target) &&
        !searchToggle?.contains(e.target)) {
        searchPanel.classList.remove('open');
    }
});

// =============================================
// MOBILE MENU TOGGLE
// =============================================
const mobileMenuBtn = document.getElementById('mobileMenuBtn');
const mobileNav = document.getElementById('headerMobileNav');

mobileMenuBtn?.addEventListener('click', function () {
    mobileNav?.classList.toggle('open');
    const isOpen = mobileNav?.classList.contains('open');
    mobileMenuBtn.setAttribute('aria-expanded', String(isOpen));
});

document.addEventListener('click', function (e) {
    if (mobileNav?.classList.contains('open') &&
        !mobileNav.contains(e.target) &&
        !mobileMenuBtn?.contains(e.target)) {
        mobileNav.classList.remove('open');
        mobileMenuBtn?.setAttribute('aria-expanded', 'false');
    }
});

// =============================================
// INIT ON LOAD
// =============================================
document.addEventListener('DOMContentLoaded', function () {
    initScrollAnimations();

    const timerEl = document.getElementById('flashTimer');
    if (timerEl) {
        initFlashTimer(10);
    }

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') closeCart();
    });
});

// ================================
// Header Search Slide
// ================================
document.addEventListener("DOMContentLoaded", function () {
    const searchWrap = document.getElementById("headerSearchWrap");
    const searchToggle = document.getElementById("searchToggle");
    const searchInput = document.querySelector(".header-search-input");

    if (!searchWrap || !searchToggle || !searchInput) return;

    searchToggle.addEventListener("click", function (e) {
        e.preventDefault();
        e.stopPropagation();

        searchWrap.classList.add("active");

        setTimeout(function () {
            searchInput.focus();
        }, 180);
    });

    document.addEventListener("click", function (e) {
        if (!searchWrap.contains(e.target)) {
            searchWrap.classList.remove("active");
        }
    });

    document.addEventListener("keydown", function (e) {
        if (e.key === "Escape") {
            searchWrap.classList.remove("active");
            searchToggle.focus();
        }
    });
});

// =============================================
// QUICK VIEW MODAL
// =============================================
function openQuickView(id, name, collection, img, desc, f1, f2, f3) {
    const overlay = document.getElementById('qvOverlay');
    if (!overlay) return;

    document.getElementById('qvImg').src = img;
    document.getElementById('qvImg').alt = name;
    document.getElementById('qvCollection').textContent = collection;
    document.getElementById('qvName').textContent = name;
    document.getElementById('qvDesc').textContent = desc;
    document.getElementById('qvFeature1').textContent = f1;
    document.getElementById('qvFeature2').textContent = f2;
    document.getElementById('qvFeature3').textContent = f3;

    const detailLink = document.getElementById('qvLinkDetail');
    if (detailLink) detailLink.href = '/Shop/ProductDetail/' + id;

    overlay.classList.add('active');
    document.body.style.overflow = 'hidden';
}

function closeQuickView(e) {
    if (e && e.target !== e.currentTarget) return;
    const overlay = document.getElementById('qvOverlay');
    if (overlay) {
        overlay.classList.remove('active');
        document.body.style.overflow = '';
    }
}

document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') closeQuickView();
});
