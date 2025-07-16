function addProductToList(list, item) {
    if (item && item.name) {
        const listItem = createHtmlEl('li', { className: 'product-item' }, list);
        const linkEl = createHtmlEl('a', { href: '#', textContent: item.name + ' -> ' + item.companyName, onclick: onItemClick }, listItem);
        linkEl.dataset.itemId = item.id;
        linkEl.dataset.itemName = item.name;
    }
    else {
        clearHtmlElement(list);
    }
}

function onItemClick(event) {
    event.preventDefault();
    const itemName = event.target.dataset.itemName;
    const itemId = event.target.dataset.itemId;

    const inputValueElement = document.querySelector('#item-value');
    inputValueElement.value = itemName;

    const inputIdElement = document.querySelector('#item-id');
    inputIdElement.value = itemId;
    
    clearHtmlElement(document.querySelector('#result-list'));
}