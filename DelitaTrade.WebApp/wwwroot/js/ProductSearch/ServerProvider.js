class ServerProvider {    
    constructor(url) {
        this.url = url;
    }

    readData(onSuccess, onError) {
        fetch(this.url)
        .then((response) => response.json())
        .then(onSuccess)
        .catch(onError);    
    }

    readObject(arg, onSuccess, onError) {
        fetch(this.url + '/' + arg)
        .then((response) => response.json())
        .then(onSuccess)
        .catch(onError); 
    }

    createData(obj, onSuccess, onError) {
        fetch(this.url, {
            method: 'POST',
            body: JSON.stringify(obj)
        })
        .then((response) => response.json())
        .then(onSuccess)
        .catch(onError);
    }

    updateData(obj, onSuccess, onError) {
        fetch(this.url + '/' + obj._id, {
            method: 'PUT',
            body: JSON.stringify(obj)
        })
        .then((response) => response.json())
        .then(onSuccess)
        .catch(onError);
    }

    deleteData(arg ,onSuccess, onError) {
        fetch(this.url + '/' + arg, {
            method: 'DELETE'
        })
        .then((response) => response.json())
        .then(onSuccess)
        .catch(onError);
    }
}