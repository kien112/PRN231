var apiUrl = 'https://localhost:7068/api';

$(document).ready(function () {
    LoadTeachers();
    LoadSubjects();

    $('#btnCreate').click(function () {
        CreateClass();
    });
});

function CreateClass() {
    var token = getCookie('Token');

    if (token) {

        var req = {
            Name: $('#ClassName').val(),
            TeacherId: $('#TeacherId').val() == -1 ? null : $('#TeacherId').val(),
            SubjectId: $('#SubjectId').val()
        }

        $.ajax({
            url: apiUrl + '/classroom/create-class',
            type: 'POST',
            data: JSON.stringify(req),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200)
                    window.location.href = '/classrooms';
                else if(data.errors)
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

function LoadTeachers() {
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
function LoadSubjects() {
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