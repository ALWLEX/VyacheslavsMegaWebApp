function previewImage(event) {
    const imagePreview = document.getElementById("imagePreview");
    let reader = new FileReader();

    imagePreview.style.display = "block";
    imagePreview.src = "https://i.gifer.com/origin/34/34338d26023e5515f6cc8969aa027bca_w200.gif"

    reader.onload = function () {
        imagePreview.src = reader.result;
    }

    if (event.target.files.length > 0) {
        reader.readAsDataURL(event.target.files[0]);
    }
}