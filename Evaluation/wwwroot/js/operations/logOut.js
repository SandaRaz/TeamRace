let idEntity = null;
let ligne = null;
let logOutUrl = null;
let logOutMethod = null;

let dialogContainer = document.getElementById('dialog-container');

document.addEventListener('keydown', (event) => {
    if (event.key === 'Escape' || event.key === 'Esc') {
        console.log("Esc pressed");
        if (dialogContainer) {
            dialogContainer.style.display = 'none';
        }
    }
});

function toogleDialog(url, method) {
    this.logOutUrl = url;
    this.logOutMethod = method;

    console.log('URL: ' + this.logOutUrl);
    console.log('Method: ' + this.logOutMethod);

    if (dialogContainer) {
        dialogContainer.style.display = 'flex';
    }
}

function dismissDialog() {
    if (dialogContainer) {
        dialogContainer.style.display = 'none';
    }
}

function logOut() {
    $.ajax({
        url: this.logOutUrl,
        type: this.logOutMethod,
        success: function (logOut) {
            console.log('Log out result: ', logOut);
            if (logOut == 'success') {
                window.location.reload();
            }
        },
        error: function (error) {
            console.log('Console.log: erreur lors de la suppression: ', error);
        }
    });
}