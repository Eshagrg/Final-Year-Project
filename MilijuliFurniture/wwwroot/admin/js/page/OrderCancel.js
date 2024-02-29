// Define a function to fetch the data from the controller
function fetchCancelledOrders() {
    fetch('/Transactional/OrderCancelReport')
        .then(response => response.json())
        .then(data => {
            // Process the data and update the HTML
            var cancelledOrders = data.cancelledOrders;
            var totalCancelledItems = data.totalCancelledItems;

            var totalSales = 0;
            var totalCancelled = 0;

            cancelledOrders.forEach(function (item) {
                totalSales += item.sales;
                totalCancelled += item.cancelled;
            });

            var averageSales = totalSales / cancelledOrders.length;
            var averageCancelled = totalCancelled / cancelledOrders.length;

            cancelledOrders.sort(function (a, b) {
                var aAverage = (a.sales + a.cancelled) / 2;
                var bAverage = (b.sales + b.cancelled) / 2;
                return bAverage - aAverage;
            });

            var itemList = document.getElementById("customer-list");
            itemList.innerHTML = ""; // Clear the existing content

            cancelledOrders.forEach(function (item) {
                var progressValue = Math.round((item.sales / 100) * 100);

                var itemHTML = `
          <div class="customer-card">
              <div class="customer-info">
                  <img src="${item.name}" alt="${item.name}" class="customer-image">
                  <h5 class="customer-name">${item.name}</h5>
              </div>
              <p class="customer-description">${item.description}</p>
              <div class="progress" data-height="4" data-toggle="tooltip" title="" data-original-title="${progressValue}% - ${item.cancelled} days" style="height: 4px;">
                  <div class="progress-bar bg-success" role="progressbar" style="width: ${progressValue}%;"
                      aria-valuenow="${progressValue}" aria-valuemin="0" aria-valuemax="100"></div>
              </div>
          </div>
        `;
                itemList.innerHTML += itemHTML;
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

// Call the fetchCancelledOrders function to fetch the data
fetchCancelledOrders();
