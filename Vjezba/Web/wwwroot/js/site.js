// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

window.CinemaUI = window.CinemaUI || {};

const validationMessage = "Ovo polje je obavezno.";

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
  return (
    element.getAttribute("data-val-required") ||
    element.dataset.valRequired ||
    validationMessage
  );
}

function setFieldState(element, isValid, message) {
  const context = getFieldContext(element);
  if (!context) {
    return;
  }

  const { field, validationEl } = context;
  field.classList.toggle("is-invalid", !isValid);

  if (validationEl) {
    validationEl.textContent = isValid ? "" : message || validationMessage;
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
  const requiredElements = form.querySelectorAll("[required]");
  requiredElements.forEach((el) => {
    if (!validateRequiredElement(el)) {
      isValid = false;
    }
  });

  if (window.jQuery && window.jQuery(form).valid) {
    if (!window.jQuery(form).valid()) {
      isValid = false;
    }
  }

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

  const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
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

  const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

  pages.forEach((page) => {
    if (page.dataset.motionReady === "1") {
      return;
    }

    page.dataset.motionReady = "1";

    if (reduceMotion) {
      page.classList.add("is-ready");
      return;
    }

    const staggerGroups = [
      { selector: ".tb-grid-cards > *", delayStep: 70 },
      { selector: ".tb-list-item", delayStep: 70 },
      { selector: ".tb-seat-row", delayStep: 42 },
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
  if (!window.jQuery) {
    return;
  }

  const $ = window.jQuery;
  const $home = $(".home-page");
  if (!$home.length || $home.data("homeReady") === true) {
    return;
  }

  $home.data("homeReady", true);

  const heroEl = $home.find("[data-home-hero]").get(0);
  const $hero = $(heroEl);
  const $heroPosters = $home.find("[data-hero-wall] .home-hero__poster");
  const $viewport = $home.find("[data-carousel-viewport]");
  const $track = $home.find("[data-carousel-track]");
  const $cards = $track.find("[data-featured-card]");
  const $counters = $home.find("[data-count-up]");
  const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

  if (heroEl && !reduceMotion) {
    let heroFrame = 0;

    const resetHero = () => {
      heroEl.style.setProperty("--hero-x", "50%");
      heroEl.style.setProperty("--hero-y", "40%");
      $heroPosters.each(function (index) {
        $(this).css("transform", `translate3d(0, 0, 0) rotateX(${index % 2 === 0 ? 1 : -1}deg) rotateY(0deg)`);
      });
    };

    const updateHero = (clientX, clientY) => {
      const rect = heroEl.getBoundingClientRect();
      const percentX = Math.max(0, Math.min(100, ((clientX - rect.left) / rect.width) * 100));
      const percentY = Math.max(0, Math.min(100, ((clientY - rect.top) / rect.height) * 100));
      const moveX = (percentX - 50) / 10;
      const moveY = (percentY - 50) / 12;

      heroEl.style.setProperty("--hero-x", `${percentX.toFixed(1)}%`);
      heroEl.style.setProperty("--hero-y", `${percentY.toFixed(1)}%`);

      $heroPosters.each(function (index) {
        const depth = index + 1;
        const offsetX = moveX * depth * 0.7;
        const offsetY = moveY * depth * 0.7;
        const tiltX = moveY * -0.45;
        const tiltY = moveX * 0.45;

        $(this).css(
          "transform",
          `translate3d(${offsetX}px, ${offsetY}px, 0) rotateX(${tiltX}deg) rotateY(${tiltY}deg)`,
        );
      });
    };

    resetHero();

    $hero.on("mousemove", (event) => {
      if (heroFrame) {
        return;
      }

      heroFrame = window.requestAnimationFrame(() => {
        heroFrame = 0;
        updateHero(event.clientX, event.clientY);
      });
    });

    $hero.on("mouseleave", () => {
      resetHero();
    });
  }

  const animateCounter = (element) => {
    const $element = $(element);
    const target = Number($element.data("target")) || 0;
    if ($element.data("counted") === true) {
      return;
    }

    $element.data("counted", true);
    $({ value: 0 }).animate(
      { value: target },
      {
        duration: 1200,
        easing: "swing",
        step(now) {
          $element.text(Math.round(now));
        },
        complete() {
          $element.text(target);
        },
      },
    );
  };

  if (window.IntersectionObserver && $counters.length) {
    const observer = new IntersectionObserver(
      (entries, io) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            animateCounter(entry.target);
            io.unobserve(entry.target);
          }
        });
      },
      { threshold: 0.45 },
    );

    $counters.each((_, element) => observer.observe(element));
  } else {
    $counters.each((_, element) => animateCounter(element));
  }

  if (!$viewport.length || !$track.length || !$cards.length) {
    return;
  }

  $cards.each((index, element) => {
    element.style.animationDelay = `${index * 60}ms`;
  });

  let isDragging = false;
  let dragStartX = 0;
  let dragScrollLeft = 0;
  let isPaused = false;
  let scrollRafId = 0;

  const getScrollLimit = () => {
    const viewport = $viewport.get(0);
    if (!viewport) {
      return 0;
    }

    return Math.max(0, viewport.scrollWidth - viewport.clientWidth);
  };

  const stopAutoScroll = () => {
    if (scrollRafId) {
      window.cancelAnimationFrame(scrollRafId);
      scrollRafId = 0;
    }
  };

  const startAutoScroll = () => {
    stopAutoScroll();

    if (reduceMotion) {
      return;
    }

    let lastTimestamp = 0;

    const step = (timestamp) => {
      if (isPaused || isDragging) {
        lastTimestamp = timestamp;
        scrollRafId = window.requestAnimationFrame(step);
        return;
      }

      if (!lastTimestamp) {
        lastTimestamp = timestamp;
      }

      const delta = timestamp - lastTimestamp;
      lastTimestamp = timestamp;

      const viewport = $viewport.get(0);
      if (!viewport) {
        return;
      }

      const nextLeft = viewport.scrollLeft + delta * 0.018;
      const limit = getScrollLimit();
      viewport.scrollLeft = nextLeft >= limit ? 0 : nextLeft;

      scrollRafId = window.requestAnimationFrame(step);
    };

    scrollRafId = window.requestAnimationFrame(step);
  };

  $viewport.on("wheel", (event) => {
    const wheelEvent = event.originalEvent;
    if (!wheelEvent || Math.abs(wheelEvent.deltaY) <= Math.abs(wheelEvent.deltaX)) {
      return;
    }

    event.preventDefault();
    $viewport.scrollLeft($viewport.scrollLeft() + wheelEvent.deltaY * 1.15);
  });

  $viewport.on("pointerdown", (event) => {
    isDragging = true;
    isPaused = true;
    dragStartX = event.clientX;
    dragScrollLeft = $viewport.scrollLeft();
    $viewport.addClass("is-dragging");
  });

  $(document).on("pointermove.homeCarousel", (event) => {
    if (!isDragging) {
      return;
    }

    const deltaX = event.clientX - dragStartX;
    $viewport.scrollLeft(dragScrollLeft - deltaX * 1.1);
  });

  $(document).on("pointerup.homeCarousel pointercancel.homeCarousel", () => {
    if (!isDragging) {
      return;
    }

    isDragging = false;
    $viewport.removeClass("is-dragging");
    isPaused = false;
  });

  $viewport.on("mouseenter focusin", () => {
    isPaused = true;
  });

  $viewport.on("mouseleave focusout", () => {
    isPaused = false;
    $viewport.removeClass("is-dragging");
  });

  startAutoScroll();
}

document.addEventListener("DOMContentLoaded", () => {
  initNavigationMotion();
  initTicketBuilderMotion();
  initHomeExperience();
});
