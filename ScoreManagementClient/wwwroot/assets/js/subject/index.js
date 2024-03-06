var apiUrl = 'https://localhost:7068/api/subject';
var subjectName = '', active = null, pageIndex = null, isCurrentSubject = null,
    pageSize = 2, totalElement = 0, sortBy = 'Id', orderBy = 'ASC';



$(document).ready(function () {

    $('#sortableTable th').click(function () {
        var columnName = $(this).attr('id');
        console.log(columnName)
        if (columnName !== undefined) {
            if (columnName !== sortBy) {
                orderBy = 'ASC';
                sortBy = columnName;
            } else {
                orderBy = orderBy === 'ASC' ? 'DESC' : 'ASC';
            }

            SearchSubject();
        }
    });

    $('#SubjectName').on('change', function () {
        subjectName = $(this).val();
        SearchSubject();
    })

    $('#IsCurrentSubject').on('change', function () {
        isCurrentSubject = $(this).val() == 1;
        SearchSubject();
    });

    $('#Active').on('change', function () {
        var status = $(this).val();
        if (status == -1)
            active = null;
        else
            active = status == 1;
        SearchSubject();
    });

    $('#PageSize').on('change', function () {
        pageSize = $(this).val();
        pageIndex = 0;
        SearchSubject();
    });

    $('#PageIndex').on('change', function () {
        var page = $(this).val();
        pageIndex = page;
        SearchSubject();
    });
});

function SearchSubject() {

    var request = {
        Name: subjectName,
        Active: active,
        IsCurrentSubject: isCurrentSubject,
        OrderBy: orderBy,
        SortBy: sortBy,
        PageIndex: pageIndex,
        PageSize: pageSize
    };

    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: apiUrl + '/search-subjects',
            type: 'POST',
            data: JSON.stringify(request),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data.data);
                if (data.statusCode == 200) {
                    displayData(data.data.result);
                    pageIndex = data.data.pageIndex;
                    pageSize = data.data.pageSize;
                    totalElement = data.data.totalElements;
                    LoadPageIndex();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('Error:', errorThrown);
            }
        });
    } else {
        console.error('Token not found in cookie');
    }
}

function displayData(data) {
    $('#data').empty();

    data.forEach(function (item) {
        var row = '<tr>';
        row += '<th  scope="row">' + item.id + '</th>';
        row += '<td>' + item.name + '</td>';
        row += '<td>' + item.description + '</td>';
        row += '<td>' + item.active + '</td>';
        row += '<td>' + item.createdAt + '</td>';
        row += '<td>' + item.creator.fullName + '</td>';
        row += '<td><a href="/subjects/update/' + item.id + '" class="btn btn-warning m-2">Edit</a></td>';
        row += '</tr>'; 
        $('#data').append(row);
    });
}

function LoadPageIndex() {

    $('#PageIndex').empty();

    console.log(totalElement, pageSize)
    var numberOfPage = totalElement % pageSize == 0
        ? totalElement / pageSize : 1 + totalElement / pageSize;
    numberOfPage = Math.floor(numberOfPage);

    for (var i = 0; i < numberOfPage; i++) {
        if (i === pageIndex)
            $('#PageIndex').append('<option value="' + (i) + '" selected="selected"> ' + (i + 1) + ' </option>');
        else
            $('#PageIndex').append('<option value="' + (i) + '" > ' + (i + 1) + ' </option>');
    }
}

function loadPagination() {
    $('#pagination').empty();
    var numberOfPage = totalElement % pageSize == 0
        ? totalElement / pageSize : 1 + totalElement / pageSize;
    numberOfPage = Math.floor(numberOfPage);

    var $btnPrevious = $('<input type="radio" class="btn-check" name="btnradio" value="-1" id="btnradio1" autocomplete="off" ' + (pageIndex == 0 ? "disabled" : "") + '>');
    var $labelPrevious = $('<label class="btn btn-outline-primary" for="btnradio1">&laquo;</label>');

    $("#pagination").append($btnPrevious).append($labelPrevious);

    if (numberOfPage <= 5) {
        for (var i = 1; i <= numberOfPage; i++) {
            var $btnPage = $('<input type="radio" class="btn-check" name="btnradio" id="btnradio' + (i + 1) + '" value="' + i + '" autocomplete="off">');
            var $labelPage = $('<label class="btn btn-outline-primary" for="btnradio' + (i + 1) + '">' + i + '</label>');

            $("#pagination").append($btnPage).append($labelPage);
        }
    } else {
        var $btnFirstPage = $('<input type="radio" class="btn-check" name="btnradio" value="' + (pageIndex + 1)
                + '" id="btnradio2" autocomplete="off" ' + (pageIndex == 0 ? "checked" : "") + '>');
        var $labelFirstPage = $('<label class="btn btn-outline-primary" for="btnradio2">' + (pageIndex + 1) +'</label>');
        $("#pagination").append($btnFirstPage).append($labelFirstPage);

        var $btnSecondPage = $('<input type="radio" class="btn-check" name="btnradio" value="' + (pageIndex + 2) + '" id="btnradio3" autocomplete="off" >');
        var $labelSecondPage = $('<label class="btn btn-outline-primary" for="btnradio3">' + (pageIndex + 2) + '</label>');
        $("#pagination").append($btnSecondPage).append($labelSecondPage);

        var $btnDots = $('<input type="radio" class="btn-check" name="btnradio" disabled id="btnradio4" autocomplete="off">');
        var $labelDots = $('<label class="btn btn-outline-primary" for="btnradio4">...</label>');
        $("#pagination").append($btnDots).append($labelDots);

        for (var i = numberOfPage - 1; i <= numberOfPage; i++) {
            var $btnPage = $('<input type="radio" class="btn-check" name="btnradio" id="btnradio' + (i + 1) + '" value="' + i + '" autocomplete="off">');
            var $labelPage = $('<label class="btn btn-outline-primary" for="btnradio' + (i + 1) + '">' + i + '</label>');

            $("#pagination").append($btnPage).append($labelPage);
        }
    }

    var $btnNext = $('<input type="radio" class="btn-check" value="-2" name="btnradio" id="btnradio' + (numberOfPage + 2) + '" autocomplete="off">');
    var $labelNext = $('<label class="btn btn-outline-primary" for="btnradio' + (numberOfPage + 2) + '">&raquo;</label>');

    $("#pagination").append($btnNext).append($labelNext);
    $('.btn-check').click(function () {
        var page = $(this).val();
        pageIndex = page - 1;
        SearchSubject();
        console.log(page);
    });
}