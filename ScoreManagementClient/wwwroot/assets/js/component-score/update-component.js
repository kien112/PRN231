﻿var apiUrl = 'https://localhost:7068/api/componentscore';

$(document).ready(function () {

    GetComponentDetail();

    $('#btnUpdate').click(function () {
        UpdateComponentScore();
    });

});

function GetComponentDetail() {
    var id = $('#Id').val();
    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: 'https://localhost:7068/api/componentscore/get-component-score-by-id/' + id,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode == 200) {
                    $("#ComponentName").val(data.data.name);
                    $("#Percent").val(data.data.percent);
                    $("#Description").val(data.data.description);
                    LoadSubjects(data.data.subject.id);
                    $("#Active").val(data.data.active ? "1" : "0");
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: data.message,
                    }).then((result) => {
                        if (result.isConfirmed || result.isDismissed) {
                            window.location.href = '/componentscores';
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

function LoadSubjects(sId) {

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

                        if (s.id == sId)
                            option.attr('selected', true);

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

function UpdateComponentScore() {
    var token = getCookie('Token');

    if (token) {

        var id = $('#Id').val();
        var active = $('#Active').val() == 1;
        var name = $("#ComponentName").val();
        var percent = $('#Percent').val() == '' ? 0 : $('#Percent').val();
        var descriptiom = $("#Description").val();
        var subjectId = $('#SubjectId').val();

        var req = {
            Id: id,
            Active: active,
            Name: name,
            Percent: percent,
            SubjectId: subjectId,
            Description: descriptiom
        };

        $.ajax({
            url: apiUrl + '/update-component-score',
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