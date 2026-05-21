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
