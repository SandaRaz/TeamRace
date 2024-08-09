let histogrammeUrl = '';
let histogrammeMethod = '';

function fetchHistogrammeDevis(url, method) {
    let annee = document.getElementById('annee-devis').value;
    console.log('Annee choisit: ' + annee);
    
    $.ajax({
        url: url,
        method: method,
        data: { anneestring: annee },
        success: function (data) {
            var mois = data.map(function (item) { return item.mois; });
            var montants = data.map(function (item) { return item.totalMontant; });

            var ctx = document.getElementById('histogramme_montantdevis').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: mois,
                    datasets: [{
                        label: 'Montant par mois',
                        data: montants,
                        backgroundColor: 'rgba(54,162,235,0.5)',
                        borderColor: 'rgba(54,162,235,1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

