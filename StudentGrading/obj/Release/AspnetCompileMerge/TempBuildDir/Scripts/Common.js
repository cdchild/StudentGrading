$(document).ready(function () {
    $(".datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        dateFormat: 'mm/dd/yy'
    });
});
$(document).ready(function () {
    $(".datepicker-3month").datepicker({
        changeMonth: true,
        changeYear: true,
        numberOfMonths: 3,
        dateFormat: 'mm/dd/yy'
    });
});
$(document).ready(function () {
    $(".timepicker").timepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'mm/dd/yy'
    });
});