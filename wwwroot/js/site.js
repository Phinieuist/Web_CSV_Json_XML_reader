// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var dropZone;
var divId;
var dropzoneType;


// Initializes the dropZone
$(document).ready(function () {
    if ((document.getElementById('dropZone') == null) && (document.getElementById('dropZone2') == null)) {
        console.log("No dropzone");
        return;
    }
    if (document.getElementById('dropZone') !== null) {
        dropZone = $('#dropZone')
        dropzoneType = 1;
    }


    if (document.getElementById('dropZone2') !== null) {
        dropZone = $('#dropZone2')
        dropzoneType = 2;
    }

    //if (dropzoneType == 0)
     //   return;
    //dropZone = $('#dropZone')

    console.log("DropzoneType is " + dropzoneType);
    console.log(dropZone);
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

        if (!isFileExtensionAllowed(file.name) && dropzoneType == 1) {
            //dropZone.text('Invalid File Type!');
            //dropZone.addClass('error');
            alert('Файл данного типа не поддерживается');
            return;
        }

        if (dropzoneType == 1) {
            var fileExtension = file.name.split('.').pop().toLowerCase();
            $('#fileName').val(file.name.split('.')[0]);
            $('#fileType').val(ToModelFileType(file.name));
            //$('#fileType').value = ToModelFileType(file.name);
        }

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

(function () {
    'use strict';

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('.needs-validation');

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms).forEach(function (form) {
        // Получение полей ввода
        var emailRegisterInput = form.querySelector('#validationRegisterEmail');
        var passwordRegisterInput = form.querySelector('#validationRegisterPassword');
        var confirmRegisterPasswordInput = form.querySelector('#validationRegisterPasswordRepeat');
        var emailLoginInput = form.querySelector('#validationLoginEmail');
        var passwordLoginInput = form.querySelector('#validationLoginPassword');

        // Добавление слушателей событий для проверки ввода при вводе или изменении значения
        if (emailRegisterInput !== null) emailRegisterInput.addEventListener('input', validateRegisterEmail);
        if (passwordRegisterInput !== null) passwordRegisterInput.addEventListener('input', validateRegisterPassword);
        if (confirmRegisterPasswordInput !== null) confirmRegisterPasswordInput.addEventListener('input', validateRegisterConfirmPassword);
        if (emailLoginInput !== null) emailLoginInput.addEventListener('input', validateLoginEmail);
        if (passwordLoginInput !== null) passwordLoginInput.addEventListener('input', validateLoginPassword);

        // Добавление слушателя события 'submit' для формы
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);

        validateFieldsOnLoad();

        // Функция для проверки полей сразу после загрузки страницы
        function validateFieldsOnLoad() {
            if (emailRegisterInput !== null) validateRegisterEmail.call(emailRegisterInput); // Проверяем поле email
            if (passwordRegisterInput !== null) validateRegisterPassword.call(passwordRegisterInput); // Проверяем поле password
            if (confirmRegisterPasswordInput !== null) validateRegisterConfirmPassword.call(confirmRegisterPasswordInput); // Проверяем поле confirmPassword
            if (emailLoginInput !== null) validateLoginEmail.call(emailLoginInput);
            if (passwordLoginInput !== null) validateLoginPassword.call(passwordLoginInput);
        }
    });

    // Функции для валидации полей
    function validateRegisterEmail() {
        var email = this.value;
        var isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
        var emailError = document.getElementById('emailRegisterError');
        if (!isValid) {
            this.setCustomValidity('Пожалуйста, введите правильный адрес электронной почты');
            emailError.textContent = 'Пожалуйста, введите правильный адрес электронной почты';
        } else {
            this.setCustomValidity('');
            emailError.textContent = '';
        }
    }

    function validateRegisterPassword() {
        var password = this.value;
        var passwordError = document.getElementById('passwordRegisterError');
        if (password.length < 6) {
            this.setCustomValidity('Пароль должен содержать не менее 6 символов');
            passwordError.textContent = 'Пароль должен содержать не менее 6 символов';
        } else {
            this.setCustomValidity('');
            passwordError.textContent = '';
        }
    }

    function validateRegisterConfirmPassword() {
        var confirmPassword = this.value;
        var password = document.getElementById('validationRegisterPassword').value;
        var confirmPasswordError = document.getElementById('confirmRegisterPasswordError');
        if (confirmPassword !== password) {
            this.setCustomValidity('Пароли не совпадают');
            confirmPasswordError.textContent = 'Пароли не совпадают';
        } else {
            this.setCustomValidity('');
            confirmPasswordError.textContent = '';
        }
    }

    function validateLoginEmail() {
        var email = this.value;
        var isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
        var emailError = document.getElementById('emailLoginError');
        if (!isValid) {
            this.setCustomValidity('Пожалуйста, введите правильный адрес электронной почты');
            emailError.textContent = 'Пожалуйста, введите правильный адрес электронной почты';
        } else {
            this.setCustomValidity('');
            emailError.textContent = '';
        }
    }

    function validateLoginPassword() {
        var password = this.value;
        var passwordError = document.getElementById('LoginPasswordError');
        if (password.length < 1) {
            this.setCustomValidity('Введите пароль');
            passwordError.textContent = 'Введите пароль';
        } else {
            this.setCustomValidity('');
            passwordError.textContent = '';
        }
    }

})();

function OnDeleteUser() {
    var answer = window.confirm("Вы дейсвительно хотите удалить аккаунт? Все ваши сохранённые файлы будут удалены");

    return answer;
}

function GetKeyPair() {
    var scheme = document.getElementById('scheme').value;
    var host = document.getElementById('host').value;
    var request = new XMLHttpRequest();
    var url = scheme + "://" + host + "/Сryptography/GetNewKeyPair";

    request.open('GET', url);
    request.responseType = 'json';
    request.send();

    request.onload = function () {
        var resp = request.response;

        console.log(resp);
        console.log('Второй ключ' + resp[1]);

        document.getElementById('PublicKeyId').value = resp[0];
        document.getElementById('PrivateKeyId').value = resp[1];
    }
}

function CopyPrivateKey() {
    document.getElementById('PrivateKeyInUseId').value = document.getElementById('PrivateKeyId').value;
}

function CopyPublicKey(PublicKeyId) {
    document.getElementById('PublicKeyInUseId').value = document.getElementById(PublicKeyId).value;
}

function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}
