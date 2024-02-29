function generateChart() {
    // Chart data
    var data = {
        labels: ["Food", "Beverages"],
        datasets: [
            {
                label: "Type A",
                data: [50, 30],
                backgroundColor: "#36A2EB",
            },
            {
                label: "Type B",
                data: [40, 20],
                backgroundColor: "#FFCE56",
            },
            {
                label: "Type C",
                data: [30, 25],
                backgroundColor: "#FF6384",
            },
        ],
    };

    // Chart options
    var options = {
        responsive: true,
        maintainAspectRatio: true,
        legend: {
            display: true,
            position: "bottom",
            labels: {
                fontColor: "#333",
                fontSize: 12,
            },
        },
        scales: {
            x: {
                stacked: true,
            },
            y: {
                stacked: true,
                beginAtZero: true,
            },
        },
        tooltips: {
            enabled: true,
        },
    };

    // Get the canvas element
    var ctx = document.getElementById("bars").getContext("2d");

    // Create the chart
    new Chart(ctx, {
        type: "bar",
        data: data,
        options: options,
    });
}

// Call the function to generate the chart
generateChart();
