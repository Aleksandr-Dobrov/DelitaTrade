document.addEventListener('DOMContentLoaded', init);

const productServerProvider = new ServerProvider(productSearchUrl);

function init() {
    document.querySelector('#product-item-value').addEventListener('input', getproductsFromServer);
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

function errorLog(error) {
    console.error(error);
}