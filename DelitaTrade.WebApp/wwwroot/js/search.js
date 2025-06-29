document.addEventListener('DOMContentLoaded', init);

const serverProvider = new ServerProvider(searchUrl);

function init() {
    document.querySelector('#item-value').addEventListener('input', getDataFromServer);
}

function getDataFromServer(e) {
    e.preventDefault();
    const inputElement = document.querySelector('#item-value');

    serverProvider.readObject('?data=' + inputElement.value, (result) => {
        const items = document.querySelector('#result-list');
        clearHtmlElement(items);
        Object.values(result).forEach((data) => addProductToList(items, data))
    }, errorLog)

}

function errorLog(error) {
    console.error(error);
}