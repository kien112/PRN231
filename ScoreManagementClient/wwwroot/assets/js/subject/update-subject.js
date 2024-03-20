var apiUrl = 'https://localhost:7068/api/subject';

$(document).ready(function () {

    $('#btnUpdate').click(function () {
        UpdateSubject();
    });

});

function UpdateSubject() {
    var token = getCookie('Token');

    if (token) {

        var name = $("#SubjectName").val();
        var descriptiom = $("#Description").val();
        var active = $('#Active').val() == 1;
        var id = $('#SubjectId').val();

        var updateSubject = {
            Id: id,
            Active: active,
            Name: name,
            Description: descriptiom
        };

        $.ajax({
            url: apiUrl + '/update-subject',
            type: 'PUT',
            data: JSON.stringify(updateSubject),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);

                if (data.errors)
                    displayErrors(data.errors);
                else if (data.statusCode == 200)
                    window.location.href = '/subjects';
                else if (data.message)
                    $('#mess').text(data.message);
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
    $('#DescriptionError').text('');

    errors.forEach(function (error) {
        console.log(error.key)
        if (error.key === 'Name') {
            $('#NameError').text(error.message);
        }
        else if (error.key === 'Description') {
            console.log(error.message)
            $('#DescriptionError').text(error.message);
        }
    });
}