var apiUrl = 'https://localhost:7068/api';

$(document).ready(function () {
    LoadSubjects();

    $('#SubjectId').on('change', function () {
        var subjectId = $(this).val();
        if (subjectId == -1) {
            $('#data').empty();
            return false;
        }

        LoadStudentScore(subjectId);
    });
});


function LoadStudentScore(subjectId) {
    var token = getCookie('Token');

    if (token) {

        var req = {
            PageSize: 9999,
            Active: true,
            IsCurrentSubject: true
        }

        $.ajax({
            url: apiUrl + '/scores/search-student-score/' + subjectId,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data.data);
                if (data.statusCode == 200) {
                    var tbody = $('#data');
                    tbody.empty();
                    var totalScore = 0, totalPerent = 0;

                    $.each(data.data, function (i, score) {
                        var row = '<tr>';
                        row += '<td>' + score.componentScore.name + ' (' + score.componentScore.percent + '%) </td>';
                        row += '<td>' + (score.mark == null ? '' : score.mark) + '</td>';
                        tbody.append(row);
                        if (score.mark != null) {
                            totalScore += score.mark * score.componentScore.percent / 100;
                        }
                        totalPerent += score.componentScore.percent;
                    });

                    tbody.append('<tr><td>Total (' + totalPerent + '%)</td><td>' + totalScore.toFixed(2) + '</td>');
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
            Active: true,
            IsCurrentSubject: true
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
                    var empt = $('<option>');
                    empt.val(-1);
                    empt.text('No Selection');
                    select.append(empt);

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