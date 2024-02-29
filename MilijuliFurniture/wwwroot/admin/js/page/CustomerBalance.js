
//Bar chart for saes in the month
var incomeData = [];
var expensesData = [];
var profitData = [];

for (var i = 1; i <= 30; i++) {
    var income = Math.floor(Math.random() * 1000) + 500; 
    var expenses = Math.floor(Math.random() * 800) + 400;
    var profit = income - expenses; 

    incomeData.push(income);
    expensesData.push(expenses);
    profitData.push(profit);
}

var ctx = document.getElementById('comboChart').getContext('2d');

var chart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: Array.from({ length: 30 }, (_, i) => `May ${i + 1}`),
        datasets: [
            {
                label: 'Income',
                data: incomeData,
                backgroundColor: 'rgba(0, 123, 255, 0.5)',
                borderColor: 'rgba(0, 123, 255, 1)',
                borderWidth: 1,
            },
            {
                label: 'Expenses',
                data: expensesData,
                backgroundColor: 'rgba(255, 99, 132, 0.5)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1,
            },
            {
                label: 'Profit',
                type: 'line',
                data: profitData,
                borderColor: 'rgba(0, 0, 0, 0.5)',
                borderWidth: 2,
                fill: false,
            },
        ],
    },
    options: {
        scales: {
            y: {
                beginAtZero: true,
            },
        },
    },
});


//Bar Chart for sales of last 12 months
var incomeData = [];
var expensesData = [];
var profitData = [];


for (var i = 0; i < 12; i++) {
    var income = Math.floor(Math.random() * 1000) + 500; 
    var expenses = Math.floor(Math.random() * 800) + 400; 
    var profit = income - expenses; 

    incomeData.push(income);
    expensesData.push(expenses);
    profitData.push(profit);
}

var ctx = document.getElementById('comboChart1').getContext('2d');

var chart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: [
            'January',
            'Februrary',
            'March',
            'April',
            'May',
            'June',
            'July',
            'August',
            'September',
            'October',
            'November',
            'December',
        ],
        datasets: [
            {
                label: 'Income',
                data: incomeData,
                backgroundColor: 'rgba(0, 123, 255, 0.5)',
                borderColor: 'rgba(0, 123, 255, 1)',
                borderWidth: 1,
            },
            {
                label: 'Expenses',
                data: expensesData,
                backgroundColor: 'rgba(255, 99, 132, 0.5)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1,
            },
            {
                label: 'Profit',
                type: 'line',
                data: profitData,
                borderColor: 'rgba(0, 0, 0, 0.5)',
                borderWidth: 2,
                fill: false,
            },
        ],
    },
    options: {
        scales: {
            y: {
                beginAtZero: true,
            },
        },
    },
});



//Data for profit/loss by customers BarGraph
var customerData = [
    { customer: 'Customer A', amount: 5000 },
    { customer: 'Customer B', amount: -3000 },
    { customer: 'Customer C', amount: 2000 },
    { customer: 'Customer D', amount: -1000 },
    { customer: 'Customer E', amount: 4000 },
    { customer: 'Customer F', amount: -5000 },
    { customer: 'Customer G', amount: 2000 }


];

var customerLabels = customerData.map(function (item) {
    return item.customer;
});

var amounts = customerData.map(function (item) {
    return item.amount;
});

var debtAmounts = amounts.map(function (amount) {
    return amount < 0 ? Math.abs(amount) : 0;
});

var creditAmounts = amounts.map(function (amount) {
    return amount >= 0 ? amount : 0;
});

var chartData = {
    labels: customerLabels,
    datasets: [
        {
            label: 'Debt Amount',
            data: debtAmounts,
            backgroundColor: 'rgba(255, 99, 132, 0.5)',
            borderColor: 'rgba(255, 99, 132, 1)',
            borderWidth: 1
        },
        {
            label: 'Credit Amount',
            data: creditAmounts,
            backgroundColor: 'rgba(0, 123, 255, 0.5)',
            borderColor: 'rgba(0, 123, 255, 1)',
            borderWidth: 1
        }
    ]
};

var ctx = document.getElementById('customerBarChart').getContext('2d');

var chart = new Chart(ctx, {
    type: 'bar',
    data: chartData,
    options: {
        scales: {
            x: {
                stacked: true
            },
            y: {
                stacked: true,
                beginAtZero: true,
                ticks: {
                    callback: function (value, index, values) {
                        return '$' + Math.abs(value);
                    }
                }
            }
        },
        plugins: {
            legend: {
                display: true,
                position: 'bottom',
                labels: {
                    usePointStyle: true
                }
            }
        }
    }
});




//Horizantal graph for Customer due vs Sales
var totalDue = 5000;
var totalSales = 8000;


var duePercentage = ((totalDue / (totalDue + totalSales)) * 100).toFixed(2);
var salesPercentage = ((totalSales / (totalDue + totalSales)) * 100).toFixed(2);


var ctx = document.getElementById('cylinderChart').getContext('2d');


