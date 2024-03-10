var apiUrl = 'https://localhost:7068/api/user';
var keyword = '', role = null, pageIndex = null, gender = null,
    pageSize = 2, totalElement = 0, sortBy = 'Id', orderBy = 'ASC';
var up = '&#8593;', down = '&#8595;';
var userId = '';



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

            SearchUsers();
        }
    });

    $('#Keyword').on('change', function () {
        keyword = $(this).val();
        SearchUsers();
    })

    $('#Role').on('change', function () {
        role = $(this).val() == 'All' ? null : $(this).val();
        SearchUsers();
    });

    $('#Gender').on('change', function () {
        gender = $(this).val() == 'All' ? null : $(this).val();
        SearchUsers();
    });

    $('#PageSize').on('change', function () {
        pageSize = $(this).val();
        pageIndex = 0;
        SearchUsers();
    });

    $('#PageIndex').on('change', function () {
        var page = $(this).val();
        pageIndex = page;
        SearchUsers();
    });

});

function SearchUsers() {

    var request = {
        Keyword: keyword,
        Gender: gender,
        Role: role,
        OrderBy: orderBy,
        SortBy: sortBy,
        PageIndex: pageIndex,
        PageSize: pageSize
    };

    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: apiUrl + '/search-users',
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

async function showAlert(uId) {
    userId = uId;
    const { value: role } = await Swal.fire({
        title: "Select new role",
        input: "select",
        inputOptions: {
            ADMIN: 'admin',
            TEACHER: 'teacher',
            STUDENT: 'student'
        },
        inputPlaceholder: "Select a role",
        showCancelButton: true,
        inputValidator: (value) => {
            return new Promise((resolve) => {
                if (value) {
                    resolve();
                } else {
                    resolve("You need to select a role!");
                }
            });
        }
    });
    if (role) {
        UpdateUserRole(role);
    }
}

function UpdateUserRole(role) {
    var token = getCookie('Token');

    if (token) {

        var req = {
            Id: userId,
            Role: role
        }

        $.ajax({
            url: 'https://localhost:7068/api/auth/update-user-role',
            type: 'PUT',
            data: JSON.stringify(req),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data.data);
                if (data.statusCode == 200) {
                    $('#row-' + userId).find('.user-role').text(data.data.role);
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
        var row = '<tr id="' + ('row-' + item.id) + '">';
        row += '<th  scope="row">' + item.id.substring(0, 8) + '</th>';
        row += '<td>' + item.fullName + '</td>';
        row += '<td>' + item.email + '</td>';
        row += '<td>' + (item.gender ? "Male" : "Female") + '</td>';
        row += '<td>' + item.active + '</td>';
        row += '<td class="user-role">' + item.roles[0] + '</td>';
        row += '<td><a href="/users/update/' + item.id + '" class="btn btn-warning m-2">Edit</a>';
        row += '<a onclick="showAlert(\'' + item.id + '\')" class="btn btn-warning m-2">Change Role</a></td>';
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