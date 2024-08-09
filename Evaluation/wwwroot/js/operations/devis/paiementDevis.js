let devisId = null;
let ligne = null;
let paimentUrl = null;
let paimentMethod = null;

let dialogContainer = document.getElementById('dialog-container');

let erreurPaiement = document.getElementById('erreurPaiement');
let erreurPaiementMessage = document.getElementById('erreurPaiementMessage');

document.addEventListener('keydown', (event) => {
    if (event.key === 'Escape' || event.key === 'Esc') {
        console.log("Esc pressed");
        if (dialogContainer) {
            dialogContainer.style.display = 'none';
        }
    }
});

function toogleDialog(devisId, ligne, url, method) {
    this.devisId = devisId;
    this.ligne = ligne;
    this.paimentUrl = url;
    this.paimentMethod = method;

    console.log('DevisId: ' + this.devisId);
    console.log('Ligne: ' + this.ligne);
    console.log('URL: ' + this.paimentUrl);
    console.log('Method: ' + this.paimentMethod);

    if (dialogContainer) {
        dialogContainer.style.display = 'block';
    }
}

function dismissDialog() {
    if (dialogContainer) {
        dialogContainer.style.display = 'none';
    }
}

function payerDevis() {
    let currentDate = document.getElementById('inputDate').value;
    let currentMontant = document.getElementById('inputMontant').value;

    console.log('DevisId: ' + this.devisId);
    console.log('CurrentDate: ' + currentDate);
    console.log('CurrentMontant: ' + currentMontant);

    $.ajax({
        url: this.paimentUrl,
        type: this.paimentMethod,
        data: { devisId: this.devisId, date: currentDate, montant: currentMontant },
        success: function (response) {
            console.log('Response Message: ', response.message);
            console.log('Response Erreur: ', response.error);
            const responseMessage = response.message;
            erreurPaiementMessage.innerText = '';

            if (responseMessage == 'success') {
                erreurPaiement.style.display = 'none';
                dismissDialog();
            } else if (responseMessage == 'alreadyPaid') {
                erreurPaiement.style.display = 'block';
                erreurPaiementMessage.innerText = 'Ce devis est deja payé en totalité, Reste a payer ' + response.data;
            } else if (responseMessage == 'amountOverflow') {
                erreurPaiement.style.display = 'block';
                erreurPaiementMessage.innerText = 'Montant total < montant payé, Reste a payer ' + response.data;
            }

            if (response.error) {
                erreurPaiement.style.display = 'block';
                erreurPaiementMessage.innerText = response.error;
            }
        },
        error: function (error) {
            console.log('Console.log: erreur lors de la paiement: ', error);
            alert(error.responseText);
        }
    });
}