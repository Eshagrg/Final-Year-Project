







//const popupWindow = document.querySelector('.popup-window');
//const billTitle = document.querySelector('#bill-title');
//const billItems = document.querySelector('#bill-items');
//const billTotalAmount = document.querySelector('#bill-total-amount');
//const popupCloseButton = document.querySelector('#popup-close');

//function showPopup(title, items) {
//    // Clear previous items
//    billItems.innerHTML = '';

//    // Set the bill title
//    billTitle.textContent = title;

//    // Add items to the bill table
//    let totalAmount = 0;
//    items.forEach((item) => {
//        const row = document.createElement('tr');
//        const nameCell = document.createElement('td');
//        const quantityCell = document.createElement('td');
//        const rateCell = document.createElement('td');
//        const amountCell = document.createElement('td');

//        nameCell.textContent = item.name;
//        quantityCell.textContent = item.quantity;
//        rateCell.textContent = item.rate;
//        amountCell.textContent = item.amount;

//        row.appendChild(nameCell);
//        row.appendChild(quantityCell);
//        row.appendChild(rateCell);
//        row.appendChild(amountCell);

//        billItems.appendChild(row);

//        // Calculate the total amount
//        totalAmount += item.amount;
//    });

//    // Set the total amount
//    billTotalAmount.textContent = totalAmount.toFixed(2);

//    popupWindow.style.display = 'block';
//}


//$(('#popup-close').on('click', () => {
//    popupWindow.style.display = 'none';
//});

//const images = document.querySelectorAll('.popup-trigger');
//images.forEach((image) => {
//    const title = image.getAttribute('data-title');
//    const items = JSON.parse(image.getAttribute('data-items'));

//    image.addEventListener('click', () => {
//        showPopup(title, items);
//    });
//});
