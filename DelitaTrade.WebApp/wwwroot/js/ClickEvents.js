document.addEventListener('DOMContentLoaded', ChangeClassOnClic(oldClassName, newClassName));

function ChangeClassOnClic(oldClassName, newClassName) {
    const itemEl = document.querySelectorAll('.' + oldClassName);
    itemEl.forEach(function (item) {
        item.addEventListener('click', function () {
            item.classList.remove(oldClassName)
            item.classList.toggle(newClassName);
        });
    });
}
