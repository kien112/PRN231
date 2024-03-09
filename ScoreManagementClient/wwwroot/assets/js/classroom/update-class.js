var apiUrl = 'https://localhost:7068/api';

$(document).ready(function () {

    GetClassById();

    $('#btnUpdate').click(function () {
        UpdateClass();
    });
});


function GetClassById() {
    var id = $('#Id').val();
    var token = getCookie('Token');

    if (token) {
        $.ajax({
            url: apiUrl + '/classroom/get-class-by-id/' + id,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200)
                    LoadDetail(data.data);
                else
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: data.message,
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('Error:', errorThrown);
            }
        });
    } else {
        console.error('Token not found in cookie');
    }
}

function LoadDetail(data) {
    LoadSubjects(data.subject.id);
    LoadTeachers(data.teacher.id);
    $('#ClassName').val(data.name);
    $('#Active').val(data.active ? 'true' : 'false');
    console.log(data);
}

function UpdateClass() {
    var token = getCookie('Token');

    if (token) {

        var req = {
            Name: $('#ClassName').val(),
            TeacherId: $('#TeacherId').val() == -1 ? null : $('#TeacherId').val(),
            SubjectId: $('#SubjectId').val(),
            Active: $('#Active').val() == 'true',
            Id: $('#Id').val()
        }
        console.log('req', req)

        $.ajax({
            url: apiUrl + '/classroom/update-class',
            type: 'PUT',
            data: JSON.stringify(req),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200)
                    window.location.href = '/classrooms';
                else if (data.errors)
                    displayErrors(data.errors);
                else if (data.statusCode === 400)
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: data.message,
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('Error:', errorThrown);
            }
        });
    } else {
        console.error('Token not found in cookie');
    }
}

function displayErrors(errors) {
    $('#NameError').text('');

    errors.forEach(function (error) {
        if (error.key === 'Name') {
            $('#NameError').text(error.message);
        }
    });
}

function LoadTeachers(id) {
    $('#TeacherId').empty();
    var token = getCookie('Token');

    if (token) {

        var req = {
            PageSize: 9999,
            Role: 'TEACHER'
        }

        $.ajax({
            url: apiUrl + '/user/search-users',
            type: 'POST',
            data: JSON.stringify(req),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200) {
                    var select = $('#TeacherId');
                    var emptyOpt = $('<option>');
                    emptyOpt.val('-1');
                    emptyOpt.text('No Selection');
                    select.append(emptyOpt);

                    $.each(data.data.result, function (index, user) {
                        var option = $('<option>');
                        option.val(user.id);
                        option.text(user.fullName);
                        if (user.id === id) {
                            option.prop('selected', true);
                        }
                        select.append(option);
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
function LoadSubjects(id) {
    $('#SubjectId').empty();
    var token = getCookie('Token');

    if (token) {

        var req = {
            PageSize: 9999,
            Active: true
        }

        $.ajax({
            url: apiUrl + '/subject/search-subjects',
            type: 'POST',
            data: JSON.stringify(req),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200) {
                    var select = $('#SubjectId');

                    $.each(data.data.result, function (index, subject) {
                        var option = $('<option>');
                        option.val(subject.id);
                        option.text(subject.name);
                        if (subject.id === id) {
                            option.prop('selected', true);
                        }
                        select.append(option);
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