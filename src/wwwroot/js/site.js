// =============================================
// THEME TOGGLE
// =============================================
function updateThemeIcon(theme) {
    const lightIcon = document.getElementById('themeIconLight');
    const darkIcon = document.getElementById('themeIconDark');
    if (lightIcon && darkIcon) {
        lightIcon.style.display = theme === 'light' ? 'block' : 'none';
        darkIcon.style.display = theme === 'dark' ? 'block' : 'none';
    }
}

function initTheme() {
    const savedTheme = localStorage.getItem('chrono-theme') || 'light';
    updateThemeIcon(savedTheme);
}

document.addEventListener('DOMContentLoaded', function() {
    initTheme();

    document.getElementById('themeToggle')?.addEventListener('click', function () {
        const current = document.documentElement.getAttribute('data-theme');
        const next = current === 'dark' ? 'light' : 'dark';
        document.documentElement.setAttribute('data-theme', next);
        localStorage.setItem('chrono-theme', next);
        updateThemeIcon(next);
    });
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

    if (badge) {
        badge.textContent = totalQty;
        // Hide badge when count is 0
        if (totalQty === 0) {
            badge.classList.add('hidden');
        } else {
            badge.classList.remove('hidden');
        }
    }
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

// addToCart: supports both old 5-param signature and new 6-param with button element
function addToCart(productId, name, salePrice, originalPrice, discount, btnElement) {
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
            if (btnElement) {
                createFlyToCartAnimation(btnElement, name);
            }
            updateCartBadge();
        } else {
            showToast(data.message || 'Failed to add to cart', 'error');
        }
    })
    .catch(() => showToast('Failed to add to cart', 'error'));
}

function createFlyToCartAnimation(btnElement, productName) {
    const cartBtn = document.getElementById('cartBtn');
    const badge = document.getElementById('cartBadge');

    if (!cartBtn) return;

    const productScope = btnElement.closest('.product-card, .pdetail-grid, .pdetail-main, .pdetail-content') || document;
    const imgElement = Array.from(productScope.querySelectorAll('.product-image-wrap img, .pdetail-main-image img, .pdetail-image img, img'))
        .find(img => img.src && img.getBoundingClientRect().width > 0 && img.getBoundingClientRect().height > 0);

    const sourceRect = imgElement?.getBoundingClientRect?.() || btnElement.getBoundingClientRect();
    const cartRect = cartBtn.getBoundingClientRect();

    const flyEl = document.createElement('div');
    flyEl.className = 'fly-to-cart-item';

    if (imgElement && imgElement.src) {
        const img = document.createElement('img');
        img.src = imgElement.src;
        img.alt = productName;
        flyEl.appendChild(img);
    } else {
        const placeholder = document.createElement('div');
        placeholder.className = 'product-placeholder';
        placeholder.innerHTML = '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><circle cx="12" cy="12" r="10"/><path d="M12 6V12L16 14"/></svg>';
        flyEl.appendChild(placeholder);
    }

    const itemSize = 72;
    const startX = sourceRect.left + sourceRect.width / 2 - itemSize / 2;
    const startY = sourceRect.top + sourceRect.height / 2 - itemSize / 2;
    flyEl.style.left = startX + 'px';
    flyEl.style.top = startY + 'px';

    document.body.appendChild(flyEl);

    const endX = cartRect.left + cartRect.width / 2 - itemSize / 2;
    const endY = cartRect.top + cartRect.height / 2 - itemSize / 2;
    const dx = endX - startX;
    const dy = endY - startY;
    const arcY = Math.min(-80, dy * 0.35 - 80);

    flyEl.style.setProperty('--fly-dx', dx + 'px');
    flyEl.style.setProperty('--fly-dy', dy + 'px');
    flyEl.style.setProperty('--fly-mid-dx', (dx * 0.55) + 'px');
    flyEl.style.setProperty('--fly-mid-dy', (dy * 0.55 + arcY) + 'px');

    const animation = flyEl.animate([
        { transform: 'translate3d(0, 0, 0) scale(1)', opacity: 1 },
        { transform: `translate3d(${dx * 0.45}px, ${dy * 0.45 + arcY}px, 0) scale(0.9)`, opacity: 0.95, offset: 0.45 },
        { transform: `translate3d(${dx}px, ${dy}px, 0) scale(0.18)`, opacity: 0 }
    ], {
        duration: 900,
        easing: 'cubic-bezier(0.22, 1, 0.36, 1)',
        fill: 'forwards'
    });

    window.setTimeout(() => {
        cartBtn.classList.add('cart-bounce');
        badge?.classList.add('cart-bounce');
        window.setTimeout(() => {
            cartBtn.classList.remove('cart-bounce');
            badge?.classList.remove('cart-bounce');
        }, 400);
    }, 620);

    animation.finished.finally(() => flyEl.remove());
}

