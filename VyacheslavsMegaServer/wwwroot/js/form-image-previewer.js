function previewImage(event) {
    const input = event.target;
    const preview = document.getElementById('imagePreview');
    const fileName = document.getElementById('fileName');

    if (!input.files || !input.files[0]) return;

    const file = input.files[0];
    fileName.textContent = file.name;

    const reader = new FileReader();
    reader.onload = function (e) {
        preview.src = e.target.result;
        preview.style.display = 'block';
    };
    reader.readAsDataURL(file);
}