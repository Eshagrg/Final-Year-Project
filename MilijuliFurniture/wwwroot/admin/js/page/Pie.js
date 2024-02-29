function generateChart() {
    // Chart data
        var data = {
            labels: ["Waiter Cancel", "Self Cancel", "Food issue"],
            datasets: [
                {
                    data: [10, 20, 30],
                    backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"],
                    hoverBackgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"],
                },
            ],
        };

        // Chart options
        var options = {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                display: true,
                position: "bottom",
                labels: {
                    fontColor: "#333",
                    fontSize: 8,
                },
            },
            animation: {
                animateScale: true,
                animateRotate: true,
            },
            tooltips: {
                enabled: true,
            },
        };

        // Get the canvas element
        var ctx = document.getElementById("pie").getContext("2d");

        // Create the chart
        new Chart(ctx, {
            type: "doughnut",
            data: data,
            options: options,
        });
   
}
// Call the function to generate the chart
generateChart();