function initHeroCounters() {
    const counters = document.querySelectorAll('.hero-stats .hero-stat h3');
    if (!counters.length) return;

    counters.forEach(counter => {
        const originalText = counter.textContent.trim();
        const target = parseInt(originalText.replace(/[^\d]/g, ''), 10);
        const suffix = originalText.replace(/[\d,.\s]/g, '');

        if (!Number.isFinite(target)) return;

        counter.dataset.target = String(target);
        counter.dataset.suffix = suffix;
        counter.textContent = '0' + suffix;
    });

    const runCounter = counter => {
        if (counter.dataset.counted === 'true') return;
        counter.dataset.counted = 'true';

        const target = parseInt(counter.dataset.target || '0', 10);
        const suffix = counter.dataset.suffix || '';
        const duration = 1400;
        const startTime = performance.now();

        const tick = now => {
            const progress = Math.min((now - startTime) / duration, 1);
            const eased = 1 - Math.pow(1 - progress, 3);
            const value = Math.round(target * eased);
            counter.textContent = value.toLocaleString('en-US') + suffix;

            if (progress < 1) {
                requestAnimationFrame(tick);
            }
        };

        requestAnimationFrame(tick);
    };

    if (!('IntersectionObserver' in window)) {
        counters.forEach(runCounter);
        return;
    }

    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (!entry.isIntersecting) return;
            runCounter(entry.target);
            observer.unobserve(entry.target);
        });
    }, { threshold: 0.35 });

    counters.forEach(counter => observer.observe(counter));
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
            if (badge) {
                badge.textContent = data.count || 0;
                // Hide badge when count is 0
                if (data.count === 0) {
                    badge.classList.add('hidden');
                } else {
                    badge.classList.remove('hidden');
                }
            }
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

// =============================================
// CAROUSEL
// =============================================
document.addEventListener('DOMContentLoaded', function () {
  document.querySelectorAll('.carousel-wrapper').forEach(function (wrapper) {
    var track = wrapper.querySelector('.carousel-track');
    var prevBtn = wrapper.querySelector('.carousel-prev');
    var nextBtn = wrapper.querySelector('.carousel-next');
    if (!track || !prevBtn || !nextBtn) return;

    function getCardStep() {
      var card = track.querySelector('.product-card');
      if (!card) return 324;
      return card.offsetWidth + 24;
    }

    function updateButtons() {
      var max = track.scrollWidth - track.clientWidth;
      var sl = track.scrollLeft;
      var atStart = sl <= 2;
      var atEnd = sl >= max - 2;
      prevBtn.style.opacity = atStart ? '0.35' : '1';
      prevBtn.style.pointerEvents = atStart ? 'none' : 'auto';
      nextBtn.style.opacity = atEnd ? '0.35' : '1';
      nextBtn.style.pointerEvents = atEnd ? 'none' : 'auto';
    }

    prevBtn.addEventListener('click', function () {
      var target = Math.max(0, track.scrollLeft - getCardStep());
      track.scrollTo({ left: target, behavior: 'smooth' });
    });

    nextBtn.addEventListener('click', function () {
      var max = track.scrollWidth - track.clientWidth;
      var target = Math.min(max, track.scrollLeft + getCardStep());
      track.scrollTo({ left: target, behavior: 'smooth' });
    });

    track.addEventListener('scroll', updateButtons, { passive: true });
    window.addEventListener('resize', function () {
      var max = track.scrollWidth - track.clientWidth;
      if (track.scrollLeft > max) track.scrollLeft = max;
      updateButtons();
    });

    track.scrollLeft = 0;
    updateButtons();
  });
});
