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
      input: field.querySelector("[data-autocomplete-input]"),
      value: field.querySelector("[data-autocomplete-value]"),
      list: field.querySelector("[data-autocomplete-list]"),
      combobox: field.querySelector("[role='combobox']") || field,
    };
  }

  function setExpanded(state, expanded) {
    state.combobox.setAttribute("aria-expanded", expanded ? "true" : "false");
    if (state.list) {
      state.list.hidden = !expanded;
    }
  }

  function syncSelectedValue(field) {
    const state = getState(field);
    const items = parseItems(field);
    const normalizedText = String(state.input?.value || "")
      .trim()
      .toLowerCase();
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

    if (!state.list) {
      return;
    }

    state.list.innerHTML = "";

    if (!filteredItems.length) {
      const empty = document.createElement("div");
      empty.className = "autocomplete-field__empty";
      empty.textContent = "Nema odgovarajućih opcija.";
      state.list.appendChild(empty);
      setExpanded(state, true);
      return;
    }

    filteredItems.slice(0, 30).forEach((item) => {
      const option = document.createElement("button");
      option.type = "button";
      option.className = "autocomplete-field__option";
      option.textContent = String(item.text || "");
      option.addEventListener("click", () => {
        state.input.value = String(item.text || "");
        state.value.value = String(item.value ?? "");
        state.value.dispatchEvent(new Event("input", { bubbles: true }));
        setExpanded(state, false);
      });
      state.list.appendChild(option);
    });

    setExpanded(state, true);
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

    state.input.addEventListener("input", () => {
      renderList(field, state.input.value);
      state.value.value = "";
      state.value.dispatchEvent(new Event("input", { bubbles: true }));
    });

    state.input.addEventListener("focus", () => {
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
