//function uploadFile() {
//    var fileInput = document.getElementById('picturePath');
//    var file = fileInput.files[0];

//    if (file) {
//        if (file.size > 500 * 1024) {
//            // Display the pop-up message
//            var popupMessage = document.getElementById('popupMessage');
//            popupMessage.style.display = 'block';

//            // Reset the file input field
//            fileInput.value = '';

//            // Display error message or perform any other UI updates
//            console.log('File size exceeds the maximum limit!');
//            return;
//        }

//        var formData = new FormData();
//        formData.append('imageFile', file);

//        // Perform additional processing or send the form data to the server
//        // ...

//        // Display the uploaded image
//        var imagePreview = document.getElementById('imagePreviewContainer');
//        var imageElement = document.createElement('img');
//        imageElement.src = URL.createObjectURL(file);
//        imageElement.style.maxWidth = '100%';
//        imageElement.style.maxHeight = '300px';
//        imagePreview.innerHTML = '';
//        imagePreview.appendChild(imageElement);

//        // Reset the file input field
//        fileInput.value = '';

//        // Display success or perform any other UI updates
//        console.log('File uploaded successfully!');
//    } else {
//        console.log('No file selected.');
//    }
//}

//document.getElementById('picturePath').addEventListener('change', uploadFile);

//function uploadFile2() {
//    var fileInput = document.getElementById('signaturePath');
//    var file = fileInput.files[0];

//    if (file) {
//        if (file.size > 500 * 1024) {
//            // Display the pop-up message
//            var popupMessage = document.getElementById('popupMessage');
//            popupMessage.style.display = 'block';

//            // Reset the file input field
//            fileInput.value = '';

//            // Display error message or perform any other UI updates
//            console.log('File size exceeds the maximum limit!');
//            return;
//        }

//        var formData = new FormData();
//        formData.append('file2', file);

//        // Perform additional processing or send the form data to the server
//        // ...

//        // Display the uploaded image
//        var imagePreview = document.getElementById('imagePreviewContainer2');
//        var imageElement = document.createElement('img');
//        imageElement.src = URL.createObjectURL(file);
//        imageElement.style.maxWidth = '100%';
//        imageElement.style.maxHeight = '300px';
//        imagePreview.innerHTML = '';
//        imagePreview.appendChild(imageElement);

//        // Reset the file input field
//        fileInput.value = '';

//        // Display success or perform any other UI updates
//        console.log('File uploaded successfully!');
//    } else {
//        console.log('No file selected.');
//    }
//}

//document.getElementById('signaturePath').addEventListener('change', uploadFile2);

//function uploadFile3() {
//    var fileInput = document.getElementById('passportPathFront');
//    var file = fileInput.files[0];

//    if (file) {
//        if (file.size > 500 * 1024) {
//            // Display the pop-up message
//            var popupMessage = document.getElementById('popupMessage');
//            popupMessage.style.display = 'block';

//            // Reset the file input field
//            fileInput.value = '';

//            // Display error message or perform any other UI updates
//            console.log('File size exceeds the maximum limit!');
//            return;
//        }

//        var formData = new FormData();
//        formData.append('file3', file);

//        // Perform additional processing or send the form data to the server
//        // ...

//        // Display the uploaded image
//        var imagePreview = document.getElementById('imagePreviewContainer3');
//        var imageElement = document.createElement('img');
//        imageElement.src = URL.createObjectURL(file);
//        imageElement.style.maxWidth = '100%';
//        imageElement.style.maxHeight = '300px';
//        imagePreview.innerHTML = '';
//        imagePreview.appendChild(imageElement);

//        // Reset the file input field
//        fileInput.value = '';

//        // Display success or perform any other UI updates
//        console.log('File uploaded successfully!');
//    } else {
//        console.log('No file selected.');
//    }
//}

//document.getElementById('passportPathFront').addEventListener('change', uploadFile3);

//function uploadFile4() {
//    var fileInput = document.getElementById('passportPathBack');
//    var file = fileInput.files[0];

//    if (file) {
//        if (file.size > 500 * 1024) {
//            // Display the pop-up message
//            var popupMessage = document.getElementById('popupMessage');
//            popupMessage.style.display = 'block';

//            // Reset the file input field
//            fileInput.value = '';

//            // Display error message or perform any other UI updates
//            console.log('File size exceeds the maximum limit!');
//            return;
//        }

//        var formData = new FormData();
//        formData.append('file4', file);

//        // Perform additional processing or send the form data to the server
//        // ...

//        // Display the uploaded image
//        var imagePreview = document.getElementById('imagePreviewContainer2');
//        var imageElement = document.createElement('img');
//        imageElement.src = URL.createObjectURL(file);
//        imageElement.style.maxWidth = '100%';
//        imageElement.style.maxHeight = '300px';
//        imagePreview.innerHTML = '';
//        imagePreview.appendChild(imageElement);

//        // Reset the file input field
//        fileInput.value = '';

//        // Display success or perform any other UI updates
//        console.log('File uploaded successfully!');
//    } else {
//        console.log('No file selected.');
//    }
//}

//document.getElementById('passportPathBack').addEventListener('change', uploadFile4);

// Close the pop-up message when the close button is clicked
var closeButton = document.querySelector('.popup-content .close');
closeButton.addEventListener('click', function () {
    var popupMessage = document.getElementById('popupMessage');
    popupMessage.style.display = 'none';
});





function changeFileLablel() {
    const container = document.getElementById("ImgInput");
    const fileInputs = container.querySelectorAll("input[type=file]");
    console.log(fileInputs)
    fileInputs.forEach((item, index) => {
        item.addEventListener("change", function (e) {

            let input = e.target
            let fileName = input.files.item(0).name;
            let id = input.getAttribute('id');

            document.getElementById(id + "Label").innerText = fileName;

        })
    })
}

changeFileLablel()