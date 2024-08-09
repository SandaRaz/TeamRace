let idEntity = null;
let ligne = null;
let deleteUrl = null;
let deleteMethod = null;

let dialogContainer = document.getElementById('dialog-container');
let errorContainer = document.getElementById('errorDefaultDisplay');
let errorContent = document.getElementById('errorContent');

document.addEventListener('keydown', (event) => {
    if (event.key === 'Escape' || event.key === 'Esc') {
        console.log("Esc pressed");
        if (dialogContainer) {
            dialogContainer.style.display = 'none';
        }
    }
});

function toogleDialog(idEntity, ligne, url, method) {
    this.idEntity = idEntity;
    this.ligne = ligne;
    this.deleteUrl = url;
    this.deleteMethod = method;

    console.log('IdEntity: ' + this.idEntity);
    console.log('Ligne: ' + this.ligne);
    console.log('URL: ' + this.deleteUrl);
    console.log('Method: ' + this.deleteMethod);

    if (dialogContainer) {
        dialogContainer.style.display = 'flex';
    }
}

function dismissDialog() {
    if (dialogContainer && errorContainer) {
        console.log("--- Dismiss dialog ---");
        dialogContainer.style.display = 'none';
        errorContainer.style.display = 'none';
    }
}

function deleteEntity() {
    const currentLigne = this.ligne;

    $.ajax({
        url: this.deleteUrl,
        type: this.deleteMethod,
        data: { id: this.idEntity },
        success: function (response) {
            if (response.result == true) {
                console.log('Affected line: '+response.affected);
                if (response.affected > 0) {
                    dismissDialog();
                    const ln = document.getElementById(currentLigne);
                    ln.remove();
                }
            } else {
                if (errorContainer && errorContent) {
                    errorContent.innerText = response.error;
                    errorContainer.style.display = 'block';
                }
            }
        },
        error: function (error) {
            console.log('Console.log: erreur lors de la suppression: ', error);
        }
    });
}