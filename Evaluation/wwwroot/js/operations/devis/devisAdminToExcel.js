document.getElementById("exportExcelButton").addEventListener("click", function () {
    var table = document.getElementById("list-table");

    table2excel.export(table, {
        name: "myfile" + new Date()+".xls",
        format: "xls"
    });
});