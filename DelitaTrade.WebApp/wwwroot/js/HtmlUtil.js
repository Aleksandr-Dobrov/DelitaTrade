function createHtmlEl(tag, properties, container) {
    const element = document.createElement(tag);
    Object.keys(properties).forEach((key) => {
        if (typeof properties[key] === 'object') {
            element[key] ??= {};
            Object.assign(element[key], properties[key]);
        }        
        else {
            element[key] = properties[key];
        }
    });

    if (container) container.append(element);

    return element;
}

function isEmpty(obj) {
    let isEmptyProperty = false;
    Object.values(obj).forEach((value) => {
        if(typeof(value) === undefined || value === '') isEmptyProperty = true;
    });
    return isEmptyProperty;
}

function clearHtmlElement(element) {
    if (element) {
        element.innerHTML = '';
    }
}