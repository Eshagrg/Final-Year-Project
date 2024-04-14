const SEARCH_VIEW = {

    searchDate: () => {

        $("#txtStartDate").val("");
        $("#txtEndDate").val("");
        $("#txtSaleNumber").val("");
        $("#txtSaleNumberSearch").val($("#txtSaleNumberSearch option:first").val());
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
let tableData;

$(document).ready(function () {
    
    SEARCH_VIEW["searchDate"]();

    $("#txtStartDate").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#txtEndDate").datepicker({ dateFormat: 'dd/mm/yy' });

    tableData = $('#tbsale').DataTable({
        "processing": true,
        "columns": [
            { "data": "saleNumber" },
            { "data": "clientName" },
            { "data": "total" },
            { "data": "registrationDate" }
        ],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Export Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Sales Report',
            }, 'pageLength'
        ]
    });

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
                    // Clear existing DataTables data
                    tableData.clear().draw();

                    // Add new data to DataTables
                    tableData.rows.add(responseJson).draw();

                });
            }
        })
})