var chart = new Chart(ctx, {
    type: 'horizontalBar',
    data: {
        labels: ['Total'],
        datasets: [
            {
                label: 'Customers\' Due',
                data: [duePercentage],
                backgroundColor: 'rgba(0, 123, 255, 0.5)',
                borderColor: 'rgba(0, 123, 255, 1)',
                borderWidth: 1,
                barThickness: '50%',
                categoryPercentage: 1.0,
            },
            {
                label: 'Total Sales',
                data: [salesPercentage],
                backgroundColor: 'rgba(255, 99, 132, 0.5)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1,
                barThickness: '50%',
                categoryPercentage: 1.0,
            }
        ]
    },
    options: {
        indexAxis: 'y',
        scales: {
            x: {
                beginAtZero: true,
                max: 100,
                ticks: {
                    callback: function (value, index, values) {
                        return value + '%';
                    }
                }
            },
            y: {
                display: false
            }
        },
        plugins: {
            legend: {
                display: true,
                position: 'bottom',
                labels: {
                    usePointStyle: true
                }
            }
        }
    }
});



//Highest paying Customers horizantal chart
var customers = ['Customer 1', 'Customer 2', 'Customer 3', 'Customer 4', 'Customer 5'];
var paymentAmounts = [5000, 8000, 3000, 7000, 9000];


var maxIndex = paymentAmounts.indexOf(Math.max(...paymentAmounts));

// Create an array to store the highlight colors
var backgroundColors = Array(customers.length).fill('rgba(0, 123, 255, 0.5)');
var borderColors = Array(customers.length).fill('rgba(0, 123, 255, 1)');

// Highlight the highest paying customer
backgroundColors[maxIndex] = 'rgba(255, 99, 132, 0.5)';
borderColors[maxIndex] = 'rgba(255, 99, 132, 1)';


var dataset = {
    label: 'Payment Amounts',
    data: paymentAmounts,//[paymentAmounts[maxIndex]],
    backgroundColor: backgroundColors,
    borderColor: borderColors,
    borderWidth: 1,
};


var ctx = document.getElementById('highestPayingChart').getContext('2d');

// Create the horizontal bar chart
var chart = new Chart(ctx, {
    type: 'horizontalBar',
    data: {
        labels: customers,//[customers[maxIndex]],
        datasets: [dataset]
    },
    options: {
        indexAxis: 'y',
        scales: {
            x: {
                beginAtZero: true
            }
        },
        plugins: {
            legend: {
                display: false
            }
        }
    }
});



var balance_chart = document.getElementById("balance-chart").getContext('2d');

var balance_chart_bg_color = balance_chart.createLinearGradient(0, 0, 0, 70);
balance_chart_bg_color.addColorStop(0, 'rgba(63,82,227,.2)');
balance_chart_bg_color.addColorStop(1, 'rgba(63,82,227,0)');

var myChart = new Chart(balance_chart, {
    type: 'line',
    data: {
        labels: ['16-07-2018', '17-07-2018', '18-07-2018', '19-07-2018', '20-07-2018', '21-07-2018', '22-07-2018', '23-07-2018', '24-07-2018', '25-07-2018', '26-07-2018', '27-07-2018', '28-07-2018', '29-07-2018', '30-07-2018', '31-07-2018'],
        datasets: [{
            label: 'Balance',
            data: [50, 61, 80, 50, 72, 52, 60, 41, 30, 45, 70, 40, 93, 63, 50, 62],
            backgroundColor: balance_chart_bg_color,
            borderWidth: 3,
            borderColor: 'rgba(63,82,227,1)',
            pointBorderWidth: 0,
            pointBorderColor: 'transparent',
            pointRadius: 3,
            pointBackgroundColor: 'transparent',
            pointHoverBackgroundColor: 'rgba(63,82,227,1)',
        }]
    },
    options: {
        layout: {
            padding: {
                bottom: -1,
                left: -1
            }
        },
        legend: {
            display: false
        },
        scales: {
            yAxes: [{
                gridLines: {
                    display: false,
                    drawBorder: false,
                },
                ticks: {
                    beginAtZero: true,
                    display: false
                }
            }],
            xAxes: [{
                gridLines: {
                    drawBorder: false,
                    display: false,
                },
                ticks: {
                    display: false
                }
            }]
        },
    }
});

var balance_chart = document.getElementById("balance-chart2").getContext('2d');

var balance_chart_bg_color = balance_chart.createLinearGradient(0, 0, 0, 70);
balance_chart_bg_color.addColorStop(0, 'rgba(63,82,227,.2)');
balance_chart_bg_color.addColorStop(1, 'rgba(63,82,227,0)');

