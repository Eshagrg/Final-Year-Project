document.addEventListener('DOMContentLoaded', function () {
    var nepaliDateInputs = document.querySelectorAll('input.nepali-date');
    // Function to apply Nepali date picker to each input field
    function applyNepaliDatePicker() {
        nepaliDateInputs.forEach(function (input) {
            input.nepaliDatePicker({
                ndpYear: true,
                ndpMonth: true,
                ndpYearCount: 158,
                dateFormat: "YYYY/MM/DD"
            });
        });
    }

    // Call the function to apply Nepali date picker
    applyNepaliDatePicker();
});

