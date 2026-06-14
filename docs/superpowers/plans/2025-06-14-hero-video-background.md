# Hero Video Background Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the two-column hero layout with featured watch card to a full-width video background with centered content.

**Architecture:** Update Home/Index view to remove right column, add video element with overlay, and center text content. Adjust CSS for video positioning, overlay, and content centering.

**Tech Stack:** ASP.NET Core MVC, Razor Views, CSS3

---

### Task 1: Update Hero Section HTML Structure

**Files:**
- Modify: `Views/Home/Index.cshtml:1-87`

- [ ] **Step 1:** Remove the right column (`col-lg-6` with `hero-image-area` and `hero-product-card`) from hero section

Existing structure (lines 51-84) will be removed. The left column `hero-content` will become the only column.

- [ ] **Step 2:** Add video background element and overlay

Replace current hero-section content with:
```html
<section class="hero-section">
    <!-- Video Background -->
    <video autoplay muted loop playsinline class="hero-video-bg"
           poster="@Url.Content("~/images/hero-poster.jpg")">
        <source src="https://assets.mixkit.co/videos/preview/mixkit-wrist-watch-on-a-white-background-32807-large.mp4"
                type="video/mp4" />
    </video>

    <!-- Dark Overlay -->
    <div class="hero-overlay"></div>

    <!-- Content Container -->
    <div class="container hero-content-wrapper">
        <div class="hero-content">
            <!-- Keep existing content from lines 13-49 -->
        </div>
    </div>
</section>
```

- [ ] **Step 3:** Commit changes

```bash
git add Views/Home/Index.cshtml
git commit -m "feat: add video background to hero section"
```

---

### Task 2: Add CSS for Video Background and Centering

**Files:**
- Modify: `wwwroot/css/site.css:689-720`

- [ ] **Step 1:** Update `hero-section` CSS for relative positioning and padding

Change:
```css
.hero-section {
  position: relative;
  overflow: hidden;
  padding: 120px 0 100px;
}
```

Keep the existing background properties (they will be behind the video).

- [ ] **Step 2:** Add new CSS rules for video, overlay, and content wrapper

Add after line 719 (after `}` of `.hero-section::after`):
```css
/* Hero Video Background */
.hero-video-bg {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
  z-index: 0;
}

.hero-overlay {
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.4);
  z-index: 1;
}

.hero-content-wrapper {
  position: relative;
  z-index: 2;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 70vh;
}

.hero-content {
  text-align: center;
  max-width: 800px;
}
```

- [ ] **Step 3:** Center the hero stats on larger screens

Find `.hero-stats` (around line 819) and ensure:
```css
.hero-stats {
  display: flex;
  gap: 32px;
  margin-top: 48px;
  justify-content: center;
  flex-wrap: wrap;
}
```

- [ ] **Step 4:** Commit changes

```bash
git add wwwroot/css/site.css
git commit -m "style: add hero video background styles"
```

---

### Task 3: Adjust Hero Content Styles for Video

**Files:**
- Modify: `wwwroot/css/site.css:721-750`

- [ ] **Step 1:** Ensure `hero-content` has proper z-index and positioning

The existing `.hero-content` has `position: relative; z-index: 1;` (line 721-724). Change `z-index` to ensure it's above video:
```css
.hero-content {
  position: relative;
  z-index: 2;
}
```

- [ ] **Step 2:** Adjust hero badge for better contrast on video

Find `.hero-badge` (line 717-728). Ensure it has semi-transparent background for readability:
```css
.hero-badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: rgba(169, 206, 194, 0.9);
  color: var(--accent-dark);
  /* rest unchanged */
}
```

- [ ] **Step 3:** Ensure text colors work on dark overlay

The existing colors should work, but verify:
- `.hero-title` uses `var(--text-primary)` which is dark in light theme, light in dark theme. Consider forcing white/light color for better video readability:
```css
.hero-title {
  color: #fff;
  /* fallback for dark overlay */
}
```

- [ ] **Step 4:** Commit changes

```bash
git add wwwroot/css/site.css
git commit -m "style: adjust hero text styles for video background"
```

---

### Task 4: Responsive Adjustments

