/**
 * Cinema UI Helper Module
 * Provides shared utilities for confirmation modals and form validation
 */

(function () {
  "use strict";

  window.CinemaUI = window.CinemaUI || {};

  /**
   * Show a confirmation modal
   * @param {Object} options - {title, body, onConfirm, onCancel}
   * @returns {Promise<boolean>} - true if confirmed, false if cancelled
   */
  window.CinemaUI.showConfirm = function (options) {
    return new Promise((resolve) => {
      const title = options.title || "Potvrdite";
      const body = options.body || "Jeste li sigurni?";
      const modal = document.getElementById("modal-confirm");

      if (!modal) {
        // Fallback to native confirm if modal not present
        if (window.confirm(title + "\n\n" + body)) {
          if (options.onConfirm) options.onConfirm();
          resolve(true);
        } else {
          if (options.onCancel) options.onCancel();
          resolve(false);
        }
        return;
      }

      // Update modal content
      modal.querySelector(".modal-confirm__title").textContent = title;
      modal.querySelector(".modal-confirm__body").textContent = body;

      // Set up event handlers
      const confirmBtn = modal.querySelector('[data-action="confirm"]');
      const cancelBtn = modal.querySelector('[data-action="cancel"]');

      const handleConfirm = () => {
        cleanup();
        if (options.onConfirm) options.onConfirm();
        resolve(true);
      };

      const handleCancel = () => {
        cleanup();
        if (options.onCancel) options.onCancel();
        resolve(false);
      };

      const cleanup = () => {
        modal.setAttribute("aria-hidden", "true");
        confirmBtn.removeEventListener("click", handleConfirm);
        cancelBtn.removeEventListener("click", handleCancel);
        document.removeEventListener("keydown", handleEscape);
      };

      const handleEscape = (e) => {
        if (e.key === "Escape") {
          e.preventDefault();
          handleCancel();
        }
      };

      // Show modal
      modal.setAttribute("aria-hidden", "false");
      confirmBtn.addEventListener("click", handleConfirm);
      cancelBtn.addEventListener("click", handleCancel);
      document.addEventListener("keydown", handleEscape);
      confirmBtn.focus();
    });
  };

  /**
   * Validate required fields on a form
   * @param {HTMLFormElement} form
   * @returns {boolean} - true if valid, false otherwise
   */
  window.CinemaUI.validateForm = function (form) {
    let isValid = true;
    const fields = form.querySelectorAll("[required]");

    fields.forEach((field) => {
      const fieldContainer = field.closest(".ui-field");
      if (!fieldContainer) return;

      const value = field.value.trim();
      const validationEl = fieldContainer.querySelector(
        ".ui-field__validation",
      );

      if (!value) {
        fieldContainer.classList.add("is-invalid");
        if (validationEl) {
          validationEl.textContent = "Ovo polje je obavezno.";
        }
        isValid = false;
      } else {
        fieldContainer.classList.remove("is-invalid");
        if (validationEl) {
          validationEl.textContent = "";
        }
      }
    });

    // Validate autocomplete fields as well
    const acValid = window.CinemaUI.validateAutocompleteFields(form);

    return isValid && acValid;
  };

  /**
   * Validate autocomplete fields (custom component)
   * - Looks for `.autocomplete-field` elements and checks their hidden value input
   * - If the label contains an asterisk (*) we treat the field as required
   */
  window.CinemaUI.validateAutocompleteFields = function (form) {
    let isValid = true;
    const acFields = form.querySelectorAll('.autocomplete-field');

    acFields.forEach((ac) => {
      const hidden = ac.querySelector('[data-autocomplete-value]');
      if (!hidden) return;

      const label = ac.querySelector('.ui-field__label');
      const isMarkedRequired = label && label.textContent && label.textContent.indexOf('*') !== -1;
      if (!isMarkedRequired) return; // not required

      const fieldContainer = ac.closest('.ui-field');
      const validationEl = fieldContainer ? fieldContainer.querySelector('.ui-field__validation') : null;

      if (!hidden.value || hidden.value.trim() === '') {
        if (fieldContainer) fieldContainer.classList.add('is-invalid');
        if (validationEl) validationEl.textContent = 'Ovo polje je obavezno.';
        isValid = false;
      } else {
        if (fieldContainer) fieldContainer.classList.remove('is-invalid');
        if (validationEl) validationEl.textContent = '';
      }
    });

    return isValid;
  };

  /**
   * Attach blur validation listeners to required fields
   */
  window.CinemaUI.initFieldValidation = function () {
    document.addEventListener(
      "blur",
      (e) => {
        const field = e.target;
        if (!field.matches("[required]")) return;

        const fieldContainer = field.closest(".ui-field");
        if (!fieldContainer) return;

        const value = field.value.trim();
        const validationEl = fieldContainer.querySelector(
          ".ui-field__validation",
        );

        if (!value) {
          fieldContainer.classList.add("is-invalid");
          if (validationEl) {
            validationEl.textContent = "Ovo polje je obavezno.";
          }
        } else {
          fieldContainer.classList.remove("is-invalid");
          if (validationEl) {
            validationEl.textContent = "";
          }
        }
      },
      true,
    );

    // Autocomplete inputs: validate on blur/input
    document.addEventListener('input', (e) => {
      const target = e.target;
      if (!target.matches('.autocomplete-field__input')) return;

      const ac = target.closest('.autocomplete-field');
      if (!ac) return;

      const hidden = ac.querySelector('[data-autocomplete-value]');
      const fieldContainer = ac.closest('.ui-field');
      const validationEl = fieldContainer ? fieldContainer.querySelector('.ui-field__validation') : null;

      const label = ac.querySelector('.ui-field__label');
      const isMarkedRequired = label && label.textContent && label.textContent.indexOf('*') !== -1;

      if (!isMarkedRequired) {
        if (fieldContainer && validationEl) {
          fieldContainer.classList.remove('is-invalid');
          validationEl.textContent = '';
        }
        return;
      }

      if (!hidden || !hidden.value || hidden.value.trim() === '') {
        if (fieldContainer) fieldContainer.classList.add('is-invalid');
        if (validationEl) validationEl.textContent = 'Ovo polje je obavezno.';
      } else {
        if (fieldContainer) fieldContainer.classList.remove('is-invalid');
        if (validationEl) validationEl.textContent = '';
      }
    }, true);
  };

  // Auto-init on DOMContentLoaded
  document.addEventListener("DOMContentLoaded", () => {
    window.CinemaUI.initFieldValidation();
  });
})();
