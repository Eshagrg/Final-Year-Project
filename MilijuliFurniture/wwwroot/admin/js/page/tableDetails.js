$(document).ready(function () {
      $('.popup-trigger').on('click', function () {
    var itemId = $(this).data('id');
    var itemData = @Html.Raw(Json.Serialize(Model.DataList));
    console.log(itemId);
    console.log(itemData);
    var selectedItem = itemData.find(function (item) {
        return item.tableid === itemId;
    });

    var popupContent = '<div class="bill-container">' +
        '<div class="bill-header">' +
        '<img class="logo" src="../img/danfe.png" alt="Restaurant Logo">' +
        '<div class="restaurant-details">' +
        '<h2 class="restaurant-name">Restaurant Name</h2>' +
        '<p class="restaurant-address">Address: Restaurant Address</p>' +
        '<p class="restaurant-contact">Contact: Restaurant Contact</p>' +
        '</div>' +
        '</div>' +
        '<hr />' +
        '<table class="bill-table">' +
        '<thead>' +
        '<tr>' +
        '<th class="item-header">Item</th>' +
        '<th class="quantity-header">Quantity</th>' +
        '<th class="rate-header">Rate</th>' +
        '<th class="amount-header">Amount</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '<tr>' +
        '<td>' + selectedItem.name + '</td>' +
        '<td>' + selectedItem.qty + '</td>' +
        '<td>' + selectedItem.rate + '</td>' +
        '<td>' + selectedItem.amount + '</td>' +
        '</tr>' +
        '</tbody>' +
        '</table>' +
        '<hr />' +
        '<div class="total-amount">' +
        '<h4>Total Amount: <span>' + selectedItem.amount + '</span></h4>' +
        '</div>' +
        '</div>';

    $('#popup .modal-body').html(popupContent);
    $('#popup').modal('show');
});

        $('.popup-trigger1').on('click', function () {
        var itemId = $(this).data('id');
        var itemData = @Html.Raw(Json.Serialize(Model.SubcategoryList));
        console.log(itemId)
        console.log(itemData)
        var selectedItem = itemData.find(function (subcategory) {
            return subcategory.subcategoryId === itemId;
        });

        var popupContent = '<p>Subcategory ID: ' + selectedItem.subcategoryId + '</p>' +
            '<p>Name: ' + selectedItem.name + '</p>' +
            '<p>Rate: ' + selectedItem.rate + '</p>';

        $('#popup .modal-body').html(popupContent);
        $('#popup').modal('show');
     });


    })