**Files:**
- Modify: `wwwroot/css/site.css` responsive sections

- [ ] **Step 1:** Check tablet breakpoint (768px-991px)

At line 1966-2091, ensure `hero-content-wrapper` min-height adjusts:
```css
@media (max-width: 991px) {
  .hero-content-wrapper {
    min-height: 60vh;
  }
  .hero-title {
    font-size: 2.4rem;
  }
}
```

- [ ] **Step 2:** Check mobile breakpoint (480px-767px)

At line 2103-2278, reduce padding and font sizes:
```css
@media (max-width: 767px) {
  .hero-section {
    padding: 60px 0 80px;
  }
  .hero-content-wrapper {
    min-height: 50vh;
  }
}
```

- [ ] **Step 3:** Check small mobile (320px-480px)

At line 2584-3044:
```css
@media (max-width: 480px) {
  .hero-section {
    padding: 40px 0 60px;
  }
  .hero-title {
    font-size: 1.65rem;
  }
  .hero-stats {
    gap: 12px;
  }
  .hero-stat h3 {
    font-size: 1.2rem;
  }
  .hero-stat p {
    font-size: 0.7rem;
  }
}
```

- [ ] **Step 4:** Commit changes

```bash
git add wwwroot/css/site.css
git commit -m "style: add responsive adjustments for hero video"
```

---

### Task 5: Manual Testing and Verification

- [ ] **Step 1:** Build and run the project

```bash
dotnet build
dotnet run
```

- [ ] **Step 2:** Navigate to home page and verify:
  - Video background loads and plays automatically (muted)
  - Content is centered horizontally and vertically
  - Text is readable against video (overlay provides sufficient contrast)
  - "Shop Now" and "Explore Collection" buttons are visible and clickable
  - Stats (500+, 12K+, 50+) are displayed inline and centered
  - No horizontal scroll or overflow issues

- [ ] **Step 3:** Test responsive behavior:
  - Resize browser to tablet width (~768px): content remains centered, readable
  - Resize to mobile width (~375px): stats wrap if needed, buttons stack or shrink appropriately
  - Check that video covers full width without distortion

- [ ] **Step 4:** Check anchor link

Click "Explore Collection" button — should scroll smoothly to the Featured Products section (`#featured`).

- [ ] **Step 5:** Test in both light and dark themes

Toggle theme switch and verify:
- Text remains readable in both themes
- Overlay provides consistent contrast
- Video visibility is good

---

### Task 6: Optional Fallback and Polish

**Files:**
- Modify: `wwwroot/css/site.css`

- [ ] **Step 1:** Add fallback background for video loading failures

Update `.hero-section` to have a fallback gradient if video fails:
```css
.hero-section {
  background: linear-gradient(135deg, var(--bg-tertiary) 0%, var(--bg-primary) 60%, rgba(56, 89, 78, 0.1) 100%);
  /* existing rules... */
}
```

This is already present, so no change needed — it serves as fallback.

- [ ] **Step 2:** Create a placeholder poster image

Place a static image at `wwwroot/images/hero-poster.jpg` as poster fallback. For now, use an existing product image or create a solid color placeholder.

- [ ] **Step 3:** Add `poster` attribute to video (already included in Task 1)

- [ ] **Step 4:** Commit final changes

```bash
git add wwwroot/css/site.css
git commit -m "style: ensure fallback background persists"
```

---

## Completion Checklist

- [ ] Hero section displays video background (autoplay, muted, loop)
- [ ] Featured watch card completely removed
- [ ] Content is centered horizontally and vertically
- [ ] Text and buttons are readable on video (overlay working)
- [ ] "Explore Collection" anchor scrolls to featured section
- [ ] Responsive on tablet and mobile
- [ ] Works in both light and dark themes
- [ ] All CSS changes saved and committed
- [ ] No console errors or broken images

---

## Future Improvement

To replace the temporary video URL with a local file:
1. Place video file in `wwwroot/videos/hero-watch.mp4`
2. Update the `<source src>` to `@Url.Content("~/videos/hero-watch.mp4")`
3. Consider video compression to reduce file size (target 5-10MB)
4. Optionally add multiple format sources (WebM) for broader browser support
