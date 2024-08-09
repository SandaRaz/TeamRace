﻿let defaultUrl = '';
let searchUrl = '';
let searchMethod = '';
let currentPage = 1;
let offset = 0;
let order = '';
let triColumn = '';

let searchField = document.getElementById('search-field');
let listTable = document.getElementById('list-table');

function listSearch(defaultUrl, searchUrl, method, currentPage, offset, order, triColumn) {
    this.defaultUrl = defaultUrl;
    this.searchUrl = searchUrl;
    this.searchMethod = method;
    this.currentPage = currentPage;
    this.offset = offset;
    this.order = order;
    this.triColumn = triColumn;

    let keyword = searchField.value;
    console.log("Default Url: " + this.defaultUrl);

    if (isBlank(keyword)) {
        $.ajax({
            url: this.defaultUrl,
            method: this.searchMethod,
            data: {
                currentPage: this.currentPage,
                offset: this.offset,
                signe: 0,
                order: this.order,
                triColumn: this.triColumn
            },
            success: function (listObjects) {
                manageTableLine(listTable, listObjects);
            },
            error: function (error) {
                console.log('Erreur lors de la recuperation du liste: ', error);
            }
        });
    } else {
        $.ajax({
            url: this.searchUrl,
            method: this.searchMethod,
            data: {
                keyword: keyword.trim()
            },
            success: function (listObjects) {
                manageTableLine(listTable, listObjects);
            },
            error: function (error) {
                console.log('Erreur lors de la recuperation du liste: ', error);
            }
        });
    }
}

function isBlank(str) {
    return !str.trim();
}

function manageTableLine(table, listObjects) {
    var tbody = table.getElementsByTagName('tbody')[0];

    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }

    listObjects.forEach(function (object) {
        var newTr = document.createElement('tr');
        newTr.id = 'ligne_' + object.id;

        var td1 = document.createElement('td');
        td1.textContent = object.type;

        var td2 = document.createElement('td');
        td2.textContent = new Date(object.dateCreation).toLocaleString('fr-FR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit', second: '2-digit' });
            object.dateCreation;

        // --------- Bouton Modifier -----------
        var td4 = document.createElement('td');
        var button1 = document.createElement('button');
        button1.type = 'button';
        button1.classList.add('btn', 'btn-light');
        const button1Url = '/AccountTypeUtils/Update?id=' + object.id;
        button1.setAttribute('onclick', 'window.location.href="' + button1Url + '"');
        button1.textContent = 'Modifier';
        td4.appendChild(button1);

        // --------- Bouton Supprimer ----------
        var td5 = document.createElement('td');
        var button2 = document.createElement('button');
        button2.type = 'button';
        button2.classList.add('btn', 'btn-danger');
        button2.setAttribute('onclick', "toogleDialog('" + object.id + "','ligne_" + object.id + "','/AccountTypeUtils/Delete','POST')");
        button2.textContent = 'Supprimer';
        td5.appendChild(button2);

        newTr.appendChild(td1);
        newTr.appendChild(td2);
        newTr.appendChild(td4);
        newTr.appendChild(td5);

        tbody.appendChild(newTr);
    });
}