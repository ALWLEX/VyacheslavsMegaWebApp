document.querySelectorAll('.color-field').forEach(field => {
    const hexInput = field.querySelector('.color-field__hex');
    const button = field.querySelector('.color-field__btn');
    const picker = field.querySelector('.color-field__picker');

    function syncPickerPosition() {
        const rect = hexInput.getBoundingClientRect();
        const parentRect = field.getBoundingClientRect();

        picker.style.left = (rect.left - parentRect.left) + 'px';
        picker.style.top = (rect.top - parentRect.top) + 'px';
        picker.style.width = rect.width + 'px';
        picker.style.height = rect.height + 'px';
    }

    button.addEventListener('click', () => {
        syncPickerPosition();
        if (/^#([0-9a-fA-F]{6})$/.test(hexInput.value)) {
            picker.value = hexInput.value;
        }
        picker.click();
    });

    picker.addEventListener('input', () => {
        hexInput.value = picker.value;
    });

    window.addEventListener('resize', syncPickerPosition);
});