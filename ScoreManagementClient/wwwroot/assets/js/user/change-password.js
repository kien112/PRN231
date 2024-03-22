

$(document).ready(function () {
    $('#btnChange').click(function () {
        ChangePassword();
    });
});

function ChangePassword() {
    var token = getCookie('Token');

    if (token) {

        var req = {
            OldPassword: $('#OldPassword').val(),
            NewPassword: $('#NewPassword').val(),
            ConfirmPassword: $('#ConfirmPassword').val()
        };

        $.ajax({
            url: 'https://localhost:7068/api/Auth/change-password',
            type: 'PUT',
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
                    Swal.fire({
                        title: "Message!",
                        text: "Change Password Successfully!",
                        icon: "success"
                    }).then((result) => {
                        if (result.isConfirmed || result.isDismissed) {
                            window.location.href = '/home';
                        }
                    });
                else if (data.message != "ok")
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
    $('#OldPasswordError').text('');
    $('#NewPasswordError').text('');
    $('#ConfirmPasswordError').text('');
    $('#mess').text('');

    errors.forEach(function (error) {
        if (error.key === 'OldPassword') {
            $('#OldPasswordError').text(error.message);
        }
        else if (error.key === 'NewPassword') {
            $('#NewPasswordError').text(error.message);
        }
        else if (error.key === 'ConfirmPassword') {
            $('#ConfirmPasswordError').text(error.message);
        }
        else if (error.key === 'Message') {
            $('#mess').text(error.message);
        }
    });

}