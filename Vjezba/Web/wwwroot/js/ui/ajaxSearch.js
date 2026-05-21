(function () {
  function buildUrl(form) {
    const url = new URL(form.action, window.location.origin);
    const data = new FormData(form);

    data.forEach((value, key) => {
      if (typeof value === "string") {
        url.searchParams.set(key, value);
      }
    });

    url.searchParams.set("partial", "true");
    return url.toString();
  }

  async function refreshResults(form) {
    const targetSelector = form.dataset.ajaxResultsTarget;
    const target = targetSelector
      ? document.querySelector(targetSelector)
      : null;

    if (!target) {
      return;
    }

    const response = await fetch(buildUrl(form), {
      headers: {
        "X-Requested-With": "XMLHttpRequest",
      },
    });

    if (!response.ok) {
      return;
    }

    target.innerHTML = await response.text();
  }

  function bindForm(form) {
    let debounceTimer = null;

    form.addEventListener("submit", (event) => {
      event.preventDefault();
      refreshResults(form);
    });

    form.querySelectorAll("input, select").forEach((control) => {
      if (control.tagName === "INPUT") {
        control.addEventListener("input", () => {
          window.clearTimeout(debounceTimer);
          debounceTimer = window.setTimeout(() => refreshResults(form), 220);
        });
      } else {
        control.addEventListener("change", () => refreshResults(form));
      }
    });
  }

  document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("[data-ajax-search-form]").forEach(bindForm);
  });
})();
