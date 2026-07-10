// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

window.CinemaUI = window.CinemaUI || {};

const validationMessage = "Ovo polje je obavezno.";

if (window.jQuery && window.jQuery.validator) {
  window.jQuery.extend(window.jQuery.validator.messages, {
    min: window.jQuery.validator.format(
      "Unesite vrijednost veću od ili jednaku {0}.",
    ),
    max: window.jQuery.validator.format(
      "Unesite vrijednost manju od ili jednaku {0}.",
    ),
    range: window.jQuery.validator.format(
      "Unesite vrijednost između {0} i {1}.",
    ),
    number: "Unesite ispravan broj.",
    required: validationMessage,
  });
}

function getFieldLabel(element) {
  const field = element.closest(".ui-field");
  if (!field) {
    return "";
  }

  const label = field.querySelector(".ui-field__label");
  return String(label?.textContent || "")
    .replace(/\*/g, "")
    .trim();
}

function normalizeRequiredMessage(rawMessage, label) {
  const message = String(rawMessage || "").trim();
  if (!message) {
    return label ? `${label} je obavezno polje.` : validationMessage;
  }

  const normalizedMessage = message
    .replace(/[.:!?]+$/g, "")
    .trim()
    .toLowerCase();
  const normalizedLabel = String(label || "")
    .trim()
    .toLowerCase();

  if (normalizedLabel && normalizedMessage === normalizedLabel) {
    return `${label} je obavezno polje.`;
  }

  return message;
}

function getFieldContext(element) {
  const field = element.closest(".ui-field");
  if (!field) {
    return null;
  }

  const validationEl = field.querySelector(".ui-field__validation");
  return { field, validationEl };
}

function isEmptyRequiredValue(element) {
  if (element.type === "checkbox") {
    return !element.checked;
  }

  return !String(element.value || "").trim();
}

function getValidationMessage(element) {
  const explicitMessage =
    element.getAttribute("data-required-message") ||
    element.dataset.requiredMessage ||
    element.getAttribute("data-msg-required");

  if (explicitMessage && element.hasAttribute("data-autocomplete-value")) {
    return String(explicitMessage).trim();
  }

  const rawMessage =
    element.getAttribute("data-val-required") ||
    element.dataset.valRequired ||
    element.getAttribute("data-msg-required") ||
    validationMessage;
  const fieldLabel =
    element.getAttribute("data-field-label") || getFieldLabel(element);

  return normalizeRequiredMessage(rawMessage, fieldLabel);
}

function setFieldState(element, isValid, message) {
  const context = getFieldContext(element);
  if (!context) {
    return;
  }

  const { field, validationEl } = context;
  field.classList.toggle("is-invalid", !isValid);

  // If this is an autocomplete field, prefer the explicit required message
  // emitted in the hidden input (`data-required-message`) so we show the
  // exact ViewModel text instead of falling back to label-based heuristics.
  let finalMessage = message;
  if (!isValid) {
    try {
      const acHidden = field.querySelector("[data-autocomplete-value]");
      const explicit =
        (acHidden &&
          (acHidden.getAttribute("data-required-message") ||
            acHidden.dataset.requiredMessage)) ||
        null;
      if (explicit) {
        finalMessage = String(explicit).trim();
      }
    } catch (e) {
      /* ignore */
    }
  }

  if (validationEl) {
    validationEl.textContent = isValid ? "" : finalMessage || validationMessage;
  }
}

function validateRequiredElement(element) {
  const valid = !element.required || !isEmptyRequiredValue(element);
  if (!valid) {
    setFieldState(element, false, getValidationMessage(element));
  } else {
    setFieldState(element, true);
  }

  return valid;
}

document.addEventListener(
  "blur",
  (e) => {
    const el = e.target;
    if (el.matches && el.matches("[required]")) {
      validateRequiredElement(el);
    }
  },
  true,
);

document.addEventListener(
  "input",
  (e) => {
    const el = e.target;
    if (el.matches && el.matches("[required]")) {
      validateRequiredElement(el);
    }
  },
  true,
);

CinemaUI.validateForm = function (form) {
  let isValid = true;

  if (window.jQuery && window.jQuery(form).valid) {
    if (!window.jQuery(form).valid()) {
      isValid = false;
    }
  }

  const requiredElements = form.querySelectorAll("[required]");
  requiredElements.forEach((el) => {
    if (!validateRequiredElement(el)) {
      isValid = false;
    }
  });

  return isValid;
};

// Confirmation Modal Helper
CinemaUI.showConfirm = function ({ title, body, onConfirm }) {
  const modal = document.getElementById("modal-confirm");
  if (!modal) {
    if (confirm(body)) onConfirm();
    return;
  }

  const titleEl = modal.querySelector(".modal-confirm__title");
  const bodyEl = modal.querySelector(".modal-confirm__body");
  const btnCancel = modal.querySelector('[data-action="cancel"]');
  const btnConfirm = modal.querySelector('[data-action="confirm"]');
  const previouslyFocused = document.activeElement;

  if (titleEl) titleEl.textContent = title || "Potvrdite";
  if (bodyEl) bodyEl.textContent = body || "Jeste li sigurni?";

  modal.classList.add("is-visible");
  modal.setAttribute("aria-hidden", "false");
  if (btnConfirm) {
    btnConfirm.focus();
  }

  const cleanup = () => {
    modal.classList.remove("is-visible");
    modal.setAttribute("aria-hidden", "true");
    btnCancel.removeEventListener("click", handleCancel);
    btnConfirm.removeEventListener("click", handleConfirm);
    if (previouslyFocused && typeof previouslyFocused.focus === "function") {
      previouslyFocused.focus();
    }
  };

  const handleCancel = () => cleanup();
  const handleConfirm = () => {
    cleanup();
    if (onConfirm) onConfirm();
  };

  btnCancel.addEventListener("click", handleCancel);
  btnConfirm.addEventListener("click", handleConfirm);
};

