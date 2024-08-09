let dialogContainer = document.getElementById('dialog-container');
let personAccountList = null;

function toogleDialog() {
    if (dialogContainer) {
        dialogContainer.style.display = 'flex';
    }
}

function dismissDialog() {
    if (dialogContainer) {
        dialogContainer.style.display = 'none';
    }
}

document.addEventListener('keydown', (event) => {
    if (event.key === 'Escape' || event.key === 'Esc') {
        console.log("Esc pressed");
        if (dialogContainer) {
            dialogContainer.style.display = 'none';
        }
    }
});

$(document).ready(function () {
    $('#importButton').click(function () {
        let ligneValidContainer = document.getElementById('ligne-valid');
        while (ligneValidContainer.firstChild) {
            ligneValidContainer.removeChild(ligneValidContainer.firstChild);
        }
        let ligneInvalidContainer = document.getElementById('ligne-invalid');
        while (ligneInvalidContainer.firstChild) {
            ligneInvalidContainer.removeChild(ligneInvalidContainer.firstChild);
        }

        event.preventDefault();

        console.log('Dialog Container' + dialogContainer);
        toogleDialog();

        var formData = new FormData();
        var fileInput = $('#inputCsvFile')[0].files[0];
        formData.append('file', fileInput);

        $.ajax({
            url: '/Import/ImportCsvJson',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                console.log(response);

                personAccountList = response.personAccounts;
                console.log('>>> ' + personAccountList);
                const lineErrorsList = response.lineErrors;

                var lvTitle = document.createElement('b');
                lvTitle.textContent = 'Nombre de ligne valide: ' + personAccountList.length;
                ligneValidContainer.append(lvTitle);

                var liTitle = document.createElement('b');
                liTitle.textContent = 'Nombre de ligne invalide: ' + lineErrorsList.length;
                ligneInvalidContainer.append(liTitle);
                lineErrorsList.forEach((lineError) => {
                    var pError = document.createElement('p');
                    pError.innerHTML = '<b>Line ' + (lineError.line) + ':</b>' + ' ' + lineError.error;
                    ligneInvalidContainer.append(pError);
                });
            },
            error: function (error) {
                console.log('Console.log: erreur lors de la suppression: ', error);
            }
        });
    });
});

function importLinesToTable() {
    console.log('PersonAccountList: ' + personAccountList);
    $.ajax({
        dataType: 'json',
        type: 'POST',
        url: '/Import/ImportCsvToTable',
        data: { personAccounts: personAccountList },
        success: function (response) {
            console.log('Repsponse => ' + response);
            if (response == 'success') {
                dismissDialog();
            }
        },
        error: function (xhr,status,error) {
            console.log('Console.log: erreur lors de l\'importation', error);
        }
    });
}