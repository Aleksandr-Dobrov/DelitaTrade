document.addEventListener('DOMContentLoaded', initiateFilter)

function initiateFilter(e) {
    const filterArg = document.querySelector('#filter-arg');
    const filteredElements = document.querySelectorAll('.day-report-list-container');
    if (filterArg == null) return;
    filteredObjects(filterArg.value, filteredElements);
    
    filterArg.addEventListener('input', filterChange);
}

function filterChange(e) {
    const filterArg = document.querySelector('#filter-arg');
    const filteredElements = document.querySelectorAll('.day-report-list-container');
    filteredObjects(filterArg.value, filteredElements);
}

function filteredObjects(filterArg, filteredElement) {
    if (filterArg == 'All') {
        filteredElement.forEach((el) => {
            setVisible(el);
        });
    }
    else if (filterArg == 'Completed') {
        document.querySelectorAll(completed).forEach((el) => {
            setVisible(el);
        });

        document.querySelectorAll(notCompleted).forEach((el) => {
            setHide(el);
        });
    }
    else if (filterArg == 'Not completed') {
        document.querySelectorAll(completed).forEach((el) => {
            setHide(el);
        });

        document.querySelectorAll(notCompleted).forEach((el) => {
            setVisible(el);
        });
    }

    const filter = document.querySelector('#filter');
    if (filter) {
        filter.value = filterArg;
    }
}

function setVisible(el) {
    el.style.display = "block";
}

function setHide(el) {
    el.style.display = "none";
}
