(function ($) {
  "use strict";

  if (!$) {
    return;
  }

  const reduceMotion = window.matchMedia(
    "(prefers-reduced-motion: reduce)",
  ).matches;

  function calculatePrice(type, is3D) {
    const normalized = String(type || "standard").toLowerCase();
    let basePrice = 7.5;

    if (normalized === "vip") {
      basePrice = 11;
    } else if (normalized === "couple") {
      basePrice = 13.5;
    }

    return is3D ? basePrice + 2 : basePrice;
  }

  function buildCheckoutUrl($layout, seatId) {
    const params = new URLSearchParams({
      cinemaId: String($layout.data("cinema-id")),
      movieId: String($layout.data("movie-id")),
      screeningId: String($layout.data("screening-id")),
      seatId: String(seatId),
    });

    return `${$layout.data("checkout-url")}?${params.toString()}`;
  }

  function renderPanel($panel, state) {
    const token = Date.now() + Math.random();
    $panel.data("render-token", token);

    const writePanel = () => {
      if ($panel.data("render-token") !== token) {
        return;
      }

      $panel.empty();

      $("<div/>", {
        class: "tb-price-panel__title",
        text: state.title,
      }).appendTo($panel);

      $("<div/>", {
        class: "tb-price-panel__value",
        text: state.value,
      }).appendTo($panel);

      $("<div/>", {
        class: "tb-price-panel__meta",
        text: state.meta,
      }).appendTo($panel);

      if (state.href) {
        const $actions = $("<div/>", {
          class: "tb-price-panel__actions",
        }).appendTo($panel);

        $("<a/>", {
          class: "ui-btn ui-btn--primary",
          href: state.href,
          text: "Nastavi na potvrdu",
        }).appendTo($actions);
      }

      $panel.addClass("is-updated");
    };

    if (reduceMotion) {
      writePanel();
      return;
    }

    $panel.removeClass("is-updated").stop(true, true).fadeTo(80, 0.62, () => {
      writePanel();
      if ($panel.data("render-token") === token) {
        $panel.fadeTo(180, 1);
      }
    });
  }

  function renderDefaultPanel($panel) {
    renderPanel($panel, {
      title: "Korak 5: potvrda rezervacije",
      value: "Odaberi sjedalo za prikaz cijene.",
      meta: "Pregled cijene i gumb za nastavak pojavit će se ovdje.",
    });
  }

  function renderSeatPanel($layout, $panel, $seat, includeAction, isHoverPreview) {
    const seatLabel = $seat.data("seat-label") || "";
    const seatType = $seat.data("seat-type") || "Standard";
    const seatId = $seat.data("seat-id");
    const is3D = String($seat.attr("data-is-3d")) === "1";
    const price = calculatePrice(seatType, is3D).toFixed(2);

    renderPanel($panel, {
      title: isHoverPreview ? "Pregled cijene" : "Odabrano sjedalo",
      value: `${seatLabel} | ${seatType} | ${price} EUR`,
      meta: isHoverPreview
        ? "Možete odmah nastaviti na potvrdu ili kliknuti sjedalo za odabir."
        : includeAction
        ? is3D
          ? "Cijena uključuje 3D nadoplatu."
          : "Standardna projekcija bez 3D nadoplate."
        : "Odaberite sjedalo i nastavite na potvrdu.",
      href: includeAction ? buildCheckoutUrl($layout, seatId) : null,
    });
  }

  function initMotion() {
    $("[data-ticket-builder-page]").each(function () {
      const $page = $(this);

      const centerActiveStep = () => {
        if (!window.matchMedia("(max-width: 767px)").matches) {
          return;
        }

        const stepLine = $page.find("[data-ticket-builder-steps]")[0];
        const activeStep = stepLine?.querySelector(".tb-step.active");

        if (stepLine && activeStep) {
          const targetLeft = activeStep.offsetLeft - (stepLine.clientWidth - activeStep.offsetWidth) / 2;
          stepLine.scrollTo({
            left: Math.max(0, targetLeft),
            behavior: reduceMotion ? "auto" : "smooth",
          });
        }
      };

      if ($page.data("ticketMotionReady")) {
        return;
      }

      $page.data("ticketMotionReady", true);

      const currentStep = Number($page.data("current-step")) || 1;
      const progress = Math.max(0, Math.min(100, ((currentStep - 1) / 4) * 100));

      $page
        .find("[data-ticket-builder-steps]")
        .css("--tb-progress", `${progress}%`)
        .css("--tb-progress-scale", String(progress / 100));

      if (reduceMotion) {
        $page.addClass("is-ready");
        centerActiveStep();
        return;
      }

      const revealSelectors = [
        ".tb-header",
        ".tb-step-line",
        ".tb-panel",
        ".tb-grid-cards > *",
        ".tb-list-item",
        ".tb-seat-row",
        ".tb-price-panel",
        ".tb-checkout-panel",
        ".tb-account-banner",
        ".tb-form-section",
        ".tb-live-ticket",
        ".tb-success-ticket",
      ];

      let index = 0;
      const compactLayout = window.matchMedia("(max-width: 767px)").matches;
      revealSelectors.forEach((selector) => {
        $page.find(selector).each(function () {
          const delay = Math.min(
            120 + index * (compactLayout ? 45 : 80),
            compactLayout ? 420 : 720,
          );
          this.style.setProperty("--tb-delay", `${delay}ms`);
          index += 1;
        });
      });

      window.setTimeout(() => {
        $page.addClass("is-ready");
        centerActiveStep();
      }, 90);
    });
  }

  function initClickFeedback() {
    $(".tb-card-link, .tb-list-item, .tb-seat, .ui-btn").on("click", function (event) {
      const $target = $(this);

      if ($target.css("position") === "static") {
        $target.css("position", "relative");
      }

      const offset = $target.offset() || { left: 0, top: 0 };
      const x = event.pageX - offset.left;
      const y = event.pageY - offset.top;

      $("<span/>", {
        class: "tb-click-ripple",
      })
        .css({ left: x, top: y })
        .appendTo($target)
        .on("animationend", function () {
          $(this).remove();
        });
    });
  }

  function initCardTilt() {
    if (reduceMotion) {
      return;
    }

    $(".tb-card-link, .tb-list-item").on("mouseenter", function () {
      this.classList.add("is-hovering");
    });

    $(".tb-card-link, .tb-list-item").on("mousemove", function (event) {
      const rect = this.getBoundingClientRect();
      const x = (event.clientX - rect.left) / rect.width - 0.5;
      const y = (event.clientY - rect.top) / rect.height - 0.5;

      this.style.setProperty("--tb-tilt-x", `${(-y * 1.2).toFixed(2)}deg`);
      this.style.setProperty("--tb-tilt-y", `${(x * 1.6).toFixed(2)}deg`);
      this.classList.add("is-tilting");
    });

    $(".tb-card-link, .tb-list-item").on("mouseleave blur", function () {
      this.classList.remove("is-hovering");
      this.classList.remove("is-tilting");
      this.style.removeProperty("--tb-tilt-x");
      this.style.removeProperty("--tb-tilt-y");
    });
  }

  function initSeats() {
    $("[data-ticket-seats]").each(function () {
      const $layout = $(this);
      const $panel = $layout.find("[data-ticket-price-panel]");
      const $seats = $layout.find(".tb-seat[data-seat-id]");

      if (!$panel.length || !$seats.length) {
        return;
      }

      $seats.on("mouseenter focus", function () {
        const $seat = $(this);

        if ($seat.data("seat-status") === "taken" || $seat.hasClass("selected")) {
          return;
        }

        renderSeatPanel($layout, $panel, $seat, true, true);
      });

      $seats.on("mouseleave blur", function () {
        const $selected = $seats.filter(".selected").first();

        if ($selected.length) {
          renderSeatPanel($layout, $panel, $selected, true, false);
        } else {
          renderDefaultPanel($panel);
        }
      });

      $seats.on("click", function () {
        const $seat = $(this);
        const seatLabel = $seat.data("seat-label") || "";

        if ($seat.data("seat-status") === "taken") {
          $seat.addClass("is-shaking");
          window.setTimeout(() => $seat.removeClass("is-shaking"), 420);

          renderPanel($panel, {
            title: "Sjedalo nedostupno",
            value: `${seatLabel} je zauzeto`,
            meta: "Odaberite drugo sjedalo ili osvježite prikaz ako se stanje promijenilo.",
          });
          return;
        }

        $seats.removeClass("selected").attr("aria-pressed", "false");
        $seat.addClass("selected").attr("aria-pressed", "true");
        renderSeatPanel($layout, $panel, $seat, true, false);

        if (window.matchMedia("(max-width: 767px)").matches) {
          window.setTimeout(() => {
            $panel[0]?.scrollIntoView({
              behavior: reduceMotion ? "auto" : "smooth",
              block: "nearest",
            });
          }, 220);
        }
      });
    });
  }

  function initCheckout() {
    $("[data-ticket-checkout-form]").each(function () {
      const $form = $(this);
      const $fields = $form.find("[data-ticket-field]");
      const $requiredFields = $fields.filter("[required]");
      const $progress = $form.find("[data-ticket-form-progress]");
      const $summary = $("[data-ticket-live-summary]");
      const $submit = $form.find("[data-ticket-submit]");

      function valueFor(selector) {
        return $.trim($form.find(selector).val() || "");
      }

      function updateSummary() {
        if (!$summary.length) {
          return;
        }

        const customerName = $.trim(
          $form
            .find("[data-ticket-live-name]")
            .map(function () {
              return $(this).val();
            })
            .get()
            .join(" "),
        );
        const email = valueFor("[data-ticket-live-email]");
        const phone = valueFor("[data-ticket-live-phone]");

        $summary
          .find("[data-ticket-live-customer]")
          .text(customerName || "Podaci jos nisu uneseni");
        $summary.find("[data-ticket-live-email-value]").stop(true, true).fadeOut(90, function () {
          $(this).text(email || "-").fadeIn(180);
        });
        $summary.find("[data-ticket-live-phone-value]").stop(true, true).fadeOut(90, function () {
          $(this).text(phone || "-").fadeIn(180);
        });
      }

      function updateProgress() {
        const filled = $requiredFields.filter(function () {
          return $.trim($(this).val() || "").length > 0;
        }).length;
        const progress = $requiredFields.length
          ? Math.round((filled / $requiredFields.length) * 100)
          : 100;

        $progress.css("width", `${progress}%`);
        $form.attr("data-progress", String(progress));
        updateSummary();
      }

      $fields.on("input change blur", updateProgress);

      $form.on("submit", function () {
        if (window.CinemaUI && !window.CinemaUI.validateForm(this)) {
          updateProgress();
          return false;
        }

        $submit.addClass("is-loading").text("Spremam rezervaciju...");
        return true;
      });

      updateProgress();
    });
  }

  $(function () {
    initMotion();
    initCardTilt();
    initClickFeedback();
    initSeats();
    initCheckout();
  });
})(window.jQuery);