function initNavigationMotion() {
  const navMenu = document.querySelector(".nav-menu");
  if (!navMenu || navMenu.dataset.motionReady === "1") {
    return;
  }

  navMenu.dataset.motionReady = "1";

  const reduceMotion = window.matchMedia(
    "(prefers-reduced-motion: reduce)",
  ).matches;
  const navItems = Array.from(navMenu.querySelectorAll("li"));
  const navLinks = Array.from(navMenu.querySelectorAll(".nav-link"));

  navItems.forEach((item, index) => {
    item.style.animationDelay = reduceMotion ? "0ms" : `${45 + index * 28}ms`;
  });

  if (reduceMotion) {
    return;
  }

  const pulseLink = (link) => {
    if (!link) {
      return;
    }

    link.classList.remove("is-animated");
    void link.offsetWidth;
    link.classList.add("is-animated");
  };

  navLinks.forEach((link) => {
    const handleAnimationEnd = () => {
      link.classList.remove("is-animated");
    };

    link.addEventListener("mouseenter", () => pulseLink(link));
    link.addEventListener("focus", () => pulseLink(link));
    link.addEventListener("animationend", handleAnimationEnd);
  });

  const activeLink = navMenu.querySelector(".nav-link.active");
  if (activeLink) {
    window.setTimeout(() => pulseLink(activeLink), 160);
  }
}

function initTicketBuilderMotion() {
  const pages = Array.from(document.querySelectorAll(".tb-page"));
  if (!pages.length) {
    return;
  }

  const reduceMotion = window.matchMedia(
    "(prefers-reduced-motion: reduce)",
  ).matches;

  pages.forEach((page) => {
    if (page.hasAttribute("data-ticket-builder-page")) {
      return;
    }

    if (page.dataset.motionReady === "1") {
      return;
    }

    page.dataset.motionReady = "1";

    if (reduceMotion) {
      page.classList.add("is-ready");
      return;
    }

    const staggerGroups = [
      { selector: ".tb-panel", delayStep: 60 },
      { selector: ".tb-grid-cards > *", delayStep: 70 },
      { selector: ".tb-list-item", delayStep: 70 },
      { selector: ".tb-seat-row", delayStep: 42 },
      { selector: ".tb-seat-layout", delayStep: 50 },
      { selector: ".tb-checkout-panel", delayStep: 50 },
      { selector: ".tb-step", delayStep: 30 },
    ];

    staggerGroups.forEach(({ selector, delayStep }) => {
      Array.from(page.querySelectorAll(selector)).forEach((element, index) => {
        element.style.setProperty("--tb-delay", `${index * delayStep}ms`);
      });
    });

    window.requestAnimationFrame(() => {
      page.classList.add("is-ready");
    });
  });
}

function initHomeExperience() {
  const carousels = document.querySelectorAll("[data-home-featured-carousel]");

  carousels.forEach((carousel) => {
    if (carousel.dataset.carouselReady === "1") {
      return;
    }

    const slides = Array.from(carousel.querySelectorAll("[data-carousel-slide]"));
    const dots = Array.from(carousel.querySelectorAll("[data-carousel-dot]"));
    const previousButton = carousel.querySelector("[data-carousel-previous]");
    const nextButton = carousel.querySelector("[data-carousel-next]");

    if (slides.length < 2) {
      return;
    }

    carousel.dataset.carouselReady = "1";
    let activeIndex = Math.max(
      0,
      slides.findIndex((slide) => slide.classList.contains("is-active")),
    );

    const setActiveSlide = (nextIndex) => {
      activeIndex = (nextIndex + slides.length) % slides.length;

      slides.forEach((slide, index) => {
        const isActive = index === activeIndex;
        slide.classList.toggle("is-active", isActive);
        slide.setAttribute("aria-hidden", String(!isActive));

        slide.querySelectorAll("a, button, input, select, textarea").forEach(
          (interactiveElement) => {
            if (isActive) {
              interactiveElement.removeAttribute("tabindex");
            } else {
              interactiveElement.setAttribute("tabindex", "-1");
            }
          },
        );
      });

      dots.forEach((dot, index) => {
        const isActive = index === activeIndex;
        dot.classList.toggle("is-active", isActive);
        dot.setAttribute("aria-current", String(isActive));
      });
    };

    previousButton?.addEventListener("click", () => {
      setActiveSlide(activeIndex - 1);
    });

    nextButton?.addEventListener("click", () => {
      setActiveSlide(activeIndex + 1);
    });

    dots.forEach((dot, index) => {
      dot.addEventListener("click", () => setActiveSlide(index));
    });

    carousel.addEventListener("keydown", (event) => {
      if (event.key === "ArrowLeft") {
        event.preventDefault();
        setActiveSlide(activeIndex - 1);
      }

      if (event.key === "ArrowRight") {
        event.preventDefault();
        setActiveSlide(activeIndex + 1);
      }
    });

    setActiveSlide(activeIndex);
  });
}

if (window.jQuery) {
  window.jQuery(() => {
    initNavigationMotion();
    initTicketBuilderMotion();
    initHomeExperience();
  });
} else {
  document.addEventListener("DOMContentLoaded", () => {
    initNavigationMotion();
    initTicketBuilderMotion();
    initHomeExperience();
  });
}
