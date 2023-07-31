// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var dropZone;
var divId;


// Initializes the dropZone
$(document).ready(function () {
    dropZone = $('#dropZone');
    //document.querySelectorAll('#outD').forEach(q => q.className = 'input-group mb-3');
    //document.querySelectorAll('#inD').forEach(q => q.className = 'input-group-prepend');
    //document.querySelectorAll('#spn').forEach(q => q.className = 'input-group-text');
    //$('#outD').addClass('input-group mb-3')
    //$('#inD').addClass('input-group-prepend')
    //$('#spn').addClass('input-group-text')
    //dropZone.removeClass('error');

    // Check if window.FileReader exists to make
    // sure the browser supports file uploads
    if (typeof (window.FileReader) == 'undefined') {
        dropZone.text('Browser Not Supported!');
        //dropZone.addClass('error');
        return;
    }

    // Add a nice drag effect
    dropZone[0].ondragover = function () {
        dropZone.addClass('hover');
        return false;
    };

    // Remove the drag effect when stopping our drag
    dropZone[0].ondragend = function () {
        dropZone.removeClass('hover');
        return false;
    };

    // The drop event handles the file sending
    dropZone[0].ondrop = function (event) {
        // Stop the browser from opening the file in the window
        event.preventDefault();
        dropZone.removeClass('hover');

        // Get the file and the file reader
        var file = event.dataTransfer.files[0];

        if (!isFileExtensionAllowed(file.name)) {
            //dropZone.text('Invalid File Type!');
            //dropZone.addClass('error');
            alert('Файл данного типа не поддерживается');
            return;
        }

        var fileExtension = file.name.split('.').pop().toLowerCase();
        $('#fileType').val(ToModelFileType(file.name));
        //$('#fileType').value = ToModelFileType(file.name);

        var reader = new FileReader();

        reader.onload = function (event) {
            // Получаем содержимое файла в виде текста
            var fileContent = event.target.result;

            // Здесь вы можете обработать содержимое файла по вашим потребностям
            dropZone.text(fileContent);
        };

        reader.readAsText(file);
    };
    function isFileExtensionAllowed(fileName) {
        var allowedExtensions = ['csv', 'json', 'xml'];
        var fileExtension = fileName.split('.').pop().toLowerCase();
        return allowedExtensions.includes(fileExtension);
    }

    function ToModelFileType(fileName) {
        switch (fileName.split('.').pop().toLowerCase()) {
            case 'csv': return 0;//@FileType.CSV;
            case 'json': return 1;//@FileType.JSON;
            case 'xml': return 2;//@FileType.XML;
            default: return;
        }
    }
});