function exportPdf(fileName, baliseId, url) {
    console.log('Chemin d\'origine: ' + window.location.origin);
    var originPath = window.location.origin;
    var contentHtml = `
        <html>
            <head>
                <link rel="stylesheet" href="${originPath}/lib/bootstrap/dist/css/bootstrap.min.css" />

                <link rel="stylesheet" href="${originPath}/template/assets/vendors/feather/feather.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/ti-icons/css/themify-icons.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/css/vendor.bundle.base.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/font-awesome/css/font-awesome.min.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/mdi/css/materialdesignicons.min.css">

                <link rel="stylesheet" href="${originPath}/template/assets/vendors/datatables.net-bs5/dataTables.bootstrap5.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/ti-icons/css/themify-icons.css">
                <link rel="stylesheet" type="text/css" href="${originPath}/template/assets/js/select.dataTables.min.css">

                <link rel="stylesheet" href="${originPath}/css/docs.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/site.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/template.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/dialog.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/template/assets/css/style.css">
                <link rel="shortcut icon" href="${originPath}/template/assets/images/favicon.png" />
            </head>
            <body>
                <div class="container-scroller">
                    <div class="container-fluid page-body-wrapper">
                        <div class="main-panel">
                            <div class="content-wrapper">
                                <div class="row">
                                    <div class="col-4">
                                        ${document.getElementById(baliseId).innerHTML}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </body>
        </html>
    `;

    //console.log(contentHtml);
    console.log('Export to pdf ' + fileName);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
    xhr.responseType = 'blob';

    xhr.onload = function () {
        if (xhr.status === 200) {
            var blob = xhr.response;
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName + ".pdf";
            link.click();
        } else {
            console.error('Error: ' + xhr.statusText);
        }
    };

    xhr.onerror = function () {
        console.error('Request failed');
    }

    var requestData = {
        htmlContent: contentHtml,
        fileName: fileName
    };

    xhr.send(JSON.stringify(requestData));
}

function exportPdfContent(fileName, url, content) {
    console.log('Chemin d\'origine: ' + window.location.origin);
    var originPath = window.location.origin;
    var contentHtml = `
        <html>
            <head>
                <link rel="stylesheet" href="${originPath}/lib/bootstrap/dist/css/bootstrap.min.css" />

                <link rel="stylesheet" href="${originPath}/template/assets/vendors/feather/feather.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/ti-icons/css/themify-icons.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/css/vendor.bundle.base.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/font-awesome/css/font-awesome.min.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/mdi/css/materialdesignicons.min.css">

                <link rel="stylesheet" href="${originPath}/template/assets/vendors/datatables.net-bs5/dataTables.bootstrap5.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/ti-icons/css/themify-icons.css">
                <link rel="stylesheet" type="text/css" href="${originPath}/template/assets/js/select.dataTables.min.css">

                <link rel="stylesheet" href="${originPath}/css/docs.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/site.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/template.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/dialog.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/template/assets/css/style.css">
                <link rel="shortcut icon" href="${originPath}/template/assets/images/favicon.png" />

                <link rel="stylesheet" href="${originPath}/css/certificat.css" asp-append-version="true" />
            </head>
            <body>
                <div class="container-scroller">
                    <div class="container-fluid page-body-wrapper">
                        <div class="main-panel">
                            <div class="content-wrapper">
                                ${content}
                            </div>
                        </div>
                    </div>
                </div>
            </body>
        </html>
    `;

    //console.log(contentHtml);
    console.log('Export to pdf ' + fileName);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
    xhr.responseType = 'blob';

    xhr.onload = function () {
        if (xhr.status === 200) {
            var blob = xhr.response;
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName + ".pdf";
            link.click();
        } else {
            console.error('Error: ' + xhr.statusText);
        }
    };

    xhr.onerror = function () {
        console.error('Request failed');
    }

    var requestData = {
        htmlContent: contentHtml,
        fileName: fileName
    };

    xhr.send(JSON.stringify(requestData));
}

function exportCertificatPdf(fileName, url) {
    console.log('Exporter certificat des premier');
    $.ajax({
        url: url,
        type: 'GET',
        success: function (response) {
            if (response) {
                if (response.result === true) {
                    champions = JSON.parse(response.champions);
                    console.log(champions);

                    champions.forEach((value) => {
                        console.log(value.Nom);

                        var certificat = `
                            <div class="row">
                                <div class="Bordure1 col-lg-10 mx-auto my-auto">
                                    <div class="Bordure2 mx-auto my-auto">
                                        <div class="Bordure3 mx-auto">
                                            <div class="row">
                                                <div class="col-8 mx-auto text-center my-2">
                                                    <h1 class="certificat-title">CERTIFICATE</h1>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-8 mx-auto text-center my-2">
                                                    <h3 class="simple-text">1st place of <span class="special-text">Ultimate Team Race</span></h3>
                                                </div>
                                            </div>
                                            <hr />
                                            <div class="row">
                                                <div class="col-8 mx-auto text-center">
                                                    <h1 class="certificat-title">-  EQUIPE ${value.Nom}  -</h1>
                                                </div>
                                            </div>
                                            <hr />
                                            <div class="row">
                                                <div class="col-4 text-center">
                                                    <p>Signature</p>
                                                    <hr />
                                                    <p class="signature">Lorem</p>
                                                </div>
                                                <div class="col-4 text-center">
                                                    <p>Points: ${value.TempPoint}</p>
                                                </div>
                                                <div class="col-4 text-center">
                                                    <p>Date</p>
                                                    <hr />
                                                    <p class="dateCertificat">${Date.now()}</p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>`;
                        exportPdfContent(fileName, '/Utils/ExportPdfA3', certificat);

                    });
                }
            }
        },
        error: function (error) {
            console.log('Console.log: erreur lors de la suppression: ', error);
        }
    });
}

