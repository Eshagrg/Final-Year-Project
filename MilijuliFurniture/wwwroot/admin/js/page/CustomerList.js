// Customer's data
var customers = [
    {
        name: "Shivan Jung Kunwar",
        image: "../img/user1.jpg",
        description: "",
        sales: 75,
        visitedDays: 5
    },
    {
        name: "Selena gomez",
        image: "../img/user2.jpg",
        description: "",
        sales: 60,
        visitedDays: 3
    },
    {
        name: "Shivan Rich",
        image: "../img/user3.jpg",
        description: "",
        sales: 90,
        visitedDays: 3
    },
   
];


var totalSales = 0;
var totalVisitedDays = 0;

customers.forEach(function (customer) {
    totalSales += customer.sales;
    totalVisitedDays += customer.visitedDays;
});

var averageSales = totalSales / customers.length;
var averageVisitedDays = totalVisitedDays / customers.length;


customers.sort(function (a, b) {
    var aAverage = (a.sales + a.visitedDays) / 2;
    var bAverage = (b.sales + b.visitedDays) / 2;
    return bAverage - aAverage;
});

var customerList = document.getElementById("customer-list");
customers.forEach(function (customer) {
    var progressValue = Math.round((customer.sales / 100) * 100); 

    var customerHTML = `
        <div class="customer-card">
            <div class="customer-info">
                <img src="${customer.image}" alt="${customer.name}" class="customer-image">
                <h5 class="customer-name">${customer.name}</h5>
            </div>
            <p class="customer-description">${customer.description}</p>
            <div class="progress" data-height="4" data-toggle="tooltip" title="" data-original-title="${progressValue}% - ${customer.visitedDays} days" style="height: 4px;">
                <div class="progress-bar bg-success" role="progressbar" style="width: ${progressValue}%;"
                    aria-valuenow="${progressValue}" aria-valuemin="0" aria-valuemax="100"></div>
            </div>
        </div>
    `;
    customerList.innerHTML += customerHTML;
});


var staff = [
    { name: "Shivan Jung Kunwar ", position: "Manager", rating: 4, image: "../img/user1.jpg" },
    { name: "Michal Smith", position: "Sales Representative", rating: 5, image: "../img/user2.jpg" },
    { name: "Selena Gomez", position: "Accountant", rating: 4.5, image: "../img/user3.jpg" },
    { name: "Shivan Jung Kunwar", position: "Accountant", rating: 4.5, image: "../img/user4.jpg" },
    { name: "Mark Johnson", position: "Accountant", rating: 4.5, image: "../img/user5.jpg" },
    
];


function generateStaffList() {
    var staffList = $("#staff-list");
    staffList.empty(); 

    staff.forEach(function (staffMember) {
        var starsHTML = getStarsHTML(staffMember.rating);

        var staffHTML = `
      <div class="staff-item">
        <div class="staff-info">
          <img src="../img/${staffMember.image}" alt="${staffMember.name}" class="staff-image">
           <div class="staff-details">
            <div class="staff-rating">${starsHTML}</div>
          </div>
        </div>
        <div class=staff=position>
        <span class="staff-position">${staffMember.name}</span>
        </div>
         
      </div>
    `;

        staffList.append(staffHTML);
    });
}


function getStarsHTML(rating) {
    var stars = "";
    var fullStars = Math.floor(rating);
    var halfStar = rating % 1 >= 0.5;

    for (var i = 0; i < fullStars; i++) {
        stars += '<i class="fas fa-star"></i> ';
    }

    if (halfStar) {
        stars += '<i class="fas fa-star-half-alt"></i> ';
    }

    return stars;
}


generateStaffList();



 //chart
    var thisMonthSales = [1000, 1500, 2000, 100, 3000];
    var lastMonthSales = [800, 1200, 1800, 2200, 2700];

    
    var ctx = document.getElementById('salesChart').getContext('2d');

    var chart = new Chart(ctx, {
        type: 'line',
        data: {
        labels: ['Week 1', 'Week 2', 'Week 3', 'Week 4', 'Week 5'],
            datasets: [
                {
        label: 'This Month',
                    data: thisMonthSales,
                    backgroundColor: 'rgba(0, 123, 255, 0.5)', 
                    borderColor: 'rgba(0, 123, 255, 1)', 
                    borderWidth: 2 
                },
                {
        label: 'Last Month',
                    data: lastMonthSales,
                    backgroundColor: 'rgba(255, 99, 132, 0.5)',
                    borderColor: 'rgba(255, 99, 132, 1)',
                    borderWidth: 2
                }
            ]
        },
        options: {
        scales: {
        y: {
        beginAtZero: true 
                }
            }
        }
    });







