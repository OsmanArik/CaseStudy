function SearchTicket() {

    var resultSearchTicketCheck = SearchTicketCheck();

    if (resultSearchTicketCheck.status) {
        var fromCity = $('#departureDropdown').val();
        var toCity = $('#arrivalDropdown').val();
        var time = $('#departureDate').val();
        var returnTime = $('#returnDate').val();

        $.ajax({
            type: 'POST',
            url: '/Home/GetSearch',
            data: {
                origin: fromCity,
                destination: toCity,
                departureDate: time,
                arrivalDate: returnTime
            },
            success: function (data) {
                SetAirSearch(data);
            }
        });
    }
    else {
        toastr.error(resultSearchTicketCheck.message);
        document.getElementById(resultSearchTicketCheck.focus).focus();
    }
   
}

function SearchTicketCheck() {
    var fromCity = $('#departureDropdown').val();
    var toCity = $('#arrivalDropdown').val();
    var time = $('#departureDate').val();
    var returnTime = $('#returnDate').val();

    if (IsNullOrEmpty(fromCity) || fromCity === "0")
        return {
            status: false,
            message: "Lütfen gidiþ havaalanýný seçiniz",
            focus: "departureDropdown"
        };

    if (IsNullOrEmpty(toCity) || toCity === "0")
        return {
            status: false,
            message: "Lütfen dönüþ havaalanýný seçiniz",
            focus: "arrivalDropdown"
        };

    if (IsNullOrEmpty(time))
        return {
            status: false,
            message: "Lütfen gidiþ tarihini seçiniz",
            focus: "departureDate"
        };

    var isRoundTrip = document.getElementById('searchType').checked;

    if (isRoundTrip && IsNullOrEmpty(returnTime))
        return {
            status: false,
            message: "Lütfen dönüþ tarihini seçiniz",
            focus: "returnDate"
        };

    return {
        status: true,
        message: ""
    };
}

function SetAirSearch(data) {
    $('#seferList').html(data.data);
}

function IsNullOrEmpty(value) {
    return value === null || value === undefined || value === '';
}

$(function () {
    SetDatetime();
    SetDepartureDropdown();
    SetArrivalDropdown();
    CheckOnewayOrRoundTrip();

    $('#searchType').change(function () {
        if (this.checked) {
            $('label[for="searchType"]').text('Gidiþ-Dönüþ');
            $('#returnDateDiv').show();
        } else {
            $('label[for="searchType"]').text('Tek Yön');
            $('#returnDateDiv').hide();
        }
    });

});

function SetDatetime() {
    jQuery('#departureDate').datetimepicker({
        format: 'Y/m/d',
        timepicker: false,
        minDate: 0
    });
    jQuery('#returnDate').datetimepicker({
        format: 'Y/m/d',
        timepicker: false,
        minDate: 0
    });
}

function SetDepartureDropdown() {
    $.ajax({
        type: 'GET',
        url: '/Home/GetAirports',
        success: function (data) {
            $('#departureDropdown').select2({
                data: data
            });
        }
    })
}

function SetArrivalDropdown() {
    $.ajax({
        type: 'GET',
        url: '/Home/GetAirports',
        success: function (data) {
            $('#arrivalDropdown').select2({
                data: data
            });
        }
    })
}

$('#departureDropdown').on('select2:select', function (e) {
    var data = e.params.data.id;
    SetDepartureDropdown();
    var nereyeDropdown = $("#arrivalDropdown");
    nereyeDropdown.find('option[value=' + data + ']').remove();
});

$('#arrivalDropdown').on('select2:select', function (e) {
    var data = e.params.data.id;
    SetArrivalDropdown();
    var neredenDropdown = $("#departureDropdown");
    neredenDropdown.find('option[value=' + data + ']').remove();
});

function CheckOnewayOrRoundTrip() {
    var isRoundTrip = document.getElementById('searchType').checked;

    if (!isRoundTrip)
        $('#returnDateDiv').hide();
}