var myChart = new Chart(balance_chart, {
    type: 'line',
    data: {
        labels: ['16-07-2018', '17-07-2018', '18-07-2018', '19-07-2018', '20-07-2018', '21-07-2018', '22-07-2018', '23-07-2018', '24-07-2018', '25-07-2018', '26-07-2018', '27-07-2018', '28-07-2018', '29-07-2018', '30-07-2018', '31-07-2018'],
        datasets: [{
            label: 'Balance',
            data: [50, 61, 80, 50, 72, 52, 60, 41, 30, 45, 70, 40, 93, 63, 50, 62],
            backgroundColor: balance_chart_bg_color,
            borderWidth: 3,
            borderColor: 'rgba(63,82,227,1)',
            pointBorderWidth: 0,
            pointBorderColor: 'transparent',
            pointRadius: 3,
            pointBackgroundColor: 'transparent',
            pointHoverBackgroundColor: 'rgba(63,82,227,1)',
        }]
    },
    options: {
        layout: {
            padding: {
                bottom: -1,
                left: -1
            }
        },
        legend: {
            display: false
        },
        scales: {
            yAxes: [{
                gridLines: {
                    display: false,
                    drawBorder: false,
                },
                ticks: {
                    beginAtZero: true,
                    display: false
                }
            }],
            xAxes: [{
                gridLines: {
                    drawBorder: false,
                    display: false,
                },
                ticks: {
                    display: false
                }
            }]
        },
    }
});

var sales_chart = document.getElementById("sales-chart").getContext('2d');

var sales_chart_bg_color = sales_chart.createLinearGradient(0, 0, 0, 80);
balance_chart_bg_color.addColorStop(0, 'rgba(63,82,227,.2)');
balance_chart_bg_color.addColorStop(1, 'rgba(63,82,227,0)');

var myChart = new Chart(sales_chart, {
    type: 'line',
    data: {
        labels: ['16-07-2018', '17-07-2018', '18-07-2018', '19-07-2018', '20-07-2018', '21-07-2018', '22-07-2018', '23-07-2018', '24-07-2018', '25-07-2018', '26-07-2018', '27-07-2018', '28-07-2018', '29-07-2018', '30-07-2018', '31-07-2018'],
        datasets: [{
            label: 'Sales',
            data: [70, 62, 44, 40, 21, 63, 82, 52, 50, 31, 70, 50, 91, 63, 51, 60],
            borderWidth: 2,
            backgroundColor: balance_chart_bg_color,
            borderWidth: 3,
            borderColor: 'rgba(63,82,227,1)',
            pointBorderWidth: 0,
            pointBorderColor: 'transparent',
            pointRadius: 3,
            pointBackgroundColor: 'transparent',
            pointHoverBackgroundColor: 'rgba(63,82,227,1)',
        }]
    },
    options: {
        layout: {
            padding: {
                bottom: -1,
                left: -1
            }
        },
        legend: {
            display: false
        },
        scales: {
            yAxes: [{
                gridLines: {
                    display: false,
                    drawBorder: false,
                },
                ticks: {
                    beginAtZero: true,
                    display: false
                }
            }],
            xAxes: [{
                gridLines: {
                    drawBorder: false,
                    display: false,
                },
                ticks: {
                    display: false
                }
            }]
        },
    }
});

var sales_chart = document.getElementById("sales-chart2").getContext('2d');

var sales_chart_bg_color = sales_chart.createLinearGradient(0, 0, 0, 80);
balance_chart_bg_color.addColorStop(0, 'rgba(63,82,227,.2)');
balance_chart_bg_color.addColorStop(1, 'rgba(63,82,227,0)');

var myChart = new Chart(sales_chart, {
    type: 'line',
    data: {
        labels: ['16-07-2018', '17-07-2018', '18-07-2018', '19-07-2018', '20-07-2018', '21-07-2018', '22-07-2018', '23-07-2018', '24-07-2018', '25-07-2018', '26-07-2018', '27-07-2018', '28-07-2018', '29-07-2018', '30-07-2018', '31-07-2018'],
        datasets: [{
            label: 'Sales',
            data: [70, 62, 44, 40, 21, 63, 82, 52, 50, 31, 70, 50, 91, 63, 51, 60],
            borderWidth: 2,
            backgroundColor: balance_chart_bg_color,
            borderWidth: 3,
            borderColor: 'rgba(63,82,227,1)',
            pointBorderWidth: 0,
            pointBorderColor: 'transparent',
            pointRadius: 3,
            pointBackgroundColor: 'transparent',
            pointHoverBackgroundColor: 'rgba(63,82,227,1)',
        }]
    },
    options: {
        layout: {
            padding: {
                bottom: -1,
                left: -1
            }
        },
        legend: {
            display: false
        },
        scales: {
            yAxes: [{
                gridLines: {
                    display: false,
                    drawBorder: false,
                },
                ticks: {
                    beginAtZero: true,
                    display: false
                }
            }],
            xAxes: [{
                gridLines: {
                    drawBorder: false,
                    display: false,
                },
                ticks: {
                    display: false
                }
            }]
        },
    }
});


$("#products-carousel").owlCarousel({
    items: 3,
    margin: 10,
    autoplay: true,
    autoplayTimeout: 5000,
    loop: true,
    responsive: {
        0: {
            items: 2
        },
        768: {
            items: 2
        },
        1200: {
            items: 3
        }
    }
});




