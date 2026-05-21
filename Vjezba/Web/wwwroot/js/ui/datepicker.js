$(function () {
  const dayNamesByLocale = {
    hr: ["Po", "Ut", "Sr", "Če", "Pe", "Su", "Ne"],
    en: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
  };

  function getLocale($el) {
    const explicitLocale = String($el.data("locale") || "").trim();
    if (explicitLocale) {
      return explicitLocale;
    }

    const documentLocale =
      document.documentElement.lang || navigator.language || "hr-HR";
    return documentLocale;
  }

  function getLocaleKey(locale) {
    return String(locale || "")
      .toLowerCase()
      .startsWith("en")
      ? "en"
      : "hr";
  }

  function parseHiddenValue(value) {
    if (!value) {
      return new Date();
    }

    const parsed = new Date(value);
    return Number.isNaN(parsed.getTime()) ? new Date() : parsed;
  }

  $.fn.cinemaDatePicker = function () {
    return this.each(function () {
      const $el = $(this);
      const $hidden = $el.find(".cinema-datepicker__hidden");
      const $trigger = $el.find(".cinema-datepicker__trigger");
      const $display = $el.find(".cinema-datepicker__display");
      const $popover = $el.find(".cinema-datepicker__popover");
      const $grid = $el.find(".cinema-datepicker__grid");
      const $title = $el.find(".cinema-datepicker__title");
      const $prev = $el.find(".cinema-datepicker__prev");
      const $next = $el.find(".cinema-datepicker__next");
      const $timeHour = $el.find(".cinema-datepicker__time-hour");
      const $timeMinute = $el.find(".cinema-datepicker__time-minute");
      const type = $el.data("type") || "date";
      const locale = getLocale($el);
      const localeKey = getLocaleKey(locale);
      const weekdayLabels = dayNamesByLocale[localeKey];
      const dateFormatter = new Intl.DateTimeFormat(locale, {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
      });
      const dateTimeFormatter = new Intl.DateTimeFormat(locale, {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
      });
      const monthTitleFormatter = new Intl.DateTimeFormat(locale, {
        month: "long",
        year: "numeric",
      });

      let currentDate = parseHiddenValue($hidden.val());
      let viewingDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth(),
        1,
      );

      function syncHiddenValue() {
        const year = currentDate.getFullYear();
        const month = String(currentDate.getMonth() + 1).padStart(2, "0");
        const day = String(currentDate.getDate()).padStart(2, "0");
        const baseDate = `${year}-${month}-${day}`;

        if (type === "datetime-local") {
          const hours = String(currentDate.getHours()).padStart(2, "0");
          const minutes = String(currentDate.getMinutes()).padStart(2, "0");
          $hidden.val(`${baseDate}T${hours}:${minutes}`);
        } else {
          $hidden.val(baseDate);
        }
      }

      function syncTimeControls() {
        if (type !== "datetime-local") {
          return;
        }

        $timeHour.val(String(currentDate.getHours()).padStart(2, "0"));
        $timeMinute.val(String(currentDate.getMinutes()).padStart(2, "0"));
      }

      function updateDisplay() {
        $display.text(
          type === "datetime-local"
            ? dateTimeFormatter.format(currentDate)
            : dateFormatter.format(currentDate),
        );
        syncHiddenValue();
        syncTimeControls();
      }

      function updateWeekdayLabels() {
        $el
          .find(".cinema-datepicker__days-header [data-day]")
          .each(function (index) {
            $(this).text(weekdayLabels[index]);
          });
      }

      function renderCalendar() {
        const year = viewingDate.getFullYear();
        const month = viewingDate.getMonth();
        const firstDay = new Date(year, month, 1).getDay();
        const daysInMonth = new Date(year, month + 1, 0).getDate();
        const emptyDays =
          localeKey === "hr" ? (firstDay === 0 ? 6 : firstDay - 1) : firstDay;

        $title.text(monthTitleFormatter.format(viewingDate));
        $grid.empty();

        for (let i = 0; i < emptyDays; i += 1) {
          $grid.append(
            '<button type="button" class="cinema-datepicker__day cinema-datepicker__day--empty" tabindex="-1" aria-hidden="true"></button>',
          );
        }

        for (let day = 1; day <= daysInMonth; day += 1) {
          const isSelected =
            currentDate.getDate() === day &&
            currentDate.getMonth() === month &&
            currentDate.getFullYear() === year;
          const $day = $(
            '<button type="button" class="cinema-datepicker__day"></button>',
          )
            .text(day)
            .toggleClass("cinema-datepicker__day--selected", isSelected)
            .attr("aria-pressed", isSelected ? "true" : "false");

          $day.on("click", function (event) {
            event.preventDefault();
            event.stopPropagation();
            currentDate.setFullYear(year, month, day);
            updateDisplay();
            renderCalendar();

            if (type === "date") {
              closePopover();
            }
          });

          $grid.append($day);
        }
      }

      function openPopover() {
        $(".cinema-datepicker__popover").not($popover).prop("hidden", true);
        $(".cinema-datepicker__trigger")
          .not($trigger)
          .attr("aria-expanded", "false");
        viewingDate = new Date(
          currentDate.getFullYear(),
          currentDate.getMonth(),
          1,
        );
        updateWeekdayLabels();
        renderCalendar();
        $popover.prop("hidden", false);
        $trigger.attr("aria-expanded", "true");
      }

      function closePopover() {
        $popover.prop("hidden", true);
        $trigger.attr("aria-expanded", "false");
      }

      $trigger.on("click", function (event) {
        event.preventDefault();
        event.stopPropagation();
        if ($popover.prop("hidden")) {
          openPopover();
        } else {
          closePopover();
        }
      });

      $trigger.on("keydown", function (event) {
        if (event.key === "Enter" || event.key === " ") {
          event.preventDefault();
          $trigger.trigger("click");
        }
      });

      $prev.on("click", function (event) {
        event.preventDefault();
        event.stopPropagation();
        viewingDate.setMonth(viewingDate.getMonth() - 1);
        renderCalendar();
      });

      $next.on("click", function (event) {
        event.preventDefault();
        event.stopPropagation();
        viewingDate.setMonth(viewingDate.getMonth() + 1);
        renderCalendar();
      });

      $timeHour.add($timeMinute).on("change", function () {
        if (type !== "datetime-local") {
          return;
        }

        const hourValue = parseInt(String($timeHour.val() || "0"), 10);
        const minuteValue = parseInt(String($timeMinute.val() || "0"), 10);
        currentDate.setHours(hourValue, minuteValue, 0, 0);
        updateDisplay();
      });

      $(document).on("click.cinemaDatePicker", function (event) {
        if (!$(event.target).closest(".cinema-datepicker").length) {
          closePopover();
        }
      });

      updateWeekdayLabels();
      updateDisplay();
      renderCalendar();
    });
  };

  $(".cinema-datepicker").cinemaDatePicker();
});
