const SEARCH_VIEW = {

    searchDate: () => {

        $("#txtStartDate").val("");
        $("#txtEndDate").val("");
        $("#txtSaleNumber").val("");

        $(".search-date").show()
        $(".search-sale").hide()
    },
    searchSale: () => {

        $("#txtStartDate").val("");
        $("#txtEndDate").val("");
        $("#txtSaleNumber").val("");

        $(".search-sale").show()
        $(".search-date").hide()
    }

}

$(document).ready(function () {
    SEARCH_VIEW["searchDate"]();

    $("#txtStartDate").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#txtEndDate").datepicker({ dateFormat: 'dd/mm/yy' });
})

$("#cboSearchBy").change(function () {

    if ($("#cboSearchBy").val() == "date") {

        SEARCH_VIEW["searchDate"]();
    } else {
        SEARCH_VIEW["searchSale"]();
    }
});

$("#btnSearch").click(function () {

    if ($("#cboSearchBy").val() == "date") {

        if ($("#txtStartDate").val().trim() == "" || $("#txtEndDate").val().trim() == "") {
            toastr.warning("", "You must enter start and end date");
            return;
        }
    } else {
        if ($("#txtSaleNumberSearch").val().trim() == "") {
            toastr.warning("", "You must enter the sales number");
            return;
        }
    }

    let saleNumber = $("#txtSaleNumberSearch").val();
    let startDate = $("#txtStartDate").val();
    let endDate = $("#txtEndDate").val();

    $(".card-body").find("div.row").LoadingOverlay("show")

    fetch(`/Report/SaleTypeHistoryData?saleNumber=${saleNumber}&startDate=${startDate}&endDate=${endDate}`)
        .then(response => {
            $(".card-body").find("div.row").LoadingOverlay("hide")
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            $("#tbsale tbody").html("");
            if (responseJson.length > 0) {

                responseJson.forEach((sale) => {
                    $("#tbsale tbody").append(
                        $("<tr>").append(
                            
                            $("<td>").text(sale.saleNumber),
                            $("<td>").text(sale.clientName),
                            $("<td>").text(sale.total)
                            $("<td>").text(sale.registrationDate),
                      
                        )
                    )

                });
            }
        })
})
