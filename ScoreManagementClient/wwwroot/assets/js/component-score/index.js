var apiUrl = 'https://localhost:7068/api/componentscore';
var componentName = '', active = null, pageIndex = null, percent = null,
    pageSize = 2, totalElement = 0, sortBy = 'Id', orderBy = 'ASC', subjectId = null;
var up = '&#8593;', down = '&#8595;';



$(document).ready(function () {

    LoadSubjects();

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

            SearchComponentScore();
        }
    });

    $('#ComponentName').on('change', function () {
        componentName = $(this).val();
        SearchComponentScore();
    })

    $('#Percent').on('change', function () {
        percent = $(this).val();
        SearchComponentScore();
    });

    $('#SubjectId').on('change', function () {
        subjectId = $(this).val();
        SearchComponentScore();
    });

    $('#Active').on('change', function () {
        var status = $(this).val();
        if (status == -1)
            active = null;
        else
            active = status == 1;
        SearchComponentScore();
    });

    $('#PageSize').on('change', function () {
        pageSize = $(this).val();
        pageIndex = 0;
        SearchComponentScore();
    });

    $('#PageIndex').on('change', function () {
        var page = $(this).val();
        pageIndex = page;
        SearchComponentScore();
    });
});

function LoadSubjects() {

    var request =
    {
        PageSize: 9999,
        Active: true
    };

    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: 'https://localhost:7068/api/subject/search-subjects',
            type: 'POST',
            data: JSON.stringify(request),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log(data)
                if (data.statusCode == 200) {
                    $('#SubjectId').empty();

                    $('#SubjectId').append('<option value="All">All</option>');

                    $.each(data.data.result, function (index, s) {
                        var option = $('<option>');
                        option.val(s.id);
                        option.text(s.name);
                        $('#SubjectId').append(option);
                    });
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

function SearchComponentScore() {

    var request = {
        Name: componentName == '' ? null : componentName,
        Active: active,
        Percent: percent == '' ? null : percent,
        SubjectId: subjectId == 'All' ? null : subjectId,
        OrderBy: orderBy,
        SortBy: sortBy,
        PageIndex: pageIndex,
        PageSize: pageSize
    };

    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: apiUrl + '/search-component-score',
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

    var role = getCookie('UserInfo');
    role = JSON.parse(role).Role;

    data.forEach(function (item) {
        var row = '<tr>';
        row += '<th  scope="row">' + item.id + '</th>';
        row += '<td>' + item.name + '</td>';
        row += '<td>' + item.percent + ' %</td>';
        row += '<td>' + item.active + '</td>';
        row += '<td>' + item.description + '</td>';
        row += '<td>' + item.subject?.name + '</td>';
        if (role == 'ADMIN')
            row += '<td><a href="/componentscores/update/' + item.id + '" class="btn btn-warning m-2">Edit</a></td>';
        row += '</tr>';
        $('#data').append(row);
    });
}

function LoadPageIndex() {

    $('#PageIndex').empty();

    console.log(pageIndex, pageSize)
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