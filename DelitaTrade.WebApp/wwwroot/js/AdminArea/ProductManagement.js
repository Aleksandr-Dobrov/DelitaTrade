document.addEventListener('DOMContentLoaded', initProductSelector);

function initProductSelector(e) {
    document.querySelector('#product-item-value').addEventListener('input', unselectProduct);
}
function addDataToProductList(list, item) {
    if (item && item.name && item.unit) {
        const listItem = createHtmlEl('li', { className: 'product-item' }, list);
        const linkEl = createHtmlEl('a', { href: '#', textContent: item.name + ' -> ' + item.unit, onclick: onProductClick }, listItem);
        linkEl.dataset.itemUnit = item.unit;
        linkEl.dataset.itemName = item.name;
        linkEl.dataset.itemNumber = item.number;
    }
    else {
        clearHtmlElement(list);
    }
}

function onProductClick(event) {
    event.preventDefault();
    const itemName = event.target.dataset.itemName;
    const itemUnit = event.target.dataset.itemUnit;
    const itemNumber = event.target.dataset.itemNumber;

    const inputValueElement = document.querySelector('#product-item-value');
    inputValueElement.value = itemName;

    selectProduct(itemName, itemUnit, itemNumber);
    fillProductFields(itemName, itemUnit, itemNumber);

    clearHtmlElement(document.querySelector('#product-result-list'));
}

function selectProduct(name, unit, number) {
    const inputValueElement = document.querySelector('#selected-product-name');
    inputValueElement.value = name;
    const inputUnitElement = document.querySelector('#selected-product-unit');
    inputUnitElement.value = unit;
    const inputNumberElement = document.querySelector('#selected-product-number');
    inputNumberElement.value = number;

    const editButton = document.querySelector('#edit-button');
    if (editButton) {
        editButton.disabled = false;
    }
} 

function unselectProduct() {
    const inputValueElement = document.querySelector('#selected-product-name');
    inputValueElement.value = '';
    const inputUnitElement = document.querySelector('#selected-product-unit');
    inputUnitElement.value = '';
    const inputNumberElement = document.querySelector('#selected-product-number');
    inputNumberElement.value = '';
    const editButton = document.querySelector('#edit-button');
    if (editButton) {
        editButton.disabled = true;
    }
}

function fillProductFields(name, unit, number) {
    const inputValueElement = document.querySelector('#product-name-value');
    inputValueElement.value = name;
    const inputUnitElement = document.querySelector('#product-unit-value');
    inputUnitElement.value = unit;
    const inputNumberElement = document.querySelector('#product-number-value');
    inputNumberElement.value = number;
}

function onEditProductClick() {
    const inputSelectedValueElement = document.querySelector('#selected-product-name');
    const inputSelectedUnitElement = document.querySelector('#selected-product-unit');
    const inputSelectedNumberElement = document.querySelector('#selected-product-number');

    const editedValueElement = document.querySelector('#edited-product-name');
    const editedUnitElement = document.querySelector('#edited-product-unit');
    const editedNumberElement = document.querySelector('#edited-product-number');

    editedValueElement.value = inputSelectedValueElement.value;
    editedUnitElement.value = inputSelectedUnitElement.value;
    editedNumberElement.value = inputSelectedNumberElement.value;

    const inputValueElement = document.querySelector('#product-name-value');
    const inputUnitElement = document.querySelector('#product-unit-value');
    const inputNumberElement = document.querySelector('#product-number-value');

    const formProductName = document.querySelector('#edit-product-name');
    const formProductUnit = document.querySelector('#edit-product-unit');
    const formProductNumber = document.querySelector('#edit-product-number');

    formProductName.value = inputValueElement.value;
    formProductUnit.value = inputUnitElement.value;
    formProductNumber.value = inputNumberElement.value;
}

function onCreateProductClick() {
    const inputValueElement = document.querySelector('#product-name-value');
    const inputUnitElement = document.querySelector('#product-unit-value');
    const inputNumberElement = document.querySelector('#product-number-value');

    const formProductName = document.querySelector('#create-product-name');
    const formProductUnit = document.querySelector('#create-product-unit');
    const formProductNumber = document.querySelector('#create-product-number');

    formProductName.value = inputValueElement.value;
    formProductUnit.value = inputUnitElement.value;
    formProductNumber.value = inputNumberElement.value;
}