var apiUrl = 'https://localhost:7068/api/componentscore';

$(document).ready(function () {

    LoadSubjects();

    $('#btnCreate').click(function () {
        CreateComponentScore();
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

function CreateComponentScore() {
    var token = getCookie('Token');

    if (token) {

        var name = $("#ComponentName").val();
        var percent = $('#Percent').val() == '' ? 0 : $('#Percent').val();
        var descriptiom = $("#Description").val();
        var subjectId = $('#SubjectId').val();

        var req = {
            Name: name,
            Percent: percent,
            SubjectId: subjectId,
            Description: descriptiom
        };

        $.ajax({
            url: apiUrl + '/create-component-score',
            type: 'POST',
            data: JSON.stringify(req),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);

                if (data.errors)
                    displayErrors(data.errors);
                else if (data.statusCode == 200)
                    window.location.href = '/componentscores';
                else if (data.statusCode == 400) {
                    refreshError();
                    $('#ErrorMessage').text(data.message);
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

function displayErrors(errors) {
    refreshError();

    errors.forEach(function (error) {
        console.log(error.key)
        if (error.key === 'Name') {
            $('#NameError').text(error.message);
        }
        else if (error.key === 'Description') {
            console.log(error.message)
            $('#DescriptionError').text(error.message);
        }
        else if (error.key === 'Percent') {
            console.log(error.message)
            $('#PercentError').text(error.message);
        }
        else if (error.key === 'SubjectId') {
            console.log(error.message)
            $('#SubjectIdError').text(error.message);
        }
    });
}

function refreshError() {
    $('#NameError').text('');
    $('#PercentError').text('');
    $('#SubjectIdError').text('');
    $('#DescriptionError').text('');
}