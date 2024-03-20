$(document).ready(function () {
    $('#btnSend').click(function () {
        $(this).text('Please wait...');
        $(this).attr('disabled', true);
        forgotPassword();
        $(this).text('Send');
        $(this).attr('disabled', false);

    });
});

function forgotPassword() {
    var email = $('#Email').val();
    if (email == '') {
        $('#mess').text('Please enter your email!');
        return;
    }

    var req = {
        Email: email
    };

    $.ajax({
        url: 'https://localhost:7068/api/Auth/forgot-password',
        type: 'PUT',
        data: JSON.stringify(req),
        headers: {
            'Content-type': 'Application/json'
        },
        success: function (data) {
            console.log('Data from API:', data.data);
            if (data.statusCode == 200) {
                $('#mess').text('You can get your new password in email!');
            } else if (data.message) {
                $('#mess').text(data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error:', errorThrown);
        }
    });

}