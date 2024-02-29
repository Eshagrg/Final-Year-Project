//$(document).ready(function () {
   
//    var data = {
//        labels: ['Vendors', 'Sales', 'Profit/Loss'],
//        datasets: [
//            {
//                label: 'Data',
//                data: [155550, 755555, -25555],
//                backgroundColor: ['rgba(75, 192, 192, 0.7)', 'rgba(255, 159, 64, 0.7)', 'rgba(255, 99, 132, 0.7)'],
//                borderWidth: 0
//            }
//        ]
//    };

 
//    var options = {
        
//        scale: {
//            angleLines: {
//                display: false
//            },
//            ticks: {
//                suggestedMin: -20000,
//                suggestedMax: 2000000,
//                stepSize: 100
//            }
//        },
//        legend: {
//            legend: {
//                display: true,
//                position: "bottom",
//                labels: {
//                    fontColor: "#333",
//                    fontSize: 8,
//                },
//            },
//        },
//        tooltips: {
//            enabled: true,
//            mode: 'single',
//            callbacks: {
//                label: function (tooltipItem, data) {
//                    var value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
//                    if (tooltipItem.index === 2) {
//                        return (value > 0 ? 'Profit: ' : 'Loss: ') + Math.abs(value) + '%';
//                    }
//                    return value;
//                }
//            }
//        }
//    };

  
//    var ctx = document.getElementById('polar-chart').getContext('2d');
//    new Chart(ctx, {
//        type: 'polarArea',
//        data: data,
//        options: options
//    });
//});

$(document).ready(function () {
    var vendorTransactions = 155550; // Total vendor transactions
    var salesDone = 755555; // Total sales done

    var profitLoss = salesDone - vendorTransactions;

    var data = {
        labels: ['Vendors', 'Sales', 'Profit/Loss'],
        datasets: [
            {
                label: 'Vendors',
                data: [vendorTransactions],
                backgroundColor: 'rgba(75, 192, 192, 0.7)',
                borderWidth: 0
            },
            {
                label: 'Sales',
                data: [salesDone],
                backgroundColor: 'rgba(255, 159, 64, 0.7)',
                borderWidth: 0
            },
            {
                label: 'Profit/Loss',
                data: [profitLoss],
                backgroundColor: 'rgba(255, 99, 132, 0.7)',
                borderWidth: 0
            }
        ]
    };

    var options = {
        scales: {
            x: {
                ticks: {
                    beginAtZero: true,
                    stepSize: 100000,
                    callback: function (value) {
                        return value.toLocaleString();
                    }
                }
            },
            y: {
                ticks: {
                    display: false
                }
            }
        },
        plugins: {
            legend: {
                display: true,
                position: 'bottom',
                labels: {
                    fontColor: '#333',
                    fontSize: 8
                }
            },
            tooltip: {
                enabled: true,
                callbacks: {
                    label: function (context) {
                        var value = context.parsed.y;
                        if (context.datasetIndex === 2) {
                            return (value > 0 ? 'Profit: ' : 'Loss: ') + Math.abs(value) + '%';
                        }
                        return value.toLocaleString();
                    }
                }
            }
        }
    };

    var ctx = document.getElementById('polar-chart').getContext('2d');
    new Chart(ctx, {
        type: 'bar',
        data: data,
        options: options
    });
});


//horizantal

$(document).ready(function () {
    var totalBudget = [20000, 30000, 25000, 40000, 35000, 45000, 30000, 35000, 40000, 50000, 45000, 35000]; // Total budget for the last 12 months
    var salesDone = [18000, 25000, 21000, 35000, 32000, 38000, 28000, 32000, 35000, 40000, 37000, 30000]; // Sales done for the last 12 months
    var vendorTransactions = [15000, 18000, 16000, 22000, 20000, 24000, 18000, 20000, 22000, 25000, 23000, 19000]; // Vendor transactions for the last 12 months

    var profitLoss = [];
    for (var i = 0; i < totalBudget.length; i++) {
        profitLoss.push(salesDone[i] - totalBudget[i]);
    }

    var labels = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    var data = {
        labels: labels,
        datasets: [
            {
                label: 'Total Used Budget',
                data: totalBudget,
                backgroundColor: 'rgba(75, 192, 192, 0.7)',
                borderWidth: 0
            },
            {
                label: 'Sales Done',
                data: salesDone,
                backgroundColor: 'rgba(255, 159, 64, 0.7)',
                borderWidth: 0
            },
            {
                label: 'Vendor Transactions',
                data: vendorTransactions,
                backgroundColor: 'rgba(255, 99, 132, 0.7)',
                borderWidth: 0
            },
            {
                label: 'Profit/Loss',
                data: profitLoss,
                backgroundColor: 'rgba(54, 162, 235, 0.7)',
                borderWidth: 0
            }
        ]
    };

    var options = {
        indexAxis: 'y',
        scales: {
            x: {
                beginAtZero: true,
                ticks: {
                    callback: function (value) {
                        return value.toLocaleString();
                    }
                }
            },
            y: {
                ticks: {
                    fontColor: '#333',
                    fontSize: 10
                }
            }
        },
        plugins: {
            legend: {
                display: true,
                position: 'bottom',
                labels: {
                    fontColor: '#333',
                    fontSize: 8
                }
            },
            tooltip: {
                enabled: true,
                callbacks: {
                    label: function (context) {
                        var datasetLabel = context.dataset.label || '';
                        var value = context.parsed.x;
                        if (datasetLabel === 'Profit/Loss') {
                            return (value > 0 ? 'Profit: ' : 'Loss: ') + Math.abs(value).toLocaleString();
                        }
                        return datasetLabel + ': ' + value.toLocaleString();
                    }
                }
            }
        }
    };

    var ctx = document.getElementById('horizontal-chart').getContext('2d');
    new Chart(ctx, {
        type: 'bar',
        data: data,
        options: options
    });
});
