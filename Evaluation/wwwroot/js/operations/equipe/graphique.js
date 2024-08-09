function getRandomColor() {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)';
}

function graphiqueEquipe() {
    $.ajax({
        url: '/Equipe/ClassementGeneraleJson',
        method: 'GET',
        success: function (response) {
            if (response.result) {
                // Parse the data from the response
                var classements = JSON.parse(response.classements);

                // Extract the team names and points
                var equipeNoms = classements.map(function (item) { return item.NomEquipe; });
                var points = classements.map(function (item) { return item.Points; });

                var backgroundColors = [];
                var borderColors = [];
                for (var i = 0; i < equipeNoms.length; i++) {
                    var color = getRandomColor();
                    backgroundColors.push(color);
                    borderColors.push(color.replace('0.2', '1'));
                }

                // Get the context of the canvas element we want to select
                var ctx = document.getElementById('pieChart').getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'pie', // You can change this to 'pie' if you want a pie chart
                    data: {
                        labels: equipeNoms,
                        datasets: [{
                            label: 'Points des équipes',
                            data: points,
                            backgroundColor: backgroundColors,
                            borderColor: borderColors,
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        legend: {
                            position: 'top',
                        },
                        animation: {
                            animateScale: true,
                            animateRotate: true
                        }
                    }
                });
            } else {
                console.error(response.error);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

function graphiqueEquipeCategorie(idCategorie) {
    $.ajax({
        url: '/Equipe/ClassementparCategorieJson',
        method: 'GET',
        data: { categorieId: idCategorie },
        success: function (response) {
            if (response.result) {
                // Parse the data from the response
                var classements = JSON.parse(response.classements);

                // Extract the team names and points
                var equipeNoms = classements.map(function (item) { return item.NomEquipe; });
                var points = classements.map(function (item) { return item.Points; });

                var backgroundColors = [];
                var borderColors = [];
                for (var i = 0; i < equipeNoms.length; i++) {
                    var color = getRandomColor();
                    backgroundColors.push(color);
                    borderColors.push(color.replace('0.2', '1'));
                }

                // Get the context of the canvas element we want to select
                var ctx = document.getElementById('pieChart').getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'pie', // You can change this to 'pie' if you want a pie chart
                    data: {
                        labels: equipeNoms,
                        datasets: [{
                            label: 'Points des équipes',
                            data: points,
                            backgroundColor: backgroundColors,
                            borderColor: borderColors,
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        legend: {
                            position: 'top',
                        },
                        animation: {
                            animateScale: true,
                            animateRotate: true
                        }
                    }
                });
            } else {
                console.error(response.error);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

let idCategorieContainer = document.getElementById("idCategorie");
if (idCategorieContainer) {
    console.log('ID Categorie: ' + idCategorieContainer.value);
    graphiqueEquipeCategorie(idCategorieContainer.value);
} else {
    graphiqueEquipe();
}