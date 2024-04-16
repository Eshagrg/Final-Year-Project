let tableData;

$(document).ready(function () {

    $("#txtStartDate").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#txtEndDate").datepicker({ dateFormat: 'dd/mm/yy' });

    tableData = $('#tbdata').DataTable({
        "processing": true,
        "ajax": {
            "url": "/Report/ReportDeleteSale?startDate=01/01/1991&endDate=01/01/1991",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "detailSales",
                "render": function (data, type, row) {
                    // Render DetailSales as a comma-separated string of DescriptionProduct
                    return data.map(ds => ds.descriptionProduct).join(', ');
                }
            },
            {
                "data": "detailSales",
                "render": function (data, type, row) {
                    // Render prices of DetailSales
                    return data.map(ds => ds.price).join(', ');
                }
            },
            { "data": "registrationDate" },
            {
                "data": "detailSales",
                "render": function (data, type, row) {
                    // Render a single value from detailSales array
                    return data.length > 0 ? data[0].brandProduct : '';
                }
            }

        ],

        order: [[1, "desc"]],
        "autoWidth": true,
        "scrollX": true,
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



$("#btnSearch").click(function () {

    if ($("#txtStartDate").val().trim() == "" || $("#txtEndDate").val().trim() == "") {
        toastr.warning("", "You must enter start and end date");
        return;
    }

    var new_url = `/Report/ReportDeleteSale?startDate=${$("#txtStartDate").val().trim()}&endDate=${$("#txtEndDate").val().trim()}`

    tableData.ajax.url(new_url).load();
})
