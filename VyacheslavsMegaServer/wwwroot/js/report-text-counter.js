const textarea = document.getElementById('reportText');
const currentLength = document.getElementById('current');
const counter = document.getElementById('counter');
const maxlength = 500;
const minlength = 5;

function onInput(event) {
    const input = event.target.value.trim();
    if (input.length >= maxlength || input.length < minlength) {
        counter.classList.add('form-group__hint--error');
    }
    else {
        counter.classList.remove('form-group__hint--error');
    }
    input.value = input.substr(0, maxlength);
    currentLength.textContent = input.length;
}

textarea.addEventListener('input', onInput);