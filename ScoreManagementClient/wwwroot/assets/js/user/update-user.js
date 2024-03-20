var apiUrl = 'https://localhost:7068/api/auth';

$(document).ready(function () {

    GetUserDetail();

    $('#btnUpdate').click(function () {
        UpdateUser();
    });

});

function GetUserDetail() {
    var id = $('#Id').val();
    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: 'https://localhost:7068/api/user/get-by-id/' + id,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode == 200) {
                    $("#FullName").val(data.data.fullName);
                    $("#Email").val(data.data.email);
                    $("#Username").val(data.data.userName);
                    $("#Gender").val(data.data.gender ? "Male" : "Female");
                    $("#Active").val(data.data.active ? "1" : "0");
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: data.message,
                    }).then((result) => {
                        if (result.isConfirmed || result.isDismissed) {
                            window.location.href = '/users';
                        }
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

function UpdateUser() {
    var token = getCookie('Token');

    if (token) {

        var id = $('#Id').val();
        var name = $("#FullName").val();
        var email = $("#Email").val();
        var username = $("#Username").val();
        var password = $("#Password").val();
        var gender = $("#Gender").val() == "Male";
        var active = $("#Active").val() == "1";

        var req = {
            Id: id,
            FullName: name,
            Email: email,
            Username: username,
            Password: password == '' ? null : password,
            Gender: gender,
            Active: active
        };
        $(this).prop('disabled', true);
        $('#mess').text('Please wait...');

        $.ajax({
            url: apiUrl + '/update-user',
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
                    window.location.href = '/users';

                $(this).prop('disabled', false);
                $('#mess').text('');
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