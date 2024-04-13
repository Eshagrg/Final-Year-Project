﻿
let TaxValue = 0;
let ProductsForSale = [];

$(document).ready(function () {
    // This code is using the Fetch API to make a GET request to the "/Sales/ListTypeDocumentSale" endpoint and populate a HTML select element with the response data.
    fetch("/Sales/ListTypeDocumentSale")
    // Check if the response was successful using the "ok" property of the response object
        // If successful, return the response data in JSON format, otherwise return a rejected Promise with the response object
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            // If the length of the responseJson array is greater than 0, iterate over each item in the array and append an option element to the HTML select element with id "cboTypeDocumentSale"
           
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboTypeDocumentSale").append(
                        $("<option>").val(item.idTypeDocumentSale).text(item.description)
                    )
                });
            }
        })

    
    $("#txtTotalTaxes").on('change', function () {
        showProducts_Prices();
    });

    $("#cboSearchProduct").select2({
        ajax: {
            url: "/Sales/GetProducts",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                return {
                    search: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data.map((item) => (
                        {
                            id: item.id,
                            category: item.categoryName,
                            name: item.name,
                            brand: item.brand,
                            quantity: item.quantity,
                            photoBase64: item.uploadImage,
                            price: parseFloat(item.price)
                        }
                    ))
                };
            }
        },
        placeholder: 'Search product...',
        minimumInputLength: 1,
        templateResult: formatResults
    });


})

function formatResults(data) {

    if (data.loading)
        return data.text;

    var container = $(
        `<table width="100%">
            <tr>
                <td style="width:60px">
                    <img style="height:60px;width:60px;margin-right:10px" src="${data.photoBase64}"/>
                </td>
                <td>
                    <p style="font-weight: bolder;margin:2px">${data.name}</p>
                    <p style="margin:2px">${data.price}</p>
                </td>
            </tr>
         </table>`
    );

    return container;
}


$(document).on('select2:open', () => {
    cdocument.querySelector('.select2-search__field').focus();
});

$('#cboSearchProduct').on('select2:select', function (e) {
    var data = e.params.data;

    let product_found = ProductsForSale.filter(prod => prod.ProductId == data.id)
    if (product_found.length > 0) {
        $("#cboSearchProduct").val("").trigger('change');
        toastr.warning("", "The product has already been added");
        return false
    }

    swal({
        title:"Product:"+data.name,
        text: "Quantity:"+data.quantity,
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        inputPlaceholder: "Enter quantity"
    }, async function (value) {

        if (value === false) return false;

        if (value === "") {
            toastr.warning("", "You need to enter the amount");
            return false
        }
        if (value === "" || parseFloat(value) <= 0 || isNaN(parseFloat(value))) {
            toastr.warning("", "You need to enter a valid positive amount");
            return false;
        }


        if (isNaN(parseInt(value))) {
            toastr.warning("", "You must enter a numeric value");
            return false
        }
   
        let quantityAvailable = await checkQuantityAvailable(data.id, parseInt(value))
        if (!quantityAvailable) {
            toastr.warning("", "The entered quantity is not available");
            return false;
        }

        let product = {
            ProductId: data.id,
            nameProduct: data.name,
            descriptionProduct: data.text,
            categoryProducty: data.category,
            brandProduct: data.brand,
            quantity: parseInt(value),
            price: data.price.toString(),
            total: (parseFloat(value) * data.price).toString()
        }

        ProductsForSale.push(product)
        showProducts_Prices();

        $("#cboSearchProduct").val("").trigger('change');
        swal.close();

    });



    });
async function checkQuantityAvailable(productId, quantity) {
    try {
        let response = await fetch(`/Sales/CheckQuantity?id=${productId}&quantity=${quantity}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        let data = await response.json();
        return data.available;
    } catch (error) {
        toastr.error('Error checking quantity:', error);
        return false;
    }
}





function showProducts_Prices() {

    let total = 0;
    let tax = parseFloat($("#txtTotalTaxes").val()) || 0;
    let subtotal = 0;
    let percentage = tax / 100;

    $("#tbProduct tbody").html("")

    ProductsForSale.forEach((item) => {

        total = total + parseFloat(item.total);

        $("#tbProduct tbody").append(
            $("<tr>").append(
                $("<td>").append(
                    $("<button>").addClass("btn btn-danger btn-delete btn-sm").append(
                        $("<i>").addClass("mdi mdi-trash-can")
                    ).data("ProductId", item.ProductId)
                ),
                $("<td>").text(item.descriptionProduct),
                $("<td>").text(item.quantity),
                $("<td>").text(item.price),
                $("<td>").text(item.total)
            )
        )

    })

    subtotal = total;
    total = total-(total * (tax / 100));


    $("#txtSubTotal").val(subtotal.toFixed(2))
    $("#txtTotalTaxes").val(tax.toFixed(2))
    $("#txtTotal").val(total.toFixed(2))

}

$(document).on("click", "button.btn-delete", function () {
    const _ProductId = $(this).data("ProductId")

    ProductsForSale = ProductsForSale.filter(p => p.ProductId != _ProductId)

    showProducts_Prices()
})

$("#btnFinalizeSale").click(function () {

    if (ProductsForSale.length < 1) {
        toastr.warning("", "You must enter products");
        return;
    }

    const vmDetailSale = ProductsForSale;

    const sale = {
        
        TypeDocumentSaleId: $("#cboTypeDocumentSale").val(),
        customerDocument: $("#txtDocumentClient").val(),
        clientName: $("#txtNameClient").val(),
        subtotal: $("#txtSubTotal").val(),
        totalTaxes: $("#txtTotalTaxes").val(),
        total: $("#txtTotal").val(),
        detailSales: vmDetailSale
    }

    $("#btnFinalizeSale").closest("div.card-body").LoadingOverlay("show")

    fetch("/Sales/RegisterSale", {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(sale)
    }).then(response => {

        $("#btnFinalizeSale").closest("div.card-body").LoadingOverlay("hide")
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {

        if (responseJson.success == true) {

            ProductsForSale = [];
            showProducts_Prices();
           /* $("#txtDocumentClient").val("");*/
            $("#txtNameClient").val("");
          /*  $("#cboTypeDocumentSale").val($("#cboTypeDocumentSale option:first").val());*/

            swal("Registered!", `Sale Number : ${responseJson.saleNumber}`, "success");
           


                    
        } else {
            swal("We're sorry", "The sale could not be registered", "error");
        }
    }).catch((error) => {
        $("#btnFinalizeSale").closest("div.card-body").LoadingOverlay("hide")
    })


})