(function () {
  window.CinemaUI = window.CinemaUI || {};

  function parseItems(field) {
    try {
      return JSON.parse(field.dataset.items || "[]");
    } catch {
      return [];
    }
  }

  function getState(field) {
    return {
      field,
      input: field.querySelector("[data-autocomplete-input]"),
      value: field.querySelector("[data-autocomplete-value]"),
      list: field.querySelector("[data-autocomplete-list]"),
      combobox: field.querySelector("[role='combobox']") || field,
      remoteSearch: field.dataset.remoteSearch === "true",
      endpoint: String(field.dataset.endpoint || "").trim(),
      emptyText: String(
        field.dataset.emptyText || "Nema odgovarajućih opcija.",
      ),
    };
  }

  function setExpanded(state, expanded) {
    state.combobox.setAttribute("aria-expanded", expanded ? "true" : "false");
    if (state.list) {
      state.list.hidden = !expanded;
    }
  }

  function renderItems(state, items, emptyText) {
    if (!state.list) {
      return;
    }

    state.list.innerHTML = "";

    if (!items.length) {
      const empty = document.createElement("div");
      empty.className = "autocomplete-field__empty";
      empty.textContent = emptyText || state.emptyText;
      state.list.appendChild(empty);
      setExpanded(state, true);
      return;
    }

    items.slice(0, 30).forEach((item) => {
      const option = document.createElement("button");
      option.type = "button";
      option.className = "autocomplete-field__option";
      option.textContent = String(item.text || "");
      option.addEventListener("click", () => {
        state.input.value = String(item.text || "");
        state.value.value = String(item.value ?? "");
        state.field.dataset.autocompleteSelectedValue = String(
          item.value ?? "",
        );
        state.field.dataset.autocompleteSelectedText = String(item.text || "");
        state.value.dispatchEvent(new Event("input", { bubbles: true }));
        setExpanded(state, false);
      });
      state.list.appendChild(option);
    });

    setExpanded(state, true);
  }

  function syncSelectedValue(field) {
    const state = getState(field);
    const items = parseItems(field);
    const normalizedText = String(state.input?.value || "")
      .trim()
      .toLowerCase();
    const remoteSelectedValue = String(
      state.field.dataset.autocompleteSelectedValue || "",
    ).trim();
    const remoteSelectedText = String(
      state.field.dataset.autocompleteSelectedText || "",
    )
      .trim()
      .toLowerCase();

    if (
      state.remoteSearch &&
      remoteSelectedValue &&
      remoteSelectedText === normalizedText
    ) {
      state.value.value = remoteSelectedValue;
      state.value.dispatchEvent(new Event("input", { bubbles: true }));
      return;
    }

    const selectedItem = items.find(
      (item) => String(item.text || "").toLowerCase() === normalizedText,
    );

    if (selectedItem) {
      state.value.value = String(selectedItem.value ?? "");
    } else {
      state.value.value = "";
    }

    state.value.dispatchEvent(new Event("input", { bubbles: true }));
  }

  function renderList(field, query) {
    const state = getState(field);
    const items = parseItems(field);
    if (state.remoteSearch) {
      return;
    }

    const normalizedQuery = String(query || "")
      .trim()
      .toLowerCase();
    const filteredItems = normalizedQuery
      ? items.filter((item) =>
          String(item.text || "")
            .toLowerCase()
            .includes(normalizedQuery),
        )
      : items;

    renderItems(state, filteredItems, "Nema odgovarajućih opcija.");
  }

  async function fetchRemoteItems(field, query, abortController) {
    const state = getState(field);
    const normalizedQuery = String(query || "").trim();

    if (!state.endpoint || !normalizedQuery) {
      renderItems(state, [], state.emptyText);
      setExpanded(state, false);
      return;
    }

    const url = new URL(state.endpoint, window.location.origin);
    url.searchParams.set("query", normalizedQuery);

    try {
      const response = await fetch(url.toString(), {
        signal: abortController.signal,
        headers: {
          "X-Requested-With": "XMLHttpRequest",
        },
      });

      if (!response.ok) {
        renderItems(state, [], state.emptyText);
        return;
      }

      const items = await response.json();
      renderItems(state, Array.isArray(items) ? items : [], state.emptyText);
    } catch (error) {
      if (error && error.name === "AbortError") {
        return;
      }

      renderItems(state, [], state.emptyText);
    }
  }

  function setupField(field) {
    const state = getState(field);

    if (!state.input || !state.value || !state.list) {
      return;
    }

    const initialItems = parseItems(field);
    const selectedInitial = initialItems.find(
      (item) => String(item.value ?? "") === String(state.value.value || ""),
    );
    if (selectedInitial && !state.input.value) {
      state.input.value = String(selectedInitial.text || "");
    }

    let remoteTimer = null;
    let remoteAbortController = null;

    state.input.addEventListener("input", () => {
      delete state.field.dataset.autocompleteSelectedValue;
      delete state.field.dataset.autocompleteSelectedText;
      state.value.value = "";
      state.value.dispatchEvent(new Event("input", { bubbles: true }));

      if (!state.remoteSearch) {
        renderList(field, state.input.value);
        return;
      }

      window.clearTimeout(remoteTimer);
      if (remoteAbortController) {
        remoteAbortController.abort();
      }

      const query = state.input.value;
      if (!String(query || "").trim()) {
        renderItems(state, [], state.emptyText);
        setExpanded(state, false);
        return;
      }

      remoteTimer = window.setTimeout(() => {
        remoteAbortController = new AbortController();
        fetchRemoteItems(field, query, remoteAbortController);
      }, 250);
    });

    state.input.addEventListener("focus", () => {
      if (state.remoteSearch) {
        return;
      }

      renderList(field, state.input.value);
    });

    state.input.addEventListener("keydown", (event) => {
      if (event.key === "Escape") {
        setExpanded(state, false);
      }
    });

    state.input.addEventListener("blur", () => {
      window.setTimeout(() => {
        syncSelectedValue(field);
        setExpanded(state, false);
      }, 150);
    });

    document.addEventListener("click", (event) => {
      if (!field.contains(event.target)) {
        syncSelectedValue(field);
        setExpanded(state, false);
      }
    });

    setExpanded(state, false);
  }

  document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("[data-autocomplete-field]").forEach(setupField);
  });
})();
