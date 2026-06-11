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
        text: "Nastavi na placanje",
      }).appendTo($actions);
    }

    $panel.removeClass("is-updated");
    window.setTimeout(() => {
      $panel.addClass("is-updated");
    }, 10);
  }

  function renderDefaultPanel($panel) {
    renderPanel($panel, {
      title: "Korak 5: placanje",
      value: "Odaberi sjedalo za prikaz cijene.",
      meta: "Pregled cijene i gumb za nastavak pojavit ce se ovdje.",
    });
  }

  function renderSeatPanel($layout, $panel, $seat, includeAction) {
    const seatLabel = $seat.data("seat-label") || "";
    const seatType = $seat.data("seat-type") || "Standard";
    const seatId = $seat.data("seat-id");
    const is3D = String($seat.attr("data-is-3d")) === "1";
    const price = calculatePrice(seatType, is3D).toFixed(2);

    renderPanel($panel, {
      title: includeAction ? "Odabrano sjedalo" : "Pregled sjedala",
      value: `${seatLabel} | ${seatType} | ${price} EUR`,
      meta: is3D
        ? "Cijena ukljucuje 3D nadoplatu."
        : "Standardna projekcija bez 3D nadoplate.",
      href: includeAction ? buildCheckoutUrl($layout, seatId) : null,
    });
  }

  function initMotion() {
    $("[data-ticket-builder-page]").each(function () {
      const $page = $(this);

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
      revealSelectors.forEach((selector) => {
        $page.find(selector).each(function () {
          this.style.setProperty("--tb-delay", `${80 + index * 75}ms`);
          index += 1;
        });
      });

      window.setTimeout(() => {
        $page.addClass("is-ready");
      }, 40);
    });
  }

  function initCardTilt() {
    if (reduceMotion) {
      return;
    }

    $(".tb-card-link, .tb-list-item").on("mousemove", function (event) {
      const rect = this.getBoundingClientRect();
      const x = (event.clientX - rect.left) / rect.width - 0.5;
      const y = (event.clientY - rect.top) / rect.height - 0.5;

      this.style.setProperty("--tb-tilt-x", `${(-y * 3).toFixed(2)}deg`);
      this.style.setProperty("--tb-tilt-y", `${(x * 4).toFixed(2)}deg`);
      this.classList.add("is-tilting");
    });

    $(".tb-card-link, .tb-list-item").on("mouseleave blur", function () {
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

        renderSeatPanel($layout, $panel, $seat, false);
      });

      $seats.on("mouseleave blur", function () {
        const $selected = $seats.filter(".selected").first();

        if ($selected.length) {
          renderSeatPanel($layout, $panel, $selected, true);
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
            meta: "Odaberi drugo sjedalo ili osvjezi prikaz ako se stanje promijenilo.",
          });
          return;
        }

        $seats.removeClass("selected");
        $seat.addClass("selected");
        renderSeatPanel($layout, $panel, $seat, true);
      });
    });
  }

  function initCheckout() {
    $("[data-ticket-checkout-form]").each(function () {
      const $form = $(this);
      const $fields = $form.find("[data-ticket-field]");
      const $progress = $form.find("[data-ticket-form-progress]");
      const $summary = $("[data-ticket-live-summary]");
      const $submit = $form.find("[data-ticket-submit]");

      function valueFor(selector) {
        return $.trim($form.find(selector).val() || "");
      }

      function updateSummary() {
        const firstName = valueFor("[data-ticket-live-name]").split(/\s+/)[0] || "";
        const lastName = $.trim(
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
        const customerName = lastName || firstName;

        $summary
          .find("[data-ticket-live-customer]")
          .text(customerName || "Podaci jos nisu uneseni");
        $summary.find("[data-ticket-live-email-value]").text(email || "-");
        $summary.find("[data-ticket-live-phone-value]").text(phone || "-");
      }

      function updateProgress() {
        const filled = $fields.filter(function () {
          return $.trim($(this).val() || "").length > 0;
        }).length;
        const progress = $fields.length
          ? Math.round((filled / $fields.length) * 100)
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

        $submit.addClass("is-loading").text("Spremam kupnju...");
        return true;
      });

      updateProgress();
    });
  }

  $(function () {
    initMotion();
    initCardTilt();
    initSeats();
    initCheckout();
  });
})(window.jQuery);
