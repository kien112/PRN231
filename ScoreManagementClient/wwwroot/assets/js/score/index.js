var apiUrl = 'https://localhost:7068/api/scores';
var componentScoreId = null, active = null, pageIndex = null, studentName = null,
    pageSize = 9999, totalElement = 0, sortBy = null, orderBy = 'ASC', classId = null,
    isClassChange = false;
var up = '&#8593;', down = '&#8595;';
var scoreRequest = [], classes = [];



$(document).ready(function () {

    LoadClasses();

    $('#StudentName').on('change', function () {
        studentName = $(this).val();
        SearchScores();
    })

    $('#ClassId').on('change', function () {
        classId = $(this).val();
        componentScoreId = null;
        isClassChange = true;
        SearchScores();
    });

    $('#ComponentScoreId').on('change', function () {
        componentScoreId = $(this).val();
        if (componentScoreId == -1)
            componentScoreId = null;
        else
            sortBy = null;
        SearchScores();
    });

    $('#PageSize').on('change', function () {
        pageSize = $(this).val();
        pageIndex = 0;
        SearchScores();
    });

    $('#PageIndex').on('change', function () {
        var page = $(this).val();
        pageIndex = page;
        SearchScores();
    });

    $('#btnSave').click(function () {
        console.log('score req', scoreRequest);
        cudScore();
    });

    $('#btnImport').click(function () {
        $('#importModal').modal('show');
    });

    $('#importModal').on('hidden.bs.modal', function () {
        $('#formFile').val('');
    });

    $('#btnSubmitImport').click(function () {
        importScore();
    });

    $('#btnExport').click(function () {
        exportScore();
    });
});

function exportScore() {
    var token = getCookie('Token');
    var cId = $('#ClassId').val();

    if (token && cId != '') {
        $.ajax({
            url: apiUrl + '/export-score/' + cId,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            },
            success: function (response, status, xhr) {
                var fileName = xhr.getResponseHeader('FileName');
                downloadExcelFile(response, fileName);
            },
            error: function (xhr, status, error) {
                Swal.fire('Error!!!');
                console.error(xhr.responseText);
            }
        });
    }
}

function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes.buffer;
}

function downloadExcelFile(base64Data, fileName) {
    var arrayBuffer = base64ToArrayBuffer(base64Data);
    var blob = new Blob([arrayBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    var url = window.URL.createObjectURL(blob);

    var a = $('<a style="display: none;"></a>');
    $('body').append(a);

    a.attr('href', url);
    a.attr('download', fileName);
    a[0].click();

    window.URL.revokeObjectURL(url);
    a.remove();
}

function importScore() {
    var file = $('#formFile').val();
    if (file == '') {
        Swal.fire('Please choose one excel file!');
        return;
    }

    var token = getCookie('Token');

    if (token) {
        var formData = new FormData();
        formData.append('ExcelFile', $('#formFile')[0].files[0]);
        formData.append('ClassId', $('#ClassId2').val());

        $.ajax({
            url: apiUrl + '/import-score',
            type: 'POST',
            data: formData,
            contentType: false, 
            processData: false, 
            headers: {
                'Authorization': 'Bearer ' + token 
            },
            success: function (response) {
                console.log(response);
                if (response.statusCode == 200) {
                    Swal.fire({
                        title: "Message!",
                        text: "Import Score Successfully!",
                        icon: "success"
                    });
                    $('#importModal').modal('hide');
                    SearchScores();
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    }
}

function cudScore() {

    if (scoreRequest.length == 0) {
        Swal.fire("No Score change!");
        return;
    }

    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: apiUrl + '/cud-scores',
            type: 'POST',
            data: JSON.stringify(scoreRequest),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode == 200) {
                    Swal.fire({
                        title: "Message!",
                        text: "Save Score Successfully!",
                        icon: "success"
                    });
                }
                if (data.errors.length > 0) {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "Save Score Successfully! But some invalid score is removed!",
                    });
                }
                SearchScores();
                scoreRequest = [];
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('Error:', errorThrown);
            }
        });
    } else {
        console.error('Token not found in cookie');
    }
}

