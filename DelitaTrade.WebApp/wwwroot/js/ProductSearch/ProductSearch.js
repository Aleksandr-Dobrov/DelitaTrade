document.addEventListener('DOMContentLoaded', init);

const productServerProvider = new ServerProvider(productSearchUrl);

function init() {
    document.querySelector('#product-item-value').addEventListener('input', getproductsFromServer);
    document.querySelector('#product-item-value').addEventListener('keydown', onProductKeyDown);
}

function getproductsFromServer(e) {
    e.preventDefault();
    const inputElement = document.querySelector('#product-item-value');

    productServerProvider.readObject('?data=' + inputElement.value, (result) => {
        const items = document.querySelector('#product-result-list');
        clearHtmlElement(items);
        Object.values(result).forEach((data) => addDataToProductList(items, data))
    }, errorLog)

}

function onProductKeyDown(e) {
    if (e.key === 'Escape') {
        const items = document.querySelector('#product-result-list');
        clearHtmlElement(items);
    }
}

function errorLog(error) {
    console.error(error);
}