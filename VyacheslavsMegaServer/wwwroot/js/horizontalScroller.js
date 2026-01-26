const content = document.getElementById("HScrollerContent");
const first = document.getElementById("HScrollerToStart");
const prev = document.getElementById("HScrollerPrev");
const next = document.getElementById("HScrollerNext");
const last = document.getElementById("HScrollerToEnd");

function goToSlide(index) {
    content.scrollLeft = index * content.offsetWidth;
}

function getCurrentSlideIndex() {
    return Math.round(content.scrollLeft / content.offsetWidth);
}

first.addEventListener('click', () => {
    goToSlide(0);
});

next.addEventListener('click', () => {
    let currentSlideIndex = getCurrentSlideIndex();
    if (currentSlideIndex == content.children.length - 1)
        return;
    goToSlide(currentSlideIndex + 1);
});

prev.addEventListener('click', () => {
    let currentSlideIndex = getCurrentSlideIndex();
    if (currentSlideIndex == 0)
        return;
    goToSlide(currentSlideIndex - 1);
});

last.addEventListener('click', () => {
    goToSlide(content.children.length - 1);
});