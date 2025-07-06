function addDataToProductList(list, item) {
    if (item && item.name && item.unit) {
        const listItem = createHtmlEl('li', { className: 'product-item' }, list);
        const linkEl = createHtmlEl('a', { href: '#', textContent: item.name + ' -> ' + item.unit, onclick: onProductClick }, listItem);
        linkEl.dataset.itemUnit = item.unit;
        linkEl.dataset.itemName = item.name;
    }
    else {
        clearHtmlElement(list);
    }
}

function onProductClick(event) {
    event.preventDefault();
    const itemName = event.target.dataset.itemName;
    const itemUnit = event.target.dataset.itemUnit;

    const inputValueElement = document.querySelector('#product-item-value');
    inputValueElement.value = itemName;

    const inputUnitElement = document.querySelector('#product-unit-value');
    inputUnitElement.value = itemUnit;

    clearHtmlElement(document.querySelector('#product-result-list'));
}