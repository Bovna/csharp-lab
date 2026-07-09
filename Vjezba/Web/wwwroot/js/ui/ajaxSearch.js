(function () {
  function buildUrl(form, partial) {
    const url = new URL(form.action, window.location.origin);
    const data = new FormData(form);

    data.forEach((value, key) => {
      if (typeof value === "string") {
        url.searchParams.set(key, value);
      }
    });

    if (partial) {
      url.searchParams.set("partial", "true");
    }

    return url;
  }

  function syncBrowserUrl(form) {
    if (form.dataset.ajaxSyncUrl !== "true") {
      return;
    }

    const url = buildUrl(form, false);
    window.history.replaceState({}, "", url.toString());
  }

  async function refreshResults(form, state) {
    const targetSelector = form.dataset.ajaxResultsTarget;
    const target = targetSelector
      ? document.querySelector(targetSelector)
      : null;

    if (!target) {
      return;
    }

    if (state.abortController) {
      state.abortController.abort();
    }

    state.abortController = new AbortController();
    state.requestId += 1;

    const requestId = state.requestId;
    const url = buildUrl(form, true);

    try {
      const response = await fetch(url.toString(), {
        signal: state.abortController.signal,
        headers: {
          "X-Requested-With": "XMLHttpRequest",
        },
      });

      if (!response.ok || requestId !== state.requestId) {
        return;
      }

      target.innerHTML = await response.text();
      syncBrowserUrl(form);
    } catch (error) {
      if (error && error.name === "AbortError") {
        return;
      }
    }
  }

  function bindForm(form) {
    const state = {
      abortController: null,
      debounceTimer: null,
      requestId: 0,
    };

    form.addEventListener("submit", (event) => {
      event.preventDefault();
      refreshResults(form, state);
    });

    form.querySelectorAll("input, select").forEach((control) => {
      if (control.tagName === "INPUT" && control.type !== "radio" && control.type !== "checkbox") {
        control.addEventListener("input", () => {
          window.clearTimeout(state.debounceTimer);
          state.debounceTimer = window.setTimeout(() => refreshResults(form, state), 220);
        });
      } else {
        control.addEventListener("change", () => refreshResults(form, state));
      }
    });
  }

  document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("[data-ajax-search-form]").forEach(bindForm);
  });
})();
