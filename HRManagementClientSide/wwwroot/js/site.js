$('#employeeFilterForm').submit(function (e) {
    e.preventDefault();

    var formData = $(this).serializeArray();
    var data = [];

    for (var i = 0; i < formData.length; i += 2) {
        var obj = {
            propertyType: parseInt(formData[i].value),
            propertyValue: formData[i + 1].value
        };
        data.push(obj);
    }

    $.ajax({
        type: 'POST',
        url: '/Employee/FilterEmployees',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (result) {
            // Handle success
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle error
        }
    });
});



