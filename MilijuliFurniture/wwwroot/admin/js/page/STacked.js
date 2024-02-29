function generateChart() {
    // Chart data

    
        var data = {
            labels: ["January", "February", "March", "April", "May"],
            datasets: [
                {
                    label: "2021",
                    data: [65, 59, 80, 81, 56],
                    backgroundColor: "#36A2EB",
                },
                {
                    label: "2022",
                    data: [28, 48, 40, 19, 86],
                    backgroundColor: "#FFCE56",
                },
                {
                    label: "2023",
                    data: [45, 25, 12, 42, 33],
                    backgroundColor: "#FF6384",
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
                    fontSize: 12,
                },
            },
            scales: {
                x: {
                    stacked: true,
                },
                y: {
                    stacked: true,
                },
            },
            tooltips: {
                enabled: true,
            },
        };

        // Get the canvas element
        var ctx = document.getElementById("stacked").getContext("2d");

        // Create the chart
        new Chart(ctx, {
            type: "bar",
            data: data,
            options: options,
        });
    });

}

// Call the function to generate the chart
generateChart();

