(function () {
  const SECRET_QUERY = "bruh";

  function isSecretQuery(value) {
    return String(value || "").trim().toLocaleLowerCase("hr-HR") === SECRET_QUERY;
  }

  function getValue(item, key, fallback) {
    const pascalKey = key.charAt(0).toUpperCase() + key.slice(1);
    return item[key] ?? item[pascalKey] ?? fallback ?? "";
  }

  function setExpanded(state, expanded) {
    state.input.setAttribute("aria-expanded", expanded ? "true" : "false");
    state.combobox.setAttribute("aria-expanded", expanded ? "true" : "false");
    state.results.hidden = !expanded;

    if (!expanded) {
      state.activeIndex = -1;
      state.input.removeAttribute("aria-activedescendant");
      getOptions(state).forEach((option) => {
        option.classList.remove("is-active");
        option.setAttribute("aria-selected", "false");
      });
    }
  }

  function getOptions(state) {
    return Array.from(
      state.results.querySelectorAll("[data-global-search-option]"),
    );
  }

  function setStatus(state, message) {
    if (state.status) {
      state.status.textContent = message;
    }
  }

  function setActiveOption(state, index) {
    const options = getOptions(state);

    if (!options.length) {
      state.activeIndex = -1;
      state.input.removeAttribute("aria-activedescendant");
      return;
    }

    const nextIndex = Math.max(0, Math.min(index, options.length - 1));
    state.activeIndex = nextIndex;

    options.forEach((option, optionIndex) => {
      const isActive = optionIndex === nextIndex;
      option.classList.toggle("is-active", isActive);
      option.setAttribute("aria-selected", isActive ? "true" : "false");
    });

    const activeOption = options[nextIndex];
    state.input.setAttribute("aria-activedescendant", activeOption.id);
    activeOption.scrollIntoView({ block: "nearest" });
  }

  function clearResults(state) {
    state.results.innerHTML = "";
    setExpanded(state, false);
  }

  function openSecretGame(state) {
    if (
      !isSecretQuery(state.input.value) ||
      !window.KinoKlikSnake ||
      typeof window.KinoKlikSnake.open !== "function"
    ) {
      return false;
    }

    window.clearTimeout(state.timer);
    if (state.abortController) {
      state.abortController.abort();
      state.abortController = null;
    }

    clearResults(state);
    state.input.value = "";
    setStatus(state, "Tajna projekcija je otvorena.");
    window.KinoKlikSnake.open();
    state.input.blur();
    return true;
  }

  function goToResultsPage(state) {
    const query = state.input.value.trim();
    if (query.length < state.minLength) {
      setStatus(
        state,
        `Upisite najmanje ${state.minLength} znaka za globalnu pretragu.`,
      );
      state.input.focus();
      return false;
    }

    const url = new URL(state.resultsUrl, window.location.origin);
    url.searchParams.set("query", query);
    window.location.assign(url.toString());
    return true;
  }

  function renderMessage(state, title, description, modifier) {
    state.results.innerHTML = "";

    const message = document.createElement("div");
    message.className = "global-search__message";
    if (modifier) {
      message.classList.add(`global-search__message--${modifier}`);
    }

    const titleEl = document.createElement("strong");
    titleEl.textContent = title;
    message.appendChild(titleEl);

    if (description) {
      const descriptionEl = document.createElement("span");
      descriptionEl.textContent = description;
      message.appendChild(descriptionEl);
    }

    state.results.appendChild(message);
    setExpanded(state, true);
  }

  function groupResults(items) {
    const groups = [];
    const byCategory = new Map();

    items.forEach((item) => {
      const category = String(getValue(item, "category", "Rezultati"));
      if (!byCategory.has(category)) {
        const group = { category, items: [] };
        byCategory.set(category, group);
        groups.push(group);
      }

      byCategory.get(category).items.push(item);
    });

    return groups;
  }

  function renderResults(state, items, query) {
    state.results.innerHTML = "";
    state.activeIndex = -1;

    if (!items.length) {
      renderMessage(
        state,
        "Nema rezultata",
        "Promijenite pojam pretrage ili otvorite jednu od glavnih stranica.",
        "empty",
      );
      setStatus(state, `Nema rezultata za ${query}.`);
      return;
    }

    let optionIndex = 0;

    groupResults(items).forEach((group) => {
      const groupEl = document.createElement("div");
      groupEl.className = "global-search__group";
      groupEl.setAttribute("role", "group");
      groupEl.setAttribute("aria-label", group.category);

      const heading = document.createElement("div");
      heading.className = "global-search__group-title";
      heading.setAttribute("aria-hidden", "true");
      heading.textContent = group.category;
      groupEl.appendChild(heading);

      group.items.forEach((item) => {
        const kind = String(getValue(item, "kind", "data"));
        const option = document.createElement("a");
        option.id = `${state.results.id}-option-${optionIndex}`;
        option.className = `global-search__option global-search__option--${kind}`;
        option.href = String(getValue(item, "url", "#"));
        option.setAttribute("role", "option");
        option.setAttribute("aria-selected", "false");
        option.dataset.globalSearchOption = "true";

        const body = document.createElement("span");
        body.className = "global-search__option-body";

        const title = document.createElement("span");
        title.className = "global-search__option-title";
        title.textContent = String(getValue(item, "title", ""));

        const description = document.createElement("span");
        description.className = "global-search__option-description";
        description.textContent = String(getValue(item, "description", ""));

        const meta = document.createElement("span");
        meta.className = "global-search__option-meta";
        meta.textContent = String(getValue(item, "meta", ""));

        body.appendChild(title);
        body.appendChild(description);
        if (meta.textContent) {
          body.appendChild(meta);
        }

        option.appendChild(body);
        groupEl.appendChild(option);

        optionIndex += 1;
      });

      state.results.appendChild(groupEl);
    });

    setExpanded(state, true);
    setStatus(
      state,
      `${items.length} rezultata za ${query}. Koristite strelice za odabir.`,
    );
  }

  async function fetchResults(state, query) {
    if (state.abortController) {
      state.abortController.abort();
    }

    state.abortController = new AbortController();

    const url = new URL(state.endpoint, window.location.origin);
    url.searchParams.set("query", query);

    renderMessage(state, "Pretrazivanje...", "", "loading");
    setStatus(state, "Pretrazivanje je u tijeku.");

    try {
      const response = await fetch(url.toString(), {
        signal: state.abortController.signal,
        headers: {
          "X-Requested-With": "XMLHttpRequest",
        },
      });

      if (state.input.value.trim() !== query || isSecretQuery(state.input.value)) {
        return;
      }

      if (!response.ok) {
        renderMessage(
          state,
          "Pretraga nije dostupna",
          "Pokusajte ponovno za nekoliko trenutaka.",
          "error",
        );
        setStatus(state, "Pretraga nije dostupna.");
        return;
      }

      const payload = await response.json();
      if (state.input.value.trim() !== query || isSecretQuery(state.input.value)) {
        return;
      }

      const items = payload.results || payload.Results || [];
      renderResults(state, Array.isArray(items) ? items : [], query);
    } catch (error) {
      if (error && error.name === "AbortError") {
        return;
      }

      if (state.input.value.trim() !== query || isSecretQuery(state.input.value)) {
        return;
      }

      renderMessage(
        state,
        "Pretraga nije dostupna",
        "Pokusajte ponovno za nekoliko trenutaka.",
        "error",
      );
      setStatus(state, "Pretraga nije dostupna.");
    }
  }

  function bindSearch(root) {
    if (root.dataset.globalSearchReady === "1") {
      return;
    }

    const input = root.querySelector("[data-global-search-input]");
    const results = root.querySelector("[data-global-search-results]");
    const status = root.querySelector("[data-global-search-status]");
    const combobox = root.querySelector("[data-global-search-combobox]") || root;
    const endpoint = String(root.dataset.endpoint || "").trim();
    const resultsUrl = root.getAttribute("action") || "/global-search/rezultati";

    if (!input || !results || !endpoint) {
      return;
    }

    root.dataset.globalSearchReady = "1";

    const state = {
      root,
      input,
      results,
      status,
      combobox,
      endpoint,
      resultsUrl,
      minLength: Number.parseInt(root.dataset.minLength || "2", 10),
      timer: null,
      abortController: null,
      activeIndex: -1,
      isComposing: false,
      suppressSubmit: false,
    };

    root.addEventListener("submit", (event) => {
      event.preventDefault();
      if (state.suppressSubmit) {
        state.suppressSubmit = false;
        return;
      }

      if (openSecretGame(state)) {
        return;
      }

      goToResultsPage(state);
    });

    input.addEventListener("input", () => {
      const query = input.value.trim();
      window.clearTimeout(state.timer);

      if (state.abortController) {
        state.abortController.abort();
        state.abortController = null;
      }

      if (isSecretQuery(query)) {
        clearResults(state);
        setStatus(state, "Pritisnite Enter za tajnu projekciju.");
        return;
      }

      if (query.length < state.minLength) {
        clearResults(state);
        setStatus(
          state,
          `Upisite najmanje ${state.minLength} znaka za globalnu pretragu.`,
        );
        return;
      }

      state.timer = window.setTimeout(() => fetchResults(state, query), 240);
    });

    input.addEventListener("focus", () => {
      if (input.value.trim().length >= state.minLength && results.children.length) {
        setExpanded(state, true);
      }
    });

    input.addEventListener("compositionstart", () => {
      state.isComposing = true;
    });

    input.addEventListener("compositionend", () => {
      state.isComposing = false;
      state.suppressSubmit = true;
      window.setTimeout(() => {
        state.suppressSubmit = false;
      }, 0);
    });

    input.addEventListener("keydown", (event) => {
      const options = getOptions(state);

      if (event.isComposing || state.isComposing) {
        if (event.key === "Enter") {
          state.suppressSubmit = true;
          window.setTimeout(() => {
            state.suppressSubmit = false;
          }, 0);
        }
        return;
      }

      if (event.key === "Enter" && openSecretGame(state)) {
        event.preventDefault();
        return;
      }

      if (event.key === "Escape") {
        setExpanded(state, false);
        return;
      }

      if (event.key === "Tab") {
        setExpanded(state, false);
        return;
      }

      if (event.key === "ArrowDown") {
        if (!options.length) {
          return;
        }

        event.preventDefault();
        setExpanded(state, true);
        setActiveOption(state, state.activeIndex + 1);
        return;
      }

      if (event.key === "ArrowUp") {
        if (!options.length) {
          return;
        }

        event.preventDefault();
        setExpanded(state, true);
        setActiveOption(
          state,
          state.activeIndex <= 0 ? options.length - 1 : state.activeIndex - 1,
        );
        return;
      }

      if (event.key === "Enter" && state.activeIndex >= 0) {
        const activeOption = options[state.activeIndex];
        if (activeOption) {
          event.preventDefault();
          window.location.assign(activeOption.href);
        }
      }
    });

    results.addEventListener("mouseover", (event) => {
      const option = event.target.closest("[data-global-search-option]");
      if (!option) {
        return;
      }

      const options = getOptions(state);
      const index = options.indexOf(option);
      if (index >= 0) {
        setActiveOption(state, index);
      }
    });

    document.addEventListener("click", (event) => {
      if (!root.contains(event.target)) {
        setExpanded(state, false);
      }
    });

    setExpanded(state, false);
  }

  document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("[data-global-search]").forEach(bindSearch);
  });
})();
