var apiUrl = 'https://localhost:7068/api/subject';
var subjectName = '', active = null, pageIndex = null, isCurrentSubject = null,
    pageSize = 2, totalElement = 0, sortBy = 'Id', orderBy = 'ASC';
var up = '&#8593;', down = '&#8595;';



$(document).ready(function () {

    $('#sortableTable th').click(function () {
        var columnName = $(this).attr('id');
        console.log(columnName)
        $('.up-down-arrow').html(up + ' ' + down);
        $('.up-down-arrow').removeClass('text-danger');
        if (columnName !== undefined) {
            if (columnName !== sortBy) {
                orderBy = 'ASC';
                sortBy = columnName;
            } else {
                orderBy = orderBy === 'ASC' ? 'DESC' : 'ASC';
            }
            var selectedField = $(this).find('span');
            selectedField.html(orderBy === 'ASC' ? up : down);
            selectedField.addClass('text-danger');

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