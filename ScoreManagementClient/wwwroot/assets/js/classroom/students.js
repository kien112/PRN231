var apiUrl = 'https://localhost:7068/api';
var studentList = [];
var searchStudent = [];

$(document).ready(function () {
    GetClassById();

    $('#StudentName').on('input', function () {
        var text = $(this).val().toLowerCase().trim();
        searchStudent = studentList.filter(function (student) {
            return student.fullName.toLowerCase().includes(text);
        });
        LoadSearchStudent();
    });

    $('#btnSave').click(function () {
        SaveStudents();
    });

    registerEvent();
});

function SaveStudents() {
    var checkedIds = studentList
        .filter(function (student) {
            return student.isChecked === true;
        })
        .map(function (student) {
            return student.id;
        });
    var id = $('#Id').val();
    var token = getCookie('Token');

    if (token) {

        var req = {
            ClassId: id,
            StudentIds: checkedIds
        }

        $.ajax({
            url: apiUrl + '/classroom/cud-students-to-class',
            type: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            data: JSON.stringify(req),
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200) {
                    Swal.fire({
                        title: "Message!",
                        text: "Save Students Successfully!",
                        icon: "success"
                    });
                }
                else
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: data.message,
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('Error:', errorThrown);
            }
        });
    } else {
        console.error('Token not found in cookie');
    }
}

function registerEvent() {
    $('.isChecked').click(function () {
        var isChecked = $(this).prop('checked');
        var id = $(this).val();

        var student = studentList.find(function (item) {
            return item.id === id;
        });

        if (student) {
            student.isChecked = isChecked;
        }
        console.log('studentList', studentList)
    });
}

function LoadSearchStudent() {
    $('#data').empty();

    searchStudent.forEach(function (item) {
        var sidSubstring = item.id.substring(0, 8);
        var row = '<tr>';
        row += '<th  scope="row"> <input class="isChecked" value="' + item.id + '" type="checkbox" ' + (item.isChecked ? "checked" : "") + '/> </th>';
        row += '<td class="sid">' + sidSubstring + '</td>';
        row += '<td>' + item.fullName + '</td>';
        row += '<td>' + item.joinDate + '</td>';
        row += '</tr>';
        $('#data').append(row);
    });
    registerEvent();
}

function GetClassById() {
    var id = $('#Id').val();
    var token = getCookie('Token');

    if (token) {
        $.ajax({
            url: apiUrl + '/classroom/get-class-by-id/' + id,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200) {
                    $('#ClassName').text(data.data.name + " and Subject " + data.data.subject.name);
                    displayData(data.data.classStudents);
                    LoadStudents();
                }
                else
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: data.message,
                    });
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
    $('#data').empty();

    data.forEach(function (item) {
        var sidSubstring = item.student.id.substring(0, 8);
        var student = {
            id: item.student.id,
            fullName: item.student.fullName,
            joinDate: item.joinDate,
            isChecked: true
        }
        studentList.push(student);
        var row = '<tr>';
        row += '<th  scope="row"> <input class="isChecked" value="' + item.student.id + '" type="checkbox" checked/> </th>';
        row += '<td class="sid">' + sidSubstring + '</td>';
        row += '<td>' + item.student.fullName + '</td>';
        row += '<td>' + item.joinDate + '</td>';
        row += '</tr>';
        $('#data').append(row);
    });
    registerEvent();
}
function LoadStudents() {
    var token = getCookie('Token');

    if (token) {

        $.ajax({
            url: apiUrl + '/classroom/get-remain-students/' + $('#Id').val(),
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-type': 'Application/json'
            },
            success: function (data) {
                console.log('Data from API:', data);
                if (data.statusCode === 200) {

                    $.each(data.data, function (index, user) {
                        var userIdSubstring = user.id.substring(0, 8);
                        var student = {
                            id: user.id,
                            fullName: user.fullName,
                            joinDate: '',
                            isChecked: false
                        }
                        studentList.push(student);
                        var row = '<tr>';
                        row += '<th scope="row"> <input class="isChecked" value="' + user.id + '" type="checkbox"/> </th>';
                        row += '<td class="limited-width">' + userIdSubstring + '</td>';
                        row += '<td>' + user.fullName + '</td>';
                        row += '<td></td>';
                        row += '</tr>';
                        $('#data').append(row);
                    });
                    registerEvent();
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