function exportCertificatPdfParCategorie(fileName, url) {
    const categorieIdContainer = document.getElementById("idCategorie");
    if (categorieIdContainer) {
        let categorieId = categorieIdContainer.value;
        console.log('ID Categorie: ' + categorieId);

        $.ajax({
            url: url,
            type: 'GET',
            data: { categorieId: categorieId },
            success: function (response) {
                if (response) {
                    if (response.result === true) {
                        champions = JSON.parse(response.champions);
                        categorie = JSON.parse(response.categorie);
                        console.log(champions);
                        console.log(categorie);

                        champions.forEach((value) => {
                            console.log(value.Nom);

                            var certificat = `
                                <div class="row">
                                    <div class="Bordure1 col-lg-10 mx-auto my-auto">
                                        <div class="Bordure2 mx-auto my-auto">
                                            <div class="Bordure3 mx-auto">
                                                <div class="row">
                                                    <div class="col-8 mx-auto text-center my-2">
                                                        <h1 class="certificat-title">CERTIFICATE</h1>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-8 mx-auto text-center my-2">
                                                        <h3 class="simple-text">1st place of <span class="special-text">Ultimate Team Race</span></h3>
                                                        <h3 class="simple-text">Categorie ${categorie.Nom}</h3>
                                                    </div>
                                                </div>
                                                <hr />
                                                <div class="row">
                                                    <div class="col-8 mx-auto text-center">
                                                        <h1 class="certificat-title">-  EQUIPE ${value.Nom}  -</h1>
                                                    </div>
                                                </div>
                                                <hr />
                                                <div class="row">
                                                    <div class="col-4 text-center">
                                                        <p>Signature</p>
                                                        <hr />
                                                        <p class="signature">Lorem</p>
                                                    </div>
                                                    <div class="col-4 text-center">
                                                        <p>Points: ${value.TempPoint}</p>
                                                    </div>
                                                    <div class="col-4 text-center">
                                                        <p>Date</p>
                                                        <hr />
                                                        <p class="dateCertificat">${Date.now()}</p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`;
                            exportPdfContent(fileName, '/Utils/ExportPdfA3', certificat);

                        });
                    }
                }
            },
            error: function (error) {
                console.log('Console.log: erreur lors de la suppression: ', error);
            }
        });
    }
}

function exportExcel(fileName, baliseId, url) {
    console.log('Chemin d\'origine: ' + window.location.origin);

    var contentHtml = document.getElementById(baliseId).innerHTML;

    //console.log(contentHtml);
    console.log('Export to excel ' + fileName);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
    xhr.responseType = 'blob';

    xhr.onload = function () {
        if (xhr.status === 200) {
            var blob = xhr.response;
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName + ".xls";
            link.click();
        } else {
            console.error('Error: ' + xhr.statusText);
        }
    };

    xhr.onerror = function () {
        console.error('Request failed');
    }

    var requestData = {
        htmlContent: contentHtml,
        fileName: fileName
    };

    xhr.send(JSON.stringify(requestData));
}



function exportCertificatChampionsPdf(fileName, baliseId, url) {
    console.log('Chemin d\'origine: ' + window.location.origin);
    var originPath = window.location.origin;
    var contentHtml = `
        <html>
            <head>
                <link rel="stylesheet" href="${originPath}/lib/bootstrap/dist/css/bootstrap.min.css" />

                <link rel="stylesheet" href="${originPath}/template/assets/vendors/feather/feather.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/ti-icons/css/themify-icons.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/css/vendor.bundle.base.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/font-awesome/css/font-awesome.min.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/mdi/css/materialdesignicons.min.css">

                <link rel="stylesheet" href="${originPath}/template/assets/vendors/datatables.net-bs5/dataTables.bootstrap5.css">
                <link rel="stylesheet" href="${originPath}/template/assets/vendors/ti-icons/css/themify-icons.css">
                <link rel="stylesheet" type="text/css" href="${originPath}/template/assets/js/select.dataTables.min.css">

                <link rel="stylesheet" href="${originPath}/css/docs.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/site.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/template.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/css/dialog.css" asp-append-version="true" />
                <link rel="stylesheet" href="${originPath}/template/assets/css/style.css">
                <link rel="shortcut icon" href="${originPath}/template/assets/images/favicon.png" />
            </head>
            <body>
                <div class="container-scroller">
                    <div class="container-fluid page-body-wrapper">
                        <div class="main-panel">
                            <div class="content-wrapper">
                                <div class="row">
                                    <div class="col-4">
                                        ${document.getElementById(baliseId).innerHTML}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </body>
        </html>
    `;

    //console.log(contentHtml);
    console.log('Export to pdf ' + fileName);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
    xhr.responseType = 'blob';

    xhr.onload = function () {
        if (xhr.status === 200) {
            var blob = xhr.response;
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName + ".pdf";
            link.click();
        } else {
            console.error('Error: ' + xhr.statusText);
        }
    };

    xhr.onerror = function () {
        console.error('Request failed');
    }

    var requestData = {
        htmlContent: contentHtml,
        fileName: fileName
    };

    xhr.send(JSON.stringify(requestData));
}