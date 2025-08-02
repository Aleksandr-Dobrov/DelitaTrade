document.addEventListener('DOMContentLoaded', initNotification);

function initNotification(e) {
    infoEl = document.querySelector('#info-message');
    successEl = document.querySelector('#success-message');
    warningEl = document.querySelector('#warning-message');
    errorEl = document.querySelector('#error-message');

    if (infoEl && infoEl.value) {
        toastr.info(infoEl.value);
    }
    if (successEl && successEl.value) {
        toastr.success(successEl.value);
    }
    if (warningEl && warningEl.value) {
        toastr.warning(warningEl.value);
    }
    if (errorEl && errorEl.value) {
        toastr.error(errorEl.value);
    }
}