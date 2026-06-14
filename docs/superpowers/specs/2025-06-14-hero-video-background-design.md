---
name: hero-video-background
description: Replace featured watch card with video background and center content
metadata:
  type: project
---

# Hero Section Video Background Design

## Overview
Transform the Home page hero section from a two-column layout with a featured watch card to a full-width video background with centered content.

## Current State
- Two-column layout: left text content, right featured watch product card
- Product card shows a watch image, price, and "Add to Cart" button
- Background uses CSS gradient with decorative pseudo-elements

## Proposed Changes

### 1. Layout
- Remove the right column (`col-lg-6` with `hero-image-area` and `hero-product-card`)
- Make `hero-content` span full width (12 columns)
- Center content horizontally using flexbox or Bootstrap utilities

### 2. Video Background
- Add `<video>` element as background of `hero-section`
- Attributes: `autoplay`, `muted`, `loop`, `playsinline`
- CSS: `position: absolute; inset: 0; width: 100%; height: 100%; object-fit: cover;`
- Add overlay with semi-transparent background for text readability
- Suggested temporary URL: `https://assets.mixkit.co/videos/preview/mixkit-wrist-watch-on-a-white-background-32807-large.mp4`
  (A watch-related video from Mixkit, free to use)

### 3. Content Centering
- Use flexbox to center `hero-content` vertically and horizontally
- Keep existing content: badge, title, description, buttons, stats
- Maintain responsive behavior (content still readable on mobile)

### 4. Technical Implementation

#### HTML Changes (`Views/Home/Index.cshtml`)
```html
<section class="hero-section">
  <!-- Video Background -->
  <video autoplay muted loop playsinline class="hero-video-bg"
         poster="fallback-image.jpg">
    <source src="https://assets.mixkit.co/videos/preview/mixkit-wrist-watch-on-a-white-background-32807-large.mp4"
            type="video/mp4" />
  </video>

  <!-- Dark Overlay -->
  <div class="hero-overlay"></div>

  <!-- Content Container -->
  <div class="container hero-content-wrapper">
    <div class="hero-content">
      <!-- Existing content unchanged -->
    </div>
  </div>
</section>
```

#### CSS Changes (`wwwroot/css/site.css`)
```css
.hero-section {
  position: relative;
  overflow: hidden;
  /* Adjust padding for video */
  padding: 120px 0;
}

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
  min-height: 70vh; /* Adjust as needed */
}

.hero-content {
  text-align: center;
  max-width: 800px;
}

/* Adjust hero badge for better visibility on video */
.hero-badge {
  background: rgba(169, 206, 194, 0.9);
  color: var(--accent-dark);
}

/* Stats remain inline or stack on mobile */
.hero-stats {
  justify-content: center;
}
```

### 5. Responsive Considerations
- Video scales with `object-fit: cover`
- Text remains readable with overlay
- Stats wrap naturally on small screens
- Buttons remain clickable with sufficient size

### 6. Future Replacement
To replace with local video:
```html
<source src="~/videos/hero-watch.mp4" type="video/mp4" />
```
Or update the video `src` attribute directly.

## Files to Modify
- `Views/Home/Index.cshtml` - Hero section HTML
- `wwwroot/css/site.css` - Hero section CSS

## Success Criteria
- Hero section displays video background
- Content is centered and readable
- Featured watch card is removed
- "Explore Collection" anchor link still works (scrolls to featured section)
- Mobile responsive maintained

## Notes
- Video autoplay requires `muted` attribute in most browsers
- Consider adding a poster image for slow connections
- Video file size should be optimized (~5-10MB compressed)
