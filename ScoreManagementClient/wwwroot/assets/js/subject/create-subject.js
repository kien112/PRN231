var apiUrl = 'https://localhost:7068/api/subject';

$(document).ready(function () {

    $('#btnCreate').click(function () {
        CreateSubject();
    });
    
});

function CreateSubject() {
    var token = getCookie('Token');

    if (token) {

        var name = $("#SubjectName").val();
        var descriptiom = $("#Description").val();

        var createSubject = {
            Name: name,
            Description: descriptiom
        };

        $.ajax({
            url: apiUrl + '/create-subject',
            type: 'POST',
            data: JSON.stringify(createSubject),
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
    console.log('aa')
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