function LoadClasses() {

    var info = getCookie('UserInfo');

    $('#ClassId').empty();
    $('#ClassId2').empty();
    var token = getCookie('Token');

    if (token) {

        var req = {
            PageSize: 9999,
            Active: true,
            IsCurrentClass: JSON.parse(info).Role != 'ADMIN' 
        }

        $.ajax({
            url: 'https://localhost:7068/api/ClassRoom/search-class',
            type: 'POST',
            data: JSON.stringify(req),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200) {
                    var select = $('#ClassId');
                    var select2 = $('#ClassId2');
                    classes = data.data.result;

                    $.each(data.data.result, function (index, subject) {
                        var option = $('<option>');
                        option.val(subject.id);
                        option.text(subject.name);
                        select.append(option);

                        var option2 = $('<option>');
                        option2.val(subject.id);
                        option2.text(subject.name);
                        select2.append(option2);
                    });

                    LoadComponentScores(data.data.result[0].subject.id);
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

function LoadComponentScores(sId) {
    $('#ComponentScoreId').empty();
    var token = getCookie('Token');

    if (token) {

        var req = {
            PageSize: 9999,
            Active: true,
            SubjectId: sId
        }

        $.ajax({
            url: 'https://localhost:7068/api/ComponentScore/search-component-score',
            type: 'POST',
            data: JSON.stringify(req),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200) {
                    var select = $('#ComponentScoreId');
                    var optionAll = $('<option>');
                    optionAll.val(-1);
                    optionAll.text('All');
                    select.append(optionAll);

                    $.each(data.data.result, function (index, cs) {
                        var option = $('<option>');
                        option.val(cs.id);
                        option.text(cs.name);
                        if (cs.id == componentScoreId) {
                            option.prop('selected', true);
                        }
                        select.append(option);
                    });
                    isClassChange = false;
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

function SearchScores() {

    var request = {
        StudentName: studentName,
        ClassId: classId,
        ComponentScoreId: componentScoreId,
        OrderBy: orderBy,
        SortBy: sortBy,
        PageIndex: pageIndex,
        PageSize: pageSize
    };

    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: apiUrl + '/search-scores',
            type: 'POST',
            data: JSON.stringify(request),
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data.data);
                if (data.statusCode == 200) {
                    displayData(data.data);
                    pageIndex = data.data.pageIndex;
                    pageSize = data.data.pageSize;
                    totalElement = data.data.totalElements;
                    $('#sp-class').text($('#ClassId option:selected').text());
                    $('#sp-subject').text(data.data.subject.name);
                    LoadPageIndex();
                    if (isClassChange == true)
                        LoadComponentScores(data.data.subject.id);
                    scoreRequest = [];
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

function displayData(data) {

    var th = $('#sortableTable');
    th.empty();
    th.append('<th scope="col">ID</th>');
    var th_fullName = $('<th>');
    th_fullName.attr('id', 'FullName');
    th_fullName.html('FullName <span class="up-down-arrow">&#8593; &#8595;</span>');
    th_fullName.click(function () {
        registerSortEvent('FullName');
    });
    th.append(th_fullName);

    var totalP = 0;
    data.componentScore.forEach(function (c) {
        var th_component_score = $('<th>');
        th_component_score.attr('id', c.name);
        th_component_score.html(c.name + ' (' + c.percent + '%) <span class="up-down-arrow">&#8593; &#8595;</span>');
        th_component_score.click(function () {
            registerSortEvent(c.name);
        });
        totalP += c.percent;
        th.append(th_component_score);
    });
    th.append('<th scope="col">Total (' + totalP + '%)</th>');


    $('.up-down-arrow').html(up + ' ' + down);
    $('.up-down-arrow').removeClass('text-danger');
    if (sortBy != null) {
        var selectedField = $('#' + sortBy).find('span');
        selectedField.html(orderBy === 'ASC' ? up : down);
        selectedField.addClass('text-danger');
    }

    $('#data').empty();

    data.studentScores.forEach(function (item) {
        var row = $('<tr>');

        var th = $('<th>').attr('scope', 'row').text(item.student.id.substring(0, 8));
        row.append(th);

        var tdFullName = $('<td>').text(item.student.fullName);
        row.append(tdFullName);

        var total = 0,
            totalPercent = 0;

        item.componentScoreIdAndMarks.forEach(function (m) {
            if (m.mark != null) {
                total += (m.percent * m.mark) / 100;
                totalPercent += m.percent;
            }
            var td = $('<td>');
            var input = $('<input>').attr({
                id: item.student.id + '.' + m.id,
                type: 'number',
                step: '0.1',
                min: 0.0,
                max: 10.0,
                style: 'width: 50px',
                value: m.mark == null ? '' : m.mark,
                'data-old-val': m.mark == null ? '' : m.mark
            }).on('input', function () {
                var score = $(this).val();
                var oldVal = $(this).data('old-val');

                if (score < 0 || score > 10) {
                    Swal.fire('Score must be in range 0 to 10!');
                    $(this).val(oldVal);
                    return false;
                }
                var studentAndComponentId = $(this).attr('id');
                $(this).data('old-val', score);
                saveScore(score, studentAndComponentId);
            });
            td.append(input);
            row.append(td);
        });

        var tdTotal = $('<td>').text(total.toFixed(2) + ' (' + totalPercent + '%)');
        row.append(tdTotal);

        $('#data').append(row);
    });

}

function saveScore(score, studentAndComponentId) {
    score = score == '' ? null : score;
    var studentId = studentAndComponentId.split('.')[0];
    var componentScoreId = studentAndComponentId.split('.')[1];
    var found = false;

    for (var i = 0; i < scoreRequest.length; i++) {
        if (scoreRequest[i].StudentId === studentId && scoreRequest[i].ComponentScoreId === componentScoreId) {
            scoreRequest[i].Mark = score;
            found = true;
            break;
        }
    }

    if (!found) {
        var req = {
            Mark: score,
            StudentId: studentId,
            ComponentScoreId: componentScoreId
        };
        scoreRequest.push(req);
    }
}

function LoadPageIndex() {

    $('#PageIndex').empty();

    if (pageSize === 9999) {
        $('#PageIndex').append('<option value="' + (0) + '" > ' + 1 + ' </option>');
        return;
    }

    var numberOfPage = totalElement % pageSize == 0
        ? totalElement / pageSize : 1 + totalElement / pageSize;
    numberOfPage = Math.floor(numberOfPage);

    for (var i = 0; i < numberOfPage; i++) {
        if (i === pageIndex)
            $('#PageIndex').append('<option value="' + (i) + '" selected="selected"> ' + (i + 1) + ' </option>');
        else
            $('#PageIndex').append('<option value="' + (i) + '" > ' + (i + 1) + ' </option>');
    }
}


function registerSortEvent(id) {

    var columnName = id;
    console.log(columnName);

    if (columnName !== undefined) {
        if (columnName !== sortBy) {
            orderBy = 'ASC';
            sortBy = columnName;
        } else {
            orderBy = orderBy === 'ASC' ? 'DESC' : 'ASC';
        }
        
        SearchScores();
    }
}