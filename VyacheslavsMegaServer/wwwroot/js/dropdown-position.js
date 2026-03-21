const dropdowns = document.querySelectorAll('.dropdown');

dropdowns.forEach(dropdown => {
    const content = dropdown.querySelector('.dropdown__menu');

    dropdown.addEventListener('mouseenter', () => {
        content.style.right = '0';
        const rect = content.getBoundingClientRect();

        if (rect.left < 0) {
            content.style.right = `${rect.left}px`;
        }
    });
});