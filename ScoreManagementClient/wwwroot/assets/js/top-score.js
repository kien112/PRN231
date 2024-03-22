var apiUrl = 'https://localhost:7068/api';
var topScores = [];
var subjectId = 0;

$(document).ready(function () {
    LoadSubjects();

    $('#Top').on('change', function () {
        var top = $('#Top').val();
        subjectId = $('#SubjectId').val();
        LoadTopScore(top, true);
    });

    $('#SubjectId').on('change', function () {
        var top = $('#Top').val();
        subjectId = $('#SubjectId').val();
        LoadTopScore(top, true);
    });

    $('#btnExport').click(function () {
        exportToExcel();
    });

    $('#profile-tab').on('shown.bs.tab', function (e) {
        subjectId = $('#SubjectId').val();
        LoadTopScore(9999, false);
    });
});

function LoadTopScore(top, loadToTable) {
    var token = getCookie('Token');

    if (token) {

        top = top == '' ? 10 : top;

        $.ajax({
            url: apiUrl + '/scores/get-top-score/' + subjectId + '/' + top,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200) {
                    if (loadToTable) {
                        displayData(data.data.topScores);
                        if (data.data.ownerRank)
                            $('#ownerRank').text('Your Score is: ' + data.data.ownerRank.score + ' in rank: ' + data.data.ownerRank.rank);
                    } else {
                        topScores = data.data.topScores;
                        generateChart();
                    }
                } else {
                    Swal.fire(data.message);
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

function generateChart() {

    var count_0_5 = 0;
    var count_5_8 = 0;
    var count_8_10 = 0;
    var subjectName = topScores.length > 0 ? topScores[0].subject.name : '';

    var oldChart = document.getElementById('myChart');
    if (oldChart) {
        oldChart.parentNode.removeChild(oldChart);
    }

    var canvas = document.createElement('canvas');
    canvas.id = 'myChart';
    document.getElementById('profile').appendChild(canvas);

    topScores.forEach(function (sc) {
        if (sc.score >= 0 && sc.score < 5) {
            count_0_5++;
        } else if (sc.score >= 5 && sc.score < 8) {
            count_5_8++;
        } else if (sc.score >= 8 && sc.score <= 10) {
            count_8_10++;
        }
    });

    var total = topScores.length;
    var percent_0_5 = (count_0_5 / total * 100).toFixed(2);
    var percent_5_8 = (count_5_8 / total * 100).toFixed(2);
    var percent_8_10 = (count_8_10 / total * 100).toFixed(2);

    var labels = [percent_0_5 + '%', percent_5_8 + '%', percent_8_10 + '%'];

    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [`[0-5) ${percent_0_5}%`, `[5-8) ${percent_5_8}%`, `[8-10] ${percent_8_10}%`],
            datasets: [{
                label: 'Score Distribution of ' + subjectName + ` (${total} students)`,
                data: [count_0_5, count_5_8, count_8_10],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1,
                    }
                }
            }
        }
    });
}

function displayData(data) {
    var thead = $('#sortableTable');
    thead.empty();
    thead.append('<th>ID</th><th>FullName</th><th>ClassRoom</th><th>Subject</th><th>Score</th><th>Rank</th>');

    var tbody = $('#data');
    tbody.empty();
    $.each(data, function (i, t) {
        var row = '<tr>';
        row += '<td>' + t.student.id.substring(0, 8) + '</td>';
        row += '<td>' + t.student.fullName + '</td>';
        row += '<td>' + t.student.className + '</td>';
        row += '<td>' + t.subject.name + '</td>';
        row += '<td>' + t.score + '</td>';
        row += '<td>' + t.rank + '</td>';
        row += '</tr>';

        tbody.append(row);
    });
}

function exportToExcel() {
    var table = document.getElementById("top-score-table");

    var wb = XLSX.utils.table_to_book(table, { sheet: "Top Score" });
    
    XLSX.writeFile(wb, `top_score.xlsx`);
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