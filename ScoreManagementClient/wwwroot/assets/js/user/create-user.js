var apiUrl = 'https://localhost:7068/api/auth';

$(document).ready(function () {

    $('#btnCreate').click(function () {
        CreateUser();
    });

});

function CreateUser() {
    var token = getCookie('Token');

    if (token) {

        var name = $("#FullName").val();
        var email = $("#Email").val();
        var username = $("#Username").val();
        var password = $("#Password").val();
        var gender = $("#Gender").val() == "Male";

        var req = {
            FullName: name,
            Email: email,
            Username: username,
            Password: password,
            Gender: gender
        };
        $(this).prop('disabled', true);
        $('#mess').text('Please wait...');

        $.ajax({
            url: apiUrl + '/create-user',
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
                    window.location.href = '/users';

                $('#mess').text('');
                $(this).prop('disabled', false);

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
    $('#EmailError').text('');
    $('#UsernameError').text('');
    $('#PasswordError').text('');

    errors.forEach(function (error) {
        if (error.key === 'FullName') {
            $('#NameError').text(error.message);
        }
        else if (error.key === 'Email') {
            $('#EmailError').text(error.message);
        }
        else if (error.key === 'UserName') {
            $('#UsernameError').text(error.message);
        }
        else if (error.key === 'Password') {
            $('#PasswordError').text(error.message);
        }
    });
}