document.addEventListener('DOMContentLoaded', init);

const serverProvider = new ServerProvider(searchProductUrl);

function init() {
    document.querySelector('#search textarea').addEventListener('input', getDataFromServer);
}

function getDataFromServer(e) {
    e.preventDefault();
    const inputElement = document.querySelector('#search textarea');

    serverProvider.readObject('?data=' + inputElement.value, (result) => {
        const products = document.querySelector('#products');
        products.innerHTML = ''; 
        Object.values(result).forEach((data) => addProductToList(data))
    }, errorLog)

}

function addProductToList(product) {
    const products = document.querySelector('#products');
    createHtmlEl('li', { textContent: product.name }, products);
}

function errorLog(error) {
    console.error(error);
}