$(document).ready(function () {
    $.ajax({
        url: '/Equipe/ClassementGeneraleJson',
        method: 'GET',
        success: function (data) {
            if (data.result) {
                var classements = JSON.parse(data.classements);
                var chartData = [];
                var chartLabels = [];
                for (var i = 0; i < classements.length; i++) {
                    chartData.push(classements[i].Points);
                    chartLabels.push(classements[i].NomEquipe);
                }

                var chart = c3.generate({
                    bindto: '#pieChart',
                    data: {
                        columns: chartLabels.map(function (label, index) {
                            return [label, chartData[index]];
                        }),
                        type: 'pie'
                    }
                });
            } else {
                console.error(data.error);
            }
        },
        error: function (error) {
            console.error('Error fetching data:', error);
        }
    });
});